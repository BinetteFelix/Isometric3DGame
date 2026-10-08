using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCreator : EditorWindow
{
    string enemyBaseName = "";
    int enemyID = 1;
    float enemyScale;
    float spawnRadius = 5f;
    float baseHealth = 50;
    Vector3 healthBarOffset;
    Vector3 colliderOffset;
    Vector3 colliderSize;

    float maxSpeed = 2;
    float stoppingDistance;
    float acceleration;
    LayerMask playerLayer;

    float viewRange = 5;
    float attackSpeed = 1;
    float attackRange = 2.5f;
    float baseDamage = 10;
    
    GameObject enemyToSpawn;
    GameObject healthBarPrefab;
    Transform parentObject;

    [MenuItem("Tools/EnemyCreator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(EnemyCreator));
    }

    private void OnGUI()
    {
        GUILayout.Label("Create New Enemy", EditorStyles.whiteLargeLabel);
        GUILayout.Space(10);
        GUILayout.Label("Spawning Variables", EditorStyles.whiteLabel);

        enemyBaseName = EditorGUILayout.TextField("Base Name", enemyBaseName);
        enemyID = EditorGUILayout.IntField("Enemy ID", enemyID);
        enemyScale = EditorGUILayout.Slider("Enemy Scale", enemyScale, 0.1f, 3f);
        spawnRadius = EditorGUILayout.FloatField("Spawn Radius", spawnRadius);
        colliderOffset = EditorGUILayout.Vector3Field("Collider Offset", colliderOffset);
        colliderSize = EditorGUILayout.Vector3Field("Collider Size", colliderSize);

        GUILayout.Space(10);
        GUILayout.Label("Health", EditorStyles.whiteLabel);

        baseHealth = EditorGUILayout.FloatField("Max Health", baseHealth);
        healthBarOffset = EditorGUILayout.Vector3Field("Healthbar Offset", healthBarOffset);

        GUILayout.Space(10);
        GUILayout.Label("Follow Player Variables", EditorStyles.whiteLabel);
        maxSpeed = EditorGUILayout.FloatField("Max Speed", maxSpeed);
        acceleration = EditorGUILayout.FloatField("Acceleration", acceleration);
        stoppingDistance = EditorGUILayout.FloatField("Stopping Distance", stoppingDistance);
        playerLayer = EditorGUILayout.LayerField("Player Layer", playerLayer);

        GUILayout.Space(10);
        GUILayout.Label("Attacking", EditorStyles.whiteLabel);

        viewRange = EditorGUILayout.FloatField("View Distance", viewRange);
        attackSpeed = EditorGUILayout.FloatField("Attack Speed", attackSpeed);
        attackRange = EditorGUILayout.FloatField("Attack Radius", attackRange);
        baseDamage = EditorGUILayout.FloatField("Base Damage", baseDamage);

        GUILayout.Space(15);
        GUILayout.Label("Attributes", EditorStyles.whiteLabel);

        enemyToSpawn = EditorGUILayout.ObjectField("Prefab To Spawn", enemyToSpawn, typeof(GameObject), false) as GameObject;
        parentObject = EditorGUILayout.ObjectField("Hierarchy Parent", parentObject, typeof(Transform), true) as Transform;
        healthBarPrefab = EditorGUILayout.ObjectField("Health Bar", healthBarPrefab, typeof(GameObject), false) as GameObject;

        if (GUILayout.Button("Spawn Object"))
        {
            SpawnEnemy();
        }
    }
    private void SpawnEnemy()
    {
        #region Debugging
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
        #endregion

        #region Spawning in Scene (For testing)
        Vector3 spawnCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = new Vector3(spawnCircle.x, 0f, spawnCircle.y);

        GameObject newEnemy = Instantiate(enemyToSpawn, parentObject);

        newEnemy.transform.position = spawnPos;
        newEnemy.name = enemyBaseName + enemyID;
        newEnemy.transform.localScale = Vector3.one * enemyScale;
        #endregion

        #region Add Collider component
        BoxCollider collider = newEnemy.AddComponent<BoxCollider>();
        collider.center = colliderOffset;
        collider.size = colliderSize;
        #endregion

        #region Get Renderer
        Renderer renderer = newEnemy.GetComponentInChildren<Renderer>();
        #endregion

        #region Health
        EnemyHealth health = newEnemy.AddComponent<EnemyHealth>();
        health.rend = renderer;                                     // Set the renderer in health script to the found renderer component within the enemy prefab's children, this is to flash the renderer red when taking damage
        health.BaseHealth = baseHealth;
        health.HealthbarPrefab = healthBarPrefab;
        health.healthbarPos = healthBarOffset;
        #endregion

        #region Movement & Attacking
        EnemyMovement enemyMovement = newEnemy.AddComponent<EnemyMovement>();
        enemyMovement.ViewRadius = viewRange;
        enemyMovement.AttackSpeed = attackSpeed;
        enemyMovement.attackRange = attackRange;
        enemyMovement.BaseDamage = baseDamage;
        

        NavMeshAgent agent = newEnemy.GetComponent<NavMeshAgent>();
        agent.speed = maxSpeed;
        agent.acceleration = acceleration;
        agent.stoppingDistance = stoppingDistance;
        #endregion

        #region Save as Asset
        string enemyName = enemyBaseName + enemyID;
        string prefabFolder = "Assets/Prefabs/Enemies/";
        string enemyPrefabPath = prefabFolder + enemyName + ".prefab";

        PrefabUtility.SaveAsPrefabAsset(newEnemy, enemyPrefabPath);
        #endregion

        enemyID++;
    }
}
