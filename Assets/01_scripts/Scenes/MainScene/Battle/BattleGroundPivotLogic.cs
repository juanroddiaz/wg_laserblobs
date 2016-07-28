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
    private Animator _animController;

    public void Init(BattleGroundLogic bgLogic, LaserLinesEnum lane)
    {
        _laserLaneType = lane;
        _bgLogic = bgLogic;
        _blobLogic.Init(bgLogic.BlobLaserForce, bgLogic.BlobHoldLaserMultiplier);
        _animController = GetComponent<Animator>();
    }

    public float GetBlobForce()
    {
        return _blobLogic.CurrentBlobLaserForce;
    }
}