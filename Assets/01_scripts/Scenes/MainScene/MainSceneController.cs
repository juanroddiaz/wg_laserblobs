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

    [SerializeField]
    private AudioSource _audioSource;

    [Header("Debug Stuff!")]
    [SerializeField]
    private List<AudioClip> _debugClips = new List<AudioClip>();

    private bool _isPaused = true;
    public bool IsPaused
    {
        get { return _isPaused; }
    }

    private int _wave = 1;
    public int Wave
    {
        get { return _wave; }
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

    public void UpdateWave()
    {
        _wave += 1;
        _hudLogic.UpdateWave();
    }

    public void UpdateScore()
    {
        _score += 1;
        _hudLogic.UpdateScore();
    }

    public List<BlobTypes> GetInitialBlobs()
    {
        return _scenarioLogic.GetInitialBlobs();
    }

    #region UI logic
    public void DisplayPreGameMenu(int debugMusic)
    {
        _audioSource.clip = _debugClips[debugMusic];
        _audioSource.Play();
        _preGameLogic.Show();
    }

    public void OnBackToMainMenu(bool onlyMain)
    {
        _audioSource.Stop();
        _mainMenuLogic.Show(onlyMain);
    }

    public void StartGame()
    {
        _isPaused = false;
        _wave = 1;
        _score = 0;
        _hudLogic.ResetGameData();
        _scenarioLogic.StartGame();
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
        TogglePauseGame();
        _wave = 1;
        _score = 0;
        _hudLogic.ResetGameData();
        _scenarioLogic.StartGame();
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
