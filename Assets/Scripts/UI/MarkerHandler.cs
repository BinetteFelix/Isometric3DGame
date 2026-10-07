using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class MarkerHandler : MonoBehaviour
{
    public static MarkerHandler Instance;
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private GameObject markerPrefab;

    public ObjectPool<GameObject> markerPool;
    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        #region Pooling
        markerPool = new ObjectPool<GameObject>(
            () =>
            {
                return Instantiate(markerPrefab, transform);
            },
            marker =>
            {
                marker.gameObject.SetActive(true);
            },
            marker =>
            {
                marker.gameObject.SetActive(false);
            },
            marker =>
            {
                DestroyImmediate(marker);
            },
            false,
            10,
            100
            );
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void AddToList(GameObject enemy)
    {
        enemies.Add(enemy);
    }
    public void ResetList()
    {
        enemies.Clear();
        MarkerBehavior[] markers = GetComponentsInChildren<MarkerBehavior>();
        for (int i = 0; i < markers.Length; i++)
        {
            markerPool.Release(markers[i].gameObject);
        }
        markerPool.Dispose();
    }
    public void SetMarkerTarget()
    {
        foreach (GameObject enemyO in enemies)
        {
            GameObject marker = markerPool.Get();
            marker.GetComponent<MarkerBehavior>().SetTarget(enemyO.transform);
        }
    }
}