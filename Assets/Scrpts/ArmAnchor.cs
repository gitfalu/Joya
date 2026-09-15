using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.Rendering.DebugUI;

public class ArmAnchor : MonoBehaviour
{
    private Transform _anchor;

    [SerializeField,Tooltip("Arm‚ÌŠp“x‚ð“®‚©‚·‘¬“x")]
    private float _controlSpeed = 1.0f;

    private float _moveDirection = 0.0f;
    private bool _isMove = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _anchor = GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        OnArmMove();
    }

    public void OnArmMoveStart(float value)
    {
        _isMove = true;
        _moveDirection = value;
    }

    public void OnArmMoveEnd()
    {
        _isMove = false;
        _moveDirection = 0.0f;
    }

    public void OnArmMove()
    {
        if (!_isMove) return;
        
        Vector3 newRot = _anchor.eulerAngles;

        newRot.z += Time.fixedDeltaTime * _moveDirection * _controlSpeed;
        // Šp“x‚É§ŒÀ‚ðŽ‚½‚¹‚é
        if(newRot.z > 60.0f)
        {
            newRot.z = 60.0f;
        }
        if (newRot.z < 20.0f)
        {
            newRot.z = 20.0f;
        }
        _anchor.eulerAngles = newRot;
    }

}
