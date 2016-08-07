using UnityEngine;

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

    [SerializeField]
    private Color _blobBaseColor;
    public Color BlobBaseColor
    {
        get { return _blobBaseColor; }
    }

    [SerializeField]
    private DraggableBlobLogic _blobDragLogic;
    public DraggableBlobLogic BlobDragLogic
    {
        get { return _blobDragLogic; }
    }

    private LaserLinesEnum _laserLaneType;
    private BattleGroundLogic _bgLogic;
    private BlobTypes _type;

    public void Init(BattleGroundLogic bgLogic, LaserLinesEnum lane, BlobTypes type)
    {
        _laserLaneType = lane;
        _bgLogic = bgLogic;
        _type = type;
        // dead blob case
        if (type != BlobTypes.MAX)
        {
            _blobLogic.Init(bgLogic.BlobLaserForce, bgLogic.BlobHoldLaserMultiplier);
            if (_bgLogic.Type == BattleGroundType.ENEMY)
            {
                SetAsReserve();
            }
        }        
    }

    public float GetBlobForce()
    {
        return _blobLogic.CurrentBlobLaserForce;
    }

    public void SetAsReserve()
    {
        _blobDragLogic.gameObject.SetActive(false);
    }
}