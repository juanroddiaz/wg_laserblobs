using UnityEngine;
using System.Collections;

public class GameOverLogic : MonoBehaviour
{
    private MainSceneController _sceneController;

    public void Init(MainSceneController sceneController)
    {
        _sceneController = sceneController;
    }

    public void ShowScreen()
    {
        gameObject.SetActive(true);
    }

    public void OnRestartClick()
    {
        gameObject.SetActive(false);
        _sceneController.RestartGame();
    }

    public void OnPrebattleClick()
    {
        gameObject.SetActive(false);
        _sceneController.EndGame(false);
    }

    public void OnMainMenuClick()
    {
        gameObject.SetActive(false);
        _sceneController.EndGame(true);
    }

    public void OnLeaderboardClick()
    {
        CustomLog.Log("TODO: CREATE LEADERBOARD SCREEN AND LOGIC"); 
    }
}
