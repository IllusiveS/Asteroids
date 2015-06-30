using UnityEngine;
using System.Collections;
using FullInspector;

public delegate void TimeTickHandler();
public interface IGameState
{
    event TimeTickHandler OnTimeTick;
}
public class GameState : BaseBehavior, IGameState {
    public int timeTicksToSpawn;
    public float tickCooldown;
    public float minTickCooldown;
    public float tickOffsetPerTick;

    protected float currentTime;
    protected float tickTime;

    public event TimeTickHandler OnTimeTick;

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
    }

}
