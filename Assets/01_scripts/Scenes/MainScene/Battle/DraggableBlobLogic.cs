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

    private bool _isHeld = false;
    public bool IsHeld
    {
        get { return _isHeld; }
    }

    public void Init(LaserLinesEnum lane, BattleGroundLogic panelLogic)
    {
        _lane = lane;
        _panelLogic = panelLogic;
        _rectTrans = GetComponent<RectTransform>();
        _initialPosition = _rectTrans.anchoredPosition;
    }

    public void UpdateLane(LaserLinesEnum lane)
    {
        _lane = lane;
    }

    #region Event triggers
    public void OnHoldStart()
    {
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
        CustomLog.Log("OnBeginDrag: lane " + _lane.ToString());
        Color imgColor = new Color(1.0f, 1.0f, 1.0f, _panelLogic.DragBlobAlpha);
        _draggableImage.color = imgColor;
    }

    public void OnDrag(BaseEventData data)
    {
        //CustomLog.Log("OnDrag: " + _lane.ToString());
        PointerEventData pointerData = data as PointerEventData;
        _rectTrans.anchoredPosition += pointerData.delta;
        if (_isHeld)
        {
            if ((_initialPosition - _rectTrans.anchoredPosition).magnitude > _panelLogic.DragStartThreshold)
            {
                _panelLogic.ToggleOnHoldState(_lane, false);
                _isHeld = false;
                CustomLog.Log("Hold state cancelled!");
            }
        }        
    }

    public void OnEndDrag(BaseEventData data)
    {
        CustomLog.Log("OnEndDrag: " + _lane.ToString());
        Color imgColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        _draggableImage.color = imgColor;

        // TODO: check end drag position
        Vector2 lastAnchoredPos = _rectTrans.anchoredPosition;
        _rectTrans.anchoredPosition = _initialPosition;
        _panelLogic.CheckBlobSwapping(_lane, lastAnchoredPos);
    }
    #endregion
}
