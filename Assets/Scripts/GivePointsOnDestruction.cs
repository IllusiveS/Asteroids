using UnityEngine;
using System.Collections;
using FullInspector;
using Zenject;

public class GivePointsOnDestruction : BaseBehavior {

    [Inject]
    protected IPlayerController playerController;

    public int pointsToGive;

    void OnDestroy()
    {
        playerController.AddPoints(pointsToGive);
    }

}
