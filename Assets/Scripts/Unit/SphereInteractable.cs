using System;
using UnityEngine;

public class SphereInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private MeshRenderer _visualRenderer;
    [SerializeField] private Material _greenMaterial;
    [SerializeField] private Material _redMaterial;

    private GridPosition _gridPosition;

    private Action _onActionComplete;
    private bool _isActive;
    private float _timer;


    private bool _isGreen;

    private void Update()
    {
        if (!_isActive) return;

        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            _isActive = false;
            _onActionComplete();
        }
    }

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableObjectAtGridPosition(_gridPosition, this);
        SetGreen();
    }


    private void SetGreen()
    {
        _isGreen = true;
        _visualRenderer.material = _greenMaterial;
    }

    private void SetRed()
    {
        _isGreen = false;
        _visualRenderer.material = _redMaterial;
    }

    public void Interact(Action onActionComplete)
    {
        _isActive = true;
        _timer = 0.5f;
        _onActionComplete = onActionComplete;

        if (_isGreen)
            SetRed();
        else
            SetGreen();
    }
}
