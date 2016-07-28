using UnityEngine;
using System.Collections;

public class TestSceneLogic : MonoBehaviour
{
    [SerializeField]
    private Animator _blobAnimController;

    public void SetIdle_F()
    {
        _blobAnimController.SetTrigger("Idle_F");
    }

	public void SetIdle_B()
	{
		_blobAnimController.SetTrigger("Idle_B");
	}

    public void SetJump_F()
    {
        _blobAnimController.SetTrigger("Jump_F");
    }

	public void SetJump_B()
	{
		_blobAnimController.SetTrigger("Jump_B");
	}

	public void SetSqueeze_F()
    {
		_blobAnimController.SetTrigger("Squeeze_F");
    }

	public void SetSqueeze_B()
	{
		_blobAnimController.SetTrigger("Squeeze_B");
	}

	public void SetSqueezeIn_F()
	{
		_blobAnimController.SetTrigger("SqueezeIn_F");
	}

	public void SetSqueezeIn_B()
	{
		_blobAnimController.SetTrigger("SqueezeIn_B");
	}

	public void SetSqueezeOut_F()
	{
		_blobAnimController.SetTrigger("SqueezeOut_F");
	}

	public void SetSqueezeOut_B()
	{
		_blobAnimController.SetTrigger("SqueezeOut_B");
	}

	public void SetDeath_F()
	{
		_blobAnimController.SetTrigger("Death_F");
	}

	public void SetDeath_B()
	{
		_blobAnimController.SetTrigger("Death_B");
	}

	public void SetFire_F()
	{
		_blobAnimController.SetTrigger("Fire_F");
	}

	public void SetFire_B()
	{
		_blobAnimController.SetTrigger("Fire_B");
	}

	public void SetFireIn_F()
	{
		_blobAnimController.SetTrigger("FireIn_F");
	}

	public void SetFireIn_B()
	{
		_blobAnimController.SetTrigger("FireIn_B");
	}

	public void SetFireOut_F()
	{
		_blobAnimController.SetTrigger("FireOut_F");
	}

	public void SetFireOut_B()
	{
		_blobAnimController.SetTrigger("FireOut_B");
	}
}
