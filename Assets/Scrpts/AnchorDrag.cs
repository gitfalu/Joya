using UnityEngine;
using UnityEngine.InputSystem;

public class AnchorDrag : MonoBehaviour
{
    private Vector3 offset;
    [SerializeField]
    private Transform _controlTrans;
    [SerializeField]
    private Transform _root;

    private void Start()
    {
        RotationReset();
    }

    void OnMouseDown()
    {
        offset = _controlTrans.position - GetMouseWorldPos();
        Debug.Log(_controlTrans.name + " is picking");
    }

    void OnMouseDrag()
    {
        _controlTrans.position = GetMouseWorldPos() + offset;
        //RotationReset();
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        Vector3 screenPos = new Vector3(
            mousePos.x,
            mousePos.y,
            -Camera.main.transform.position.z
        );

        return Camera.main.ScreenToWorldPoint(screenPos);
    }

    public void RotationReset()
    {
        transform.rotation = Quaternion.identity;
    }
}
