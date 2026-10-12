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
        newUpgrade.name = upgradeName;

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
        GameObject newUpgradeImage = new GameObject();
        newUpgradeImage.name = "Image";
        newUpgradeImage.transform.SetParent(newUpgrade.transform);

        Image imageComponent = newUpgradeImage.AddComponent<Image>();
        imageComponent.rectTransform.anchoredPosition = new Vector2(0, -15f);
        imageComponent.rectTransform.sizeDelta = new Vector2(75, 75);
        imageComponent.transform.localScale = new Vector3(3, 3, 3);
        imageComponent.sprite = upgradeImage;
        imageComponent.preserveAspect = true;
        #endregion

        #region Add Name Child
        GameObject newUpgradeName = new GameObject();
        newUpgradeName.name = "Name";
        newUpgradeName.transform.SetParent(newUpgrade.transform);

        TextMeshProUGUI nameComponent = newUpgradeName.AddComponent<TextMeshProUGUI>();
        nameComponent.rectTransform.anchoredPosition = new Vector2(-67, 135);
        nameComponent.rectTransform.sizeDelta = new Vector2(175, 25);
        nameComponent.transform.localScale = Vector3.one;
        nameComponent.fontSize = 38;
        nameComponent.horizontalAlignment = HorizontalAlignmentOptions.Left;
        nameComponent.verticalAlignment = VerticalAlignmentOptions.Middle;
        nameComponent.text = upgradeName;
        #endregion

        #region Add Value Child
        GameObject newUpgradeValue = new GameObject();
        newUpgradeValue.name = "Value";
        newUpgradeValue.transform.SetParent(newUpgrade.transform);

        TextMeshProUGUI valueComponent = newUpgradeValue.AddComponent<TextMeshProUGUI>();
        valueComponent.rectTransform.anchoredPosition = new Vector2(-67, 95);
        valueComponent.rectTransform.sizeDelta = new Vector2(175, 25);
        valueComponent.transform.localScale = Vector3.one;
        valueComponent.fontSize = 32;
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
