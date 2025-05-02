using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject _actionCamera;


    private void Start()
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;
        HideShootActionCamera();
    }

    private void BaseAction_OnAnyActionStarted(object sender, System.EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:

                Unit shooterUnit = shootAction.GetUnit();
                Unit targetUnit = shootAction.GetTargetUnit();

                float characterHeightOffset = 1.7f;

                Vector3 cameraCharacterHeight = Vector3.up * characterHeightOffset;
                Vector3 toTargetDirection = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;

                float shoudlerOffsetAmount = 0.5f;
                Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * toTargetDirection * shoudlerOffsetAmount;

                Vector3 actionCameraPosition = shooterUnit.GetWorldPosition() + cameraCharacterHeight + shoulderOffset + (toTargetDirection * -1);

                _actionCamera.transform.position = actionCameraPosition;
                _actionCamera.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);

                ShowShootActionCamera();

                break;
        }
    }

    private void BaseAction_OnAnyActionCompleted(object sender, System.EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                HideShootActionCamera();
                break;
        }
    }

    private void ShowShootActionCamera()
    {
        _actionCamera.SetActive(true);
    }

    private void HideShootActionCamera()
    {
        _actionCamera.SetActive(false);
    }
}
