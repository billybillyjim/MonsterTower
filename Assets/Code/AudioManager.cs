using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    public AudioClip buttonClick;
    AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void playButtonClip()
    {
        audio.PlayOneShot(buttonClick, 1.0f);
    }
}
