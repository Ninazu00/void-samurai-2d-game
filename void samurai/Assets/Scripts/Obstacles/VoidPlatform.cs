using UnityEngine;

public class VoidPlatform : MonoBehaviour
{
    public float visibleTime = 2f;
    public float hiddenTime = 2f;

    public float warningTime = 0.5f;
    public float flickerSpeed = 0.1f;
    public float shakeAmount = 0.05f;

    private SpriteRenderer rend;
    private Collider2D col;

    private float timer;
    private bool isVisible = true;

    private float flickerTimer = 0f;
    private Vector3 startPos;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        timer = visibleTime;
        startPos = transform.position;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (isVisible && timer <= warningTime)
        {
            Flicker();
            Shake();
        }

        if (timer <= 0f)
        {
            isVisible = !isVisible;

            rend.enabled = isVisible;
            col.enabled = isVisible;

            transform.position = startPos;

            timer = isVisible ? visibleTime : hiddenTime;
        }
    }

    void Flicker()
    {
        flickerTimer += Time.deltaTime;
        if (flickerTimer >= flickerSpeed)
        {
            rend.enabled = !rend.enabled;
            flickerTimer = 0f;
        }
    }

    void Shake()
    {
        Vector3 offset = Random.insideUnitCircle * shakeAmount;
        transform.position = startPos + offset;
    }
}
