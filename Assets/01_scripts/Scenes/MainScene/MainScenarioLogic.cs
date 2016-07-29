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
        _laserGroupLogic.LaserSetting(_blobSelection);
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
}
