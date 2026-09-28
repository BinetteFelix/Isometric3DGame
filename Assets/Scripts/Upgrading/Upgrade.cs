using UnityEngine;
using UnityEngine.EventSystems;

public class Upgrade : MonoBehaviour, IPointerClickHandler
{
    public UpgradeButton UpgradeData;
    [SerializeField] private GameObject UpgradePanel;

    public void OnPointerClick(PointerEventData eventData)
    {
        UpgradeManager.Instance.UpgradePlayer(UpgradeData.upgradeName, UpgradeData.value);
        UpgradePanel.SetActive(false);
    }
}
