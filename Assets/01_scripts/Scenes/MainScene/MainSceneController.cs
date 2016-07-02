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

    public override IEnumerator InitSequence(SceneModel model)
    {
        _sceneModel = model;
        _mainSceneModel = model as MainSceneModel;
        yield break;
    }
}
