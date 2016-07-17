using UnityEngine;
using System.Collections.Generic;

public class DraggablePanelBattleLogic : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _blobPivots = new List<Transform>();

    [SerializeField]
    private List<DraggableBlobLogic> _blobDragObjs = new List<DraggableBlobLogic>();

    public void Init()
    {
        if (_blobPivots.Count != (int)LaserLinesEnum.Max && _blobPivots.Count != _blobDragObjs.Count)
        {
            CustomLog.Log("Wrong settings for draggable panel! Check transforms reference list!");
            return;
        }

        for (int i = 0; i < _blobPivots.Count; i++)
        {
            _blobDragObjs[i].Init((LaserLinesEnum)i, this);
        }
    }
}
