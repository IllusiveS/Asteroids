using UnityEngine;
using System.Collections;
using FullInspector;


public interface IBulletView
{

}
public class Bullet : BaseBehavior, IBulletView
{
    public float startSpeed;

    protected Rigidbody2D rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(transform.right * startSpeed);
    }

    #region Controller
    public BulletController Controller;
    void OnEnable()
    {
        Controller.View = this;
    }
    #endregion

    void OnTriggerEnter2D(Collider2D other)
    {
        IAsteroid potentialAsteroid = other.gameObject.GetComponent<IAsteroid>();
        if (potentialAsteroid == null) return;

        potentialAsteroid.GetRekt();
        Destroy(gameObject);
    }
}

public interface IBulletController
{
    
}
public class BulletController
{
    #region View
    protected IBulletView view;
    public IBulletView View
    {
        set { view = value; }
    }
    #endregion
}
