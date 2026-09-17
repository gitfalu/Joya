using UnityEngine;
using UnityEngine.UI;

public class BackGroundManager : MonoBehaviour
{

    private bool _isGoriyaku = false;

    private bool _isGoriyaku_1 = true;

    [SerializeField]
    private Sprite _Goriyaku_1;
    [SerializeField]
    private Sprite _Goriyaku_2;
    [SerializeField]
    private float _changeTime = 0.3f;
    private float _currentTimer;

    [SerializeField]
    private GameObject _defaultBG;
    [SerializeField]
    private GameObject _goriyakuBG;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentTimer = _changeTime;
        _goriyakuBG.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(_isGoriyaku)
        {
            _currentTimer -= Time.deltaTime;

            if(_currentTimer <= 0.0f)
            {
                _currentTimer = _changeTime;
                if(_isGoriyaku_1)
                {
                    _goriyakuBG.GetComponent<Image>().sprite = _Goriyaku_2;
                    _isGoriyaku_1 = false;
                }
                else
                {
                    _goriyakuBG.GetComponent<Image>().sprite = _Goriyaku_1;
                    _isGoriyaku_1 = true;
                }
            }
        }
    }

    public void FinalAttack()
    {
        _goriyakuBG.SetActive(true);
        var img = _goriyakuBG.GetComponent<Image>();
        img.color = new Color(1.0f,1.0f,1.0f,0.2f);
    }

    public void StartFever()
    {
        var img = _goriyakuBG.GetComponent<Image>();
        img.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        _isGoriyaku = true;
        _currentTimer = _changeTime;
        _defaultBG.SetActive(false);
    }
}
