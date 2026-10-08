using JetBrains.Annotations;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using Utility;

public class EnemyController : SingletonBehaviour<EnemyController>
{
    #region ENEMIES
    private GameObject removableEnemy;
    [SerializeField] public List<GameObject> Enemies;
    [SerializeField] public List<NavMeshAgent> EnemyTypes;
    public float EnemiesToSpawn;
    #endregion

    [SerializeField] public Transform spawnArea;
    public float SpawnRadius;
    [SerializeField] private TextMeshProUGUI enemiesKilledText;
    [SerializeField] private GameObject worldSpaceCanvas;
    public bool HasSpawnedEnemies { get; private set; }
    int terrorToSpawn = 3;
    int soulEaterToSpawn;
    public int EnemiesKilled;
    public int enemyKilledLoopNumber;

    public ObjectPool<GameObject> EnemyPool;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
            UpdateEnemyCount();

        if (Enemies.Count == 0 && HasSpawnedEnemies && (EnemiesKilled > 0 || UIManager.Instance.JustResetGame))
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
        }
        else
            removableEnemy = null;
    }

    #region OBJECTIVE UPDATE
    public void RespawnEnemies()
    {
        terrorToSpawn = 3;
        if (!UIManager.Instance.JustResetGame)
        {
            UpgradeManager.Instance.OpenUpgradeScreen();
            EnemiesToSpawn++;
        }
        
        Enemies.Clear();
        EnemyPool.Dispose();
        EnemyHealthBar[] healthBars = worldSpaceCanvas.GetComponentsInChildren<EnemyHealthBar>(true);
        foreach (EnemyHealthBar healthBar in healthBars)
        {
            Destroy(healthBar.gameObject);
        }
        SpawnEnemies();
        UIManager.Instance.JustResetGame = false;
    }
    #endregion

    private void SpawnEnemies()
    {
        for (int i = 0; i < EnemiesToSpawn; i++)
        {
            GameObject newEnemy = EnemyPool.Get();

            if (newEnemy.name == "Terrorbringer0(Clone)")
                terrorToSpawn--;

            Vector3 spawnCircle = Random.insideUnitCircle * SpawnRadius;
            newEnemy.transform.position = new Vector3(spawnCircle.x, 0, spawnCircle.y);
            Enemies.Add(newEnemy);
            //MarkerHandler.Instance.AddToList(newEnemy);
            
        }
        //MarkerHandler.Instance.SetMarkerTarget();
        HasSpawnedEnemies = true;
    }
    public void ClearAllEnemies()
    {
        EnemiesToSpawn = 20;
        Enemies.Clear();
        EnemiesKilled = 0;
        enemiesKilledText.text = EnemiesKilled.ToString();

        foreach (NavMeshAgent enemy in spawnArea.GetComponentsInChildren<NavMeshAgent>(true))
        {
            EnemyPool.Release(enemy.gameObject);
        }
    }
    public void UpdateEnemyCountUI()
    {
        enemyKilledLoopNumber++;
        enemiesKilledText.text = EnemiesKilled.ToString();
    }
    public void InitializeStart()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            spawnArea = GameObject.FindGameObjectWithTag("EnemySpawnArea").transform;

            #region Pooling
            EnemyPool = new ObjectPool<GameObject>(
                () =>
                {
                    if (terrorToSpawn > 0)
                        return Instantiate(EnemyTypes[0].gameObject, spawnArea);
                    else
                        return Instantiate(EnemyTypes[1].gameObject, spawnArea);

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

            EnemiesToSpawn = 50;
            SpawnEnemies();
        }
    }
    public override void Instantiate()
    {
    }
}