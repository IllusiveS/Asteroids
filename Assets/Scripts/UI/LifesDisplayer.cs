using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FullInspector;
using Zenject;
public class LifesDisplayer : BaseBehavior {

    protected Text text;

    [Inject]
    protected IPlayerController playerController;

    void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        text.text = "Lifes: " + playerController.Lifes.ToString();
    }

}
