using UnityEngine;
using UnityEngine.EventSystems;

public class BattleGroundPivotLogic : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        CustomLog.Log("OnPointDown!");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CustomLog.Log("OnPointUp!");
    }
}