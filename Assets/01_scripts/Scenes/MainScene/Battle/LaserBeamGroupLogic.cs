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
            idx++;
        }
    }

    public void UpdateLogic()
    {
        int idx = 0;
        foreach (LaserBeamLogic lbl in _laserBeamList)
        {
            if (lbl.IsActive)
            {
                lbl.UpdateLaserLane(_enemyBattleLogic.GetBlobForce((LaserLinesEnum)idx), _playerBattleLogic.GetBlobForce((LaserLinesEnum)idx));
            }            
            idx++;
        }
    }

    public void EndGame()
    {
        _playerBattleLogic.Reset();
        _enemyBattleLogic.Reset();
    }

    public void PlayerBlobDeath(LaserLinesEnum lane)
    {
        BlobTypes type = BlobTypes.MAX;
        if (_scenarioLogic.CurrentBlobSelection.Count > 0)
        {
            type = _scenarioLogic.CurrentBlobSelection[0];
            _scenarioLogic.RemoveNextBlobFromQueue();
            _playerBattleLogic.RemoveNextBlobFromReserve();
        }

        _playerBattleLogic.BlobDeath(lane, type);

        // blob reserve is over!
        if (type == BlobTypes.MAX)
        {
            _laserBeamList[(int)lane].DeactivateLaser();
            return;
        }

        _laserBeamList[(int)lane].LasetSet(_enemyBattleLogic.GetBlobForce(lane), _playerBattleLogic.GetBlobForce(lane));
        _laserBeamList[(int)lane].UpdatePlayerLaserColor(_playerBattleLogic.GetBlobStartColor(lane));
    }

    // [TODO] enemy reserve from seed!
    public void EnemyBlobDeath(LaserLinesEnum lane)
    {
        BlobTypes type = BlobTypes.MAX;
        if (_scenarioLogic.CurrentEnemyQueue.Count > 0)
        {
            type = _scenarioLogic.CurrentEnemyQueue[0];
            _scenarioLogic.RemoveNextBlobFromEnemyQueue();
            _enemyBattleLogic.RemoveNextBlobFromReserve();
        }

        _enemyBattleLogic.BlobDeath(lane, type);

        // blob reserve is over!
        if (type == BlobTypes.MAX)
        {
            _laserBeamList[(int)lane].DeactivateLaser();
            return;
        }

        _laserBeamList[(int)lane].LasetSet(_enemyBattleLogic.GetBlobForce(lane), _playerBattleLogic.GetBlobForce(lane));
        _laserBeamList[(int)lane].UpdateEnemyLaserColor(_enemyBattleLogic.GetBlobStartColor(lane));
    }

    public void ReactivateLaserLane(LaserLinesEnum lane)
    {
        _laserBeamList[(int)lane].LasetSet(_enemyBattleLogic.GetBlobForce(lane), _playerBattleLogic.GetBlobForce(lane));
        _laserBeamList[(int)lane].UpdateEnemyLaserColor(_enemyBattleLogic.GetBlobStartColor(lane));
    }

    public void UpdateLaserLaneColor(int laneIdx, Color color, bool isPlayer, bool blobAlive)
    {
        if (isPlayer)
        {
            if (blobAlive && !_laserBeamList[laneIdx].IsActive)
            {
                // reactivating the laser
                _laserBeamList[laneIdx].LasetSet(_enemyBattleLogic.GetBlobForce((LaserLinesEnum)laneIdx), _playerBattleLogic.GetBlobForce((LaserLinesEnum)laneIdx));
            }
            _laserBeamList[laneIdx].UpdatePlayerLaserColor(color);
            return;
        }
        _laserBeamList[laneIdx].UpdateEnemyLaserColor(color);
    }
}
