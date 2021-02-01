using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTiempo : MonoBehaviour
{
    public AudioSource[] audioSource;
    public AudioClip[] clips;
    private int indiceAudio;
    public bool automatico;
    public int tiempo = 30;
    public int state = 0;
    
    private void Start()
    {
        StartCoroutine(SoundOut());
        if (automatico)
        {
            PlayRandom();
        }
    }

    public void ChangeState(int s)
    {
        state = s;
    }
    IEnumerator SoundOut()
    {
        while (automatico)
        {
            PlayRandom();
            
            yield return new WaitForSeconds(tiempo);
        }
    }

    public void Play(int i)
    {
        indiceAudio = (indiceAudio + 1) % audioSource.Length;
        audioSource[indiceAudio].clip = clips[i];
        audioSource[indiceAudio].Play();
    }

    public void PlayRandom()
    {
        if (state == 0)
        {
            Play(Random.Range(12, clips.Length));
            tiempo = 30;
        }
        else if (state == 1)
        {
            Play(Random.Range(8, 12));
            tiempo = 20;
        }
        else if (state == 2)
        {
            Play(Random.Range(0, 7));
            tiempo = 5;
        }
    }

        public void Stop(int i)
    {
        audioSource[i].Stop();
    }

    public void StopActual()
    {
        Stop(indiceAudio);
    }

    public void Silencio()
    {
        for (int i = 0; i < audioSource.Length; i++)
        {
            Stop(i);
        }
    }
}
