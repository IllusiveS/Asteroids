using UnityEngine;
using System.Collections;
using FullInspector;
using Zenject;
using UnityEngine.UI;

public class PointsDisplayer : BaseBehavior {

    [Inject]
    protected IPlayerController player;

    protected Text textField;
    void Start()
    {
        textField = GetComponent<Text>();
    }
    void Update()
    {
        textField.text = "Points: " + player.Points.ToString();
    }

}
