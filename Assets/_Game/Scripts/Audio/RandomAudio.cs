using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudio : MonoBehaviour
{
    public AudioSource[] audioSource;
    public AudioClip[] clips;
    private int indiceAudio;

    public void Play(int i)
    {
        indiceAudio = (indiceAudio + 1) % audioSource.Length;
        audioSource[indiceAudio].clip = clips[i];
        audioSource[indiceAudio].Play();
    }

    public void PlayRandom()
    {
        Play(Random.Range(0, clips.Length));
    }
}
