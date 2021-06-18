using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class AudioClipDict
{
    public string key;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public List<AudioClipDict> audioFiles;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ChangeAudioClip(string key)
    {
        audioSource.clip = audioFiles.Where(dict => dict.key == key).Select(dict => dict.clip).ElementAt(0);
        audioSource.loop = true;
        audioSource.Play();
    }
}
