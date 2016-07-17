using UnityEngine;
using System.Collections.Generic;

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
    private DraggablePanelBattleLogic _draggablePanelLogic;

    public void Init()
    {
        _laserGroupLogic.Init();
        _draggablePanelLogic.Init();
    }

    public void StartGame(List<string> _blobSelection)
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
