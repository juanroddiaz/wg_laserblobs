using UnityEngine;

/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;
	
	public static T Instance
	{
		get
		{
            if (_instance == null)
            {
                _instance = (T) FindObjectOfType(typeof(T));
                if(_instance != null)
                    return _instance;
                CustomLog.LogWarning("Singleton not on scene: " + typeof(T).ToString() + ". Be sure SingletonManager scene will be loaded!!");
                return null;
            }
            return _instance;
        }
	}

    void Awake()
    {
		if (_instance == null)
		{
			_instance = this.GetComponent<T>();
            CustomLog.Log("Creating Singleton for " + _instance.GetType().ToString());
		}
        onAwake();
    }

    protected virtual void onAwake()
    {

    }

	public void OnDestroy () {
		//applicationIsQuitting = true;
		_instance = null;
	}

	protected virtual void onDestroy()
	{

	}
}