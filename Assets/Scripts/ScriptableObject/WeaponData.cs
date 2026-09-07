using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("Guns")]
    public int GunBaseDamage;
    public string GunName;

    [Space(15)]

    [Header("Magic")]
    public int MagicBaseDamage;
    public string MagicName;
}