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

    public void Init()
    {
        foreach (LaserBeamLogic lbl in _laserBeamList)
        {
            lbl.Init();
        }
    }

    public void UpdateLogic()
    {
        foreach (LaserBeamLogic lbl in _laserBeamList)
        {
            lbl.UpdateLaserLane();
        }

        int pressedBlobIdx = _playerBattleLogic.GetPressedBlobIndex();
        if (pressedBlobIdx >= 0 && pressedBlobIdx < (int)LaserLinesEnum.Max)
        {
            _laserBeamList[pressedBlobIdx].PlayerBlobOnHoldLogic();
        }
    }
}
