using UnityEngine;
using System.Collections;
using FullInspector;
using Zenject;

public class ScreenBorderChangeComponent : BaseBehavior {

	void Update()
    {
        Vector3 position = Camera.main.WorldToViewportPoint(transform.position);

        if (position.x > 1) transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0, position.y, position.z));
        if (position.y > 1) transform.position = Camera.main.ViewportToWorldPoint(new Vector3(position.x, 0, position.z));
        if (position.x < 0) transform.position = Camera.main.ViewportToWorldPoint(new Vector3(1, position.y, position.z));
        if (position.y < 0) transform.position = Camera.main.ViewportToWorldPoint(new Vector3(position.x, 1, position.z));
    }

}
