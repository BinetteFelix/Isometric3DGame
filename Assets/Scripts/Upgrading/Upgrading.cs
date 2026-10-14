using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrading : MonoBehaviour
{
    [SerializeField] private Slider timeSlider;
    [SerializeField] private GameObject UpgradePanel;
    [SerializeField] private GameObject UpgradeGrid;
    public int upgradesToActivate;
    List<Upgrade> upgradesInGrid = new List<Upgrade>();

    // Update is called once per frame
    void Update()
    {
        if (timeSlider != null)
        {
            if (timeSlider.value == 0)
            {
                UpgradePanel.SetActive(false);
            }
        }
    }
    private void OnEnable()
    {   
        int upgradesActiveOnScreen = 0;
        upgradesInGrid.Clear();
        UpgradeGrid.GetComponentsInChildren(true, upgradesInGrid);
        foreach(Upgrade upgrade in upgradesInGrid)
        {
            upgrade.gameObject.SetActive(false);
        }
        for (int i = 0; i < upgradesInGrid.Count; i++)
        {
            Upgrade upgrade = upgradesInGrid[i];
            int randomUpgradeIndex = Random.Range(0, upgradesInGrid.Count);

            if (upgradesActiveOnScreen < upgradesToActivate)
            {
                upgradesInGrid[randomUpgradeIndex].gameObject.SetActive(true);
                upgradesInGrid.RemoveAt(randomUpgradeIndex);
                upgradesActiveOnScreen++;
            }
        }
        upgradesInGrid.Clear();
        UpgradeGrid.GetComponentsInChildren(false, upgradesInGrid);
        foreach (Upgrade upgrade in upgradesInGrid)
        {
            UIManager.Instance.eventSystem.SetSelectedGameObject(upgrade.gameObject);
        }
    }
}
