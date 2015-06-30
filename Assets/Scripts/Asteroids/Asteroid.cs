using UnityEngine;
using System.Collections;
using FullInspector;
public interface IAsteroid
{
    void GetRekt();
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
    #endregion
}

public interface IAsteroidController
{

}
public class AsteroidController : IAsteroidController
{
    public void ProcessRekt()
    {
        view.Explode();
    }

    #region view
    protected IAsteroidView view;
    public IAsteroidView View
    {
        set { view = value; }
    }
    #endregion
}
