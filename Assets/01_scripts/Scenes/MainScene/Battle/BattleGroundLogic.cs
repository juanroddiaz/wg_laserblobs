﻿using UnityEngine;
using System.Collections.Generic;

public class BattleGroundLogic : MonoBehaviour
{
    [SerializeField]
    private GameObject _blobObject;

    [SerializeField]
    private BattleGroundType _type;

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

    public void Init(List<BlobTypes> blobSelection, MainScenarioLogic scenarioLogic)
    {
        int idx = 0;
        for(int i=0; i<(int)LaserLinesEnum.Max; i++)
        {
            GameObject obj = null;
            switch(_type)
            {
                case BattleGroundType.PLAYER:
                    obj = Instantiate(scenarioLogic.BlobPrefabs[(int)blobSelection[i]], Vector3.zero, Quaternion.identity) as GameObject;
                    break;
                case BattleGroundType.ENEMY:
                    obj = Instantiate(_blobObject, Vector3.zero, Quaternion.identity) as GameObject;
                    break;
            }
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = new Vector3(_blobCustomSize, _blobCustomSize, 1.0f);
            BattleGroundPivotLogic bgLogic = obj.GetComponent<BattleGroundPivotLogic>();
            _blobLogicList.Add(bgLogic);
            bgLogic.Init(this, (LaserLinesEnum)idx);
            if (bgLogic.BlobDragLogic != null && _type == BattleGroundType.PLAYER)
            {
                bgLogic.BlobDragLogic.Init((LaserLinesEnum)i, this);
                _blobDragObjs.Add(bgLogic.BlobDragLogic);
            }        
            idx++;
        }
    }

    public void ToggleOnHoldState(LaserLinesEnum lane, bool held)
    {
        _blobLogicList[(int)lane].BlobLogic.ToggleOnHoldState(held);
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
        fromLogic.gameObject.SetActive(false);
        BattleGroundPivotLogic toLogic = _blobLogicList[toIdx];
        toLogic.gameObject.SetActive(false);
        DraggableBlobLogic fromDragLogic = _blobDragObjs[fromIdx];
        DraggableBlobLogic toDragLogic = _blobDragObjs[toIdx];
        int fromSibling = fromLogic.transform.GetSiblingIndex();
        int toSibling = toLogic.transform.GetSiblingIndex();
        fromLogic.transform.SetSiblingIndex(toSibling);
        toLogic.transform.SetSiblingIndex(fromSibling);
        _blobLogicList[fromIdx] = toLogic;
        _blobLogicList[toIdx] = fromLogic;
        fromDragLogic.UpdateLane(to);
        toDragLogic.UpdateLane(from);
        _blobDragObjs[fromIdx] = toDragLogic;
        _blobDragObjs[toIdx] = fromDragLogic;
        fromLogic.gameObject.SetActive(true);
        toLogic.gameObject.SetActive(true);
    }

    public void Reset()
    {
        foreach (BattleGroundPivotLogic o in _blobLogicList)
        {
            Destroy(o.gameObject);
        }
        _blobLogicList.Clear();
        _blobDragObjs.Clear();
    }
}
