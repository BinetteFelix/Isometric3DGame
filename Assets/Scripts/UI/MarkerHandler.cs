using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MarkerHandler : MonoBehaviour
{
    public static MarkerHandler Instance;
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private GameObject markerPrefab;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void AddToList(GameObject enemy)
    {
        enemies.Add(enemy);
    }
    public void RemoveFromList(GameObject enemy)
    {
        enemies.Remove(enemy);
    }
    public void SetMarkerTarget()
    {
        foreach (GameObject enemyO in enemies)
        {
            GameObject marker = Instantiate(markerPrefab, transform);
            marker.GetComponent<MarkerBehavior>().SetTarget(enemyO.transform);
        }
    }
}