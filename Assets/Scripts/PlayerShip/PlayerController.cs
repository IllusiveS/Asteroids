using UnityEngine;
using System.Collections;
using FullInspector;

public delegate void DirectionHandler(Vector2 direction);

public interface IPlayerController
{
    event DirectionHandler OnDirectionChange;
}

public class PlayerController : BaseBehavior, IPlayerController
{

    void Update()
    {
        if (OnDirectionChange != null)
        {
            Vector2 directionVector = new Vector2(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
            OnDirectionChange(directionVector);
        }
        
    }

    #region IPlayerController
    public event DirectionHandler OnDirectionChange;
    #endregion

}
