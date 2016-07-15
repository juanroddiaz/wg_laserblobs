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
    private MainSceneModel _mainSceneModel;

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

    private List<string> _lastBlobSelection = new List<string>();

    public override IEnumerator InitSequence(SceneModel model)
    {
        _sceneModel = model;
        _mainSceneModel = model as MainSceneModel;
        _scenarioLogic.Init();
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

    #region UI logic
    public void DisplayPreGameMenu()
    {
        _preGameLogic.Show();
    }

    public void OnBackToMainMenu(bool onlyMain)
    {
        _mainMenuLogic.Show(onlyMain);
    }

    public void StartGame(List<string> _blobSelection)
    {
        _isPaused = false;
        _lastBlobSelection = _blobSelection;
        _scenarioLogic.StartGame(_blobSelection);
    }

    public void TogglePauseGame()
    {
        _isPaused = !_isPaused;
    }

    public void RestartGame()
    {
        string blobLog = "Restart game's Selected blobs: ";
        foreach (string s in _lastBlobSelection)
        {
            blobLog += (s + " ");
        }
        CustomLog.Log(blobLog);
        TogglePauseGame();
        _scenarioLogic.StartGame(_lastBlobSelection);
    }

    public void EndGame(bool goToMainMenu)
    {
        if (goToMainMenu)
        {
            _mainMenuLogic.Show();
            return;
        }

        _preGameLogic.Show();
    }
    #endregion
}
