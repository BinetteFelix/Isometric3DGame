using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour, IPointerClickHandler
{
    public UpgradeData UpgradeData;
    public GameObject Panel;
    public void OnPointerClick(PointerEventData eventData)
    {
        UpgradeManager.Instance.UpgradePlayer(UpgradeData.upgradeName, UpgradeData.value);
        Panel.SetActive(false);
    }
}
