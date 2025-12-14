using UnityEngine;
/*
public class MemoryEchoField : MonoBehaviour
{
    public float damagePerSecond = 5f;
    public float staminaDrainPerSecond = 5f;
    public float blurIncreasePerSecond = 0.5f;
    public float maxBlur = 5f;

    private bool inside;
    private float currentBlur = 0f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            inside = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            inside = false;
    }

    void Update()
    {
        if (!inside) return;

        // Damage
        PlayerStats stats = PlayerStats.instance;
        stats.health -= damagePerSecond * Time.deltaTime;
        stats.stamina -= staminaDrainPerSecond * Time.deltaTime;

        // Blur effect (placeholder variable)
        currentBlur += blurIncreasePerSecond * Time.deltaTime;
        currentBlur = Mathf.Clamp(currentBlur, 0, maxBlur);
        ScreenEffects.BlurAmount = currentBlur;

        // Death
        if (stats.health <= 0)
        {
            stats.Die();
        }
    }
}
*/