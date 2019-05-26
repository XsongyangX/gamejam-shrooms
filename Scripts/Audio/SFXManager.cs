using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public enum Style { SELECT_TILE, COLONISE_SHROOM, COLONISE_HUMAN}

    public AudioClip[] clipsSelect;
    public AudioClip[] clipsColoniseShroom;
    public AudioClip[] clipsColoniseHuman;
    public AudioClip transition;

    private static SFXManager main;

    private void Awake()
    {
        main = this;
    }

    private AudioClip[] GetClipsOfStyle(Style style)
    {
        switch (style)
        {
            case Style.SELECT_TILE:
                return clipsSelect;
            case Style.COLONISE_SHROOM:
                return clipsColoniseShroom;
            case Style.COLONISE_HUMAN:
                return clipsColoniseHuman;
            default:
                return new AudioClip[] { };
        }
    }

    public static void Play(Style style)
    {
        AudioSource s = new GameObject("SFX - " + style.ToString(), typeof(AudioSource)).GetComponent<AudioSource>();
        s.gameObject.hideFlags = HideFlags.HideAndDontSave;
        AudioClip[] clips = main.GetClipsOfStyle(style);
        int clipID = Random.Range(0, clips.Length);
        s.clip = clips[clipID];
        s.Play();
        s.spatialize = false;
        Destroy(s.gameObject, s.clip.length + 1);
    }

    private void PlayTransition()
    {
        AudioSource s = new GameObject("Music - Transition", typeof(AudioSource)).GetComponent<AudioSource>();
        s.gameObject.hideFlags = HideFlags.HideAndDontSave;
        s.clip = transition;
        s.Play();
        s.spatialize = false;
        Destroy(s.gameObject, s.clip.length + 1);
    }

    public static void PlayMusicTransition(float delay)
    {
        main.Invoke("PlayTransition", delay);
    }
}
