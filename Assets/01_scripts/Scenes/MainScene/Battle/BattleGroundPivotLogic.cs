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

    private bool _isHeld = false;
    public bool IsHeld
    {
        get { return _isHeld; }
    }

    private LaserLinesEnum _laserLaneType;
    private BattleGroundLogic _bgLogic;
    private float _dragThreshold = 0.0f;

    public void Init(BattleGroundLogic bgLogic, LaserLinesEnum lane)
    {
        _laserLaneType = lane;
        _bgLogic = bgLogic;
        _dragThreshold = bgLogic.DragThreshold;
    }

    #region Event triggers
    public void OnHoldStart()
    {
        CustomLog.Log("OnHoldStart: " + _laserLaneType.ToString());
        _isHeld = true;
        _blobLogic.ToggleOnHoldState(true);
    }

    public void OnHoldEnd()
    {
        if (_isHeld)
        {
            //CustomLog.Log("OnHoldEnd");
            _blobLogic.ToggleOnHoldState(false);
            _isHeld = false;
        }        
    }

    public void OnBeginDrag(BaseEventData data)
    {
        PointerEventData pointerData = data as PointerEventData;
        if (Mathf.Abs(pointerData.delta.x) >= _dragThreshold)
        {
            _blobLogic.ToggleOnHoldState(false);
            _isHeld = false;
            CustomLog.Log("OnBeginDrag: lane " + _laserLaneType.ToString() + ", direction? " + Mathf.Sign(pointerData.delta.x));
            _bgLogic.BlogSwipeEvent(_laserLaneType, Mathf.Sign(pointerData.delta.x));
        }
    }
    #endregion

    public float GetBlobForce()
    {
        return _blobLogic.CurrentBlobLaserForce;
    }

    public void UpdateLane(LaserLinesEnum lane)
    {
        _laserLaneType = lane;
    }
}