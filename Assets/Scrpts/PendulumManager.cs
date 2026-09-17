using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
public class PendulumManager : MonoBehaviour
{
    [Header("===== 有効化管理 ======")]
    [SerializeField]
    private TrailRenderer _trailRenderer;

    [SerializeField,Tooltip("実行した後に動く振り子のヒンジ")]
    private List<ArticulationBody> _weights = new List<ArticulationBody>();

    [SerializeField,Tooltip("実行後に非表示にするオブジェクトリスト")]
    private List<GameObject> _hiddenList = new List<GameObject>();

    [Header("====== キャンバス =======")]
    [SerializeField, Tooltip("実行後に有効化するUI")]
    private GameObject _controllerCanvas;
    [SerializeField,Tooltip("実行後に無効化するUI")]
    private GameObject _scoreCanvas;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var b in _weights)
        {
            b.enabled = false;
        }
        _controllerCanvas?.SetActive(true);
        _scoreCanvas?.SetActive(false);
        if(_trailRenderer != null) _trailRenderer.enabled = false;
        foreach (var b in _hiddenList) 
        {
            b.SetActive(true);
        }
    }

    public void OnStart()
    {
        foreach (var b in _weights)
        {
            b.enabled = true;
            b.WakeUp();
            b.maxAngularVelocity = float.MaxValue;
            b.linearVelocity+= new Vector3(0.0f,5.0f,0.0f);
        }
        _controllerCanvas?.SetActive(false);
        _scoreCanvas?.SetActive(true);
        if (_trailRenderer != null) _trailRenderer.enabled = true;
        foreach (var h in _hiddenList)
        {
            h.SetActive(false);
        }

        // カメラが近づく処理
        var mainCam = Camera.main;
        mainCam.transform.position = new Vector3(0.0f, -2.0f, -10.0f);
        var camComp = mainCam.GetComponent<Camera>();
        camComp.orthographicSize = 6.0f;
        mainCam.GetComponent<CameraShake>().Initialize();
    }

    public void OnStop()
    {
        SceneManager.LoadScene("Game");
    }
}
