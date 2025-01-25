

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    
    public AudioClip BubblePop;
    public AudioClip BubbleDestoryed;
    public AudioClip BubbleExpanding;

    static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<AudioManager>();
            }

            return instance;
        }
    }


    public static void PlayBubblePop()
    {
        GameObject tmp = new GameObject("AudioBubblePop");
        var audioSource = tmp.AddComponent<AudioSource>();
        audioSource.PlayOneShot(Instance.BubblePop);
    }

    public static void PlayBubbleDestroyed()
    {
        GameObject tmp = new GameObject("AudioBubbleDestroyed");
        var audioSource = tmp.AddComponent<AudioSource>();
        audioSource.PlayOneShot(Instance.BubbleDestoryed);
    }

    public static void PlayBubbleExpanding()
    {
        GameObject tmp = new GameObject("AudioBubbleExpanding");
        var audioSource = tmp.AddComponent<AudioSource>();
        audioSource.PlayOneShot(Instance.BubbleExpanding);
    }
}       