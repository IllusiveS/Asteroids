using UnityEngine;
using System.Collections;
using FullInspector;
using Zenject;
public interface IAsteroid
{
    void GetRekt();
    void AddRektListener(RektHandler listener);
}
public interface IAsteroidView
{
    void Explode();
}
public class Asteroid : BaseBehavior, IAsteroid, IAsteroidView
{
    #region controller
    public AsteroidController Controller;
    void OnEnable()
    {
        Controller.View = this;
    }
    #endregion

    protected Rigidbody2D rigidbody;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(Controller.CalculateDirection() * (2.5f - transform.localScale.x));

        state.AddAsteroid(this);
    }
    void OnDestroy()
    {
        state.RemoveAsteroid(this);
    }
    #region IAsteroidView
    void IAsteroidView.Explode()
    {
        Destroy(gameObject);
    }
    #endregion

    #region IAsteroid
    public void GetRekt()
    {
        Controller.ProcessRekt();
    }
    public void AddRektListener(RektHandler listener)
    {
        Controller.OnRekt += listener;
    }
    #endregion

    public class Factory : GameObjectFactory<Asteroid> { }

    [Inject]
    protected IGameState state;
}

public delegate void RektHandler();
public interface IAsteroidController
{
    event RektHandler OnRekt;
}
public class AsteroidController : IAsteroidController
{
    public static System.Random randomizer = new System.Random();
    public Vector2 CalculateDirection()
    {
        Vector2 returnValue;

        int i = randomizer.Next(36);
        float distanceFromCenter = randomizer.Next(350) + 200;

        float Xcoord = distanceFromCenter * Mathf.Cos(i * 10 * Mathf.Deg2Rad);
        float YCoord = distanceFromCenter * Mathf.Sin(i * 10 * Mathf.Deg2Rad);

        returnValue = new Vector2(Xcoord, YCoord);

        return returnValue;
    }
    public void ProcessRekt()
    {
        view.Explode();
        if (OnRekt != null)
            OnRekt();
    }
    public event RektHandler OnRekt;

    #region view
    protected IAsteroidView view;
    public IAsteroidView View
    {
        set { view = value; }
    }
    #endregion
}
