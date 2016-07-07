using UnityEngine;
using UnityEngine.UI;

public class LaserBeamLogic : MonoBehaviour
{
    [SerializeField]
    private Image _enemyLaserImage;
    [SerializeField]
    private Image _playerLaserImage;

    private float _initialLaserValue = 0.5f;
    private float _currentPlayerLaserValue = 0.0f;
    private float _currentPlayerLaserForceFactor = 0.0f;

    public void Init(float enemyForce, float playerForce)
    {
        // TODO: get the right values from DM
        _currentPlayerLaserValue = _initialLaserValue;
        _playerLaserImage.fillAmount = _initialLaserValue;
        _enemyLaserImage.fillAmount = 1.0f - _initialLaserValue;

        _currentPlayerLaserForceFactor = enemyForce - playerForce;
    }

    // TODO: get the enemy and player type too, it will have an impact in laser force
    public void UpdateLaserLane(float enemyForce, float playerForce)
    {
        _currentPlayerLaserValue -= _currentPlayerLaserForceFactor;
        _currentPlayerLaserValue = Mathf.Max(0.0f, _currentPlayerLaserValue);
        _currentPlayerLaserValue = Mathf.Min(_currentPlayerLaserValue, 1.0f);
        _playerLaserImage.fillAmount = _currentPlayerLaserValue;
        _enemyLaserImage.fillAmount = 1.0f - _currentPlayerLaserValue;
        _currentPlayerLaserForceFactor = enemyForce - playerForce;
    }
}
