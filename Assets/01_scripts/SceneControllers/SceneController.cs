using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Main data container for scene initialization.
/// </summary>
public class SceneModel
{
	public SceneIndexEnum sceneIndex;
}

/// <summary>
/// Main base class for single scene control. This will communicate with SceneTransitionManager to set game's current environment and entry data.
/// </summary>
public class SceneController : MonoBehaviour
{
	protected	SceneModel _sceneModel;
	public		SceneModel SceneModel
	{
		get {return _sceneModel;}
	}

	[SerializeField]
	protected GameObject _vfxPoolForScene;
	public GameObject VfxPoolForScene
	{
		get { return _vfxPoolForScene; }
	}

	protected bool	_isInitialized = false;

	void Awake()
	{
		if (BootManager.Instance == null)
		{
			// Load the singleton management scene into this one!!
			StartCoroutine(LoadSingletonScene());
			return;
		}
	}

	protected IEnumerator LoadSingletonScene()
	{
		AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(SceneIndexEnum.BootManagerScene.ToString(), LoadSceneMode.Additive);
		while (!loadSceneOperation.isDone)
		{
			yield return null;
		}
		yield break;
	}

	/// <summary>
	/// Main initializer for scene controllers.
	/// </summary>
	public virtual IEnumerator InitSequence(SceneModel sceneModel)
	{
		_isInitialized = true;
		yield break;
	}

	/// <summary>
	/// Event raised by the SceneTransitionManager when the instance of a scene is finished loading.
	/// </summary>
	/// <returns>True in case an additive scene was triggered to be loaded on post load. Otherwise, false.</returns>
	public virtual IEnumerator OnPostLoading()
	{
		yield break;
	}

	/// <summary>
	/// Called before the scene is destroyed
	/// </summary>
	/// <returns></returns>
	public virtual IEnumerator OnDispose()
	{
		yield break;
	}


	public virtual void Update()
	{
		if (_isInitialized && SceneTransitionManager.Instance.IsAvailableForTransition())
		{
			// This works for Android's back button too.
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				if (SceneTransitionManager.Instance.GetLastSceneIndexOnScreen() == _sceneModel.sceneIndex)
				{
					OnBackTransition();
				}				
			}
		}		
	}

	/// <summary>
	/// Override this method in any Context Scene Controller to give specific back transition logic to the current scene.
	/// </summary>
	public virtual void OnBackTransition()
	{
		if (_isInitialized && SceneTransitionManager.Instance.IsAvailableForTransition())
		{
			//if (PopUpManager.Instance.IsAnyActivePopUp())
			//{
			//	PopUpManager.Instance.CloseCurrentPopUp();
			//	return;
			//}

			if (SceneTransitionManager.Instance.IsAnyAdditiveSceneActive())
			{
				SceneTransitionManager.Instance.CloseCurrentAdditiveScene();
				return;
			}

			StartCoroutine(SceneTransitionManager.Instance.BackToLastScene());
		}
	}

	public virtual void OnFocus()
	{
	}
}
