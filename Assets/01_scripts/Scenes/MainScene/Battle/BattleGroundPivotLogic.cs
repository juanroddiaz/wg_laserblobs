using UnityEngine;
using UnityEngine.EventSystems;

public class BattleGroundPivotLogic : MonoBehaviour
{
    private bool _isHeld = false;

    public void OnHoldStart()
    {
        CustomLog.Log("OnHoldStart");
        _isHeld = true;
    }

    public void OnHoldEnd()
    {
        if (_isHeld)
        {
            CustomLog.Log("OnHoldEnd");
            _isHeld = false;
        }        
    }

    public void OnHold()
    {
        CustomLog.Log("OnHold");
    }
}