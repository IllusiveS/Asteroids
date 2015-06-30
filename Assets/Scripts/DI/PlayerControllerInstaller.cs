using UnityEngine;
using System.Collections;
using Zenject;

public class PlayerControllerInstaller : MonoInstaller {

    public GameObject playerControllerPrefab;
    public GameObject asteroidPrefab;

    public override void InstallBindings()
    {
        GameObject newObj = Instantiate(playerControllerPrefab);
        Container.Bind<IPlayer>().ToInstance(newObj.GetComponent<IPlayer>());
        Container.Bind<IPlayerController>().ToInstance(newObj.GetComponent<Player>().Controller);
        Container.BindGameObjectFactory<Asteroid.Factory>(asteroidPrefab);
    }

}
