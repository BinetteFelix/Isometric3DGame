using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class EnemySpawner : EditorWindow
{
    string enemyBaseName = "";
    int enemyID = 1;
    float enemyScale;
    float spawnRadius = 5f;
    float baseHealth = 50;
    Vector3 healthBarOffset;
    LayerMask playerLayer;

    float viewRange = 5;
    float attackSpeed = 1;
    float attackRange = 2.5f;
    
    GameObject enemyToSpawn;
    GameObject healthBarPrefab;
    Transform parentObject;

    [MenuItem("Tools/EnemySpawner")]
    public static void ShowWindow()
    {
        GetWindow(typeof(EnemySpawner));
    }

    private void OnGUI()
    {
        
        GUILayout.Label("Spawn New Enemy", EditorStyles.whiteLargeLabel);
        GUILayout.Space(10);
        GUILayout.Label("Spawning Variables", EditorStyles.whiteLabel);

        enemyBaseName = EditorGUILayout.TextField("Base Name", enemyBaseName);
        enemyID = EditorGUILayout.IntField("Enemy ID", enemyID);
        enemyScale = EditorGUILayout.Slider("Enemy Scale", enemyScale, 0.5f, 3f);
        spawnRadius = EditorGUILayout.FloatField("Spawn Radius", spawnRadius);

        GUILayout.Space(10);
        GUILayout.Label("Health", EditorStyles.whiteLabel);

        baseHealth = EditorGUILayout.FloatField("Max Health", baseHealth);
        healthBarOffset = EditorGUILayout.Vector3Field("Healthbar Offset", healthBarOffset);
        playerLayer = EditorGUILayout.LayerField("Player Layer", playerLayer);

        GUILayout.Space(10);
        GUILayout.Label("Attacking", EditorStyles.whiteLabel);

        viewRange = EditorGUILayout.FloatField("View Distance", viewRange);
        attackSpeed = EditorGUILayout.FloatField("Attack Speed", attackSpeed);
        attackRange = EditorGUILayout.FloatField("Attack Radius", attackRange);

        GUILayout.Space(15);
        GUILayout.Label("Attributes", EditorStyles.whiteLabel);

        enemyToSpawn = EditorGUILayout.ObjectField("Prefab To Spawn", enemyToSpawn, typeof(GameObject), false) as GameObject;
        parentObject = EditorGUILayout.ObjectField("Parent", parentObject, typeof(Transform), true) as Transform;
        healthBarPrefab = EditorGUILayout.ObjectField("Health Bar", healthBarPrefab, typeof(GameObject), false) as GameObject;

        if (GUILayout.Button("Spawn Object"))
        {
            SpawnEnemy();
        }
    }
    private void SpawnEnemy()
    {
        if (enemyToSpawn == null)
        {
            Debug.LogError("Error: Please assign an enemy to be spawned.");
            return;
        }
        if (enemyBaseName == string.Empty)
        {
            Debug.LogError("Error: Please enter a base name for the object");
            return;
        }

        Vector3 spawnCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = new Vector3(spawnCircle.x, 0f, spawnCircle.y);

        GameObject newEnemy = Instantiate(enemyToSpawn, parentObject);

        newEnemy.transform.position = spawnPos;
        newEnemy.name = enemyBaseName + enemyID;
        newEnemy.transform.localScale = Vector3.one * enemyScale;

        BoxCollider collider = newEnemy.AddComponent<BoxCollider>();
        collider.center = new Vector3(0, 1, 0);
        collider.size = new Vector3(1.5f, 2, 1.25f);

        Renderer renderer = newEnemy.GetComponentInChildren<Renderer>();

        EnemyHealth health = newEnemy.AddComponent<EnemyHealth>();
        health.rend = renderer;
        health.BaseHealth = baseHealth;
        health.HealthbarPrefab = healthBarPrefab;
        health.healthbarPos = healthBarOffset;

        EnemyMovement enemyMovement = newEnemy.AddComponent<EnemyMovement>();
        enemyMovement.ViewRadius = viewRange;
        enemyMovement.AttackSpeed = attackSpeed;
        enemyMovement.attackRange = attackRange;

        string enemyName = enemyBaseName + enemyID;
        string prefabFolder = "Assets/Prefabs/Enemies/";
        string enemyPrefabPath = prefabFolder + enemyName + ".prefab";

        PrefabUtility.SaveAsPrefabAsset(newEnemy, enemyPrefabPath);

        enemyID++;
    }
}
