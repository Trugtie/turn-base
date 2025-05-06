using UnityEngine;

public class ScreenShakeAction : MonoBehaviour
{
    [SerializeField] private float _intensity = 1f;

    private void Start()
    {
        ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
    }

    private void ShootAction_OnAnyShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        ScreenShake.Instance.Shake(_intensity);
    }
}
