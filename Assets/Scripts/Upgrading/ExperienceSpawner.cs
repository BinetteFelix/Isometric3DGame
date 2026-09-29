using System.Collections.Generic;
using UnityEngine;
using Utility;
using UnityEngine.Pool;

public class ExperienceSpawner : SingletonBehaviour<ExperienceSpawner>
{
    public List<Experience> ExperienceTypes = new List<Experience>();
    public ObjectPool<Experience> xpPool;
    private void Start()
    {
        xpPool = new ObjectPool<Experience>(
            () =>
            {
                int randomXPType = Random.Range(0, ExperienceTypes.Count);
                return Instantiate(ExperienceTypes[randomXPType], transform);

            },
            xp =>
            {
                xp.gameObject.SetActive(true);
            },
            xp =>
            {
                xp.gameObject.SetActive(false);
            },
            xp =>
            {
                Destroy(xp.gameObject);
            },
            false,
            5,
            100
            );
    }
    public void SpawnXP(Vector3 enemyOrigin)
    {
        Experience xp = xpPool.Get();
        xp.transform.position = new Vector3(enemyOrigin.x, 1.5f, enemyOrigin.z);
    }
    public override void Instantiate()
    {
    }
}
