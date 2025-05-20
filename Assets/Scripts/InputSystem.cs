#define USE_NEW_INPUT_SYSTEM
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : MonoBehaviour
{
    public static InputSystem Instance;

    private InputSystem_Actions _inputActions;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
        _inputActions = new InputSystem_Actions();
        _inputActions.Player.Enable();
    }

    public Vector3 GetMousePosition()
    {
#if USE_NEW_INPUT_SYSTEM
        return Mouse.current.position.ReadValue();
#else
        return Input.mousePosition;
#endif
    }

    public float GetZoomAmount()
    {
#if USE_NEW_INPUT_SYSTEM
        return _inputActions.Player.CameraZoom.ReadValue<float>();
#else
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
#endif
    }

    public float GetCameraRotateAmount()
    {
#if USE_NEW_INPUT_SYSTEM
        return _inputActions.Player.CameraRotate.ReadValue<float>();
#else
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
#endif
    }

    public Vector2 GetCameraMoveVector()
    {
#if USE_NEW_INPUT_SYSTEM
        return _inputActions.Player.CameraMove.ReadValue<Vector2>();
#else
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
#endif
    }

    public bool GetMouseButtonDown()
    {
#if USE_NEW_INPUT_SYSTEM
        return _inputActions.Player.Click.WasPressedThisFrame();
#else
        return Input.GetMouseButtonDown(0);
#endif
    }
}
