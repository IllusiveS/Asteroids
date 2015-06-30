using UnityEngine;
using System.Collections;
using FullInspector;
using Zenject;

public class DJAsteroids : BaseBehavior {

    protected AudioSource audioSource;

    public AudioClip lowTone;
    public AudioClip highTone;

    [Inject]
    protected IGameState gameState;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameState.OnTimeTick += DropTheBass;
    }

    void DropTheBass()
    {
        if(audioSource.clip == lowTone)
        {
            audioSource.clip = highTone;
        }
        else
        {
            audioSource.clip = lowTone;
        }
        audioSource.Play();
    }
}
