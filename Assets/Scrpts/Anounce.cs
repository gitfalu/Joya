using UnityEngine;

public class Anounce : MonoBehaviour
{
    private RectTransform _rect;
    [SerializeField]
    private float _freezeTime = 1.0f;
    [SerializeField]
    private float _moveTime = 0.2f;
    private float _pastTime = 0.0f;
    private float _duration; 
    private Vector3 _targetPosition;
    private int _state; // ステートもどき

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rect = GetComponent<RectTransform>();
        _rect.position = new Vector3(2000.0f, 0.0f, 0.0f);
        _targetPosition = new Vector3(0.0f, 0.0f, 0.0f);
        _pastTime = 0.0f;
        _duration = _moveTime;
        _state = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _pastTime += Time.deltaTime;
        if (_state == 2 && _pastTime > (_moveTime * 2.0f + _freezeTime))
            Destroy(gameObject);
        if (_state == 1 && _pastTime > _moveTime + _freezeTime)
        {
            _targetPosition.x = -2000.0f;
            _duration += _moveTime;
            _state++;
        }
        if (_state == 0 && _pastTime > _moveTime)
        {
            _duration += _freezeTime;
            _state++;
        }

        float t = Mathf.Clamp01(_pastTime / _duration);
        _rect.position = Vector3.Lerp(_rect.position, _targetPosition,t);
    }
}
