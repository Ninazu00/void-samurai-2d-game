using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // It is static so the whole game will only have one

    public AudioSource musicSource; // Source that plays Background music
    public AudioSource sfxSource; // Source that plays sound effects
    public AudioSource voiceLines; // Source that plays sound effects
    public AudioClip overworldMusic; // Audio clip of background music
    public AudioClip[] variousSFX; // Array of sound effects clips
    public AudioClip yukiPhaseOne; // Audio clip for the first Phase of the Yuki boss fight
    public AudioClip yukiPhaseTwo; // Audio clip for the second Phase of the Yuki boss fight
    public AudioClip yukiLaugh;
    public AudioClip swordsRain;
    public AudioClip voidBurst;
    public AudioClip voidDrownYou;
    public AudioClip worldAblaze;
    public AudioClip yukiMelee;
    public AudioClip yukiPointlessStruggle;
    public AudioClip yukiFullAttention;
    public AudioClip yukiTauntPainful;
    public AudioClip yukiDeath;
    public AudioClip yukiShortLaugh;

    [Header("Player SFX")]
    public AudioClip lightSlash; // Light attack sound
    public AudioClip heavySlash; // Heavy attack sound
    public AudioClip parry;      // Parry sound
    public AudioClip dash;       // Dash sound
    public AudioClip jumpSound;  // Jump sound

    [Header("Memory Echo")]
    public AudioClip Memechoaudio;

    void Start() { }

    void Update() { }

    void Awake()
    {
        // Make sure the entire game only has one AudioManager throughout
        Instance = this;
    }

    // Public in case another object needs to call for a specific sound effect to begin playing
    public void PlayMusicSFX(AudioClip clip)
    {
        sfxSource.Stop();
        sfxSource.clip = clip;
        sfxSource.Play();
    }

    // Public in case another object needs to call for a specific soundtrack to begin playing
    public void PlayMusic(AudioClip clip)
    {
        musicSource.volume = 1f;
        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.Play();
    }
    
    public void PlayVoiceLine(AudioClip clip)
    {
        voiceLines.clip = clip;
        voiceLines.Play();
    }

    // Function takes a bunch of sound clips as parameters
    public void PlayRandomSFX(params AudioClip[] clips)
    {
        variousSFX = clips;
        int index = Random.Range(0, variousSFX.Length);
        sfxSource.PlayOneShot(variousSFX[index]);
    }

    // ---------------- PLAYER ACTIONS SFX ----------------
    public void PlayLightSlash()
    {
        if (lightSlash != null)
            PlayMusicSFX(lightSlash);
    }

    public void PlayHeavySlash()
    {
        if (heavySlash != null)
            PlayMusicSFX(heavySlash);
    }

    public void PlayParry()
    {
        if (parry != null)
            PlayMusicSFX(parry);
    }

    public void PlayDash()
    {
        if (dash != null)
            PlayMusicSFX(dash);
    }

    public void PlayJump()
    {
        if (jumpSound != null)
            PlayMusicSFX(jumpSound);
    }
    // ---------------------------------------------------

    private IEnumerator yukiFadeOutCoroutine(float duration)
    {
        float startVolume = musicSource.volume;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            yield return null;
        }

        musicSource.volume = 0f;
        musicSource.Stop();
    }

    public void yukiFadeOut() { StartCoroutine(yukiFadeOutCoroutine(3f)); }
    public void playYukiOne() { PlayMusic(yukiPhaseOne); Debug.Log("Playing Yuki One"); }
    public void playYukiTwo() { PlayMusic(yukiPhaseTwo); Debug.Log("Playing Yuki Two"); }
    public void playSwordsRain() { PlayMusicSFX(swordsRain); }
    public void playVoidBurst() { PlayMusicSFX(voidBurst); }
    public void playVoidDrownYou() { PlayVoiceLine(voidDrownYou); }
    public void playWorldAblaze() { PlayMusicSFX(worldAblaze); }
    public void playYukiLaugh() { PlayVoiceLine(yukiLaugh); }
    public void playYukiMelee() { PlayMusicSFX(yukiMelee); }
    public void playYukiTaunt1() { PlayVoiceLine(yukiFullAttention); }
    public void playYukiTaunt2() { PlayVoiceLine(yukiPointlessStruggle); }
    public void playYukiTaunt3() { PlayVoiceLine(yukiTauntPainful); }
    public void playYukiDeath() { PlayVoiceLine(yukiDeath); }
    public void playYukiShortLaugh() 
    { 
        PlayVoiceLine(yukiShortLaugh); 
    }

    public void FadeOutMEFA(float duration) { StartCoroutine(MEFcoroutihne(duration)); }

    IEnumerator MEFcoroutihne(float duration)
    {
        float startVolume = voiceLines.volume;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            voiceLines.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            yield return null;
        }

        voiceLines.volume = startVolume;
        voiceLines.Stop();
    }
}
