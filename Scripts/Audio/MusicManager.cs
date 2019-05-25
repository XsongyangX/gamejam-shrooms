using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager main;

    public AudioClip[] clips;

    private List<AudioSource> channels = new List<AudioSource>();

    private void Awake()
    {
        main = this;
    }
    
    void Start()
    {
        SetupMusicChannels();

        FadeInChannel(0, 5); // Play the first channel at the beginning of the game
    }

    private void SetupMusicChannels()
    {
        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i] == null) continue;

            GameObject obj = new GameObject(clips[i].name, typeof(AudioSource));
            obj.transform.SetParent(transform);
            AudioSource source = obj.GetComponent<AudioSource>();
            source.playOnAwake = false;
            source.clip = clips[i];
            source.volume = 0;
            source.loop = true;
            source.Play();

            channels.Add(source);
        }
    }

    public static void FadeInChannel(int channelID, float time)
    {
        main.StartCoroutine(main.FadeCoroutine(channelID, 1, time));
    }

    public static void FadeOutChannel(int channelID, float time)
    {
        main.StartCoroutine(main.FadeCoroutine(channelID, 0, time));
    }

    public static void FadeChannel(int channelID, float volume, float time)
    {
        main.StartCoroutine(main.FadeCoroutine(channelID, volume, time));
    }

    IEnumerator FadeCoroutine(int channelID, float targetVolume, float time)
    {
        AudioSource source = channels[channelID];
        float startVolume = source.volume;
        float timer = 0;

        while (timer < time)
        {
            float t = timer / time;
            float volume = Mathf.Lerp(startVolume, targetVolume, t);
            source.volume = volume;
            timer += Time.deltaTime;
            yield return new WaitForSeconds(0);
        }
    }
}
