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
    private int _playerBlobQueue = 0;
    private int _enemyBlobQueue = 0;

    public void Init(MainScenarioLogic scenarioLogic)
    {
        _scenarioLogic = scenarioLogic;
        int idx = 0;
        foreach (LaserBeamLogic lbl in _laserBeamList)
        {
            lbl.Init(_laserTotalLenght, _laserEnemyTipPos, _laserPlayerTipPos, (LaserLinesEnum)idx, this);
            idx++;
        }
    }

    public void LaserSetting()
    {
        _playerBattleLogic.Init(_scenarioLogic);
        // TODO: enemy list
        _enemyBattleLogic.Init(_scenarioLogic);

        int idx = 0;
        foreach (LaserBeamLogic lbl in _laserBeamList)
        {
            lbl.LasetSet(_enemyBattleLogic.GetBlobForce((LaserLinesEnum)idx), _playerBattleLogic.GetBlobForce((LaserLinesEnum)idx));
            lbl.UpdateEnemyLaserColor(_enemyBattleLogic.GetBlobStartColor((LaserLinesEnum)idx));
            _playerBlobQueue++;
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
        _playerBlobQueue = 0;
        _playerBattleLogic.Reset();
        _enemyBattleLogic.Reset();
    }

    public void PlayerBlobDeath(LaserLinesEnum lane)
    {
        BlobTypes type = BlobTypes.BLACK;
        if (_scenarioLogic.CurrentBlobSelection.Count > _playerBlobQueue)
        {
            type = _scenarioLogic.CurrentBlobSelection[_playerBlobQueue];
            _playerBlobQueue++;
        }

        _playerBattleLogic.BlobDeath(lane, type);
        _laserBeamList[(int)lane].LasetSet(_enemyBattleLogic.GetBlobForce(lane), _playerBattleLogic.GetBlobForce(lane));
        _laserBeamList[(int)lane].UpdatePlayerLaserColor(_playerBattleLogic.GetBlobStartColor(lane));
    }

    public void EnemyBlobDeath(LaserLinesEnum lane)
    {
        //_enemyBattleLogic.BlobDeath(lane);
    }

    public void UpdateLaserLaneColor(int laneIdx, Color color, bool isPlayer)
    {
        if (isPlayer)
        {
            _laserBeamList[laneIdx].UpdatePlayerLaserColor(color);
            return;
        }
        _laserBeamList[laneIdx].UpdateEnemyLaserColor(color);
    }
}
