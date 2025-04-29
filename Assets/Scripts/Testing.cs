using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Unit _unit;

    private void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            List<GridPosition> validGridPosition = _unit.GetMoveAction().GetValidListGridPosition();
        }
    }
}
