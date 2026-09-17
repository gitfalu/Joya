using UnityEngine;
using UnityEngine.SceneManagement;

public class Hummer : MonoBehaviour
{
    private ArticulationBody _body;

    private float _BrokeTime = 3.0f;

    private float _currentBrokeTime;
    private bool _isGoBroking;

    private bool _isBroken;
    private bool _isIron;

    [SerializeField]
    private float _alwaysAccel = 0.02f;

    [SerializeField]
    private SpriteRenderer _hummerImage;
    [SerializeField]
    private Sprite _ironHummer;
    [SerializeField]
    private float _ironWeight = 1.0f;
    [SerializeField]
    private Sprite _woodHummer;
    [SerializeField]
    private float _woodWeight = 0.5f;


    [SerializeField]
    private TMPro.TextMeshProUGUI _brokenTimer;

    public bool IsIron => _isIron;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _body = GetComponent<ArticulationBody>();
        _currentBrokeTime = _BrokeTime;
        _isGoBroking = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newlinear = _body.linearVelocity;
        float addVel = 0.1f;
        if(newlinear.magnitude < 1.0f && _body.linearVelocity.magnitude > 0.0f) newlinear.x += addVel * 
                (_body.linearVelocity.x / Mathf.Abs(_body.linearVelocity.x));
        newlinear.x *= (1.0f + (_alwaysAccel * _body.mass * Time.deltaTime));
        _body.linearVelocity = newlinear;

        if (_body.linearVelocity.magnitude >= 30.0f)
        {
            _currentBrokeTime -= Time.deltaTime;
            _isGoBroking = true;
            if(_currentBrokeTime <= 0.0f)
            {
                Broken();
            }
        }
        else
        {
            _currentBrokeTime = _BrokeTime;
            _isGoBroking = false;
        }

        if(_isGoBroking)
        {
            _brokenTimer.enabled = true;
            _brokenTimer.text = _currentBrokeTime.ToString("F0");
        }
        else
        {
            _brokenTimer.enabled = false;
        }
    }

    void Broken()
    {
        SoundManager.instance?.StopBGM();
        SceneManager.LoadScene("Ending");
    }

    public void ChnageHummer(bool iron)
    {
        _isIron = iron;
        if(iron)
        {
            _hummerImage.sprite = _ironHummer;
            _body.mass = _ironWeight;
        }
        else
        {
            _hummerImage.sprite = _woodHummer;
            _body.mass = _woodWeight;
        }

    }
}
