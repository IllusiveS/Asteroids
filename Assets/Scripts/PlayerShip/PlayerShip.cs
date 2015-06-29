using UnityEngine;
using System.Collections;
using FullInspector;
using Zenject;

public interface IPlayerShipView
{
    void AddForce(Vector2 Direction);
    void Shoot();
}
public class PlayerShip : BaseBehavior, IPlayerShipView {

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

    }
    #endregion

    void OnEnable()
    {
        Controller.View = this;
    }

    [PostInject]
    void Setup(IPlayerController playerController)
    {
        Controller.Controller = playerController;
    }
}
public class PlayerShipController
{

    protected IPlayerController controller;
    public IPlayerController Controller
    {
        set
        {
            controller = value;
            controller.OnDirectionChange += CalculateMovement;
        }
    }

    [SerializeField]
    protected float Speed;
    [SerializeField]
    protected float RotationSpeed;
    
    public void CalculateMovement(Vector2 direction)
    {
        direction.x = direction.x * Speed;
        direction.y = direction.y * RotationSpeed; 
        view.AddForce(direction);
    }

    #region view
    protected IPlayerShipView view;
    public IPlayerShipView View
    {
        set { view = value; }
    }
    #endregion
}
