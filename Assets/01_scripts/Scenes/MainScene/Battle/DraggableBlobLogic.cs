using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableBlobLogic : MonoBehaviour
{
    [SerializeField]
    private Image _draggableImage;

    private LaserLinesEnum _lane;
    private BattleGroundLogic _panelLogic;
    private Vector2 _initialPosition;
    private RectTransform _rectTrans;
	private float _blobCustomSize;
    private Color _blobColor;

    private bool _isHeld = false;
    public bool IsHeld
    {
        get { return _isHeld; }
    }

    private bool _isBlocked = false;
    private float _scalerFactorForHeight = 0.0f;
    private Vector2 _deltaPos = new Vector2();

    public void Init(LaserLinesEnum lane, BattleGroundLogic panelLogic, Color blobColor)
    {
        _lane = lane;
        _panelLogic = panelLogic;
        _blobColor = blobColor;
        _rectTrans = _draggableImage.GetComponent<RectTransform>();
        _initialPosition = _rectTrans.anchoredPosition;
		_blobCustomSize = panelLogic.BlobCustomSize;
        _scalerFactorForHeight = _panelLogic.ScenarioLogic.CanvasScaler.referenceResolution.y / Screen.height;
    }

    public void UpdateLane(LaserLinesEnum lane)
    {
        _lane = lane;
    }

    public void BlockDragLogic()
    {
        _isBlocked = true;
    }

    #region Event triggers
    public void OnHoldStart()
    {
        if (_isBlocked)
        {
            return;
        }
        CustomLog.Log("OnHoldStart: " + _lane.ToString());
        _isHeld = true;
        _panelLogic.ToggleOnHoldState(_lane, true);
    }

    public void OnHoldEnd()
    {
        if (_isHeld)
        {
            //CustomLog.Log("OnHoldEnd");
            _panelLogic.ToggleOnHoldState(_lane, false);
            _isHeld = false;
        }
    }

    public void OnBeginDrag(BaseEventData data)
    {
        if (_isBlocked)
        {
            return;
        }

        CustomLog.Log("OnBeginDrag: lane " + _lane.ToString());
    }

    public void OnDrag(BaseEventData data)
    {
        if (_isBlocked)
        {
            return;
        }

        //CustomLog.Log("OnDrag: " + _lane.ToString());
        PointerEventData pointerData = data as PointerEventData;
        _deltaPos.x = pointerData.delta.x * _scalerFactorForHeight;
        _deltaPos.y = pointerData.delta.y * _scalerFactorForHeight;
        _rectTrans.anchoredPosition += (_deltaPos / _blobCustomSize);
        
        if (_isHeld)
        {
            //CustomLog.Log((_initialPosition - _rectTrans.anchoredPosition).magnitude.ToString());
            if ((_initialPosition - _rectTrans.anchoredPosition).magnitude > _panelLogic.DragStartThreshold)
            {
                _panelLogic.ToggleOnHoldState(_lane, false);
                _isHeld = false;
                CustomLog.Log("Hold state cancelled!");

                _blobColor.a = _panelLogic.DragBlobAlpha;
                _draggableImage.color = _blobColor;
            }            
        }        
    }

    public void OnEndDrag(BaseEventData data)
    {
        if (_isBlocked)
        {
            return;
        }

        CustomLog.Log("OnEndDrag: " + _lane.ToString());
        _blobColor.a = 0.0f;
        _draggableImage.color = _blobColor;

        // check end drag position
        Vector3 lastAnchoredPos = _rectTrans.position;
        _rectTrans.anchoredPosition = _initialPosition;
        _panelLogic.CheckBlobSwapping(_lane, lastAnchoredPos);
    }
    #endregion
}
