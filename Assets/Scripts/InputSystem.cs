using UnityEngine;

public class InputSystem : MonoBehaviour
{
    public static InputSystem Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    public Vector3 GetMousePosition()
    {
        return Input.mousePosition;
    }

    public float GetZoomAmount()
    {
        float zoomAmount = 0f;

        if (Input.mouseScrollDelta.y > 0)
        {
            zoomAmount = +1f;
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            zoomAmount = -1f;
        }

        return zoomAmount;
    }

    public float GetCameraRotateAmount()
    {
        float rotateAmount = 0f;

        if (Input.GetKey(KeyCode.Q))
        {
            rotateAmount = -1f;
        }

        if (Input.GetKey(KeyCode.E))
        {
            rotateAmount = 1f;
        }

        return rotateAmount;
    }

    public Vector2 GetCameraMoveVector()
    {
        Vector2 inputMoveVector = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            inputMoveVector.y = 1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            inputMoveVector.y = -1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            inputMoveVector.x = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputMoveVector.x = 1f;
        }

        return inputMoveVector;
    }

    public bool GetMouseButtonDown()
    {
        return Input.GetMouseButtonDown(0);
    }
}
