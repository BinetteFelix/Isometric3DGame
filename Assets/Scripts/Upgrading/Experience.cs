using UnityEngine;

public class Experience : MonoBehaviour
{
    [SerializeField] private ExperienceData xpData;
    float timeSinceSpawn = 60f;
    private void Update()
    {
        timeSinceSpawn -= Time.deltaTime;

        if (timeSinceSpawn < 0)
        {
            ReleaseToPool();
        }
    }
    private void OnEnable()
    {
        timeSinceSpawn = 60f;
    }
    public void ReleaseToPool()
    {
        if (ExperienceHandler.HasInstance)
        {
            ExperienceHandler.Instance.GainXP(xpData.XPAmount);
            ExperienceHandler.Instance.xpPool.Release(this);
        }
    }
}
