using UnityEngine;
using System.Collections;
using FullInspector;
using Zenject;

public class AlienShip : BaseBehavior, IAsteroid {

    static System.Random randomizer = new System.Random();

    public GameObject bulletPrefab;

    void IAsteroid.AddRektListener(RektHandler listener)
    {

    }
    void IAsteroid.GetRekt()
    {

    }

	void OnTriggerEnter2D(Collider2D coll)
    {
        IAsteroid potentialAsteroid = coll.gameObject.GetComponent<IAsteroid>();

        if (potentialAsteroid == null) return;
        potentialAsteroid.GetRekt();

        Destroy(gameObject);
    }

    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(CalculateMovementDirection());
        shotTime = shotCooldown + Time.time;
    }

    public float shotCooldown;
    protected float shotTime;

    void Update()
    {
        if (shotTime < Time.time) Fire();
    }
    void Fire()
    {
        shotTime = Time.time + shotCooldown;
        GameObject bullet = Instantiate<GameObject>(bulletPrefab);

        Vector3 direction = CalculateDirection();

        bullet.transform.position = direction + transform.position;

        Vector3 v3 = bullet.transform.position - transform.position;
        float angle = Mathf.Atan2(v3.y, v3.x);
        bullet.transform.rotation = Quaternion.FromToRotation(Vector3.right, v3 + bullet.transform.position - transform.position);

    }
    protected Vector3 CalculateMovementDirection()
    {
        Vector3 returnValue;

        int i = randomizer.Next(36);
        float distanceFromCenter = randomizer.Next(5) + 5;

        float Xcoord = distanceFromCenter * Mathf.Cos(i * 10 * Mathf.Deg2Rad);
        float YCoord = distanceFromCenter * Mathf.Sin(i * 10 * Mathf.Deg2Rad);

        returnValue = new Vector3(Xcoord, YCoord, 0);

        return returnValue;
    }
    protected Vector3 CalculateDirection()
    {
        Vector3 returnValue;

        int i = randomizer.Next(36);
        float distanceFromCenter = 0.7f;

        float Xcoord = distanceFromCenter * Mathf.Cos(i * 10 * Mathf.Deg2Rad);
        float YCoord = distanceFromCenter * Mathf.Sin(i * 10 * Mathf.Deg2Rad);

        returnValue = new Vector3(Xcoord, YCoord, 0);

        return returnValue;
    }

    public class Factory : GameObjectFactory<AlienShip> { }

}
