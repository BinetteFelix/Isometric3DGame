using Unity.Cinemachine;
using UnityEngine;

public class MainMenuCameraBehavior : MonoBehaviour
{
    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MoveCamera(int nextPosIndex, string triggerName)
    {
        animator.SetTrigger(triggerName);
        animator.SetInteger("CurrentIndex", nextPosIndex);
    }
}
