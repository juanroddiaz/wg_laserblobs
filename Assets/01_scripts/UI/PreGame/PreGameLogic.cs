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

    [SerializeField][Tooltip("Sorted by Easy,Medium,Hard")]
    private List<Toggle> _difficultyToggleList = new List<Toggle>();

    [SerializeField][Tooltip("From blob #1 to #9")]
    private List<Toggle> _blobsToggleList = new List<Toggle>();

    private Dictionary<int, string> _blobSelectionMap = new Dictionary<int, string>();
    private List<string> _sortedBlobSelection = new List<string>();

    private GameDifficulty _selectedDifficulty;

    private MainSceneController _sceneController;

    public void Init(MainSceneController controller)
    {
        _sceneController = controller;
        _pregameContentObject.SetActive(false);

        // TODO: blob grid data configuration
        for (int i = 0; i < _blobsToggleList.Count; i++)
        {
            _blobSelectionMap.Add(i, "blob_" + i.ToString("d2"));
        }
         
    }

    public void Show()
    {
        _selectedDifficulty = GameDifficulty.Max;
        int diffInt = (int)GameDifficulty.Medium;
        for (int i = 0; i < (int)GameDifficulty.Max; i++)
        {
            if (i == diffInt)
            {
                _difficultyToggleList[i].isOn = true;
                _difficultyToggleList[i].interactable = false;
                continue;
            }
            _difficultyToggleList[i].isOn = false;
            _difficultyToggleList[i].interactable = true;
        }

        foreach(Toggle t in _blobsToggleList)
        {
            t.isOn = false;
        }

        _sortedBlobSelection.Clear();
        // initialize prebattle screen logic setting the difficulty to its default
        _selectedDifficulty = GameDifficulty.Medium;
        _pregameContentObject.SetActive(true);
    }

    public void OnToggleDifficulty(int difficultyIdx)
    {
        if (_selectedDifficulty == GameDifficulty.Max)
        {
            // uninitialized logic
            return;
        }

        GameDifficulty diff = (GameDifficulty)difficultyIdx;
        if (_selectedDifficulty != diff)
        {
            _difficultyToggleList[(int)_selectedDifficulty].interactable = true;
            _selectedDifficulty = diff;
            _difficultyToggleList[difficultyIdx].interactable = false;
            CustomLog.Log("Selected difficulty: " + _selectedDifficulty.ToString());
        }
    }

    public void OnToggleBlobs(int blobIdx)
    {
        if (_selectedDifficulty == GameDifficulty.Max)
        {
            // uninitialized logic
            return;
        }

        if (_blobsToggleList[blobIdx].isOn)
        {
            _sortedBlobSelection.Add(_blobSelectionMap[blobIdx]);
        }
        else
        {
            _sortedBlobSelection.Remove(_blobSelectionMap[blobIdx]);
        }
    }

    public void OnBackButton()
    {
        _pregameContentObject.SetActive(false);
        // show whole Play Menu (TBD)
        _sceneController.OnBackToMainMenu(true);
    }

    public void OnStartButton()
    {
        // TODO: difficluty dependant blob quantity limit
        string blobLog = "Selected blobs: ";
        foreach (string s in _sortedBlobSelection)
        {
            blobLog += (s + " ");
        }
        CustomLog.Log(blobLog);

        _pregameContentObject.SetActive(false);
        _sceneController.StartGame(_sortedBlobSelection);
    }
}