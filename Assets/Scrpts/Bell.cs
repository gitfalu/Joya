using UnityEngine;
using System.Collections;

public class Bell : MonoBehaviour
{
    [Header("===== 鐘のパラメータ ======")]
    [SerializeField]
    private Vector3 _basePosition;
    [SerializeField]
    private JoyaScoreData _score;
    [SerializeField]
    private GameObject _damageVal;
    [SerializeField,Range(0.0f,1.0f)]
    private float _bellhitMulti = 0.1f;
    [SerializeField,Range(0.0f,1.0f)]
    private float _bellhitGoriyakuMulti = 0.5f;
    [SerializeField] private float _gravScale = -30.0f;
    [SerializeField] private bool _gravSubtract = false;

    [SerializeField]
    private GameObject _hitEffect;
    [SerializeField]
    private GameObject _orb;

    [SerializeField]
    private float _damageInterval = 0.2f;
    private float _currentInterval;

    private int _hp = 108;
    // ゲージの１で変わる割合
    private float _gaugeHPinOne;

    private bool _isStart = false;
    private bool _isGoriyaku = false;
    private bool _isLimit = false;

    [SerializeField]
    private float _blinkingCycle = 0.2f;
    [SerializeField]
    private float _blinkThreshold = 5.0f;
    private float _currentCycle;
    private bool _isRed = false;

    [Header("===== ステート管理 ======")]
    [SerializeField]
    private Canvas _worldCanvas;
    [SerializeField]
    private TMPro.TextMeshProUGUI _timerTMP;
    [SerializeField, Tooltip("失敗時のCanvas")]
    private GameObject _missCanvas;
    [SerializeField,Tooltip("残りの煩悩の数")] 
    private TMPro.TextMeshProUGUI _tmpMissResult;
    [SerializeField]
    private GameObject _AnounceCanvas;

    [SerializeField, Tooltip("ごりやくりざると画面")]
    private GameObject _goriyakuEndCanvas;
    [SerializeField, Tooltip("獲得したご利益の数")]
    private TMPro.TextMeshProUGUI _tmpGoriResult;

    [SerializeField]
    private Sprite _halfBell;

    [SerializeField]
    private BackGroundManager _backGroundManager;

    private float _timer = 30.0f;
    [Header("===== ゲージ管理 =====")]
    [SerializeField,Tooltip("表の緑")] private GameObject _gauge;
    
    [SerializeField,Tooltip("ダメージ時の赤")] private GameObject _graceGauge;
    [SerializeField, Tooltip("赤が消えるまでの時間")] private float _waitingTime = 0.5f;
    [SerializeField, Tooltip("煩悩/ご利益")] private TMPro.TextMeshProUGUI _scoreText;

    public int HP => _hp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _basePosition = transform.localPosition;
        _gaugeHPinOne = _gauge.GetComponent<RectTransform>().sizeDelta.x / _hp;
        GetComponent<BoxCollider2D>().enabled = false;
        _currentInterval = 0.0f;
        _missCanvas.SetActive(false);
        _goriyakuEndCanvas.SetActive(false);
        Physics.gravity = new Vector3(0.0f,_gravScale,0.0f);
        Physics2D.gravity = new Vector3(0.0f,_gravScale,0.0f);
        _score.Initialize();
        _isLimit = false;
        _currentCycle = _blinkingCycle;
        SoundManager.instance?.StopBGM();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isStart) return;
        if(_gravSubtract)
        {
            var grav = Physics.gravity;

            if(grav.y < -9.8f)
            {
                grav.y += Time.deltaTime;
                Physics.gravity = grav;
            }
            var grav2D = Physics2D.gravity;
            if (grav2D.y < -9.8f)
            {
                grav2D.y += Time.deltaTime;
                Physics2D.gravity = grav2D;
            }
        }
        _timer -= Time.deltaTime;
        if (_timer < _blinkThreshold)
            _isLimit = true;
        else
            _isLimit = false;
        if(_isLimit)
        {
            _currentCycle -= Time.deltaTime;
            if (_currentCycle <= 0.0f)
            {
                _currentCycle = _blinkingCycle;
                _isRed = !_isRed;
            }
        }
        else
        {
            _isRed = false;
        }

        if (_timer < 0.0f)
        {
            _timer = 0.0f;
            Finish();
        }
        if(_isRed)
            _timerTMP.color = Color.red;
        else
            _timerTMP.color = Color.white;
        _timerTMP.text = _timer.ToString("F0");
        if(_currentInterval > 0.0f)
            _currentInterval -= Time.deltaTime;
        transform.localPosition = Vector3.Lerp(transform.localPosition, _basePosition, Time.deltaTime * 2.0f);
    }

    public void OnStart()
    {
        _isStart = true;
        _timer = 30.0f;
        GetComponent<BoxCollider2D>().enabled = true;
    }

    void Finish()
    {
        Destroy(GetComponent<BoxCollider2D>());
        if(_isGoriyaku)
        {
            _goriyakuEndCanvas.SetActive(true);
            _tmpGoriResult.text = "獲得したご利益の数　" + _score.Goriyaku.ToString();

        }
        else
        {
            _missCanvas.SetActive(true);
            _tmpMissResult.text = "祓えなかった煩悩　" + _hp.ToString();
        }
        _scoreText.enabled = false;
        _timerTMP.enabled = false;
        _gauge.SetActive(false);
        _graceGauge.SetActive(false);
        _basePosition.y = 3.0f;
        var img = GetComponent<SpriteRenderer>();
        img.sprite = _halfBell;
        SoundManager.instance?.StopBGM();
    }

    void GoriyakuTimeStart()
    {
        Time.timeScale = 0.5f;
        Invoke("TimeScaleReset", 0.5f);
        // 煩悩をなくす最後の攻撃
        _backGroundManager.FinalAttack();
    }

    void TimeScaleReset()
    {
        Time.timeScale = 1.0f;
        // タイムスケールをリセットするタイミングでご利益フィーバーアナウンス
        Instantiate(_AnounceCanvas,_worldCanvas.transform);
        // ご利益フィーバー開始
        _backGroundManager.StartFever();
        SoundManager.instance?.PlayBGM("kakegoe");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_currentInterval > 0.0f) return;
        if (collision.gameObject.tag != "Hummer") return;

        // 衝突を検知したときの位置から衝突したときの方向を算出
        Vector3 hitPostion = collision.transform.position;
        Vector3 hitDirection = (this.transform.position - hitPostion).normalized;

        float hitPower = 1.0f;
        var parentArti = collision.GetComponentInParent<ArticulationBody>();
        if (parentArti != null)
        {
            hitPower = (parentArti.linearVelocity.magnitude * parentArti.mass);
            Vector3 newlinear = parentArti.linearVelocity;
            if(_isGoriyaku)
                newlinear.x *= (_bellhitMulti * hitPower + 1.0f);
            else
                newlinear.x *= (_bellhitGoriyakuMulti * hitPower + 1.0f);
            parentArti.linearVelocity = newlinear;
        }

        HitReaction(hitDirection, hitPower);

        if(_isGoriyaku)
        {
            _score.AddGoriyaku((int)hitPower);
            _scoreText.text = "ご利益 " + _score.Goriyaku.ToString();
            // オーブを生成
            var obj = Instantiate(_orb,transform.position,Quaternion.identity);
            var scl = obj.transform.localScale;
            obj.transform.localScale = new Vector3(hitPower * scl.x, hitPower * scl.y, 1.0f);
            var rb = obj.GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector2((Random.value - 0.5f) * 10.0f, (Random.value - 0.5f) * 10.0f));
        }
        else
        {
            if (_hp - (int)hitPower <= 0)
            {
                _hp = (int)hitPower;
                _score.AddGoriyaku((int)hitPower - _hp);
                _scoreText.color = Color.green;
                _scoreText.text = "ご利益 " + _score.Goriyaku.ToString();
                _timer += 10.0f;
                _isGoriyaku = true;
                GoriyakuTimeStart();
            }
            else
            {
                _hp -= (int)hitPower;
                _scoreText.text = "残り煩悩 " + _hp.ToString();
            }
        }

        if (_damageVal)
        {
            var obj = Instantiate(_damageVal, hitPostion, Quaternion.identity,_worldCanvas.transform);
            obj.GetComponent<DamageText>().initialize(hitPower,_isGoriyaku);
            Destroy(obj, 1.0f);
        }
    }

    private void HitReaction(Vector3 hitDirection,float hitPower)
    {
        transform.localPosition += (hitDirection * hitPower * 0.1f);

        BeInjured((int)hitPower);
        _currentInterval = _damageInterval;

        // カメラシェイク
        var mainCam = Camera.main;
        if(mainCam.TryGetComponent<CameraShake>(out CameraShake camShake))
        {
            StartCoroutine(camShake.Shake(0.2f,hitPower * 0.2f));
        }

        // ヒットエフェクトを表示
        if (_hitEffect != null)
        {
            float angle = Mathf.Atan2(hitDirection.y, hitDirection.x) * Mathf.Rad2Deg;
            // Z軸の回転を適用
            var obj = Instantiate(_hitEffect,
                transform.position, 
                Quaternion.Euler(0f, 0f, angle - 90.0f), transform);
            obj.transform.localScale = new Vector3(hitPower * 0.1f, hitPower * 0.1f, 1.0f);
            Destroy(obj, 0.1f);
        }

        SoundManager.instance?.PlaySE("bell", hitPower * 0.2f);
    }

    // 攻撃力をそれぞれのボタンで設定
    public void BeInjured(int atacck)
    {
        // 攻撃力と体力1あたりの幅の積が実際に体力ゲージから減らす幅
        float damege = _gaugeHPinOne * atacck;

        // 減らす幅を設定してコルーチン”damegeEm”を呼び出し
        StartCoroutine(damegeEm(damege));
    }

    // 体力ゲージを減らすコルーチン
    IEnumerator damegeEm(float damege)
    {
        // 体力ゲージの幅と高さをVector2で取り出す(Width,Height)
        Vector2 nowsafes = _gauge.GetComponent<RectTransform>().sizeDelta;
        // 体力ゲージの幅からダメージ分の幅を引く
        nowsafes.x -= damege;
        // 体力ゲージに計算済みのVector2を設定する
        _gauge.GetComponent<RectTransform>().sizeDelta = nowsafes;

        // ”_waitingTime”秒待つ
        yield return new WaitForSeconds(_waitingTime);
        // 猶予ゲージに計算済みのVector2を設定する
        _graceGauge.GetComponent<RectTransform>().sizeDelta = nowsafes;
    }
}
