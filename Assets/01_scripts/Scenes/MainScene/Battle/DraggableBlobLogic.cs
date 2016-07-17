using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableBlobLogic : MonoBehaviour
{
    private LaserLinesEnum _lane;
    private DraggablePanelBattleLogic _panelLogic;

    public void Init(LaserLinesEnum lane, DraggablePanelBattleLogic panelLogic)
    {
        _lane = lane;
        _panelLogic = panelLogic;
    }

    #region Event triggers
    public void OnBeginDrag(BaseEventData data)
    {
        CustomLog.Log("OnBeginDrag: " + _lane.ToString());
    }

    public void OnDrag(BaseEventData data)
    {
        CustomLog.Log("OnDrag: " + _lane.ToString());
    }

    public void OnEndDrag(BaseEventData data)
    {
        CustomLog.Log("OnEndDrag: " + _lane.ToString());
    }
    #endregion
}
