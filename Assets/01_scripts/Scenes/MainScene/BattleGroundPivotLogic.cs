using UnityEngine;
using UnityEngine.EventSystems;

public class BattleGroundPivotLogic : MonoBehaviour
{
    public void OnHoldStart()
    {
        CustomLog.Log("OnHoldStart");
    }

    public void OnHoldEnd()
    {
        CustomLog.Log("OnHoldEnd");
    }
}