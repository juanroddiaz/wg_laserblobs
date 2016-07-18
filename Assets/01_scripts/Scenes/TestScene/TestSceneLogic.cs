using UnityEngine;
using System.Collections;

public class TestSceneLogic : MonoBehaviour
{
    [SerializeField]
    private Animator _blobAnimController;

    public void SetIdle()
    {
        _blobAnimController.SetTrigger("Idle");
    }

    public void SetJump()
    {
        _blobAnimController.SetTrigger("Jump");
    }

    public void SetHold()
    {
        _blobAnimController.SetTrigger("Press");
    }
}
