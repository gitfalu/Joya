using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    private bool _isShake = false;
    private float _shakeTime = 0.2f;



    private Vector3 _initialPosition;

    public void Start()
    {
        _initialPosition = transform.localPosition;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Initialize()
    {
        _initialPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition,
            _initialPosition,Time.deltaTime * 2.0f);
    }

    public void OnCameraShake(Vector3 shakeDir,float power)
    {
        _isShake = true;
        transform.localPosition = (shakeDir * power);
    }
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = originalPosition + Random.insideUnitSphere * magnitude;
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition;
    }
}
