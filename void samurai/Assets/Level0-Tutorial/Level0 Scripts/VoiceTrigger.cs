using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceTrigger : MonoBehaviour
{
    public AudioClip voiceLine;
    public MeshRenderer textRenderer;
    public float textDuration = 3f;

    private bool triggered = false;

    void Start()
    {
        if (textRenderer != null)
            textRenderer.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        AudioManager.Instance.PlayVoiceLine(voiceLine);

        if (textRenderer != null)
        {
            textRenderer.enabled = true;
            Invoke(nameof(HideText), textDuration);
        }

        GetComponent<Collider2D>().enabled = false;
    }

    void HideText()
    {
        if (textRenderer != null)
            textRenderer.enabled = false;
    }
}
