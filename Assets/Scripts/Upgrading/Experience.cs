using UnityEngine;

public class Experience : MonoBehaviour
{
    float timeSinceSpawn = 60f;
    private void Update()
    {
        timeSinceSpawn -= Time.deltaTime;

        if (timeSinceSpawn < 0)
        {
            ExperienceSpawner.Instance.xpPool.Release(this);
        }
    }
    private void OnEnable()
    {
        timeSinceSpawn = 60f;
    }
}
