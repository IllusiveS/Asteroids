using UnityEngine;
using System.Collections.Generic;
using FullInspector;
using Zenject;

public delegate void TimeTickHandler();
public interface IGameState
{
    event TimeTickHandler OnTimeTick;
    void Die();

    void AddAsteroid(IAsteroid asteroidToAdd);
    void RemoveAsteroid(IAsteroid asteroidToRemove);
}
public class GameState : BaseBehavior, IGameState {
    public int timeTicksToSpawn;
    public float tickCooldown;
    public float minTickCooldown;
    public float tickOffsetPerTick;

    protected float currentTime;
    protected float tickTime;

    [Inject]
    protected IPlayerController playerController;

    public event TimeTickHandler OnTimeTick;

    protected List<IAsteroid> asteroids = new List<IAsteroid>();

    public void AddAsteroid(IAsteroid asteroidToAdd)
    {
        asteroids.Add(asteroidToAdd);
    }
    public void RemoveAsteroid(IAsteroid asteroidToRemove)
    {
        asteroids.Remove(asteroidToRemove);
    }

    void Start()
    {
        currentTime = Time.time;
        tickTime = Time.time + tickCooldown;
    }
    void Update()
    {
        currentTime = Time.time;
        if(currentTime > tickTime)
        {
            if (OnTimeTick != null) OnTimeTick();
            tickTime = Time.time + tickCooldown;
            if (tickCooldown > minTickCooldown)
                tickCooldown -= tickOffsetPerTick;
        }
        if(asteroids.Count == 0)
        {

        }
    }
    void BeginNewStage()
    {

    }
    public void Die()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    [Inject]
    protected Asteroid.Factory asteroidFactory;


}
