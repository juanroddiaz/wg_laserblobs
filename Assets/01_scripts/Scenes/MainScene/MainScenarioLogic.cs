using UnityEngine;
using System.Collections.Generic;

public enum BlobTypes
{
    RED = 0,
    BLUE,
    YELLOW,
    ORANGE,
    PURPLE,
    GREEN,
    GRAY,
    WHITE,
    BLACK,
    MAX
}


public enum GameDifficulty
{
    Easy = 0,
    Medium,
    Hard,
    Max
}

public class MainScenarioLogic : MonoBehaviour
{
    [SerializeField]
    private LaserBeamGroupLogic _laserGroupLogic;

    [SerializeField]
    private List<GameObject> _blobPrefabs = new List<GameObject>();

    public void Init()
    {
        if (_blobPrefabs.Count != (int)BlobTypes.MAX)
        {
            CustomLog.LogError("Missing blob prefab entries in MainScenarioLogic object!! Aborting :(");
            return;
        }
        _laserGroupLogic.Init();
    }

    public void StartGame(List<BlobTypes> _blobSelection)
    {
        _laserGroupLogic.LaserSetting();
    }

    public void EndGame()
    {

    }

    public void UpdateLogic()
    {
        _laserGroupLogic.UpdateLogic();
    }
}
