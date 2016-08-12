using UnityEngine;
using System.Collections;

public class BlobInstanceLogic : MonoBehaviour
{
    private float _currentBlobLaserForce = 0.0f;
    public float CurrentBlobLaserForce
    {
        get { return _currentBlobLaserForce; }
    }

    private float _blobLaserForce = 0.0f;
    private float _blobHoldLaserOffset = 0.0f;
    private float _currentDamageMult = 1.0f;

    public void Init(float laserForce, float holdLaserMulti)
    {
        _blobLaserForce = laserForce;
        _blobHoldLaserOffset = holdLaserMulti;
        _currentBlobLaserForce = _blobLaserForce;
    }

    public void ToggleOnHoldState(bool toggle)
    {
        _currentBlobLaserForce = toggle ? _blobLaserForce + _blobHoldLaserOffset : _blobLaserForce * 1.0f;
        _currentBlobLaserForce *= _currentDamageMult;
    }

    public void UpdateLaserForce(float force)
    {
        _currentDamageMult = (force/100.0f);
        _currentBlobLaserForce = _blobLaserForce;
        _currentBlobLaserForce *= _currentDamageMult;
    }

    // DEBUG
    public void IncreaseBaseLaserForce(float inc)
    {
        _blobLaserForce += inc;
        _currentBlobLaserForce = _blobLaserForce;
    }
}