using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LaserBeamLogic : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _playerLaser;
    [SerializeField]
    private ParticleSystem _enemyLaser;
    [SerializeField]
    private ParticleSystem _enemyStarGlow;
    [SerializeField]
    private List<ParticleSystem> _collisionGlow;
    [SerializeField]
    private GameObject _collisionGlowObj;

    private ParticleSystemRenderer _playerLaserRenderer;
    private ParticleSystemRenderer _enemyLaserRenderer;

    private float _initialLaserValue = 0.5f;
    private float _currentPlayerLaserValue = 0.0f;
    private float _currentPlayerLaserForceFactor = 0.0f;
    private float _totalLaserValue = 0.0f;

    public void Init(float enemyForce, float playerForce, float lenght, float enemyTipPos, float playerTipPos)
    {
        // TODO: get the right values from DM
        _currentPlayerLaserValue = _initialLaserValue;

        _currentPlayerLaserForceFactor = enemyForce - playerForce;
        _totalLaserValue = -1.0f*lenght;
        _playerLaserRenderer = _playerLaser.GetComponent<ParticleSystemRenderer>();
        _enemyLaserRenderer = _enemyLaser.GetComponent<ParticleSystemRenderer>();
        _playerLaserRenderer.lengthScale = 0.5f * lenght;
        _enemyLaserRenderer.lengthScale = 0.5f * lenght;
    }

    // TODO: get the enemy and player type too, it will have an impact in laser force
    public void UpdateLaserLane(float enemyForce, float playerForce)
    {
        _currentPlayerLaserValue -= _currentPlayerLaserForceFactor;
        _currentPlayerLaserValue = Mathf.Max(0.0f, _currentPlayerLaserValue);
        _currentPlayerLaserValue = Mathf.Min(_currentPlayerLaserValue, 1.0f);
        _playerLaserRenderer.lengthScale = _currentPlayerLaserValue * _totalLaserValue;
        _enemyLaserRenderer.lengthScale = _totalLaserValue * (1.0f - _currentPlayerLaserValue);
        _currentPlayerLaserForceFactor = enemyForce - playerForce;
    }
}
