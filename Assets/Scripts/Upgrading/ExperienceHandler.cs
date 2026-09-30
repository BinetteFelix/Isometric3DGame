using System.Collections.Generic;
using UnityEngine;
using Utility;
using UnityEngine.Pool;
using TMPro;

public class ExperienceHandler : SingletonBehaviour<ExperienceHandler>
{
    public List<Experience> ExperienceTypes = new List<Experience>();
    
    public ObjectPool<Experience> xpPool;

    #region UI
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private TextMeshProUGUI levelText;
    #endregion

    public int Level;
    private float startLevelUpXPAmount = 100f;
    private float levelUpXPAmount;
    public float LevelUpXPAmount
    {
        get
        {
            return Mathf.Clamp(levelUpXPAmount, startLevelUpXPAmount, maxLevelUpXP);
        }
        set
        {
            return;
        }
    }
    private float maxLevelUpXP = 1000f;
    private float perLevelUpXPMult = 1.1f;
    private float xpCount;
    public float XPCount
    {
        get
        {
            return Mathf.Clamp(xpCount, 0, levelUpXPAmount);
        }
        set
        {
            return;
        }
    }
    private void Start()
    {
        #region XP Pooling
        xpPool = new ObjectPool<Experience>(
            () =>
            {
                int randomXPType = Random.Range(0, ExperienceTypes.Count);
                return Instantiate(ExperienceTypes[randomXPType], transform);
            },
            xp =>
            {
                xp.gameObject.SetActive(true);
            },
            xp =>
            {
                xp.gameObject.SetActive(false);
            },
            xp =>
            {
                Destroy(xp.gameObject);
            },
            false,
            5,
            100
            );
        #endregion

        levelUpXPAmount = startLevelUpXPAmount;
    }

    #region XP SPAWNING
    public void SpawnXP(Vector3 enemyOrigin)
    {
        Experience xp = xpPool.Get();
        if (xp != null)
            xp.transform.position = new Vector3(enemyOrigin.x, 1.5f, enemyOrigin.z);
    }
    #endregion

    #region LEVEL HANDLING METHODS
    private void LevelUp()
    {
        xpCount = Mathf.RoundToInt(xpCount - levelUpXPAmount);
        levelUpXPAmount *= perLevelUpXPMult;
        Level++;

        xpText.text = $"{xpCount} / {Mathf.Round(levelUpXPAmount)}";
        levelText.text = Level.ToString();

        UpgradeManager.Instance.OpenLevelUpScreen();
    }
    public void GainXP(float amount)
    {
        xpCount += amount;
        xpText.text = $"{xpCount} / {Mathf.Round(levelUpXPAmount)}";

        if (xpCount >= levelUpXPAmount)
            LevelUp();
    }
    #endregion

    #region INSTANCE HANDLER
    public override void Instantiate()
    {
    }
    #endregion
}
