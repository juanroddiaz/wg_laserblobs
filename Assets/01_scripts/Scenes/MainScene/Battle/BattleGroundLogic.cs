using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleGroundLogic : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _blobPivotList = new List<Transform>();

    [SerializeField]
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

    private List<BattleGroundPivotLogic> _blobLogicList = new List<BattleGroundPivotLogic>();

    public void Init()
    {
        for (int i = 0; i< _blobDragObjs.Count; i++)
        {
            _blobDragObjs[i].Init((LaserLinesEnum)i, this);
        }

        int idx = 0;
        foreach (Transform t in _blobPivotList)
        {
            BattleGroundPivotLogic bgLogic = t.GetComponent<BattleGroundPivotLogic>();
            _blobLogicList.Add(bgLogic);
            bgLogic.Init(this, (LaserLinesEnum)idx);            
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
        // BattleGroundPivotLogic fromLogic = _blobLogicList[(int)from];
        //BattleGroundPivotLogic toLogic = _blobLogicList[(int)to];
        //_blobDragObjs[(int)from].UpdateLane(to);
        //_blobDragObjs[(int)to].UpdateLane(from);
        //fromLogic.transform.SetSiblingIndex((int)to);
        //toLogic.transform.SetSiblingIndex((int)from);
        //_blobLogicList[(int)from] = toLogic;
        //_blobLogicList[(int)to] = fromLogic;
    }
}
