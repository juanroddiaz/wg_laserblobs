using UnityEngine;
using UnityEngine.EventSystems;

public class BattleGroundPivotLogic : MonoBehaviour
{
    public BattleGroundPivotLogic(BattleGroundPivotLogic logic)
    {
        _blobLogic = logic._blobLogic;
        _laserLaneType = logic._laserLaneType;
        _bgLogic = logic._bgLogic;
    }

    [SerializeField]
    private BlobInstanceLogic _blobLogic;
    public BlobInstanceLogic BlobLogic
    {
        get { return _blobLogic; }
    }

    private LaserLinesEnum _laserLaneType;
    private BattleGroundLogic _bgLogic;

    public void Init(BattleGroundLogic bgLogic, LaserLinesEnum lane)
    {
        _laserLaneType = lane;
        _bgLogic = bgLogic;
    }

    public float GetBlobForce()
    {
        return _blobLogic.CurrentBlobLaserForce;
    }
}