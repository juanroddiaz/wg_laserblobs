using UnityEngine;
using UnityEngine.UI;

public class GameOverLogic : MonoBehaviour
{
    [SerializeField]
    private Text _violenceLevelLabel;
    [SerializeField]
    private Text _violenceChartLabel;

    private MainSceneController _sceneController;

    public void Init(MainSceneController sceneController)
    {
        _sceneController = sceneController;
    }

    public void ShowScreen()
    {
        gameObject.SetActive(true);
        _violenceLevelLabel.text = _sceneController.Score.ToString();
        _violenceChartLabel.text = "6669th";
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
