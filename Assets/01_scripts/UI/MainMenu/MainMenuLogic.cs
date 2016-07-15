using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuLogic : MonoBehaviour
{
    [SerializeField]
    private GameObject _mainContentObject;

    [SerializeField]
    private GameObject _logInContentObject;

    [SerializeField]
    private Button _leaderboardButton;
    [SerializeField]
    private Button _facebookButton;
    [SerializeField]
    private Button _guestButton;

    private MainSceneController _sceneController;

    public void Init(MainSceneController controller)
    {
        _sceneController = controller;
        Show();
    }

    public void Show()
    {
        _mainContentObject.SetActive(true);
        _logInContentObject.SetActive(false);
    }

    public void OnPlayButton()
    {
        _logInContentObject.SetActive(true);
    }

    public void OnGuestButton()
    {
        _mainContentObject.SetActive(false);
        _logInContentObject.SetActive(false);
        _sceneController.DisplayPreGameMenu();
    }
}
