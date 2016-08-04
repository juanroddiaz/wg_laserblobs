using UnityEngine;
using System.Collections.Generic;

public class ReserveQueueLogic : MonoBehaviour
{
    [SerializeField]
    private Transform _queueTransform;

    [SerializeField]
    private float _blobReserveCustomSize = 10.0f;

    private MainScenarioLogic _bgLogic;
    private List<GameObject> _reserveObjList = new List<GameObject>();

    public void Init(MainScenarioLogic bgLogic)
    {
        _bgLogic = bgLogic;

        for (int i = 0; i < _bgLogic.CurrentBlobSelection.Count; i++)
        {
            GameObject obj = Instantiate(_bgLogic.BlobPrefabs[(int)_bgLogic.CurrentBlobSelection[i]], Vector3.zero, Quaternion.identity) as GameObject;
            obj.transform.SetParent(_queueTransform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = new Vector3(_blobReserveCustomSize, _blobReserveCustomSize, 1.0f);
            _reserveObjList.Add(obj);
            BattleGroundPivotLogic bgpLogic = obj.GetComponent<BattleGroundPivotLogic>();
            bgpLogic.SetAsReserve();
        }
    }

    public void RemoveNextBlobFromReserve()
    {
        Destroy(_reserveObjList[0]);
        _reserveObjList.RemoveAt(0);
    }

    public void Reset()
    {
        foreach (GameObject o in _reserveObjList)
        {
            Destroy(o);
        }
        _reserveObjList.Clear();
    }
}