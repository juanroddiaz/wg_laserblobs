using UnityEngine;
using System.Collections;

public class BlobInstanceLogic : MonoBehaviour
{
    [SerializeField]
    private float _blobLaserForce = 0.00125f;
    [SerializeField]
    private float _blobHoldLaserMultiplier = 3.0f;

    private float _currentBlobLaserForce = 0.0f;
    public float CurrentBlobLaserForce
    {
        get { return _currentBlobLaserForce; }
    }

    void Awake()
    {
        _currentBlobLaserForce = _blobLaserForce;
    }

    public void ToggleOnHoldState(bool toggle)
    {
        _currentBlobLaserForce = toggle ? _blobLaserForce * _blobHoldLaserMultiplier : _blobLaserForce * 1.0f;
    }
}