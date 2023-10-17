using System;
using UnityEngine;

public class SelectableGameObject : MonoBehaviour
{
    public event Action<SelectableGameObject> Selected;

    public virtual void Select()
    {
        Selected?.Invoke(this);
    }
}