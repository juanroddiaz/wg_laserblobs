using UnityEngine;

public class BattleGroundPivotLogic : MonoBehaviour
{
    private bool _isHeld = false;
    public bool IsHeld
    {
        get { return _isHeld; }
    }

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
}