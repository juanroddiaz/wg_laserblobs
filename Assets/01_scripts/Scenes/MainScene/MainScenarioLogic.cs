﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum IBlobDifficulty
{
    VeryEasy = 0,
    Easier,
    Easy,
    Medium,
    Hard,
    Harder,
    VeryHard,
    Max
}

[System.Serializable]
public class DifficultyBlobEntries
{
    public IBlobDifficulty Difficulty;
    public List<GameObject> BlobTypeObjects;
}

public class MainScenarioLogic : MonoBehaviour
{
    [SerializeField]
    private LaserBeamGroupLogic _laserGroupLogic;

    [Header("Color blob prefabs, sorted by BlobTypes enumeration.")]
    [SerializeField]
    private List<DifficultyBlobEntries> _difficultyBlobLists = new List<DifficultyBlobEntries>();
    public List<DifficultyBlobEntries> DifficultyBlobLists
    {
        get { return _difficultyBlobLists; }
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

    // blob gameplay's queue data
    private List<BlobTypes> _totalEnemyQueue = new List<BlobTypes>();
    private int _totalEnemyAmount = 0;
    private int _currentEnemyTurn = 0;
    private int _currentPlayerTurn = 0;

    [Header("Main configuration for affinity and damage tables")]
    [SerializeField]
    private AffinityConfiguration _affinityConfig;

    [Header("Main configuration gameplay waves")]
    [SerializeField]
    private GameplayConfiguration _gameplayConfig;

    private MainSceneController _sceneController;
    public MainSceneController SceneController
    {
        get { return _sceneController; }
    }

    [SerializeField]
    private CanvasScaler _canvasScaler;
    public CanvasScaler CanvasScaler
    {
        get { return _canvasScaler; }
    }

    private int _currentEnemyCountIndex = 0;
    private int _currentWave = 0;

    public int EnemyCountForEarningBlob { get; private set; }

    public void IncreaseEnemyCountIndex()
    {
        _currentEnemyCountIndex++;
    }

    public List<BlobTypes> GetInitialBlobs()
    {
        return _gameplayConfig.startingTypes;
    }

    private const int c_enemyReserveCount = 3;

    public GameObject GetBlobTypePerDifficulty(int type)
    {
        BlobsWaveConfig currentWave = _gameplayConfig.blobWavesConfig.waveInstancesList[_currentWave];
        DifficultyBlobEntries entries = _difficultyBlobLists.Find(x => x.Difficulty == currentWave.difficulty);

        return entries.BlobTypeObjects[type];
    }

    public void Init(MainSceneController sceneController)
    {
        _sceneController = sceneController;
        _laserGroupLogic.Init(this);
    }

    public void StartGame()
    {
        _currentBlobSelection = new List<BlobTypes>(_gameplayConfig.startingTypes);
        _currentEnemyQueue.Clear();
        _currentEnemyTurn = 0;
        _currentPlayerTurn = 0;
        _currentWave = 0;

        CalculateEnemyQueue();

        _totalEnemyAmount = _totalEnemyQueue.Count;

        _currentEnemyCountIndex = 0;
        CalculateNextEnemyCountForReward();

        for (int i = 0; i < c_enemyReserveCount; i++)
        {
            _currentEnemyQueue.Add(_totalEnemyQueue[i]);
            _currentEnemyTurn++;
        }
        _laserGroupLogic.LaserSetting();
    }

    private void CalculateEnemyQueue()
    {
        _totalEnemyQueue.Clear();
        // creating enemy blobs total queue for this gameplay
        foreach (BlobsWaveConfig waveConfig in _gameplayConfig.blobWavesConfig.waveInstancesList)
        {
            if (waveConfig.isRandomPool)
            {
                int randomIdx = 0;
                for (int i = 0; i < waveConfig.randomIterations; i++)
                {
                    randomIdx = Random.Range(0, waveConfig.availableTypes.Count);
                    _totalEnemyQueue.Add(waveConfig.availableTypes[randomIdx]);
                }
            }
            else
            {
                for (int i = 0; i < waveConfig.availableTypes.Count; i++)
                {
                    _totalEnemyQueue.Add(waveConfig.availableTypes[i]);
                }
            }
        }
    }

    private void CalculateNextEnemyCountForReward()
    {
        BlobsWaveConfig currentWave = _gameplayConfig.blobWavesConfig.waveInstancesList[_currentWave];
        int nextWaveLimit = currentWave.isRandomPool ? currentWave.randomIterations : currentWave.availableTypes.Count;
        EnemyCountForEarningBlob = _currentEnemyCountIndex + nextWaveLimit;
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
        // enemy spawing generetion according to difficulty and game settings
        for (int i = 0; i < amount; i++)
        {
            _currentEnemyQueue.Add(_totalEnemyQueue[_currentEnemyTurn]);
            _currentEnemyTurn++;
            if (_totalEnemyQueue.Count == _currentEnemyTurn)
            {
                _currentEnemyTurn = 0;
                CalculateEnemyQueue();
            }
        }
        // calculatingblob earning indexation
        _currentPlayerTurn += amount;
        if (_totalEnemyQueue.Count <= _currentPlayerTurn)
        {
            _currentPlayerTurn -= _totalEnemyQueue.Count;
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

    #region Blob earning methods
    public void AddBlobToPlayerReserve()
    {
        // earning the current enemy wave turn's blob
        foreach (BlobTypes bt in _gameplayConfig.blobWavesConfig.waveInstancesList[_currentWave].rewardsTypes)
        {
            _currentBlobSelection.Add(bt);
            _laserGroupLogic.AddBlobToPlayerReserve();
        }

        //remove current enemy prize turn
        _currentWave++;
        if (_currentWave == _gameplayConfig.blobWavesConfig.waveInstancesList.Count)
        {
            _currentWave = 0;
        }
        CalculateNextEnemyCountForReward();
    }
    #endregion
}
