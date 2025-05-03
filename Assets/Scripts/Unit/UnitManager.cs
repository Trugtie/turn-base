using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }

    [SerializeField] private List<Unit> _units;
    [SerializeField] private List<Unit> _friendlyUnits;
    [SerializeField] private List<Unit> _enemyUnits;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;

        _units = new List<Unit>();
        _friendlyUnits = new List<Unit>();
        _enemyUnits = new List<Unit>();
    }

    private void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    private void Unit_OnAnyUnitSpawned(object sender, System.EventArgs e)
    {
        Unit unit = sender as Unit;

        _units.Add(unit);

        if ((unit.IsEnemy()))
        {
            _enemyUnits.Add(unit);
        }
        else
        {
            _friendlyUnits.Add(unit);
        }
    }

    private void Unit_OnAnyUnitDead(object sender, System.EventArgs e)
    {
        Unit unit = sender as Unit;

        _units.Remove(unit);

        if ((unit.IsEnemy()))
        {
            _enemyUnits.Remove(unit);
        }
        else
        {
            _friendlyUnits.Remove(unit);
        }
    }

    public List<Unit> GetUnits() => _units;
    public List<Unit> GetFriendlyUnits() => _friendlyUnits;
    public List<Unit> GetEnemyUnits() => _enemyUnits;
}
