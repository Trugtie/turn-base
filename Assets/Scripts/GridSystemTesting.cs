using UnityEngine;

public class GridSystemTesting : MonoBehaviour
{
    [SerializeField] private Transform _gridDebugObject;
    private GridSystem _gridSystem;
    private void Awake()
    {
        _gridSystem = new GridSystem(10, 10, 2);

        Debug.Log(new GridPosition(5, 7));

        _gridSystem.CreateGridDebugObjects(_gridDebugObject);
    }

    private void Update()
    {
        Debug.Log(_gridSystem.GetGridPosition(MouseWorld.GetPosition()));
    }
}
