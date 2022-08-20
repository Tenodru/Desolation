using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    [Header("Music Clips")]
    [SerializeField] AudioClip mainMenuMusic;
    [SerializeField] AudioClip gameMusic1;
    [SerializeField] AudioClip gameMusic2;

    AudioSource audioPlayer;

    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMainMenuMusic()
    {
        audioPlayer.PlayOneShot(mainMenuMusic);
    }
}
