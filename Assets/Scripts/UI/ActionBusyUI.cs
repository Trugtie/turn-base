using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
    private void Start()
    {
        UnitActionSystem.Instance.OnBusyChange += UnitActionSystem_OnBusyChange;
        Hide();
    }

    private void UnitActionSystem_OnBusyChange(object sender, System.EventArgs e)
    {
        bool isBusy = UnitActionSystem.Instance.IsBusy();

        if (isBusy)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
