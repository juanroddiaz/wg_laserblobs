using UnityEngine;

public class BattleGroundPivotLogic : MonoBehaviour
{
    [SerializeField]
    private BlobInstanceLogic _blobLogic;

    private bool _isHeld = false;
    public bool IsHeld
    {
        get { return _isHeld; }
    }

    public void OnHoldStart()
    {
        CustomLog.Log("OnHoldStart");
        _isHeld = true;
        _blobLogic.ToggleOnHoldState(true);
    }

    public void OnHoldEnd()
    {
        if (_isHeld)
        {
            CustomLog.Log("OnHoldEnd");
            _blobLogic.ToggleOnHoldState(false);
            _isHeld = false;
        }        
    }

    public float GetBlobForce()
    {
        return _blobLogic.CurrentBlobLaserForce;
    }
}