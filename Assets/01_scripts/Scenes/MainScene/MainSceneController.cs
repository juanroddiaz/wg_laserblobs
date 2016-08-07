using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainSceneModel : SceneModel
{
    public MainSceneModel()
    {
        sceneIndex = SceneIndexEnum.MainScene;
    }
}

public class MainSceneController : SceneController
{
    private MainSceneModel _mainSceneModel = new MainSceneModel();

    [SerializeField]
    private MainScenarioLogic _scenarioLogic;
    public MainScenarioLogic ScenarioLogic
    {
        get { return _scenarioLogic; }
    }

    [SerializeField]
    private MainMenuLogic _mainMenuLogic;

    [SerializeField]
    private PreGameLogic _preGameLogic;

    [SerializeField]
    private HudUiLogic _hudLogic;

    private bool _isPaused = true;
    public bool IsPaused
    {
        get { return _isPaused; }
    }

    private List<BlobTypes> _lastBlobSelection;

    private int _multiplier = 1;
    public int Multiplier
    {
        get { return _multiplier; }
    }

    private int _score = 0;
    public int Score
    {
        get { return _score; }
    }

    public override IEnumerator InitSequence(SceneModel model)
    {
        yield return null;
        _sceneModel = model;
        _mainSceneModel = model as MainSceneModel;
        _scenarioLogic.Init(this);
        _mainMenuLogic.Init(this);
        _preGameLogic.Init(this);
        _hudLogic.Init(this);
        yield break;
    }

    public override void Update()
    {
        if (!_isPaused)
        {
            _scenarioLogic.UpdateLogic();
        }
        base.Update();
    }

    public void UpdateMultiplier(int m)
    {
        _multiplier = m;
        _hudLogic.UpdateMultiplier();
    }

    public void UpdateScore()
    {
        _score += _multiplier;
        _hudLogic.UpdateScore();
    }

    #region UI logic
    public void DisplayPreGameMenu()
    {
        _preGameLogic.Show();
    }

    public void OnBackToMainMenu(bool onlyMain)
    {
        _mainMenuLogic.Show(onlyMain);
    }

    public void StartGame(List<BlobTypes> _blobSelection)
    {
        _isPaused = false;
        _multiplier = 1;
        _score = 0;
        // making a copy of original selection for restart game logic
        _lastBlobSelection = new List<BlobTypes>(_blobSelection);
        _scenarioLogic.StartGame(_blobSelection);
    }

    public void TogglePauseGame()
    {
        _isPaused = !_isPaused;
    }

    public void ShowGameOver()
    {
        _hudLogic.OnGameOver();
    }

    public void RestartGame()
    {
        _scenarioLogic.EndGame();
        string blobLog = "Restart game's Selected blobs: ";
        foreach (BlobTypes s in _lastBlobSelection)
        {
            blobLog += (s.ToString() + " ");
        }
        CustomLog.Log(blobLog);
        TogglePauseGame();
        _scenarioLogic.StartGame(new List<BlobTypes>(_lastBlobSelection));
    }

    public void EndGame(bool goToMainMenu)
    {
        _scenarioLogic.EndGame();
        if (goToMainMenu)
        {
            _mainMenuLogic.Show();
            return;
        }

        _preGameLogic.Show();
    }
    #endregion
}
