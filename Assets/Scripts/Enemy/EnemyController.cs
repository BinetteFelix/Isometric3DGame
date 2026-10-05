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
    public float EnemiesToSpawn;
    #endregion
    [SerializeField] private Transform spawnArea;
    [SerializeField] private TextMeshProUGUI enemiesKilledText;
    [SerializeField] private GameObject worldSpaceCanvas;
    public bool HasSpawnedEnemies { get; private set; }
    public int EnemiesKilled;
    public int enemyKilledLoopNumber;

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
                Destroy(enemy);
            },
            false,
            10,
            100
            );
        #endregion

        EnemiesToSpawn = 20;
        SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEnemyCount();

        if (Enemies.Count == 0 && HasSpawnedEnemies && EnemiesKilled > 0)
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
        else
            removableEnemy = null;
    }

    #region OBJECTIVE UPDATE
    public void RespawnEnemies()
    {
        UpgradeManager.Instance.OpenUpgradeScreen();
        EnemiesToSpawn++;

        Enemies.Clear();
        EnemyPool.Dispose();
        EnemyHealthBar[] healthBars = worldSpaceCanvas.GetComponentsInChildren<EnemyHealthBar>(true);
        foreach (EnemyHealthBar healthBar in healthBars)
        {
            Destroy(healthBar.gameObject);
        }
        MarkerHandler.Instance.ResetList();

        SpawnEnemies();
    }
    #endregion

    private void SpawnEnemies()
    {
        for (int i = 0; i < EnemiesToSpawn; i++)
        {
            GameObject newEnemy = EnemyPool.Get();
            newEnemy.transform.position = spawnArea.position + new Vector3(Random.Range(-22.5f, 22.5f), 0, Random.Range(-22.5f, 22.5f));
            Enemies.Add(newEnemy);
            MarkerHandler.Instance.AddToList(newEnemy);
        }
        MarkerHandler.Instance.SetMarkerTarget();
        HasSpawnedEnemies = true;
    }
    public void UpdateEnemyCountUI()
    {
        enemyKilledLoopNumber++;
        enemiesKilledText.text = EnemiesKilled.ToString();
    }
    public override void Instantiate()
    {
    }
}