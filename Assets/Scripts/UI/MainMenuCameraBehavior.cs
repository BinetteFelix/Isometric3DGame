using UnityEngine;
using Utility;

public class MainMenuCameraBehavior : SingletonBehaviour<MainMenuCameraBehavior>
{
    private PanelMovingMainMenu[] panelButtons;

    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        panelButtons = MainMenuButtonEnabler.Instance.GetComponentsInChildren<PanelMovingMainMenu>(true);
    }
    public void MoveCamera(int nextPosIndex, string triggerName)
    {
        animator.SetTrigger(triggerName);
        animator.SetInteger("CurrentIndex", nextPosIndex);
    }
    public override void Instantiate()
    {
    }
}
