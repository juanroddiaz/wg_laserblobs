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
    private float _blobHoldLaserMultiplier = 0.0f;
    private float _currentDamageMult = 1.0f;

    public void Init(float laserForce, float holdLaserMulti)
    {
        _blobLaserForce = laserForce;
        _blobHoldLaserMultiplier = holdLaserMulti;
        _currentBlobLaserForce = _blobLaserForce;
    }

    public void ToggleOnHoldState(bool toggle)
    {
        _currentBlobLaserForce = toggle ? _blobLaserForce * _blobHoldLaserMultiplier : _blobLaserForce * 1.0f;
        _currentBlobLaserForce *= _currentDamageMult;
    }

    public void UpdateLaserForce(float force)
    {
        _currentDamageMult = (force/100.0f);
        _currentBlobLaserForce = _blobLaserForce;
        _currentBlobLaserForce *= _currentDamageMult;
    }
}