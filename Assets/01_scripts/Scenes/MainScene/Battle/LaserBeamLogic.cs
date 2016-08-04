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
    private Transform _collisionGlowTransform;

    private ParticleSystemRenderer _playerLaserRenderer;
    private ParticleSystemRenderer _enemyLaserRenderer;

    private float _initialLaserValue = 0.5f;
    private float _currentPlayerLaserValue = 0.0f;
    private float _currentPlayerLaserForceFactor = 0.0f;
    private float _totalLaserValue = 0.0f;
    private float _enemyTipPos = 0.0f;
    private float _totalDistanceTipPos = 0.0f;
    private Vector3 _collisionPosition;

    private LaserLinesEnum _lane;
    private LaserBeamGroupLogic _laserBeamLogic;

    public void Init(float lenght, float enemyTipPos, float playerTipPos, LaserLinesEnum lane, LaserBeamGroupLogic laserLogic)
    {
        _totalLaserValue = -1.0f * lenght;
        _playerLaserRenderer = _playerLaser.GetComponent<ParticleSystemRenderer>();
        _enemyLaserRenderer = _enemyLaser.GetComponent<ParticleSystemRenderer>();
        _playerLaserRenderer.lengthScale = 0.5f * lenght;
        _enemyLaserRenderer.lengthScale = 0.5f * lenght;

        _enemyTipPos = enemyTipPos;
        _totalDistanceTipPos = enemyTipPos - playerTipPos;
        _collisionPosition = _collisionGlowTransform.localPosition;
        _lane = lane;
        _laserBeamLogic = laserLogic;
    }

    public void LasetSet(float enemyForce, float playerForce)
    {
        // TODO: get the right values from DM
        _currentPlayerLaserValue = _initialLaserValue;
        _currentPlayerLaserForceFactor = enemyForce - playerForce;        
        SetCollisionObjectPosition();
    }

    // TODO: get the enemy and player type too, it will have an impact in laser force
    public void UpdateLaserLane(float enemyForce, float playerForce)
    {
        _currentPlayerLaserValue -= _currentPlayerLaserForceFactor;
        _currentPlayerLaserValue = Mathf.Max(0.0f, _currentPlayerLaserValue);
        _currentPlayerLaserValue = Mathf.Min(_currentPlayerLaserValue, 1.0f);

        float playerLife = _currentPlayerLaserValue * _totalLaserValue;
        _playerLaserRenderer.lengthScale = playerLife;
        float enemyLife = _totalLaserValue * (1.0f - _currentPlayerLaserValue);
        _enemyLaserRenderer.lengthScale = enemyLife;
        SetCollisionObjectPosition();
        _currentPlayerLaserForceFactor = enemyForce - playerForce;

        if (playerLife == 0.0f)
        {
            _laserBeamLogic.PlayerBlobDeath(_lane);
            return;
        }
        if (enemyLife == 0.0f)
        {
            _laserBeamLogic.EnemyBlobDeath(_lane);
        }
    }

    private void SetCollisionObjectPosition()
    {
        float pos = _enemyTipPos - (_totalDistanceTipPos) * (1.0f - _currentPlayerLaserValue);
        _collisionPosition.x = pos;
        _collisionGlowTransform.localPosition = _collisionPosition;
    }

    public void UpdatePlayerLaserColor(Color playerColor)
    {
        playerColor.a = _playerLaser.startColor.a;
        _playerLaser.startColor = playerColor;
    }

    public void UpdateEnemyLaserColor(Color enemyColor)
    {
        enemyColor.a = _enemyLaser.startColor.a;
        _enemyLaser.startColor = enemyColor;
        enemyColor.a = _enemyStarGlow.startColor.a;
        _enemyStarGlow.startColor = enemyColor;
    }
}
