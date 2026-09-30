using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using Utility;

public class EnemyController : SingletonBehaviour<EnemyController>
{
    #region ENEMIES
    private GameObject removableEnemy;
    [SerializeField] public List<GameObject> Enemies;
    [SerializeField] public List<NavMeshAgent> EnemyTypes;
    #endregion
    [SerializeField] private Transform spawnArea;
    [SerializeField] private TextMeshProUGUI objectiveText;
    public bool HasSpawnedEnemies { get; private set; }
    public int EnemiesKilled;

    public ObjectPool<GameObject> EnemyPool;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        #region Pooling
        EnemyPool = new ObjectPool<GameObject>(
            () =>
            {
                int randomEnemyIndex = Random.Range(0, EnemyTypes.Count);
                return Instantiate(EnemyTypes[randomEnemyIndex].gameObject, spawnArea);
            },
            enemy =>
            {
                if (!enemy.activeSelf)
                    enemy.GetComponent<EnemyHealth>().ResetAttributes();

                enemy.gameObject.SetActive(true);
            },
            enemy =>
            {
                enemy.gameObject.SetActive(false);
            },
            enemy =>
            {
                DestroyImmediate(enemy);
            },
            false,
            10,
            100
            );
        #endregion

        SpawnEnemies();

        

    }

    // Update is called once per frame
    void Update()
    {
        UpdateEnemyCount();

        if (Enemies.Count == 0 && HasSpawnedEnemies)
        {
            RespawnEnemies();
        }
    }
    public void UpdateEnemyCount()
    {
        bool removeEnemy = false;
        foreach (GameObject agent in Enemies)
        {
            if (!agent.activeSelf)
            {
                removableEnemy = agent;
                removeEnemy = true;
            }
        }
        if (removeEnemy)
        {
            Enemies.Remove(removableEnemy);
            MarkerHandler.Instance.RemoveFromList(removableEnemy);
        }
    }

    #region OBJECTIVE UPDATE
    public void RespawnEnemies()
    {
        SpawnEnemies();
        UpgradeManager.Instance.OpenUpgradeScreen();
    }
    #endregion

    private void SpawnEnemies()
    {
        for (int i = 0; i < 20; i++)
        {
            GameObject newEnemy = EnemyPool.Get();
            newEnemy.transform.position = spawnArea.position + new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            Enemies.Add(newEnemy);
            MarkerHandler.Instance.AddToList(newEnemy);
        }
        MarkerHandler.Instance.SetMarkerTarget();
        HasSpawnedEnemies = true;
    }

    public override void Instantiate()
    {
    }
}