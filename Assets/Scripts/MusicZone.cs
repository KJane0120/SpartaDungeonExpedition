using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicZone : MonoBehaviour
{
    public AudioSource source;
    public float fadeTime;
    public float maxVolume;
    private float targetVolume;

    private void Start()
    {
        targetVolume = 0.0f;
        source = GetComponent<AudioSource>();
        source.volume = targetVolume;
        source.Play();
    }

    private void Update()
    {
        if(!Mathf.Approximately(source.volume, targetVolume))
        {
            source.volume = Mathf.MoveTowards(source.volume, targetVolume, (maxVolume/fadeTime)*Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetVolume = maxVolume;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetVolume = 0.0f;
        }
    }
}
