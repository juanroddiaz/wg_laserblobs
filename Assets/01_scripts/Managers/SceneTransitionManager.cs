using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Diagnostics;

/// <summary>
/// Main enumeration for scenes indexation. Please respect the scene name shown in Build Settings.
/// </summary>
public enum SceneIndexEnum
{
	MainScene = 0,
    BootManagerScene,
    Max
}

/// <summary>
/// Main manager for scene transitions: it must be able to recognize the current scene and the last one as well to ensure back button functionality
/// It creates smooth transitions between scene loadings and it check with PopUpController if there's any active popUp in game to avoid messy screens flow.
/// </summary>
public class SceneTransitionManager : Singleton<SceneTransitionManager>
{
	private bool _isLoading = false;
	private bool _isClosingAdditiveScene = false;

	public bool IsAvailableForTransition()
	{
		return !_isLoading && !_isClosingAdditiveScene;
	}

	private SceneController _currentScene;
	public SceneController CurrentSceneController
	{
		get { return _currentScene; }
	}
	private SceneModel		_currentSceneModel;
	private SceneModel		_previousSceneModel;

	// additive scene stack to enable back button logic
	private Stack _additiveSceneStack = new Stack(0);

	protected override void onAwake()
	{
		// get the current scene who called the singleton
		SceneController controller = GameObject.FindObjectOfType<SceneController>();
		if (controller == null)
		{
			CustomLog.Log("Current Scene Controller is null! Check the main scene object!");
			return;
		}
		_currentScene = controller;		
		_previousSceneModel = null;
		StartCoroutine(WaitForGameInitialization());
	}

	private IEnumerator WaitForGameInitialization()
	{
		//while (!BootManager.Instance.IsInitializationReady)
		//{
		//	yield return null;
		//}

		CustomLog.Log("Game Initialization -> Scene name: " + _currentScene.name);
		yield return StartCoroutine(_currentScene.InitSequence(null));
		_currentSceneModel = _currentScene.SceneModel;
		//BootManager.Instance.LoadingScreenLogic.Hide();
		yield return StartCoroutine(_currentScene.OnPostLoading());
		yield break;
	}

	#region Scene Transition
	public IEnumerator AddSceneAsync(SceneIndexEnum sceneToAdd, SceneModel model, bool closeLastAdditiveScene = false, bool closeAllAdditiveScene = false)
	{
		if (_isLoading)
		{
			CustomLog.Log("Already loading a scene! Scene: " + _currentSceneModel.sceneIndex.ToString());
			yield break;
		}
		AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(sceneToAdd.ToString(), LoadSceneMode.Additive);
		yield return StartCoroutine(WaitingForAdditiveOperation(loadSceneOperation, sceneToAdd, model, closeLastAdditiveScene, closeAllAdditiveScene));
	}

	/// <summary>
	/// Main method to call a scene transition. You can create an ad-hok model for a particular scene. Don't pass a empty new SceneModel as argument!
	/// </summary>
	/// <param name="sceneToLoad"></param>
	/// <param name="model"></param>
	/// <returns></returns>
	public IEnumerator LoadSceneAsync(SceneIndexEnum sceneToLoad, SceneModel model = null)
	{
		if (_isLoading)
		{
			CustomLog.Log("Already loading a scene! Scene: " + _currentSceneModel.sceneIndex.ToString());
			yield break;
		}
	
		AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(sceneToLoad.ToString(), LoadSceneMode.Single);
		yield return StartCoroutine(WaitingForLoadingOperation(loadSceneOperation, sceneToLoad, model));
	}

	private IEnumerator WaitingForAdditiveOperation(AsyncOperation loadSceneOperation, SceneIndexEnum sceneToAdd, SceneModel model,
													bool closeLastAdditiveScene, bool closeAllAdditiveScene)
	{
		_isLoading = true;
		// Blocking InputManager's gesture logic while there's a additive scene in screen
		InputManager.Instance.DeactivateInput(true);

		while (!loadSceneOperation.isDone)
		{
			yield return null;
		}

		Scene additiveScene = SceneManager.GetSceneByName(sceneToAdd.ToString());
		if (!string.IsNullOrEmpty(additiveScene.name))
		{
			GameObject[] rootObjects = additiveScene.GetRootGameObjects();
			SceneController asc = rootObjects[0].GetComponent<SceneController>();

			if (closeAllAdditiveScene)
			{
				// checking if there's any additive scene in current scene, remove them all. Don't clean resoruces for now.
				while (IsAnyAdditiveSceneActive())
				{
					yield return StartCoroutine(CloseCurrentAdditiveSceneRoutine(false));
				}
			}
			else
			{
				if (closeLastAdditiveScene && _additiveSceneStack.Count > 0)
				{
					SceneController sc = _additiveSceneStack.Pop() as SceneController;
					Destroy(sc.gameObject);
				}
			}
			
			_additiveSceneStack.Push(asc);
			SceneManager.MergeScenes(additiveScene, SceneManager.GetActiveScene());

			yield return StartCoroutine(asc.InitSequence(model));
		}
		_isLoading = false;

		//Check if the additive scene that was just loaded has any process to do now that it's loaded
		if (IsAnyAdditiveSceneActive())
		{			
			SceneController sceneToCheck	= _additiveSceneStack.Peek() as SceneController;
			yield return StartCoroutine(sceneToCheck.OnPostLoading());
			//BootManager.Instance.LoadingScreenLogic.Hide();
		}

		yield break;
	}

	private IEnumerator WaitingForLoadingOperation(AsyncOperation loadSceneOperation, SceneIndexEnum sceneIndex, SceneModel model = null)
	{
		_isLoading = true;
		//BootManager.Instance.LoadingScreenLogic.ActivateLoadingLoop();
		InputManager.Instance.DeactivateInput();

		// Create new stopwatch and start it
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();

		// unloading last active scene
		// [JD] TODO: I've intended to use ad-hok UnloadScene, but this happened 
		// https://www.reddit.com/r/Unity3D/comments/3yuoih/i_i_think_scenemanagerunloadscene_is_destroying/
		//SceneManager.UnloadScene(SceneManager.GetActiveScene().name);

		if (_currentScene != null)
		{
			yield return StartCoroutine(_currentScene.OnDispose());
		}
		_currentScene = null;
		InputManager.Instance.ResetEvents();

		// checking if there's any additive scene in current scene, remove them all. Don't clean resoruces for now.
		while (IsAnyAdditiveSceneActive())
		{
			yield return StartCoroutine(CloseCurrentAdditiveSceneRoutine(false));
		}

		while (!loadSceneOperation.isDone)
		{
			yield return null;
		}

		_previousSceneModel = _currentSceneModel;

		Scene loadedScene = SceneManager.GetSceneByName(sceneIndex.ToString());
		if (!string.IsNullOrEmpty(loadedScene.name))
		{
			GameObject[] rootObjects = loadedScene.GetRootGameObjects();
			SceneController controller = rootObjects[0].GetComponent<SceneController>();
			if (controller == null)
			{
				CustomLog.Log("Current Scene Controller is null! Check the main scene object!");
				InputManager.Instance.ActivateInput();
				yield break;
			}
			_currentScene = controller;

			CustomLog.Log("Scene Transition clean process");
			System.GC.Collect();
			AsyncOperation unloadOperation = Resources.UnloadUnusedAssets();
			while (!unloadOperation.isDone)
			{
				yield return null;
			}

			CustomLog.Log("Scene Transition new controller's initSequence");
			yield return StartCoroutine(controller.InitSequence(model));
			_currentSceneModel = controller.SceneModel;
			CustomLog.Log("Scene is initialized! " + sceneIndex.ToString() + ", time elapsed: " + stopwatch.Elapsed);
		}
		else
		{
			CustomLog.LogError("Cannot find the current loaded scene!!! Name: " + sceneIndex.ToString());
		}

		stopwatch.Stop();
		_isLoading = false;		
		InputManager.Instance.ActivateInput();

		yield return new WaitForEndOfFrame();

		//Raise event of the loading process being ready
		yield return StartCoroutine(_currentScene.OnPostLoading());
		//BootManager.Instance.LoadingScreenLogic.Hide();

		yield break;
	}

	/// <summary>
	/// Use this if you want to go back to the last scene.
	/// </summary>
	/// <returns></returns>
	public IEnumerator BackToLastScene()
	{
		if (_previousSceneModel == null)
		{
			CustomLog.LogWarning("There's no previous scene!!");
			yield break;
		}
		yield return StartCoroutine(LoadSceneAsync(_previousSceneModel.sceneIndex, _previousSceneModel));
		yield break;
	}

	public bool IsAnyAdditiveSceneActive()
	{
		return _additiveSceneStack.Count > 0;
	}

	public void CloseCurrentAdditiveScene()
	{
		StartCoroutine(CloseCurrentAdditiveSceneRoutine());
	}

	private IEnumerator CloseCurrentAdditiveSceneRoutine(bool cleaningResources = true)
	{
		if (_isClosingAdditiveScene)
		{
			CustomLog.LogWarning("Already closing another additive scene!");
			yield break;
		}

		_isClosingAdditiveScene = true;
		if (_isLoading)
		{
			yield return null;
		}

		if (_additiveSceneStack.Count > 0)
		{
			SceneController sc = _additiveSceneStack.Pop() as SceneController;
			if(sc != null)
			{
				Destroy(sc.gameObject);
			}			
			if (_additiveSceneStack.Count == 0)
			{
				if (_currentScene != null)
				{
					_currentScene.OnFocus();
				}
			}
		}

		if (cleaningResources)
		{
			System.GC.Collect();
			AsyncOperation unloadOperation = Resources.UnloadUnusedAssets();
			while (!unloadOperation.isDone)
			{
				yield return null;
			}
		}		

		_isClosingAdditiveScene = false;
		yield break;
	}
	#endregion

	public IEnumerator ResetScene()
	{
		if (_currentSceneModel == null)
		{
			CustomLog.LogError("There's no current scene?!");
			yield break;
		}
		yield return StartCoroutine(LoadSceneAsync(_currentSceneModel.sceneIndex, _currentSceneModel));
		yield break;
	}

	public SceneIndexEnum GetLastSceneIndexOnScreen()
	{
		if (_additiveSceneStack.Count > 0)
		{
			SceneController sc = _additiveSceneStack.Peek() as SceneController;
			return sc.SceneModel.sceneIndex;
		}

		// no additive screens in game
		return _currentSceneModel.sceneIndex;
	}


	public SceneController GetLastSceneControllerOnScreen()
	{
		if (_additiveSceneStack.Count > 0)
		{
			SceneController sc = _additiveSceneStack.Peek() as SceneController;
			return sc;
		}

		// no additive screens in game, return current
		return _currentScene;
	}
}