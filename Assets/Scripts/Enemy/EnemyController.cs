using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public static EnemyController Instance;
    [SerializeField] private  Transform target;

    #region ENEMIES
    private NavMeshAgent removableEnemy;
    [SerializeField] private NavMeshAgent levelOneEnemyPrefab;
    [SerializeField] private NavMeshAgent levelTwoEnemyPrefab;
    [SerializeField] public List<NavMeshAgent> Enemies;
    #endregion
    [SerializeField] private List<Transform> spawnAreas;
    [SerializeField] private TextMeshProUGUI objectiveText;
    private float updateDestinationTime;
    public int CurrentWave { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnEnemies(CurrentWave);
        SetObjectiveProgress(CurrentWave);
    }

    // Update is called once per frame
    void Update()
    {
        #region TIMERS
        updateDestinationTime -= Time.deltaTime;
        #endregion

        UpdateEnemyCount(CurrentWave);

        #region DESTINATION HANDLER
        if (updateDestinationTime < 0)
            SetDestination();
        #endregion
    }
    public void UpdateEnemyCount(int wave)
    {
        bool removeEnemy = false;
        foreach (NavMeshAgent agent in Enemies)
        {
            if (agent == null)
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
        SetObjectiveProgress(CurrentWave);
    }

    #region OBJECTIVE UPDATE
    public void SetObjectiveProgress(int wave)
    {
        objectiveText.text = "Kill Enemies: " + Enemies.Count;

        if (Enemies.Count == 0 && CurrentWave == 0)
        {
            CurrentWave = 1;
            SpawnEnemies(CurrentWave);
        }
    }
    #endregion

    private void SpawnEnemies(int wave)
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                int randomEnemy = Random.Range(0, 2);
                NavMeshAgent enemyToSpawn = randomEnemy switch
                {
                    (0) => levelOneEnemyPrefab,
                    (1) => levelTwoEnemyPrefab,
                    _ => levelOneEnemyPrefab,
                };

                NavMeshAgent newEnemy = Instantiate(enemyToSpawn, spawnAreas[i]);
                newEnemy.transform.position = spawnAreas[i].position + new Vector3(Random.Range(0, 5), 0, Random.Range(0, 5));
                Enemies.Add(newEnemy);
                MarkerHandler.Instance.AddToList(newEnemy);
                SetObjectiveProgress(CurrentWave);
            }
        }
        MarkerHandler.Instance.SetMarkerTarget();
    }

    #region DESTINATON METHODS
    private void SetDestination()
    {
        updateDestinationTime = 0.5f;
        foreach (var agent in Enemies)
        {
            if (agent != null)
                agent.SetDestination(target.position);
        }
    }
    #endregion
}