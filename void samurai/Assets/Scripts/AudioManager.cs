using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // It is static so the whole game will only have one

    public AudioSource musicSource; // Source that plays Background music
    public AudioSource sfxSource; // Source that plays sound effects

    public AudioClip overworldMusic; // Audio clip of background music
    public AudioClip yukiPhaseOne; // Audio clip for the first Phase of the Yuki boss fight
    public AudioClip yukiPhaseTwo; // Audio clip for the second Phase of the Yuki boss fight

    public AudioClip[] variousSFX; // Array of sound effects clips

    // Start is called before the first frame update
    void Start()
    {
        // Background music clip is assigned, and volume starts off being zero
        musicSource.clip = overworldMusic;

        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        // Make sure the entire game only has one AudioManager throughout
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Public in case another object needs to call for a specific sound effect to begin playing
    public void PlayMusicSFX(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.Play();
    }

    // Public in case another object needs to call for a specific soundtrack to begin playing
    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    // Function takes a bunch of sound clips as paramters
    public void PlayRandomSFX(params AudioClip[] clips)
    {
        // Assign the incoming array of items to our local arraylist varible called 'variousSFX'
        variousSFX = clips;

        // Randomly select a sound clip from the arraylist, then play that clip
        int index = Random.Range(0, variousSFX.Length);
        sfxSource.PlayOneShot(variousSFX[index]);
    }
}
