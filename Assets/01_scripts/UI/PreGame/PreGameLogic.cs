using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class PreGameLogic : MonoBehaviour
{
    [SerializeField]
    private GameObject _pregameContentObject;

    [SerializeField]
    private Button _playButton;

    [SerializeField]
    private List<Toggle> _difficultyToggleList = new List<Toggle>();

    private Dictionary<int, string> _blobSelectionMap = new Dictionary<int, string>();
    private GameDifficulty _selectedDifficulty;

    private MainSceneController _sceneController;

    public void Init(MainSceneController controller)
    {
        _sceneController = controller;
        _pregameContentObject.SetActive(false);
        _selectedDifficulty = GameDifficulty.Medium;
        _difficultyToggleList[(int)_selectedDifficulty].interactable = false;
    }

    public void Show()
    {
        _pregameContentObject.SetActive(true);
    }

    public void OnToggleDifficulty(int difficultyIdx)
    {
        GameDifficulty diff = (GameDifficulty)difficultyIdx;
        if (_selectedDifficulty != diff)
        {
            _difficultyToggleList[(int)_selectedDifficulty].interactable = true;
            _selectedDifficulty = diff;
            _difficultyToggleList[difficultyIdx].interactable = false;
            CustomLog.Log("Selected difficulty: " + _selectedDifficulty.ToString());
            return;
        }        
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
        _sceneController.StartGame(_blobSelectionMap.Values.ToList());
    }
}
