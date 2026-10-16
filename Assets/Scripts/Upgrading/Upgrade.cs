using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public UpgradeData UpgradeData;
    public GameObject Panel;
    public void SendUpgradeInfo()
    {
        UpgradeManager.Instance.UpgradePlayer(UpgradeData.upgradeName, UpgradeData.value, Panel);
    }
}
