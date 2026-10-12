using UnityEngine;

public class GameSceneInitializer : MonoBehaviour
{
    private void Start()
    {
        EnemyController.Instance.InitializeStart();
        ExperienceHandler.Instance.InitializeStart();
    }
}
