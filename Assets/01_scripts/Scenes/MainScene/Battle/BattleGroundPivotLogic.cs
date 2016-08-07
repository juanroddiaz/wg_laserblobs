using UnityEngine;
using System.Collections;

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
    private Animator _blobAnimator;

    private BlobTypes _type = BlobTypes.MAX;
    public BlobTypes Type
    {
        get { return _type; }
    }

    private BlobAnimType _teamAnimType = BlobAnimType.Max;
    private BlobAnimations _currentAnim = BlobAnimations.Max;

    void Awake()
    {
        _blobAnimator = GetComponent<Animator>();
    }

    public void Init(BattleGroundLogic bgLogic, LaserLinesEnum lane, BlobTypes type)
    {
        _laserLaneType = lane;
        _bgLogic = bgLogic;
        _type = type;
        _teamAnimType = bgLogic.Type == BattleGroundType.PLAYER ? BlobAnimType._B : BlobAnimType._F;
        _currentAnim = BlobAnimations.Idle;
        // if not dead blob case
        if (type != BlobTypes.MAX)
        {
            _blobLogic.Init(bgLogic.BlobLaserForce, bgLogic.BlobHoldLaserMultiplier);
            if (_bgLogic.Type == BattleGroundType.ENEMY)
            {
                SetAsReserve();
            }
            return;
        }
    }

    public float GetBlobForce()
    {
        return _blobLogic.CurrentBlobLaserForce;
    }

    #region Blob States
    public void SetAsReserve()
    {
        _blobDragLogic.gameObject.SetActive(false);
        _currentAnim = BlobAnimations.Idle;
        _blobAnimator.SetTrigger(_currentAnim.ToString() + BlobAnimType._F.ToString());
    }

    private void SetAnimation()
    {
        _blobAnimator.SetTrigger(_currentAnim.ToString() + _teamAnimType.ToString());
    }

    public void StartLaserFire()
    {
        _currentAnim = BlobAnimations.FireIn;
        SetAnimation();
    }

    public void DeathEvent()
    {
        _currentAnim = BlobAnimations.Death;
        SetAnimation();
    }

    public void SqueezeIn()
    {
        _currentAnim = BlobAnimations.SqueezeIn;
        SetAnimation();
    }

    public void SqueezeOut()
    {
        _currentAnim = BlobAnimations.SqueezeIn;
        SetAnimation();
    }

    public void JumpToBattleground()
    {
        _currentAnim = BlobAnimations.Jump;
        SetAnimation();
    }
    #endregion
}