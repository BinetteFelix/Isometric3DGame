using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCreator : EditorWindow
{
    string upgradeName = "";
    float upgradeValue;
    bool isMultiApplicable;
    Sprite upgradeImage;
    Transform upgradeGrid;
    GameObject upgradePanel;
    GameObject upgradeLayoutPrefab;



    [MenuItem("Tools/UpgradeCreator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(UpgradeCreator));
    }

    private void OnGUI()
    {
        GUILayout.Label("Create New Upgrade", EditorStyles.whiteLargeLabel);
        GUILayout.Space(10);
        GUILayout.Label("Upgrade Data", EditorStyles.whiteLabel);

        GUILayout.Space(10);

        upgradeName = EditorGUILayout.TextField("Upgrade name", upgradeName);
        upgradeValue = EditorGUILayout.FloatField("Upgrade value", upgradeValue);
        isMultiApplicable = EditorGUILayout.Toggle("Multi Applicable", isMultiApplicable);
        upgradeImage = EditorGUILayout.ObjectField("Upgrade image", upgradeImage, typeof(Sprite), false) as Sprite;

        GUILayout.Space(10);
        GUILayout.Label("Spawning Variables", EditorStyles.whiteLabel);
        GUILayout.Space(10);

        upgradeGrid = EditorGUILayout.ObjectField("Grid", upgradeGrid, typeof(Transform), true) as Transform;
        upgradePanel = EditorGUILayout.ObjectField("Panel", upgradePanel, typeof(GameObject), true) as GameObject;
        upgradeLayoutPrefab = EditorGUILayout.ObjectField("UI Layout", upgradeLayoutPrefab, typeof(GameObject), false) as GameObject;

        if (GUILayout.Button("Spawn Object"))
        {
            SpawnUpgrade();
        }
    }
    private void SpawnUpgrade()
    {
        #region Debugging
        if (upgradeName == string.Empty)
        {
            Debug.LogError("Error: Please enter a base name for the object");
            return;
        }
        if (upgradeGrid == null)
        {
            Debug.LogError("Error: Please Enter a Grid for the upgrade to spawn in");
            return;
        }
        if (upgradeValue == float.NaN)
        {
            upgradeValue = 0;
            Debug.LogError("Error: Please enter a number for the upgrade value");
            return;
        }
        if (upgradeLayoutPrefab == null)
        {
            Debug.LogError("Error: Please enter a upgrade UI layout");
            return;
        }
        #endregion

        #region Spawning in Scene (For testing)
        GameObject newUpgrade = Instantiate(upgradeLayoutPrefab, upgradeGrid);

        #region Add ScriptableObject
        UpgradeData data = CreateInstance<UpgradeData>();
        data.name = upgradeName;
        data.upgradeName = upgradeName;
        data.value = upgradeValue;
        data.image = upgradeImage;
        #endregion

        #region Add Upgrade Execution Component
        Upgrade upgradeExecution = newUpgrade.AddComponent<Upgrade>();
        upgradeExecution.UpgradeData = data;
        upgradeExecution.Panel = upgradePanel;
        #endregion

        #region Add Image Child
        GameObject newImage = new GameObject();
        newImage.transform.SetParent(newUpgrade.transform);
        newImage.transform.position = newUpgrade.transform.position + new Vector3(0, -25f / 3, 0);

        Image imageComponent = newImage.AddComponent<Image>();
        imageComponent.rectTransform.sizeDelta = new Vector2(75, 75);
        imageComponent.sprite = upgradeImage;
        #endregion

        #region Add Name Child
        GameObject newUpgradeName = new GameObject();
        newUpgradeName.transform.SetParent(newUpgrade.transform);
        newUpgradeName.transform.position = newUpgrade.transform.position + new Vector3(-15f, 42.5f);

        TextMeshProUGUI nameComponent = newUpgradeName.AddComponent<TextMeshProUGUI>();
        nameComponent.rectTransform.sizeDelta = new Vector2(75, 25);
        nameComponent.fontSize = 16;
        nameComponent.horizontalAlignment = HorizontalAlignmentOptions.Left;
        nameComponent.verticalAlignment = VerticalAlignmentOptions.Middle;
        nameComponent.text = upgradeName;
        #endregion

        #region Add Value Child
        GameObject newUpgradeValue = new GameObject();
        newUpgradeValue.transform.SetParent(newUpgrade.transform);
        newUpgradeValue.transform.position = newUpgrade.transform.position + new Vector3(-15f, 27.5f);

        TextMeshProUGUI valueComponent = newUpgradeValue.AddComponent<TextMeshProUGUI>();
        valueComponent.rectTransform.sizeDelta = new Vector2(75, 25);
        valueComponent.fontSize = 12;
        valueComponent.horizontalAlignment = HorizontalAlignmentOptions.Left;
        valueComponent.verticalAlignment = VerticalAlignmentOptions.Middle;
        valueComponent.text = "+ " + upgradeValue.ToString();
        #endregion

        #endregion

        #region Save as Asset
        string objectName = upgradeName;
        string prefabFolder = "Assets/ScriptableObjects/Upgrades/";
        string upgradePrefabPath = prefabFolder + objectName + ".asset";
        AssetDatabase.CreateAsset(data, upgradePrefabPath);
        #endregion
    }
}
