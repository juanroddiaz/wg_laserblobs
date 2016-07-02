using UnityEngine;
using System.Collections;

public class BootManager : Singleton<BootManager>
{
    private bool _isBootReady = false;
    public bool IsBootReady
    {
        get { return _isBootReady; }
    }

    protected override void onAwake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        StartCoroutine(WaitingForInitializationReady());
    }

    private IEnumerator WaitingForInitializationReady()
    {
        _isBootReady = true;
        yield break;
    }
}
