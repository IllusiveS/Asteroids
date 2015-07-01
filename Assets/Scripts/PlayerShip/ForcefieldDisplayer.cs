using UnityEngine;
using System.Collections;
using Zenject;

public class ForcefieldDisplayer : MonoBehaviour {
    protected SpriteRenderer sprite;

    [Inject]
    protected IPlayer controller;

	// Use this for initialization
	void Start () {
        sprite = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        sprite.enabled = controller.isInvincible;
	}
}
