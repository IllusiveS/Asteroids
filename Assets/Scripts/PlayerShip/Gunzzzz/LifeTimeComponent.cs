using UnityEngine;
using System.Collections;
using FullInspector;

public class LifeTimeComponent : BaseBehavior {

    public float lifeTime;
    protected float deathTime;
    
    void Start()
    {
        deathTime = Time.time + lifeTime;
    }

    void Update()
    {
        if (deathTime < Time.time) Destroy(gameObject);
    }

}
