using UnityEngine;

public class OrbMove : MonoBehaviour
{
    [SerializeField] private float speed = 300f;
    [SerializeField] private Vector2 direction = new Vector2(1f, 0.7f);

    private RectTransform rectTransform;
    private RectTransform canvasRect;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasRect = rectTransform.parent.GetComponent<RectTransform>();

        direction.Normalize();
    }

    private void Update()
    {
        Vector2 position = rectTransform.anchoredPosition;

        position += direction * speed * Time.deltaTime;

        Vector2 objectSize = rectTransform.rect.size;
        Vector2 canvasSize = canvasRect.rect.size;

        float halfWidth = objectSize.x * 0.5f;
        float halfHeight = objectSize.y * 0.5f;

        float minX = -canvasSize.x * 0.5f + halfWidth;
        float maxX = canvasSize.x * 0.5f - halfWidth;

        float minY = -canvasSize.y * 0.5f + halfHeight;
        float maxY = canvasSize.y * 0.5f - halfHeight;

        if (position.x <= minX)
        {
            position.x = minX;
            direction.x *= -1f;
        }
        else if (position.x >= maxX)
        {
            position.x = maxX;
            direction.x *= -1f;
        }

        if (position.y <= minY)
        {
            position.y = minY;
            direction.y *= -1f;
        }
        else if (position.y >= maxY)
        {
            position.y = maxY;
            direction.y *= -1f;
        }

        rectTransform.anchoredPosition = position;
    }
}
