using UnityEngine;
using System.Collections;

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

    public override IEnumerator InitSequence(SceneModel model)
    {
        _sceneModel = model;
        _mainSceneModel = model as MainSceneModel;
        _scenarioLogic.Init();
        _mainMenuLogic.Init(this);
        _preGameLogic.Init(this);
        _hudLogic.Init();
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

    public void StartGame()
    {
        _isPaused = false;
    }
    #endregion
}
