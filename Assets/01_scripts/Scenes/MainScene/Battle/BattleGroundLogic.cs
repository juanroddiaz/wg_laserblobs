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
    private float _dragThreshold = 50.0f;
    public float DragThreshold
    {
        get { return _dragThreshold; }
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
}
