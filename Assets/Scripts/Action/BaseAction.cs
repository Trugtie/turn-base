using System;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Unit _unit;
    protected bool _isActive;

    public Action OnActionComplete;

    protected virtual void Awake()
    {
        _unit = GetComponent<Unit>();
    }
}
