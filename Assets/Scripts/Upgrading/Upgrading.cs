using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrading : MonoBehaviour
{
    [SerializeField] float TimeToUpgrade = 15;
    private float timeSet;
    public float TimeSet
    {
        get
        {
            return Mathf.Clamp(timeSet, 0, TimeToUpgrade);
        }
        set
        {
            return;
        }
    }

    [SerializeField] private Slider timeSlider;
    [SerializeField] private GameObject UpgradePanel;
    [SerializeField] private GameObject UpgradeGrid;

    List<Upgrade> upgradesInGrid = new List<Upgrade>();

    // Update is called once per frame
    void Update()
    {
        timeSet -= Time.deltaTime;

        timeSlider.value = timeSet;

        if (timeSlider.value == 0)
        {
            UpgradePanel.SetActive(false);
        }   
    }
    private void OnEnable()
    {
        timeSet = TimeToUpgrade;
        
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

            if (upgradesActiveOnScreen < 3)
            {
                upgradesInGrid[randomUpgradeIndex].gameObject.SetActive(true);
                upgradesInGrid.RemoveAt(randomUpgradeIndex);
                upgradesActiveOnScreen++;
            }
        }
    }
}
