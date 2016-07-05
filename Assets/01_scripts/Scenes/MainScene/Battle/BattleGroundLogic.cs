using UnityEngine;
using System.Collections.Generic;

public class BattleGroundLogic : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _blobPivotList = new List<Transform>();

    [SerializeField]
    private float _dragThreshold = 4.0f;

    private List<BattleGroundPivotLogic> _blobLogicList = new List<BattleGroundPivotLogic>();

    public void Init()
    {
        foreach (Transform t in _blobPivotList)
        {
            BattleGroundPivotLogic bgLogic = t.GetComponent<BattleGroundPivotLogic>();
            _blobLogicList.Add(bgLogic);
            bgLogic.Init(_dragThreshold);
        }
    }

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
}
