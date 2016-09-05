using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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

    private List<BlobTypes> _sortedBlobSelection = new List<BlobTypes>();

    private GameDifficulty _selectedDifficulty;

    private MainSceneController _sceneController;

    public void Init(MainSceneController controller)
    {
        _sceneController = controller;
        _pregameContentObject.SetActive(false);
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

        _sortedBlobSelection = _sceneController.GetInitialBlobs();
        // initialize prebattle screen logic setting the difficulty to its default
        _selectedDifficulty = GameDifficulty.Medium;
        _playButton.enabled = true;
        _pregameContentObject.SetActive(true);
    }

    public void OnToggleDifficulty(int difficultyIdx)
    {
        //if (_selectedDifficulty == GameDifficulty.Max)
        //{
        //    // uninitialized logic
        //    return;
        //}

        //GameDifficulty diff = (GameDifficulty)difficultyIdx;
        //if (_selectedDifficulty != diff)
        //{
        //    _difficultyToggleList[(int)_selectedDifficulty].interactable = true;
        //    _selectedDifficulty = diff;
        //    _difficultyToggleList[difficultyIdx].interactable = false;
        //    CustomLog.Log("Selected difficulty: " + _selectedDifficulty.ToString());
        //}
    }

    public void OnToggleBlobs(int blobIdx)
    {
        //if (_selectedDifficulty == GameDifficulty.Max)
        //{
        //    // uninitialized logic
        //    return;
        //}

        //if (_blobsToggleList[blobIdx].isOn)
        //{
        //    _sortedBlobSelection.Add((BlobTypes)blobIdx);
        //}
        //else
        //{
        //    _sortedBlobSelection.Remove((BlobTypes)blobIdx);
        //}

        //// TODO: difficluty dependant blob quantity limit
        //_playButton.enabled = _sortedBlobSelection.Count >= 3;
    }

    public void OnBackButton()
    {
        _pregameContentObject.SetActive(false);
        // show whole Play Menu (TBD)
        _sceneController.OnBackToMainMenu(true);
    }

    public void OnStartButton()
    {   
        string blobLog = "Selected blobs: ";
        foreach (BlobTypes s in _sortedBlobSelection)
        {
            blobLog += (s.ToString() + " ");
        }
        CustomLog.Log(blobLog);

        _pregameContentObject.SetActive(false);
        _sceneController.StartGame();
    }
}