using UnityEngine;

public class Unit : MonoBehaviour
{
    private Vector3 _targetPosition;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SetTargetPosition(MouseWorld.GetPosition());
        }

        float stopThresoldDistance = 0.1f;
        float moveDistance = Vector3.Distance(transform.position, _targetPosition);
        if (moveDistance < stopThresoldDistance) return;

        float speed = 4f;
        Vector3 moveDirection = (_targetPosition - transform.position).normalized;
        transform.position += speed * Time.deltaTime * moveDirection;
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }
}
