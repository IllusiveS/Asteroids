using UnityEngine;
using System.Collections;
using FullInspector;
using Zenject;

public class Fracturer : BaseBehavior {

    public int numberOfFracturedClones;
    public int numberOfFractures;

    [Inject]
    protected Asteroid.Factory factory;

    void Start()
    {
        GetComponent<IAsteroid>().AddRektListener(OnRekt);
    }

    void OnRekt()
    {
        if (numberOfFractures == 0) return;
        --numberOfFractures;
        for(int i = 0; i < numberOfFracturedClones; i++)
        {
            Asteroid newObject = factory.Create();
            newObject.transform.position = transform.position;
            newObject.GetComponent<Fracturer>().numberOfFractures = numberOfFractures;
            newObject.transform.localScale = new Vector3(0.5f + (0.5f * numberOfFractures), 0.5f + (0.5f * numberOfFractures), 0.5f + (0.5f * numberOfFractures));
        }
    }

}
