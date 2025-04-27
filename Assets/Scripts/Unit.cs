using UnityEngine;

public class Unit : MonoBehaviour
{
    private Vector3 _targetPosition;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SetTargetPosition(new Vector3(4, 0, 4));
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
