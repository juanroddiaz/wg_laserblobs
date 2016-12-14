using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ReserveQueueLogic : MonoBehaviour
{
    [SerializeField]
    private Transform _queueTransform;

    [SerializeField]
    private float _blobReserveCustomSize = 10.0f;

    private MainScenarioLogic _bgLogic;
    private List<GameObject> _reserveObjList = new List<GameObject>();
    private BattleGroundType _type;
    private int _reserveIdx = 0;

    public void Init(MainScenarioLogic bgLogic, BattleGroundType type)
    {
        _bgLogic = bgLogic;
        _type = type;
        List<BlobTypes> blobList = new List<BlobTypes>();
        switch (_type)
        {
            case BattleGroundType.PLAYER:
                blobList = _bgLogic.CurrentBlobSelection;
                break;
            case BattleGroundType.ENEMY:
                blobList = _bgLogic.CurrentEnemyQueue;
                break;
        }
        for (int i = 0; i < blobList.Count; i++)
        {
            GameObject obj = Instantiate(_bgLogic.GetBlobTypePerDifficulty((int)blobList[i]), Vector3.zero, Quaternion.identity) as GameObject; ;
            obj.transform.SetParent(_queueTransform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = new Vector3(_blobReserveCustomSize, _blobReserveCustomSize, 1.0f);
            obj.name = obj.name + "_" + i.ToString();
            _reserveObjList.Add(obj);
            BattleGroundPivotLogic bgpLogic = obj.GetComponent<BattleGroundPivotLogic>();
            bgpLogic.SetBattleGroundTeam(_type);
            bgpLogic.SetAsReserve(true);
            _reserveIdx++;
        }
    }

    public void RemoveNextBlobFromReserve(Vector3 targetPos)
    {
        StartCoroutine(ReserveJumpRoutine(_reserveObjList[0], targetPos));
        _reserveObjList.RemoveAt(0);
    }

    private IEnumerator ReserveJumpRoutine(GameObject obj, Vector3 targetPos)
    {
        obj.GetComponent<BattleGroundPivotLogic>().JumpToBattleground();
        LeanTween.move(obj, targetPos, 0.5f).setEase(LeanTweenType.linear);
        yield return new WaitForSeconds(0.5f);
        Destroy(obj);
        yield break;
    }

    public void AddBlobToReserve()
    {
        BlobTypes blobType = BlobTypes.MAX;
        switch (_type)
        {
            case BattleGroundType.PLAYER:
                blobType = _bgLogic.CurrentBlobSelection.Last();
                break;
            case BattleGroundType.ENEMY:
                blobType = _bgLogic.CurrentEnemyQueue.Last();
                break;
        }
        GameObject obj = Instantiate(_bgLogic.GetBlobTypePerDifficulty((int)blobType), Vector3.zero, Quaternion.identity) as GameObject; ;
        obj.transform.SetParent(_queueTransform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = new Vector3(_blobReserveCustomSize, _blobReserveCustomSize, 1.0f);
        obj.name = obj.name + "_" + _reserveIdx.ToString();
        _reserveObjList.Add(obj);
        BattleGroundPivotLogic bgpLogic = obj.GetComponent<BattleGroundPivotLogic>();
        bgpLogic.SetBattleGroundTeam(_type);
        bgpLogic.SetAsReserve(true);
        _reserveIdx++;
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