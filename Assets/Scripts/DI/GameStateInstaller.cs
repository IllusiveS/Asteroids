using UnityEngine;
using System.Collections;
using Zenject;

public class GameStateInstaller : MonoInstaller {

    public GameObject gameStatePrefab;

    public override void InstallBindings()
    {
        GameObject newObj = Instantiate(gameStatePrefab);
        Container.Bind<IGameState>().ToInstance(newObj.GetComponent<IGameState>());
    }

}
