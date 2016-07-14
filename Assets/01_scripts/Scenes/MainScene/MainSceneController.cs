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
    private GameUIPanelLogic _gameUiLogic;

    [SerializeField]
    private MenuUIPanelLogic _menuUiLogic;

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
        _gameUiLogic.Init();
        _menuUiLogic.Init();
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
}
