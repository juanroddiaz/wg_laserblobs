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

    private bool _isPaused = false;
    public bool IsPaused
    {
        get { return _isPaused; }
    }

    public override IEnumerator InitSequence(SceneModel model)
    {
        _sceneModel = model;
        _mainSceneModel = model as MainSceneModel;
        _scenarioLogic.Init();
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
