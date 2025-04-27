using Unity.Hierarchy;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    [SerializeField] private LayerMask _mousePlaneLayerMask;
    private static MouseWorld _instance;

    private void Awake()
    {
        _instance = this;
    }

    public static Vector3 GetPosition()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(mouseRay, out RaycastHit raycastHit, float.MaxValue, _instance._mousePlaneLayerMask);

        return raycastHit.point;

    }
}
