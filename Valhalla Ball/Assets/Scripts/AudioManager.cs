using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    // Awake is called before the first frame update and before Start()
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        
    }

    private void Start()
    {
        Play("BackgroundMusic1");
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound " + name + " was not found.");
            return;
        }
            
        s.source.Play();
    }

    public void Play(string name, float volume, float pitch, bool loop)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound " + name + " was not found.");
            return;
        }
        s.source.volume = volume;
        s.source.pitch = pitch;
        s.source.loop = loop;

        s.source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
