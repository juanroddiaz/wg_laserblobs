using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableBlobLogic : MonoBehaviour
{
    [SerializeField]
    private Image _draggableImage;

    private LaserLinesEnum _lane;
    private BattleGroundLogic _panelLogic;
    private Vector3 _initialPosition;

    public void Init(LaserLinesEnum lane, BattleGroundLogic panelLogic)
    {
        _lane = lane;
        _panelLogic = panelLogic;
        _initialPosition = transform.position;
    }

    #region Event triggers
    public void OnBeginDrag(BaseEventData data)
    {
        CustomLog.Log("OnBeginDrag: " + _lane.ToString());
    }

    public void StartDrag()
    {
        _draggableImage.raycastTarget = true;
    }

    public void OnDrag(BaseEventData data)
    {
        CustomLog.Log("OnDrag: " + _lane.ToString());
        PointerEventData pointerData = data as PointerEventData;
        GetComponent<RectTransform>().anchoredPosition += pointerData.delta;
    }

    public void OnEndDrag(BaseEventData data)
    {
        CustomLog.Log("OnEndDrag: " + _lane.ToString());
        transform.position = _initialPosition;
        _draggableImage.raycastTarget = false;
    }
    #endregion
}
