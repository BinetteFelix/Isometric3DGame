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
        Upgrade[] upgradesInGrid = UpgradeGrid.GetComponentsInChildren<Upgrade>(true);
        foreach (Upgrade upgrade in upgradesInGrid)
        {
            int randomUpgradeIndex = Random.Range(0, upgradesInGrid.Length);
            
            if (upgradesActiveOnScreen < 4)
            {
                upgradesActiveOnScreen++;
                upgradesInGrid[randomUpgradeIndex].gameObject.SetActive(true);
            }
        }
        
    }
}
