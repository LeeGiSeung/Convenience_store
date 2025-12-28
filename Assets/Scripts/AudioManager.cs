using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip audioClip;
    public AudioSource titleMusic;
    public List<AudioSource> bgm = new List<AudioSource>();

    public static AudioManager instance;

    public List<AudioSource> sfx = new List<AudioSource>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject); //새 장면을 로드할때 이것을 파괴하지않음
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private bool bgmPlaying;
    private int currentTrack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(bgmPlaying == true)
        {
            if(bgm[currentTrack].isPlaying == false)
            {
                // currentTrack++;
                // currentTrack %= bgm.Count;

                StartBGM();
            }
        }
    }

    public void StopMusic()
    {
        titleMusic.Stop();

        foreach(AudioSource track in bgm)
        {
            track.Stop();
        }

        bgmPlaying = false;
    }

    public void StartBGM()
    {
        StopMusic();

        bgmPlaying = true;

        currentTrack = Random.Range(0, bgm.Count);

        bgm[currentTrack].Play();
    }

    public void StartTitleMusic()
    {
        StopMusic();

        titleMusic.Play();
    }

    public void PlaySFX(int sfxToPlay)
    {
        sfx[sfxToPlay].Stop();

        sfx[sfxToPlay].Play();
    }
}
