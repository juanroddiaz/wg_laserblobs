using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HudUiLogic : MonoBehaviour
{
    [SerializeField]
    private PauseLogic _pauseLogic;
    [SerializeField]
    private GameOverLogic _gameOverLogic;

    [SerializeField]
    private Text _scoreLabel;
    [SerializeField]
    private Text _waveNumberLabel;

    private MainSceneController _sceneController;

    public void Init(MainSceneController controller)
    {
        _sceneController = controller;
        _pauseLogic.Init(controller);
        _gameOverLogic.Init(controller);
        ResetGameData();
    }

    public void ResetGameData()
    {
        _scoreLabel.text = _sceneController.Score.ToString();
        _waveNumberLabel.text = _sceneController.Wave.ToString();
    }

    public void OnPauseButton()
    {
        _pauseLogic.ToggleScreen(!_sceneController.IsPaused);
        _sceneController.TogglePauseGame();
    }

    public void OnGameOver()
    {
        _gameOverLogic.ShowScreen();
        _sceneController.TogglePauseGame();
    }

    public void UpdateScore()
    {
        _scoreLabel.text = _sceneController.Score.ToString();
    }

    public void UpdateWave()
    {
        _waveNumberLabel.text = _sceneController.Wave.ToString();
    }
}
