using System.Collections.Generic;
using UnityEngine;

public class SetSelectedButton : MonoBehaviour
{
    List<Upgrade> upgrades;
    private void OnEnable()
    {
        //GetComponentsInChildren(false, upgrades);
        //UIManager.Instance.eventSystem.SetSelectedGameObject(upgrades[0].gameObject);
    }
}
