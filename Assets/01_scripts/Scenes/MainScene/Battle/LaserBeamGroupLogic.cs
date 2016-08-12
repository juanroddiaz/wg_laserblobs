using UnityEngine;
using System.Collections;
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
            LaserLinesEnum lane = (LaserLinesEnum)idx;
            lbl.LasetSet(_enemyBattleLogic.GetBlobForce(lane), _playerBattleLogic.GetBlobForce(lane));
            lbl.UpdateEnemyLaserColor(_enemyBattleLogic.GetBlobStartColor(lane));
            CalculateDamageMultiplierForLane(lane);
            idx++;
        }
    }

    public void CalculateDamageMultiplierForLane(LaserLinesEnum lane)
    {
        float playerDmg = 0.0f;
        float enemyDmg = 0.0f;
        _scenarioLogic.GetBlobsDamageRelation(_playerBattleLogic.GetBlobType(lane), _enemyBattleLogic.GetBlobType(lane), out playerDmg, out enemyDmg);
        _playerBattleLogic.UpdateBlobForce(lane, playerDmg);
        _enemyBattleLogic.UpdateBlobForce(lane, enemyDmg);
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
        foreach (LaserBeamLogic lbl in _laserBeamList)
        {
            lbl.ActivateLaser();
        }
    }

    public IEnumerator PlayerBlobDeath(LaserLinesEnum lane)
    {
        BlobTypes type = BlobTypes.MAX;
        if (_scenarioLogic.CurrentBlobSelection.Count > 0)
        {
            type = _scenarioLogic.CurrentBlobSelection[0];
            _scenarioLogic.RemoveNextBlobFromQueue();
            _playerBattleLogic.RemoveNextBlobFromReserve(_playerBattleLogic.BlobLogicList[(int)lane].transform.position);
        }

        _enemyBattleLogic.SetLaserAnim(lane, false);
        yield return StartCoroutine(_playerBattleLogic.BlobDeath(lane, type));

        // blob reserve is over!
        if (type == BlobTypes.MAX)
        {
            _laserBeamList[(int)lane].DeactivateLaser();
            yield break;
        }

        _enemyBattleLogic.SetLaserAnim(lane, true);
        _laserBeamList[(int)lane].LasetSet(_enemyBattleLogic.GetBlobForce(lane), _playerBattleLogic.GetBlobForce(lane));
        _laserBeamList[(int)lane].UpdatePlayerLaserColor(_playerBattleLogic.GetBlobStartColor(lane));
    }

    // [TODO] enemy reserve from seed!
    public IEnumerator EnemyBlobDeath(LaserLinesEnum lane)
    {
        BlobTypes type = BlobTypes.MAX;
        if (_scenarioLogic.CurrentEnemyQueue.Count > 0)
        {
            type = _scenarioLogic.CurrentEnemyQueue[0];
            _scenarioLogic.RemoveNextBlobFromEnemyQueue();
            _enemyBattleLogic.RemoveNextBlobFromReserve(_enemyBattleLogic.BlobLogicList[(int)lane].transform.position);
            _enemyBattleLogic.AddBlobToReserve();
        }

        _playerBattleLogic.SetLaserAnim(lane, false);
        yield return StartCoroutine(_enemyBattleLogic.BlobDeath(lane, type));

        // blob reserve is over!
        if (type == BlobTypes.MAX)
        {
            _laserBeamList[(int)lane].DeactivateLaser();
            yield break;
        }

        _playerBattleLogic.SetLaserAnim(lane, true);
        _laserBeamList[(int)lane].LasetSet(_enemyBattleLogic.GetBlobForce(lane), _playerBattleLogic.GetBlobForce(lane));
        _laserBeamList[(int)lane].UpdateEnemyLaserColor(_enemyBattleLogic.GetBlobStartColor(lane));
    }

    public void UpdateLaserLaneColor(int laneIdx, Color color, bool isPlayer, bool blobAlive)
    {
        if (isPlayer)
        {
            if (blobAlive)
            {
                if (!_laserBeamList[laneIdx].IsActive)
                {
                    // reactivating the laser
                    _enemyBattleLogic.SetLaserAnim((LaserLinesEnum)laneIdx, true);
                    _laserBeamList[laneIdx].LasetSet(_enemyBattleLogic.GetBlobForce((LaserLinesEnum)laneIdx), _playerBattleLogic.GetBlobForce((LaserLinesEnum)laneIdx));
                }
                _laserBeamList[laneIdx].UpdatePlayerLaserColor(color);
            }
            else
            {
                _enemyBattleLogic.SetLaserAnim((LaserLinesEnum)laneIdx, false);
                _laserBeamList[laneIdx].DeactivateLaser();
            }
            return;
        }
        _laserBeamList[laneIdx].UpdateEnemyLaserColor(color);
    }

    public void UpdateDifficulty()
    {
        _enemyBattleLogic.UpdateDifficulty();
        _playerBattleLogic.UpdateDifficulty();
    }

    public void AddBlobToPlayerReserve()
    {
        _playerBattleLogic.AddBlobToReserve();
        // checkin if any battleground blob is dead to replace it inmediately
        LaserLinesEnum lane = _playerBattleLogic.CheckDeathBlobLane();
        if (lane != LaserLinesEnum.Max)
        {
            StartCoroutine(ResurrectRoutine(lane));
        }
    }

    private IEnumerator ResurrectRoutine(LaserLinesEnum lane)
    {
        yield return new WaitForEndOfFrame();
        BlobTypes type = _scenarioLogic.CurrentBlobSelection[0];
        _scenarioLogic.RemoveNextBlobFromQueue();
        _playerBattleLogic.RemoveNextBlobFromReserve(_playerBattleLogic.BlobLogicList[(int)lane].transform.position);

        yield return new WaitForSeconds(0.5f);
        _playerBattleLogic.BlobResurrectionFromReserve(lane, type);

        _enemyBattleLogic.SetLaserAnim(lane, true);
        _laserBeamList[(int)lane].LasetSet(_enemyBattleLogic.GetBlobForce(lane), _playerBattleLogic.GetBlobForce(lane));
        _laserBeamList[(int)lane].UpdatePlayerLaserColor(_playerBattleLogic.GetBlobStartColor(lane));
        yield break;
    }
}
