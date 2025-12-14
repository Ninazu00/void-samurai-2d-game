using UnityEngine;

public class VoidPlatform : MonoBehaviour
{
    public float visibleTime = 2f;
    public float hiddenTime = 2f;

    private Renderer rend;
    private Collider col;
    private float timer;
    private bool isVisible = true;

    void Start()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();
        timer = visibleTime;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            isVisible = !isVisible;

            rend.enabled = isVisible;
            col.enabled = isVisible;

            timer = isVisible ? visibleTime : hiddenTime;
        }
    }
}
