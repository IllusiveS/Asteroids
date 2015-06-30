using UnityEngine;
using System.Collections;
using Zenject;

public class PlayerControllerInstaller : MonoInstaller {

    public GameObject playerControllerPrefab;

    public override void InstallBindings()
    {
        Container.Bind<IPlayer>().ToSinglePrefab(playerControllerPrefab);
    }

}
