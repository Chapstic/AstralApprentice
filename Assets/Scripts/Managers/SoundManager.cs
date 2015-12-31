using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public AudioSource efxSource;
    public AudioSource musicSource;
    public static SoundManager instance = null;

    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;

    private AudioClip FForestMusic;
    private AudioClip UnMarsMusic;
    private AudioClip IndustrialMusic;

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        // Load sounds
        FForestMusic = Resources.Load("Audio/Background/ASTAPP_Forest_loop") as AudioClip;
        UnMarsMusic = Resources.Load("Audio/Background/ASTAPP_UnMars_loop1") as AudioClip;
        IndustrialMusic = Resources.Load("Audio/Background/ASTAPP_Mining_loop1") as AudioClip;
    }

    void Update()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public void PlaySingle(AudioClip clip)
    {
        //efxSource.clip = clip;
        //efxSource.Play();
        efxSource.PlayOneShot(clip);
    }

    public void RandomizeSfx(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        efxSource.pitch = randomPitch;
        efxSource.clip = clips[randomIndex];
        efxSource.Play();
    }

    public void PlayMusic(string TrackName)
    {
        if(TrackName == "Forest")
        {
            if (FForestMusic)
            {
                musicSource.clip = FForestMusic;
                musicSource.Play();
            }
        }
        else if(TrackName == "UnMars")
        {
            if (UnMarsMusic)
            {
                musicSource.clip = UnMarsMusic;
                musicSource.Play();
            }
        }
        else if (TrackName == "Industrial")
        {
            if (IndustrialMusic)
            {
                musicSource.clip = IndustrialMusic;
                musicSource.Play();
            }
        }
    }

}
