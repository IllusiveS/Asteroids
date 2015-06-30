using UnityEngine;
using System.Collections;
using FullInspector;
using Zenject;

public interface IPlayerShipView
{
    void AddForce(Vector2 Direction);
    void Shoot();
}
public interface IPlayerShip
{
    event FireTheCannonsHandler OnFireTheCannons;
}
public class PlayerShip : BaseBehavior, IPlayerShipView, IPlayerShip {

    public PlayerShipController Controller;

    protected Rigidbody2D rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
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
    #region view
    protected IPlayerShipView view;
    public IPlayerShipView View
    {
        set { view = value; }
    }
    #endregion
}
