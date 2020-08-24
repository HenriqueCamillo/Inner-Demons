using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource _audioIntro, _audioLoop;
    [SerializeField] AudioClip songIntro, songLoop;

    // Start is called before the first frame update
    void Start()
    {
        _audioIntro.clip = songIntro;
        _audioLoop.clip = songLoop;
        _audioIntro.Play();
        _audioLoop.PlayDelayed(songIntro.length);
    }
}
