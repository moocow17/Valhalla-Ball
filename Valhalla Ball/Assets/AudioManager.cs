using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    // Awake is called before the first frame update and before Start()
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
