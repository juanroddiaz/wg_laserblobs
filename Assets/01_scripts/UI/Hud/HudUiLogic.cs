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
    private Text _multiplierLabel;

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
        _multiplierLabel.text = "x" + _sceneController.Multiplier.ToString();
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

    public void UpdateMultiplier()
    {
        _multiplierLabel.text = "x" + _sceneController.Multiplier.ToString();
    }
}
