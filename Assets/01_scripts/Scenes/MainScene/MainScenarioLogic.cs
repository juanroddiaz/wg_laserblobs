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

    [Header("Debug stuff!")]
    [SerializeField]
    private int _debugEnemyCount = 20;

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
    }
    #endregion
}
