using UnityEngine;
using System.Collections;
using FullInspector;
using Zenject;

public class SpaceShipShootSound : BaseBehavior {

    [Inject]
    protected IPlayer player;

    protected AudioSource audioSource;
	void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player.AddFireObserver(PlayShootSound);
    }
    void PlayShootSound()
    {
        audioSource.Play();
    }
}
