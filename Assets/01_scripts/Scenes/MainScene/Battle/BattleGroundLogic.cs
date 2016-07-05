using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleGroundLogic : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _blobPivotList = new List<Transform>();

    [SerializeField]
    private float _dragThreshold = 4.0f;
    public float DragThreshold
    {
        get { return _dragThreshold; }
    }

    [SerializeField]
    private float _dragCooldown = 0.5f;

    private List<BattleGroundPivotLogic> _blobLogicList = new List<BattleGroundPivotLogic>();
    private bool _onDragCooldown = false;

    public void Init()
    {
        int idx = 0;
        foreach (Transform t in _blobPivotList)
        {
            BattleGroundPivotLogic bgLogic = t.GetComponent<BattleGroundPivotLogic>();
            _blobLogicList.Add(bgLogic);
            bgLogic.Init(this, (LaserLinesEnum)idx);
            idx++;
        }
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
        int ret = _blobLogicList.FindIndex(x => x.IsHeld);        
        return ret;
    }

    /// <summary>
    /// Get a swipe event from lane's blob and desired direction
    /// </summary>
    /// <param name="lane"></param>
    /// <param name="directionSign"></param>
    public void BlogSwipeEvent(LaserLinesEnum lane, float directionSign)
    {
        if (_onDragCooldown)
        {
            return;
        }

        if (directionSign > 0)
        {
            // right
            switch (lane)
            {
                case LaserLinesEnum.Left:
                    // left - middle swap
                    BattleGroundPivotLogic ll = _blobLogicList[(int)LaserLinesEnum.Left];
                    BattleGroundPivotLogic lm = _blobLogicList[(int)LaserLinesEnum.Middle];
                    ll.UpdateLane(LaserLinesEnum.Middle);
                    lm.UpdateLane(LaserLinesEnum.Left);
                    _blobLogicList[(int)LaserLinesEnum.Left] = lm;
                    _blobLogicList[(int)LaserLinesEnum.Middle] = ll;
                    break;
                case LaserLinesEnum.Middle:
                    // middle - right swap
                    BattleGroundPivotLogic mm = _blobLogicList[(int)LaserLinesEnum.Middle];
                    BattleGroundPivotLogic mr = _blobLogicList[(int)LaserLinesEnum.Right];
                    mm.UpdateLane(LaserLinesEnum.Right);
                    mr.UpdateLane(LaserLinesEnum.Middle);
                    _blobLogicList[(int)LaserLinesEnum.Middle] = mr;
                    _blobLogicList[(int)LaserLinesEnum.Right] = mm;
                    break;
                case LaserLinesEnum.Right:
                    // no movement
                    break;
            }
        }
        else if (directionSign < 0)
        {
            // left
            switch (lane)
            {
                case LaserLinesEnum.Left:
                    // no movement
                    break;
                case LaserLinesEnum.Middle:
                    // middle - left swap
                    break;
                case LaserLinesEnum.Right:
                    // right - middle swap
                    break;
            }
        }
        StartCoroutine(DragCooldown());
    }


    private IEnumerator DragCooldown()
    {
        _onDragCooldown = true;
        yield return new WaitForSeconds(_dragCooldown);
        _onDragCooldown = false;
        yield break;
    }
}
