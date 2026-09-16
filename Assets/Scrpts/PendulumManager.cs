using System.Collections.Generic;
using System.Drawing;
using UnityEngine;


public class PendulumManager : MonoBehaviour
{

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
        foreach (var b in _hiddenList) 
        {
            b.SetActive(true);
        }

    }

    public void OnStart()
    {
        for(int i = (_weights.Count - 1);i >= 0;--i)
        {
            _weights[i].enabled = true;
            _weights[i].WakeUp();
        }
        _controllerCanvas?.SetActive(false);
        _scoreCanvas?.SetActive(true);
        foreach (var h in _hiddenList)
        {
            h.SetActive(false);
        }
    }
}
