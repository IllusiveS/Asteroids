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
    public GameObject spaceDogePrefab;

    public int startSmallAsteroidAmmount;

    public int smallAsteroidIncrease;

    public int timeTicksToSpawn;
    protected int maxTimeTicksToSpawn;
    public float tickCooldown;
    protected float maxTickCooldown;
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
        maxTickCooldown = tickCooldown;
        currentTime = Time.time;
        tickTime = Time.time + tickCooldown;
        maxTimeTicksToSpawn = timeTicksToSpawn;
    }
    void Update()
    {
        currentTime = Time.time;
        if(currentTime > tickTime)
        {
            if (OnTimeTick != null) OnTimeTick();
            tickTime = Time.time + tickCooldown;
            if (tickCooldown > minTickCooldown)
            {
                tickCooldown -= tickOffsetPerTick;
                timeTicksToSpawn -= 1;
            }
                
        }
        if(timeTicksToSpawn < 0)
        {
            timeTicksToSpawn = maxTimeTicksToSpawn;
            spawnSpaceDoge();
        }

        if(asteroids.Count == 0)
        {
            BeginNewStage();
        }
        if (Time.time > invincibilityTime)
            player.isInvincible = false;
    }
    void BeginNewStage()
    {
        player.isInvincible = true;
        invincibilityTime = Time.time + 4.0f;
        SpawnNewAsteroids();
        tickCooldown = maxTickCooldown;
        startSmallAsteroidAmmount += smallAsteroidIncrease;
        timeTicksToSpawn = maxTimeTicksToSpawn;
    }
    protected float invincibilityTime;
    public void Die()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
    protected void spawnSpaceDoge()
    {
        AlienShip spaceDoge = alienShipFactory.Create();

        Vector3 pos = new Vector3(Random.Range(0.0f, 0.2f), Random.Range(0.0f, 1.0f), 10);
        Vector3 newPos = Camera.main.ViewportToWorldPoint(pos);

        spaceDoge.transform.position = newPos;
    }
    void SpawnNewAsteroids()
    {
        for(int i = 0; i < startSmallAsteroidAmmount; i++)
        {
            SpawnAsteroid();
        }
    }
    void SpawnAsteroid()
    {
        Asteroid asteroid = asteroidFactory.Create();

        Vector3 pos = new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 10);
        Vector3 newPos = Camera.main.ViewportToWorldPoint(pos);

        asteroid.transform.position = newPos;
    }

    [Inject]
    protected Asteroid.Factory asteroidFactory;
    [Inject]
    protected AlienShip.Factory alienShipFactory;
    [Inject]
    protected IPlayer player;

}
