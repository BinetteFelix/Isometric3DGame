using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectUpgrade : MonoBehaviour
{
    [SerializeField] private GameObject UpgradeGrid;
    public List<Upgrade> upgrades;
    public List<InputAction> upgradeActions;
    private void Awake()
    {
        upgradeActions.Add(InputManager.Instance.actions.UI.PressWest);
        upgradeActions.Add(InputManager.Instance.actions.UI.PressSouth);
        upgradeActions.Add(InputManager.Instance.actions.UI.PressEast);
        UpgradeGrid.GetComponentsInChildren(true, upgrades);
    }
    private void OnEnable()
    {
        UpgradeGrid.GetComponentsInChildren(upgrades);

        for (int i = 0; i < upgrades.Count; i++)
        {
            if (upgrades[i].gameObject.activeSelf)
            {
                upgrades[i].upgradeAction = upgradeActions[i];
                switch (i)
                {
                    case 0:
                        {
                            if (upgrades[i].upgradeAction.bindings.Count > 0)
                                upgrades[i].upgradeAction.ApplyBindingOverride(0,"<Gamepad>/buttonWest");
                            else
                                upgrades[i].upgradeAction.AddBinding("<Gamepad>/buttonWest");
                            Debug.Log("Added west");
                            break;
                        }
                    case 1:
                        {
                            if (upgrades[i].upgradeAction.bindings.Count > 0)
                                upgrades[i].upgradeAction.ApplyBindingOverride(0, "<Gamepad>/buttonSouth");
                            else
                                upgrades[i].upgradeAction.AddBinding("<Gamepad>/buttonSouth");
                            Debug.Log("Added south");
                            break;
                        }
                    case 2:
                        {
                            if (upgrades[i].upgradeAction.bindings.Count > 0)
                                upgrades[i].upgradeAction.ApplyBindingOverride(0, "<Gamepad>/buttonEast");
                            else
                                upgrades[i].upgradeAction.AddBinding("<Gamepad>/buttonEast");
                            Debug.Log("Added east");
                            break;
                        }
                }
                
            }
            
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
