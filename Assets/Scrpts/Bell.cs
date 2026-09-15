using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class Bell : MonoBehaviour
{

    private Vector3 _basePosition;
    [SerializeField]
    private JoyaScoreData _score;

    private int _hp = 108;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _basePosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _basePosition, 1.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Hummer") return;
        Debug.Log("Bell: Hummer Hit");

        // 衝突を検知したときの位置から衝突したときの方向を算出
        Vector3 hitPostion = collision.transform.position;
        Vector3 hitDirection = (this.transform.position - hitPostion).normalized;

        float hitPower = 1.0f;
        if(collision.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            hitPower = rb.angularVelocity;
        }

        HitReaction(hitDirection, hitPower);

        if(_hp - (int)hitPower <= 0)
        {
            _score.AddGoriyaku((int)hitPower - _hp);
        }
        _hp -= (int)hitPower;
    }

    private void HitReaction(Vector3 hitDirection,float hitPower)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        // 当たった時のベクトルと当たった時の力を元にリアクション
        Vector2 ImpulseDir = new Vector2(hitDirection.x * hitPower, hitDirection.y * hitPower);
        rb.AddForce(ImpulseDir, ForceMode2D.Impulse);
    }
}
