using UnityEngine;
using System.Collections;
using FullInspector;
using Zenject;
using UnityEngine.Events;

[System.Serializable]
public class DieHandler : UnityEvent { }

public interface IPlayerShipView
{
    void AddForce(Vector2 Direction);
    void Shoot();
    void Die(IAsteroid asteroidHit);
}
public interface IPlayerShip
{
    event FireTheCannonsHandler OnFireTheCannons;
}
public class PlayerShip : BaseBehavior, IPlayerShipView, IPlayerShip {

    public PlayerShipController Controller;

    public DieHandler OnDie;

    protected Rigidbody2D rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    void IPlayerShipView.Die(IAsteroid asteroidHit)
    {
        asteroidHit.GetRekt();
        OnDie.Invoke();
        transform.localPosition = new Vector3(0, 0, 0);
    }
    #region IPlayerShipView
    void IPlayerShipView.AddForce(Vector2 Direction)
    {
        rigidbody.AddForce(transform.up * Direction.x);
        rigidbody.AddTorque(- Direction.y);
    }
    void IPlayerShipView.Shoot()
    {
        if (OnFireTheCannons != null) OnFireTheCannons();
    }
    #endregion
    #region IPlayerShip
    public event FireTheCannonsHandler OnFireTheCannons;
    #endregion
    void OnEnable()
    {
        Controller.View = this;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        IAsteroid potentialAsteroid = other.gameObject.GetComponent<IAsteroid>();
        if (potentialAsteroid == null) return;
        Controller.Die(potentialAsteroid);
    }

    [PostInject]
    void Setup(IPlayer playerController)
    {
        Controller.Controller = playerController;
    }
}
public class PlayerShipController
{

    protected IPlayer controller;
    public IPlayer Controller
    {
        set
        {
            controller = value;
            controller.AddDirectionObserver(CalculateMovement);
            controller.AddFireObserver(FireTheCannons);
        }
    }
    
    public void CalculateMovement(Vector2 direction)
    {
        view.AddForce(direction);
    }
    public void FireTheCannons()
    {
        view.Shoot();
    }
    public void Die(IAsteroid asteroidHit)
    {
        if (controller.isInvincible) return;
        view.Die(asteroidHit);
        controller.Die();
    }
    #region view
    protected IPlayerShipView view;
    public IPlayerShipView View
    {
        set { view = value; }
    }
    #endregion
}
