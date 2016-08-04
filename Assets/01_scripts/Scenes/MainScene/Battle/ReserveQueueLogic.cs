﻿using UnityEngine;

public class ReserveQueueLogic : MonoBehaviour
{
    [SerializeField]
    private Transform _queueTransform;

    private MainScenarioLogic _bgLogic;

    public void Init(MainScenarioLogic bgLogic)
    {
        _bgLogic = bgLogic;

        for (int i = 0; i < _bgLogic.CurrentBlobSelection.Count; i++)
        {
            GameObject obj = Instantiate(_bgLogic.BlobPrefabs[(int)_bgLogic.CurrentBlobSelection[i]], Vector3.zero, Quaternion.identity) as GameObject;
            obj.transform.SetParent(_queueTransform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = new Vector3(10.0f, 10.0f, 1.0f);
        }
    }
}