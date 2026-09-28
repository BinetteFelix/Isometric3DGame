using UnityEngine;
using UnityEngine.UI;

public class UpgradeTime : MonoBehaviour
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

    // Update is called once per frame
    void Update()
    {
        timeSet -= Time.deltaTime;

        timeSlider.value = timeSet;
        Debug.Log(TimeSet);

        if (timeSlider.value == 0)
        {
            UpgradePanel.SetActive(false);
        }   
    }
    private void OnEnable()
    {
        timeSet = TimeToUpgrade;
    }
}
