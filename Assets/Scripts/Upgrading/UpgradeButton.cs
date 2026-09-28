using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade Button", menuName = "ScriptableObjects/ButtonData")]
public class UpgradeButton : ScriptableObject
{
    public string upgradeName;
    public float value;
    public Sprite image;
}
