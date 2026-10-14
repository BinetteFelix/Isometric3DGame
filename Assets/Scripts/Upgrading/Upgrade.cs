using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Upgrade : MonoBehaviour, IPointerClickHandler
{
    public InputAction upgradeAction;
    public UpgradeData UpgradeData;
    public GameObject Panel;
    public void OnPointerClick(PointerEventData eventData)
    {
        UpgradeManager.Instance.UpgradePlayer(UpgradeData.upgradeName, UpgradeData.value);
        Panel.SetActive(false);
    }
    private void Update()
    {
        if (upgradeAction.WasPressedThisFrame())
        {
            UpgradeManager.Instance.UpgradePlayer(UpgradeData.upgradeName, UpgradeData.value);
            Panel.SetActive(false);
            Debug.Log(UpgradeData.upgradeName);
        }
    }
}
