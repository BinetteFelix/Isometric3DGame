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
    [SerializeField] public List<NavMeshAgent> levelOneEnemies;

    [SerializeField] private NavMeshAgent levelTwoEnemyPrefab;
    [SerializeField] public List<NavMeshAgent> levelTwoEnemies;
    #endregion

    [SerializeField] private List<Transform> SpawnAreas;
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
        updateDestinationTime -= Time.deltaTime;

        UpdateEnemyCount(CurrentWave);
        
        if (updateDestinationTime < 0)
        {
            updateDestinationTime = 0.5f;
            foreach (var agent in levelOneEnemies)
            {
                if (agent != null)
                    agent.SetDestination(target.position);
            }
            foreach (var agent in levelTwoEnemies)
            {
                if (agent != null)
                    agent.SetDestination(target.position);
            }
        }
    }
    public void UpdateEnemyCount(int wave)
    {
        switch (wave)
        {
            case 0:
                {
                    bool removeEnemy = false;
                    foreach (NavMeshAgent agent in levelOneEnemies)
                    {
                        if (agent == null)
                        {
                            removableEnemy = agent;
                            removeEnemy = true;
                        }
                    }
                    if (removeEnemy)
                    {
                        levelOneEnemies.Remove(removableEnemy);
                        MarkerHandler.Instance.RemoveFromList(removableEnemy);
                    }
                    SetObjectiveProgress(CurrentWave);
                    break;
                }
            case 1:
                {
                    bool removeEnemy = false;
                    foreach (NavMeshAgent agent in levelTwoEnemies)
                    {
                        if (agent == null)
                        {
                            removableEnemy = agent;
                            removeEnemy = true;
                        }
                    }
                    if (removeEnemy)
                    {
                        levelTwoEnemies.Remove(removableEnemy);
                        MarkerHandler.Instance.RemoveFromList(removableEnemy);
                    }
                    SetObjectiveProgress(CurrentWave);
                    break;
                }
            default:
                {
                    break;
                }       
        }
    }
    public void SetObjectiveProgress(int wave)
    {
        switch (wave)
        {
            case 0:
                {
                    objectiveText.text = "Kill Enemies: " + levelOneEnemies.Count;
                    break;
                }
            case 1:
                {
                    objectiveText.text = "Kill Enemies: " + levelTwoEnemies.Count;
                    break;
                }
            default:
                break;
        }

        if (levelOneEnemies.Count == 0 && CurrentWave == 0)
        {
            CurrentWave = 1;
            SpawnEnemies(CurrentWave);
        }
    }
    private void SpawnEnemies(int wave)
    {
        switch (wave)
        {
            case 0:
                {
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            NavMeshAgent newEnemy = Instantiate(levelOneEnemyPrefab, SpawnAreas[i]);
                            newEnemy.transform.position = SpawnAreas[i].position + new Vector3(Random.Range(0, 5), 0, Random.Range(0, 5));
                            levelOneEnemies.Add(newEnemy);
                            MarkerHandler.Instance.AddToList(newEnemy);
                            SetObjectiveProgress(CurrentWave);
                        }
                    }
                    
                    break;
                }
            case 1:
                {
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            NavMeshAgent newEnemy = Instantiate(levelTwoEnemyPrefab, SpawnAreas[i]);
                            newEnemy.transform.position = SpawnAreas[i].position + new Vector3(Random.Range(0, 5), 0, Random.Range(0, 5));
                            levelTwoEnemies.Add(newEnemy);
                            MarkerHandler.Instance.AddToList(newEnemy);
                            SetObjectiveProgress(CurrentWave);
                        }
                    }
                    break;
                }
            default:
                break;
        }
        MarkerHandler.Instance.SetMarkerTarget();
    }
}