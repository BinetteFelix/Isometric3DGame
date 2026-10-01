using UnityEngine;
using UnityEngine.EventSystems;

public class Upgrade : MonoBehaviour, IPointerClickHandler
{
    public UpgradeButton UpgradeData;
    [SerializeField] private GameObject UpgradePanel;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (UpgradeData.upgradeName == "Poison Tears")
        {
            EyeBall eyeBall = WeaponCollection.Instance.Weapons[1].GetComponent<EyeBall>();
            eyeBall.SwitchTearState(EyeBall.TearState.poisonous);
        }
            
        UpgradeManager.Instance.UpgradePlayer(UpgradeData.upgradeName, UpgradeData.value);
        UpgradePanel.SetActive(false);
    }
}
