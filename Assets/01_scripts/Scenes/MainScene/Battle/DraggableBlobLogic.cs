﻿using UnityEngine;
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

    public void Init(LaserLinesEnum lane, BattleGroundLogic panelLogic, Color blobColor)
    {
        _lane = lane;
        _panelLogic = panelLogic;
        _blobColor = blobColor;
        _rectTrans = _draggableImage.GetComponent<RectTransform>();
        _initialPosition = _rectTrans.anchoredPosition;
		_blobCustomSize = panelLogic.BlobCustomSize;
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
        _blobColor.a = _panelLogic.DragBlobAlpha;
        _draggableImage.color = _blobColor;
    }

    public void OnDrag(BaseEventData data)
    {
        //CustomLog.Log("OnDrag: " + _lane.ToString());
        PointerEventData pointerData = data as PointerEventData;
		_rectTrans.anchoredPosition += (pointerData.delta / _blobCustomSize);
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
        _blobColor.a = 0.0f;
        _draggableImage.color = _blobColor;

        // check end drag position
        Vector3 lastAnchoredPos = _rectTrans.position;
        _rectTrans.anchoredPosition = _initialPosition;
        _panelLogic.CheckBlobSwapping(_lane, lastAnchoredPos);
    }
    #endregion
}
