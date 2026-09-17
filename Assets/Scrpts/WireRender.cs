using UnityEngine;

public class WireRender : MonoBehaviour
{
    [SerializeField]
    private Transform _anchorRoot;
    [SerializeField]
    private Transform _anchor_1;
    [SerializeField]
    private Transform _anchor_2;

    [SerializeField]
    private Material _wireColor;

    private LineRenderer _lineRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _lineRenderer = gameObject.AddComponent<LineRenderer>();
        _lineRenderer.positionCount = 3;
        _lineRenderer.startWidth = 0.5f;
        _lineRenderer.endWidth = 0.5f;
        _lineRenderer.material = _wireColor != null ? _wireColor : null;
    }

    // Update is called once per frame
    void Update()
    {
        _lineRenderer.SetPosition(0, _anchorRoot.position);
        _lineRenderer.SetPosition(1, _anchor_1.position);
        _lineRenderer.SetPosition(2, _anchor_2.position);
    }
}
