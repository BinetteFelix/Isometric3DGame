using UnityEngine;
using Utility;

public class MainMenuButtonEnabler : SingletonBehaviour<MainMenuButtonEnabler>
{
    bool areActive;
    [SerializeField] private GameObject[] Buttons;

    public override void Instantiate()
    {
    }

    public void SetButtonsActive()
    {
        areActive = !areActive;
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].SetActive(areActive);
        }
    }
}
