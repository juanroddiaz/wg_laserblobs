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

    private List<BlobTypes> _playerBlobSelection = new List<BlobTypes>();

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

    public void LaserSetting(List<BlobTypes> blobSelection)
    {
        _playerBattleLogic.Init(blobSelection, _scenarioLogic);
        // TODO: enemy list
        _enemyBattleLogic.Init(null, _scenarioLogic);

        int idx = 0;
        foreach (LaserBeamLogic lbl in _laserBeamList)
        {
            lbl.LasetSet(_enemyBattleLogic.GetBlobForce((LaserLinesEnum)idx), _playerBattleLogic.GetBlobForce((LaserLinesEnum)idx)); 
            lbl.SetLaserColors(_enemyBattleLogic.GetBlobStartColor((LaserLinesEnum)idx), _playerBattleLogic.GetBlobStartColor((LaserLinesEnum)idx));
            _playerBlobQueue++;
            idx++;
        }

        _playerBlobSelection = blobSelection;
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

    public void PlayerDeath(LaserLinesEnum lane)
    {
        _playerBattleLogic.BlobDeath(lane, _playerBlobSelection[_playerBlobQueue]);
        _laserBeamList[(int)lane].LasetSet(_enemyBattleLogic.GetBlobForce(lane), _playerBattleLogic.GetBlobForce(lane));
        _laserBeamList[(int)lane].SetLaserColors(_enemyBattleLogic.GetBlobStartColor(lane), _playerBattleLogic.GetBlobStartColor(lane));
    }

    public void EnemyDeath(LaserLinesEnum lane)
    {
        //_enemyBattleLogic.BlobDeath(lane);
    }
}
