using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager main;
    public bool loopStarted = false;
    public bool isMainLoop = true;

    public AudioClip[] clips;

    private List<AudioSource> channels = new List<AudioSource>();

    private void Awake()
    {

        DontDestroyOnLoad(gameObject);
        if (main != null)
        {
            if (main.clips[0] != clips[0])
            {
                for (int i = 0; i < main.channels.Count; i++)
                {
                    main.CreateFade(i, 0, 2, 0);
                }
                Destroy(main.gameObject, 5);
            } else
            {
                Destroy(gameObject);
                return;
            }
        }
        main = this;
    }
    
    void Start()
    {
        SetupMusicChannels();

        FadeInChannel(0, 2); // Play the first channel at the beginning of the game
        channels[0].Play();
        if (isMainLoop)
        {
            FadeChannelDelay(0, 0, 2, channels[0].clip.length - 2);
            FadeChannelDelay(1, 1, 2, channels[0].clip.length - 2);
            Invoke("StartLoop", channels[0].clip.length - 2);
            SFXManager.PlayMusicTransition(channels[0].clip.length - 2);
            //FadeChannelDelay(0, 1, 5, 5);
        }
    }

    public void StartLoop()
    {
        if(!loopStarted)
        {
            Debug.Log("Loop Started");
            loopStarted = true;
            for (int i = 1; i<channels.Count; i++)
            {
            channels[i].Play();
            }
        }
    }

    private void Update()
    {
        UpdateInstructions();
    }

    private void UpdateInstructions()
    {
        for (int i = 0; i < fadeInstructions.Count; i++)
        {
            ApplyFade(i);
        }
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
            //source.Play();

            channels.Add(source);
        }
    }

    public static void FadeInChannel(int channelID, float time)
    {
        Debug.Log("Fade in " + channelID);
        //main.StartCoroutine(main.FadeCoroutineDelay(channelID, 1, time,0));
        if(channelID == 3)
            FadeChannel(channelID, 0.3f, time);
        else
            FadeChannel(channelID, 0.5f, time);
    }

    public static void FadeOutChannel(int channelID, float time)
    {
        Debug.Log("Fade out " + channelID);
        FadeChannel(channelID, 0, time);
    }

    public static void FadeChannel(int channelID, float volume, float time)
    {
        //main.StartCoroutine(main.FadeCoroutineDelay(channelID, volume, time,0));
        FadeChannelDelay(channelID, volume, time, 0);
    }

    public static void FadeChannelDelay(int channelID, float volume, float time, float delay)
    {
        main.CreateFade(channelID, volume, time, delay);
    }

    private struct FadeInstruction
    {
        public int channelID;
        public float startVolume;
        public float targetVolume;
        public float timer;
        public float duration;
    }

    private List<FadeInstruction> fadeInstructions = new List<FadeInstruction>();

    private void CreateFade(int channelID, float targetVolume, float duration, float delay)
    {
        FadeInstruction fade = new FadeInstruction();
        fade.channelID = channelID;
        fade.startVolume = channels[channelID].volume;
        fade.targetVolume = targetVolume;
        fade.duration = duration;
        fade.timer = -delay;
        fadeInstructions.Add(fade);
    }

    private void ApplyFade(int id)
    {
        FadeInstruction fade = fadeInstructions[id];
        AudioSource source = channels[fade.channelID];
        fade.timer += Time.deltaTime;
        if (fade.timer > 0)
        {
            float t = fade.timer / fade.duration;
            source.volume = Mathf.Lerp(fade.startVolume, fade.targetVolume, t);
            if (fade.timer > fade.duration)
            {
                fadeInstructions.RemoveAt(id);
                return;
            }
        } else
        {
            fade.startVolume = source.volume;
        }
        fadeInstructions[id] = fade;
    }
    /*
    IEnumerator FadeCoroutineDelay(int channelID, float targetVolume, float time, float delay)
    {
        AudioSource source = channels[channelID];
        float startVolume = source.volume;
        float timer = -delay;

        while (timer < time)
        {
            if (timer > 0)
            {
                float t = timer / time;
                float volume = Mathf.Lerp(startVolume, targetVolume, t);
                source.volume = volume;
            }
            timer += Time.deltaTime;
            yield return new WaitForSeconds(0);
        }
        Debug.Log("Fade End");
    }*/
}
