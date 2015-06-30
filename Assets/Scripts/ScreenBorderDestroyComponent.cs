using UnityEngine;
using System.Collections;
using FullInspector;

public class ScreenBorderDestroyComponent : BaseBehavior {

    public float topValue;
    public float bottomValue;

    void Update()
    {
        Vector3 position = Camera.main.WorldToViewportPoint(transform.position);

        bool shouldBeDestroyed = false;

        if (position.x > topValue) shouldBeDestroyed = true;
        if (position.y > topValue) shouldBeDestroyed = true;
        if (position.x < bottomValue) shouldBeDestroyed = true;
        if (position.y < bottomValue) shouldBeDestroyed = true;

        if (shouldBeDestroyed)
            Destroy(gameObject);
    }



}
