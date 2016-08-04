using UnityEngine;
using System.Collections;

public class ReserveQueueLogic : MonoBehaviour
{
    [SerializeField]
    private Transform _queueTransform;

    [SerializeField]
    private GameObject _blobObject;

    private MainSceneController _controller;

    public void Init(MainSceneController controller)
    {
        _controller = controller;
    }


}