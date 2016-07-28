using UnityEngine;
using System.Collections.Generic;

public enum LaserLinesEnum
{
    Left,
    Middle,
    Right,
    Max
}

public class LaserBeamGroupLogic : MonoBehaviour
{
    [SerializeField]
    private BattleGroundLogic _enemyBattleLogic;
    [SerializeField]
    private BattleGroundLogic _playerBattleLogic;
    [SerializeField]
    private List<LaserBeamLogic> _laserBeamList = new List<LaserBeamLogic>();
    [SerializeField]
    private float _laserTotalLenght = 4.75f;
    [SerializeField]
    private float _laserEnemyTipPos = 68.0f;
    [SerializeField]
    private float _laserPlayerTipPos = -45.0f;

    private MainScenarioLogic _scenarioLogic;

    public void Init(MainScenarioLogic scenarioLogic)
    {
        _scenarioLogic = scenarioLogic;
    }

    public void LaserSetting(List<BlobTypes> blobSelection)
    {
        _playerBattleLogic.Init(blobSelection, _scenarioLogic);
        // TODO: enemy list
        _enemyBattleLogic.Init(null, _scenarioLogic);

        int idx = 0;
        foreach (LaserBeamLogic lbl in _laserBeamList)
        {
            lbl.Init(_enemyBattleLogic.GetBlobForce((LaserLinesEnum)idx), _playerBattleLogic.GetBlobForce((LaserLinesEnum)idx), 
                    _laserTotalLenght, _laserEnemyTipPos, _laserPlayerTipPos);
            lbl.SetLaserColors(_enemyBattleLogic.GetBlobStartColor((LaserLinesEnum)idx), _playerBattleLogic.GetBlobStartColor((LaserLinesEnum)idx));
            idx++;
        }
    }

    public void UpdateLogic()
    {
        int idx = 0;
        foreach (LaserBeamLogic lbl in _laserBeamList)
        {
            lbl.UpdateLaserLane(_enemyBattleLogic.GetBlobForce((LaserLinesEnum)idx), _playerBattleLogic.GetBlobForce((LaserLinesEnum)idx));
            idx++;
        }
    }

    public void EndGame()
    {
        _playerBattleLogic.Reset();
        _enemyBattleLogic.Reset();
    }
}
