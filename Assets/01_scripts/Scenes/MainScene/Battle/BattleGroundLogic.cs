﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleGroundLogic : MonoBehaviour
{
    [SerializeField]
    private GameObject _blobObject;

    [SerializeField]
    private BattleGroundType _type;
    public BattleGroundType Type
    {
        get { return _type; }
    }

    [SerializeField]
    private ReserveQueueLogic _selectionQueueLogic;

    private List<DraggableBlobLogic> _blobDragObjs = new List<DraggableBlobLogic>();

    [SerializeField]
    private float _dragStartThreshold = 50.0f;
    public float DragStartThreshold
    {
        get { return _dragStartThreshold; }
    }

    [SerializeField]
    private float _dropThreshold = 0.125f;
    public float DropThreshold
    {
        get { return _dropThreshold; }
    }

    [SerializeField]
    private float _dragBlobAlpha = 0.75f;
    public float DragBlobAlpha
    {
        get { return _dragBlobAlpha; }
    }

    [SerializeField]
    private float _blobLaserForce = 0.00125f;
    public float BlobLaserForce
    {
        get { return _blobLaserForce; }
    }

    [SerializeField]
    private float _blobHoldLaserMultiplier = 3.0f;
    public float BlobHoldLaserMultiplier
    {
        get { return _blobHoldLaserMultiplier; }
    }

    [SerializeField]
	private float _blobCustomSize = 30.0f;
	public float BlobCustomSize
	{
		get { return _blobCustomSize; }
	}

    private List<BattleGroundPivotLogic> _blobLogicList = new List<BattleGroundPivotLogic>();
    private MainScenarioLogic _scenarioLogic;

    public void Init(MainScenarioLogic scenarioLogic)
    {
        _scenarioLogic = scenarioLogic;
        int idx = 0;
        for(int i=0; i<(int)LaserLinesEnum.Max; i++)
        {
            int blobTypeIdx = _type == BattleGroundType.PLAYER ? (int)scenarioLogic.CurrentBlobSelection[i] : i;
            GameObject obj = Instantiate(scenarioLogic.BlobPrefabs[blobTypeIdx], Vector3.zero, Quaternion.identity) as GameObject;
            LaserLinesEnum lane = (LaserLinesEnum)idx;
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = new Vector3(_blobCustomSize, _blobCustomSize, 1.0f);
            BattleGroundPivotLogic bgLogic = obj.GetComponent<BattleGroundPivotLogic>();
            _blobLogicList.Add(bgLogic);
            bgLogic.Init(this, lane, (BlobTypes)blobTypeIdx);
            if (bgLogic.BlobDragLogic != null && _type == BattleGroundType.PLAYER)
            {
                bgLogic.BlobDragLogic.Init(lane, this, bgLogic.BlobBaseColor);
                _blobDragObjs.Add(bgLogic.BlobDragLogic);
            }

            _scenarioLogic.UpdateLaserColors(lane, bgLogic.BlobBaseColor, _type == BattleGroundType.PLAYER, true);

            idx++;
        }

        switch (_type)
        {
            case BattleGroundType.PLAYER:
                _scenarioLogic.RemoveNextBlobFromQueue(3);
                break;
            case BattleGroundType.ENEMY:
                _scenarioLogic.RemoveNextBlobFromEnemyQueue(3);
                break;
        }        

        // reserve queue initialization
        _selectionQueueLogic.Init(_scenarioLogic, _type);
    }

    public void ToggleOnHoldState(LaserLinesEnum lane, bool held)
    {
        _blobLogicList[(int)lane].BlobLogic.ToggleOnHoldState(held);
        _blobLogicList[(int)lane].Squeeze(held);
    }

    /// <summary>
    /// Get the updated lane's blob force. This already handles OnHold/Normal case for the target blob.
    /// </summary>
    /// <param name="lane"></param>
    /// <returns></returns>
    public float GetBlobForce(LaserLinesEnum lane)
    {
        return _blobLogicList[(int)lane].GetBlobForce();
    }

    public Color GetBlobStartColor(LaserLinesEnum lane)
    {
        return _blobLogicList[(int)lane].BlobBaseColor;
    }

    /// <summary>
    /// Returns the player's blob index who is currently being pressed, if any
    /// </summary>
    /// <returns></returns>
    public int GetPressedBlobIndex()
    {
        int ret = _blobDragObjs.FindIndex(x => x.IsHeld);        
        return ret;
    }

    public void CheckBlobSwapping(LaserLinesEnum lane, Vector3 lastPos)
    {
        int laneIdx = (int)lane;
        for (int i = 0; i < (int)LaserLinesEnum.Max; i++)
        {
            if (i == laneIdx)
            {
                continue;
            }
            float distance = (lastPos - _blobLogicList[i].transform.position).magnitude;
            //CustomLog.Log("drag from " + lane.ToString() + " to " + (LaserLinesEnum)i + " distance: " + distance);
            if (distance < _dropThreshold)
            {
                SwapBlobs(lane, (LaserLinesEnum)i);
                return;
            }
        }
    }

    private void SwapBlobs(LaserLinesEnum from, LaserLinesEnum to)
    {
        CustomLog.Log("Swapping from " + from.ToString() + " to " + to.ToString());
        int fromIdx = (int)from;
        int toIdx = (int)to;
        BattleGroundPivotLogic fromLogic = _blobLogicList[fromIdx];
        Color fromColor = GetBlobStartColor(from);
        fromLogic.gameObject.SetActive(false);
        BattleGroundPivotLogic toLogic = _blobLogicList[toIdx];
        Color toColor = GetBlobStartColor(to);
        toLogic.gameObject.SetActive(false);
        DraggableBlobLogic fromDragLogic = _blobDragObjs[fromIdx];
        DraggableBlobLogic toDragLogic = _blobDragObjs[toIdx];
        int fromSibling = fromLogic.transform.GetSiblingIndex();
        int toSibling = toLogic.transform.GetSiblingIndex();
        fromLogic.transform.SetSiblingIndex(toSibling);
        toLogic.transform.SetSiblingIndex(fromSibling);
        _blobLogicList[fromIdx] = toLogic;
        _blobLogicList[toIdx] = fromLogic;
        if (fromDragLogic != null)
        {
            fromDragLogic.UpdateLane(to);
        }
        if (toDragLogic != null)
        {
            toDragLogic.UpdateLane(from);
        }        
        _blobDragObjs[fromIdx] = toDragLogic;
        _blobDragObjs[toIdx] = fromDragLogic;
        fromLogic.gameObject.SetActive(true);
        toLogic.gameObject.SetActive(true);
        fromLogic.LaserFireEvent();
        toLogic.LaserFireEvent();

        _scenarioLogic.UpdateLaserColors(from, toColor, true, toDragLogic != null);
        _scenarioLogic.UpdateLaserColors(to, fromColor, true, fromDragLogic != null);
    }

    public IEnumerator BlobDeath(LaserLinesEnum lane, BlobTypes blobType)
    {
        CustomLog.Log("Blob death!! Lane " + lane.ToString() + ", team: " + _type.ToString());
        int deadIdx = (int)lane;
        if (_type == BattleGroundType.PLAYER && _blobDragObjs[deadIdx] != null)
        {
            _blobDragObjs[deadIdx].BlockDragLogic();
        }
        BattleGroundPivotLogic deadBlob = _blobLogicList[deadIdx];
        int siblingIdx = deadBlob.transform.GetSiblingIndex();
        deadBlob.DeathEvent();
        yield return new WaitForSeconds(0.5f);
        Destroy(deadBlob.gameObject);

        // blob reserve is over?
        GameObject blobObj = blobType == BlobTypes.MAX ? _scenarioLogic.DeadBlob : _scenarioLogic.BlobPrefabs[(int)blobType];
        GameObject obj = Instantiate(blobObj, Vector3.zero, Quaternion.identity) as GameObject;
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = new Vector3(_blobCustomSize, _blobCustomSize, 1.0f);
        obj.transform.SetSiblingIndex(siblingIdx);

        BattleGroundPivotLogic bgLogic = obj.GetComponent<BattleGroundPivotLogic>();
        _blobLogicList[deadIdx] = bgLogic;
        bgLogic.Init(this, lane, blobType);

        switch (_type)
        {
            case BattleGroundType.PLAYER:
                // Dead blob logic and game over checking
                if (blobType == BlobTypes.MAX)
                {
                    _blobDragObjs[deadIdx] = null;
                    bool gameOver = true;
                    for (int i = 0; i < _blobLogicList.Count; i++)
                    {
                        if (_blobLogicList[i].Type != BlobTypes.MAX)
                        {
                            gameOver = false;
                            break;
                        }
                    }
                    if (gameOver)
                    {
                        _scenarioLogic.SceneController.ShowGameOver();
                    }
                    yield break;
                }

                if (bgLogic.BlobDragLogic != null)
                {
                    bgLogic.BlobDragLogic.Init(lane, this, bgLogic.BlobBaseColor);
                    _blobDragObjs[deadIdx] = bgLogic.BlobDragLogic;
                }
                break;
            case BattleGroundType.ENEMY:
                // score updating
                _scenarioLogic.SceneController.UpdateScore();
                break;
        }
        yield break;
    }

    public void SetLaserAnim(LaserLinesEnum lane, bool fire)
    {
        _blobLogicList[(int)lane].LaserFireEvent(fire);
    }

    public void Reset()
    {
        foreach (BattleGroundPivotLogic o in _blobLogicList)
        {
            Destroy(o.gameObject);
        }
        _blobLogicList.Clear();
        _blobDragObjs.Clear();
        _selectionQueueLogic.Reset();
    }

    #region Reserve logic
    public void RemoveNextBlobFromReserve()
    {
        _selectionQueueLogic.RemoveNextBlobFromReserve();
    }

    public void AddBlobToReserve()
    {
        _selectionQueueLogic.AddBlobToReserve();
    }
    #endregion
}
