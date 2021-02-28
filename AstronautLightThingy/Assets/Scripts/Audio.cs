using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Audio : MonoBehaviour
{
    public static Audio instance;

    private void Awake()
    {
        instance = this;
        audiosources = new AudioSource[sounds.Length];
        for(int x = 0;x < sounds.Length;x++)
        {
            audiosources[x] = gameObject.AddComponent<AudioSource>();
            audiosources[x].pitch = 0.9f;
            audiosources[x].volume = 1f;
        }
    }

    public AudioClip[] sounds;
    public AudioSource[] audiosources;

    public void Start()
    {
    }
    public void playHurt()
    {
        audiosources[0].clip = sounds[0];
        audiosources[0].Play();
    }

    public void playShoot()
    {
        audiosources[1].clip = sounds[1];
        audiosources[1].Play();
    }

    public void playEnemy()
    {
        audiosources[2].clip = sounds[2];
        audiosources[2].Play();
    }

    public void playPortal()
    {
        audiosources[3].clip = sounds[3];
        audiosources[3].Play();
    }

    public void playExplosion()
    {
        audiosources[4].clip = sounds[4];
        audiosources[4].Play();
    }
    public void playLazer()
    {
        audiosources[5].clip = sounds[5];
        audiosources[5].Play();
    }
    public void playRestore()
    {
        audiosources[6].clip = sounds[6];
        audiosources[6].Play();
    }

    bool jetpackPlaying = false;
    public void playJetpack()
    {
        if (jetpackPlaying)
            return;
        jetpackPlaying = true;
        audiosources[7].clip = sounds[7];
        audiosources[7].loop = true;
        audiosources[7].Play();
    }

    public void stopJetpack()
    {
        if (!jetpackPlaying)
            return;
        jetpackPlaying = false;
        audiosources[7].Stop();

    }
}
