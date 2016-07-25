using UnityEngine;
using System.Collections;

public class TestSceneLogic : MonoBehaviour
{
    [SerializeField]
    private Animator _blobAnimController;

    public void SetIdleFront()
    {
        _blobAnimController.SetTrigger("IdleFront");
    }

    public void SetJumpFront()
    {
        _blobAnimController.SetTrigger("JumpFront");
    }

    public void SetHoldFront()
    {
        _blobAnimController.SetTrigger("SqueezeFront");
    }

	public void SetIdleBack()
	{
		_blobAnimController.SetTrigger("IdleBack");
	}

	public void SetJumpBack()
	{
		_blobAnimController.SetTrigger("JumpBack");
	}

	public void SetHoldBack()
	{
		_blobAnimController.SetTrigger("SqueezeBack");
	}

	public void SetGradient()
	{
		_blobAnimController.SetTrigger("ColorGradient");
	}
}
