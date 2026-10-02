using UnityEngine;
using UnityEngine.EventSystems;

public class Upgrade : MonoBehaviour, IPointerClickHandler
{
    public UpgradeButton UpgradeData;
    [SerializeField] private GameObject Panel;
    public void OnPointerClick(PointerEventData eventData)
    {
        UpgradeManager.Instance.UpgradePlayer(UpgradeData.upgradeName, UpgradeData.value);
        Panel.SetActive(false);
    }
}
