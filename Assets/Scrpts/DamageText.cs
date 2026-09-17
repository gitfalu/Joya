using UnityEngine;

public class DamageText : MonoBehaviour
{
    private TMPro.TextMeshProUGUI _damageText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }
    public void initialize(float val ,bool isGoriyaku)
    {
        _damageText = GetComponent<TMPro.TextMeshProUGUI>();
        if (isGoriyaku)
        {
            _damageText.text = ((int)val).ToString() + "‚²—˜‰v";
            _damageText.fontSize = val * 20.0f;
            Color col = Color.green;
            col.a = 0.5f;
            _damageText.color = col;
        }
        else
        {
            _damageText.text = ((int)val).ToString() + "”Ï”Y";
            _damageText.fontSize = val * 10.0f;
            Color col = Color.white;
            col.a = 0.5f;
            _damageText.color = col;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Color col = _damageText.color;
        col.a -=Time.deltaTime;
        _damageText.color = col;

        Vector3 pos = transform.position;
        pos.y += Time.deltaTime;
        transform.position = pos;
    }
}
