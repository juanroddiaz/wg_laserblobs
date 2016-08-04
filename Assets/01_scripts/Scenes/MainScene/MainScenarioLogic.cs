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

    private List<BlobTypes> _currentBlobSelection = new List<BlobTypes>();
    public List<BlobTypes> CurrentBlobSelection
    {
        get { return _currentBlobSelection; }
    }

    public void Init()
    {
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

    public void UpdateLaserColors(LaserLinesEnum line, Color lineColor, bool isPlayer)
    {
        _laserGroupLogic.UpdateLaserLaneColor((int)line, lineColor, isPlayer);
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
    #endregion
}
