using UnityEngine;
using System.Collections;

public class HudUiLogic : MonoBehaviour
{
    [SerializeField]
    private PauseLogic _pauseLogic;

    private MainSceneController _sceneController;

    public void Init(MainSceneController controller)
    {
        _sceneController = controller;
        _pauseLogic.Init(controller);
    }

    public void OnPauseButton()
    {
        _pauseLogic.ToggleScreen(!_sceneController.IsPaused);
        _sceneController.TogglePauseGame();
    }
}
