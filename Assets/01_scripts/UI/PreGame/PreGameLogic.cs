using UnityEngine;
using System.Collections;

public class PreGameLogic : MonoBehaviour
{
    [SerializeField]
    private GameObject _pregameContentObject;



    private MainSceneController _sceneController;

    public void Init(MainSceneController controller)
    {
        _sceneController = controller;
        _pregameContentObject.SetActive(false);
    }

    public void Show()
    {
        _pregameContentObject.SetActive(true);
    }

    public void OnToggleDifficulty(int difficultyIdx)
    {

    }

    public void OnToggleBlobs(int blobIdx)
    {

    }

    public void OnBackButton()
    {
        _pregameContentObject.SetActive(false);
        // show whole Play Menu (TBD)
        _sceneController.OnBackToMainMenu(true);
    }

    public void OnStartButton()
    {
        _pregameContentObject.SetActive(false);
        _sceneController.StartGame();
    }
}
