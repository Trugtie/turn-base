using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float MAX_ZOOM_OFFSET = 12f;
    private const float MIN_ZOOM_OFFSET = 2f;

    [SerializeField] private CinemachineFollow _cinemachineFollow;
    private Vector3 _targetFollowOffset;

    private void Awake()
    {
        if (_cinemachineFollow != null) return;
        _cinemachineFollow = GameObject.Find("CinemachineCamera").GetComponent<CinemachineFollow>();
        _targetFollowOffset = _cinemachineFollow.FollowOffset;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    private void HandleZoom()
    {
        float zoomSpeed = 5f;
        _targetFollowOffset.y += InputSystem.Instance.GetZoomAmount() * zoomSpeed;
        _targetFollowOffset.y = Mathf.Clamp(_targetFollowOffset.y, MIN_ZOOM_OFFSET, MAX_ZOOM_OFFSET);
        _cinemachineFollow.FollowOffset = Vector3.Lerp(_cinemachineFollow.FollowOffset, _targetFollowOffset, zoomSpeed * Time.deltaTime);
    }

    private void HandleRotation()
    {
        Vector3 rotateVector = new Vector3(0, 0, 0);

        rotateVector.y = InputSystem.Instance.GetCameraRotateAmount();

        float rotateSpeed = 100f;

        transform.eulerAngles += rotateVector * rotateSpeed * Time.deltaTime;
    }

    private void HandleMovement()
    {
        Vector2 cameraMoveVector = InputSystem.Instance.GetCameraMoveVector();

        Vector3 moveVector = transform.forward * cameraMoveVector.y + transform.right * cameraMoveVector.x;
        float moveSpeed = 10f;
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }
}
