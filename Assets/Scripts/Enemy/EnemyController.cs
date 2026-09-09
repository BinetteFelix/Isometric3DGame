using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public static EnemyController Instance;
    [SerializeField] private  Transform target;
    [SerializeField] public List<NavMeshAgent> Agents;

    [SerializeField] private TextMeshProUGUI objectiveText;
    private float updateDestinationTime;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetObjectiveProgress();
    }

    // Update is called once per frame
    void Update()
    {
        updateDestinationTime -= Time.deltaTime;

        if (updateDestinationTime < 0)
        {
            updateDestinationTime = 0.5f;
            foreach (var agent in Agents)
            {
                if (agent != null)
                    agent.SetDestination(target.position);
            }
        }
        UpdateEnemyCount();
    }
    public void UpdateEnemyCount()
    {
        foreach (NavMeshAgent agent in Agents)
        {
            if (agent == null)
            {
                Agents.Remove(agent);
            }
            SetObjectiveProgress();
        }
        
    }
    public void SetObjectiveProgress()
    {
        objectiveText.text = "Kill Enemies: " + Agents.Count;
    }
}