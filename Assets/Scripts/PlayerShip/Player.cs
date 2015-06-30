using UnityEngine;
using System.Collections;
using FullInspector;
using UnityEngine.Events;
using Zenject;

public delegate void DirectionHandler(Vector2 direction);
public delegate void FireTheCannonsHandler();


public interface IPlayer
{
    void GetHit();

    GameObject BulletPrefab { get; }

    void AddDirectionObserver(DirectionHandler observer);
    void AddFireObserver(FireTheCannonsHandler observer);

    void Die();

    bool isInvincible
    {
        get;
        set;
    }
}
public interface IPlayerView
{
    void Fire();
}
public class Player : BaseBehavior, IPlayer, IPlayerView
{
    public PlayerController Controller;

    [SerializeField]
    protected float ShotCooldown;
    protected float shotTime;
    [SerializeField]
    protected GameObject bulletPrefab;
    [SerializeField]
    protected bool isInvincibleValue;

    public bool isInvincible 
    { 
        get { return isInvincibleValue; }
        set { isInvincibleValue = value; }
    }
    
    void Start()
    {
        shotTime = Time.time;
    }
    void Update()
    {
        Vector2 directionVector = new Vector2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
        Controller.ProcessKeyboardInput(directionVector);

        if (Input.GetAxisRaw("Fire1") > 0)
        {
            Controller.Fire();
        }
        if(shotTime < Time.time)
        {
            Controller.Reload();
        }
    }
    public void Die()
    {
        Controller.Die();
    }
    void IPlayerView.Fire()
    {
        shotTime = Time.time + ShotCooldown;
    }
    public void AddDirectionObserver(DirectionHandler observer)
    {
        Controller.OnDirectionChange += observer;
    }
    public void AddFireObserver(FireTheCannonsHandler observer)
    {
        Controller.OnFireTheCannons += observer;
    }

    void OnEnable()
    {
        Controller.View = this;
    }
    #region IPlayerController

    public void GetHit()
    {

    }
    public GameObject BulletPrefab
    {
        get { return bulletPrefab; }
    }
    
    #endregion
    [PostInject]
    void Setup(IGameState state)
    {
        Controller.GameState = state;
    }
}
public interface IPlayerController
{
    void Reload();
    void Fire();
    void ProcessKeyboardInput(Vector2 input);

    void AddPoints(int points);
    int Points { get; }

    void Die();
    int Lifes { get; }
}
public class PlayerController : IPlayerController
{
    [SerializeField]
    protected float MovementSpeed;
    [SerializeField]
    protected float RotationSpeed;
    [SerializeField]
    protected bool CanFire = true;

    protected IGameState state;
    public IGameState GameState
    {
        set { state = value; }
    }

    protected int points;
    [SerializeField]
    protected int lifes;

    public void Die()
    {
        lifes--;
        if (lifes < 1)
            state.Die();
        //view.Die();
    }
    public int Lifes
    {
        get { return lifes; }
    }

    public int Points
    {
        get { return points; }
    }

    public void Reload()
    {
        CanFire = true;
    }
    public void AddPoints(int points)
    {
        this.points += points;
    }
    public void Fire()
    {
        if(OnFireTheCannons != null && CanFire)
        {
            OnFireTheCannons();
            view.Fire();
        }
        CanFire = false;
    }
    public void ProcessKeyboardInput(Vector2 directionVector)
    {
        directionVector.x = directionVector.x * MovementSpeed;
        directionVector.y = directionVector.y * RotationSpeed;
        if(OnDirectionChange != null)
        {
            OnDirectionChange(directionVector);
        }
    }

    public event DirectionHandler OnDirectionChange;
    public event FireTheCannonsHandler OnFireTheCannons;

    #region IPlayerView
    protected IPlayerView view;
    public IPlayerView View
    {
        set { view = value; }
    }
    #endregion
}