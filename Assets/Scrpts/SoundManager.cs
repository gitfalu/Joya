using UnityEngine;
using System.Collections.Generic;
using System;

public class SoundManager : MonoBehaviour
{
    [Serializable]
    public struct AudioData
    {
        public string name;
        public AudioClip clip;
    };

    public static SoundManager instance;

    [SerializeField]
    private List<AudioData> _soundData = new List<AudioData>();
    private AudioSource _source;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(instance == null) { instance = this; }
        _source = GetComponent<AudioSource>();
        if(FindObjectsByType<AudioListener>(FindObjectsSortMode.None).Length <= 0)
        {
            gameObject.AddComponent<AudioListener>();
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySE(string name,float volume = 0.5f)
    {
        var clip = _soundData.Find(data => data.name == name).clip;

        if (clip == null) return;
        volume = Mathf.Clamp01(volume);
        _source.PlayOneShot(clip,volume);
    }

    public void PlayBGM(string name)
    {
        StopBGM();
        var clip = _soundData.Find(data => data.name == name).clip;

        if (clip == null) return;
        _source.clip = clip;
        _source.Play();
    }

    public void StopBGM()
    {
        _source.Stop();   
    }
}
