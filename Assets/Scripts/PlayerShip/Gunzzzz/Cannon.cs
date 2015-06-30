using UnityEngine;
using System.Collections;
using FullInspector;
using Zenject;

public class Cannon : BaseBehavior {

    [Inject]
    protected IPlayer playerController;

    public Transform spawnLocation;

    void Start()
    {
        GetComponentInParent<IPlayerShip>().OnFireTheCannons += Fire;
    }
    protected void Fire()
    {
        GameObject bullet = Instantiate<GameObject>(playerController.BulletPrefab);
        bullet.transform.position = spawnLocation.position;
        bullet.transform.rotation = transform.rotation;
        bullet.transform.Rotate(new Vector3(0, 0, 1), 90.0f);
    }

}
