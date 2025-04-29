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
        float zoomAmount = 1f;
        float zoomSpeed = 5f;

        if (Input.mouseScrollDelta.y > 0)
        {
            _targetFollowOffset.y += zoomAmount;
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            _targetFollowOffset.y -= zoomAmount;
        }

        _targetFollowOffset.y = Mathf.Clamp(_targetFollowOffset.y, MIN_ZOOM_OFFSET, MAX_ZOOM_OFFSET);
        _cinemachineFollow.FollowOffset = Vector3.Lerp(_cinemachineFollow.FollowOffset, _targetFollowOffset, zoomSpeed * Time.deltaTime);
    }

    private void HandleRotation()
    {
        Vector3 inputRotateVector = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.Q))
        {
            inputRotateVector.y = -1f;
        }

        if (Input.GetKey(KeyCode.E))
        {
            inputRotateVector.y = 1f;
        }

        float rotateSpeed = 100f;

        transform.eulerAngles += inputRotateVector * rotateSpeed * Time.deltaTime;
    }

    private void HandleMovement()
    {
        Vector3 inputMoveVector = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            inputMoveVector.z = 1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            inputMoveVector.z = -1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            inputMoveVector.x = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputMoveVector.x = 1f;
        }

        Vector3 moveVector = transform.forward * inputMoveVector.z + transform.right * inputMoveVector.x;
        float moveSpeed = 10f;
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }
}
