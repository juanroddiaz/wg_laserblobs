using UnityEngine;
using System.Collections.Generic;

public class MainScenarioLogic : MonoBehaviour
{
    [SerializeField]
    private LaserBeamGroupLogic _laserGroupLogic;

    [Header("Color blob prefabs, sorted by BlobTypes enumeration.")]
    [SerializeField]
    private List<GameObject> _blobPrefabs = new List<GameObject>();
    public List<GameObject> BlobPrefabs
    {
        get { return _blobPrefabs; }
    }

    [SerializeField]
    private GameObject _deadBlob;
    public GameObject DeadBlob
    {
        get { return _deadBlob; }
    }

    private List<BlobTypes> _currentBlobSelection = new List<BlobTypes>();
    public List<BlobTypes> CurrentBlobSelection
    {
        get { return _currentBlobSelection; }
    }

    private List<BlobTypes> _currentEnemyQueue = new List<BlobTypes>();
    public List<BlobTypes> CurrentEnemyQueue
    {
        get { return _currentEnemyQueue; }
    }

    [Header("Main configuration for affinity and damage tables")]
    [SerializeField]
    private AffinityConfiguration _affinityConfig;

    [Header("Debug stuff!")]
    [SerializeField]
    private int _debugEnemyCount = 20;
    [SerializeField]
    private int _debugEnemyCountForDifficultyInc = 10;
    public int DebugEnemyCountForDifficultyInc
    {
        get { return _debugEnemyCountForDifficultyInc; }
    }
    [SerializeField]
    private float _debugDifficultyIncForceStep = 0.0005f;
    public float DebugDifficultyIncForceStep
    {
        get { return _debugDifficultyIncForceStep; }
    }

    private MainSceneController _sceneController;
    public MainSceneController SceneController
    {
        get { return _sceneController; }
    }

    public void Init(MainSceneController sceneController)
    {
        _sceneController = sceneController;

        if (_blobPrefabs.Count != (int)BlobTypes.MAX)
        {
            CustomLog.LogError("Missing blob prefab entries in MainScenarioLogic object!! Aborting :(");
            return;
        }
        _laserGroupLogic.Init(this);
    }

    public void StartGame(List<BlobTypes> _blobSelection)
    {
        _currentBlobSelection = _blobSelection;
        for (int i = 0; i < _debugEnemyCount; i++)
        {
            _currentEnemyQueue.Add((BlobTypes)Random.Range(0, (int)BlobTypes.MAX));
        }
        _laserGroupLogic.LaserSetting();
    }

    public void EndGame()
    {
        _laserGroupLogic.EndGame();
    }

    public void UpdateLogic()
    {
        _laserGroupLogic.UpdateLogic();
    }

    public void UpdateLaserColors(LaserLinesEnum line, Color lineColor, bool isPlayer, bool blobAlive)
    {
        _laserGroupLogic.UpdateLaserLaneColor((int)line, lineColor, isPlayer, blobAlive);
    }

    #region Blob reserve logic
    public void RemoveNextBlobFromQueue(int amount = 1)
    {
        if (_currentBlobSelection.Count < amount)
        {
            CustomLog.LogError("Trying to remove more blobs than the current queue contains!!");
            return;
        }
        _currentBlobSelection.RemoveRange(0, amount);
    }

    public void RemoveNextBlobFromEnemyQueue(int amount = 1)
    {
        if (_currentEnemyQueue.Count < amount)
        {
            CustomLog.LogError("Trying to remove more blobs than the current ENEMY queue contains!!");
            return;
        }
        _currentEnemyQueue.RemoveRange(0, amount);
        // TODO: enemy spawing generetaion according to difficulty and game settings
        for (int i = 0; i < amount; i++)
        {
            _currentEnemyQueue.Add((BlobTypes)Random.Range(0, (int)BlobTypes.MAX));
        }
    }
    #endregion

    #region Affinity and Damage methods
    public void GetBlobsDamageRelation(BlobTypes playerType, BlobTypes enemyType, out float playerPerc, out float enemyPerc)
    {
        CustomLog.Log("Calculating " + playerType.ToString() + " VS " + enemyType.ToString() + " damage percentages!");
        playerPerc = 100.0f;
        enemyPerc = 100.0f;

        if (playerType == BlobTypes.MAX || enemyType == BlobTypes.MAX)
        {
            return;
        }

        BlobSetting playerBs = _affinityConfig.blobsSetting.Find(x => x.type == playerType);
        if (playerBs == null)
        {
            CustomLog.LogError("Blobtype " + playerType.ToString() + " is not configured in AffinityConfig's blobSetting list!! Aborting!");
            return;
        }

        BlobSetting enemyBs = _affinityConfig.blobsSetting.Find(x => x.type == enemyType);
        if (enemyBs == null)
        {
            CustomLog.LogError("Blobtype " + enemyType.ToString() + " is not configured in AffinityConfig's blobSetting list!! Aborting!");
            return;
        }

        BlobAffinities playerBa = BlobAffinities.MAX;
        BlobAffinities enemyBa = BlobAffinities.MAX;
        AffinitySetting affs = null;
        AffinityRelationship ar = null;
        DamageConfiguration dc = null;
        float damagePercAcc = 0.0f;

        // player affinities and damage
        for (int i = 0; i < playerBs.affinities.Count; i++)
        {
            playerBa = playerBs.affinities[i];
            affs = _affinityConfig.affinitySetting.Find(x => x.affinity == playerBa);
            if (affs == null)
            {
                CustomLog.LogError("Affinity " + playerBa.ToString() + " is not configured in AffinityConfig's affinitySetting list!! Aborting!");
                return;
            }
            // check targetType intro affs and get the right multiplier at the end of all resistances/weaknesses
            for (int j = 0; j < enemyBs.affinities.Count; j++)
            {
                enemyBa = enemyBs.affinities[j];
                ar = affs.affinities.Find(x => x.affinity == enemyBa);
                if (ar != null)
                {
                    // affinity relationship found
                    dc = _affinityConfig.damageConfiguration.Find(x => x.damageHierarchy == ar.damage);
                    if (dc == null)
                    {
                        CustomLog.LogError("DamageConfiguration " + ar.damage.ToString() + " is not configured in AffinityConfig's damageConfiguration list!! Aborting!");
                        return;
                    }
                    // bigger than 100? enemy weakness. below 100? enemy resistance
                    damagePercAcc += dc.percentage - 100.0f;
                }
            }
        }

        playerPerc = 100.0f + damagePercAcc;
        CustomLog.Log("playerPerc: " + playerPerc.ToString());
        damagePercAcc = 0.0f;
        // enemy affinities and damage
        for (int i = 0; i < enemyBs.affinities.Count; i++)
        {
            enemyBa = enemyBs.affinities[i];
            affs = _affinityConfig.affinitySetting.Find(x => x.affinity == enemyBa);
            if (affs == null)
            {
                CustomLog.LogError("Affinity " + enemyBa.ToString() + " is not configured in AffinityConfig's affinitySetting list!! Aborting!");
                return;
            }
            // check targetType intro affs and get the right multiplier at the end of all resistances/weaknesses
            for (int j = 0; j < playerBs.affinities.Count; j++)
            {
                playerBa = playerBs.affinities[j];
                ar = affs.affinities.Find(x => x.affinity == playerBa);
                if (ar != null)
                {
                    // affinity relationship found
                    dc = _affinityConfig.damageConfiguration.Find(x => x.damageHierarchy == ar.damage);
                    if (dc == null)
                    {
                        CustomLog.LogError("DamageConfiguration " + ar.damage.ToString() + " is not configured in AffinityConfig's damageConfiguration list!! Aborting!");
                        return;
                    }
                    // bigger than 100? enemy weakness. below 100? enemy resistance
                    damagePercAcc += dc.percentage - 100.0f;
                }
            }
        }

        enemyPerc = 100.0f + damagePercAcc;
        CustomLog.Log("enemyPerc: " + enemyPerc.ToString());
    }

    public void CalculateDamageMultiplierForLane(LaserLinesEnum lane)
    {
        _laserGroupLogic.CalculateDamageMultiplierForLane(lane);
    }
    #endregion
}
