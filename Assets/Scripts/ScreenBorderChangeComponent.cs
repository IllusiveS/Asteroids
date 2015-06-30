using UnityEngine;
using System.Collections;
using FullInspector;
using Zenject;

public class ScreenBorderChangeComponent : BaseBehavior {


    public float topValue;
    public float bottomValue;

	void Update()
    {
        Vector3 position = Camera.main.WorldToViewportPoint(transform.position);

        if (position.x > topValue) transform.position = Camera.main.ViewportToWorldPoint(new Vector3(bottomValue, position.y, position.z));
        if (position.y > topValue) transform.position = Camera.main.ViewportToWorldPoint(new Vector3(position.x, bottomValue, position.z));
        if (position.x < bottomValue) transform.position = Camera.main.ViewportToWorldPoint(new Vector3(topValue, position.y, position.z));
        if (position.y < bottomValue) transform.position = Camera.main.ViewportToWorldPoint(new Vector3(position.x, topValue, position.z));
    }

}
