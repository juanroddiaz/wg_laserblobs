using UnityEngine;
using System.Collections;

public class InputManager : Singleton<InputManager> {

#if UNITY_EDITOR
	//Debug messages
	public bool debug = false;
	private bool _pressed = false;
#endif

	// to avoid last pressed finger as movement when a pinch gesture is ending
	private bool _zooming = false;

	enum swipeDirection
    {
        RIGHT,
        LEFT,
        DOWN,
        UP,
        MAX
    }

	const float DEFAULT_PPC = 70.0f;

	const float s_dragTimerReset = 0.6f;
	const float s_verticalSwipeDistance = 0.3f; //in centimeters
	const float s_horizontalSwipeDistance = 0.6f; //in centimeters

	private float _startDragTime = 0.0f;
	private bool _dragTimerReset = false;
	private bool _movedThisSwipe = false;
	private Vector2 _currentDragMovement = new Vector2();
	private Vector2 _fingerLastPos;
	private float _ppc = 100.0f; //pixels per centimeter
    private int _lastFingerId = -1;
	private float _verticalDistancePixels;
	private float _horizontalDistancePixels;

	[SerializeField]
	private GameObject _eventSystemObject = null;
	// [JD] Double tap event cannot be retrived by OnPointerClick due an Unity bug. Using a tap timer to catch double click intention from player.
	public float doubleTapInterval = 0.25f;
	// [JD] Pinch sensibility value
	public float pinchMinumumDiff = 0.05f;

	// Event Handler
	public delegate void onTapEventHandler(Vector2 fingerPos);
	public event onTapEventHandler onTapEvent;
	public delegate bool onSwipeEventHandler( );
	public event onSwipeEventHandler onSwipeEventLeft;
	public event onSwipeEventHandler onSwipeEventRight;
	public event onSwipeEventHandler onSwipeEventUp;
	public event onSwipeEventHandler onSwipeEventDown;
	public delegate void onPressEventHandler(Vector2 fingerPos);
	public event onPressEventHandler onPressEvent;
	public event onPressEventHandler onPressingEvent;
	public delegate void onReleaseEventHandler();
	public event onReleaseEventHandler onReleaseEvent;
	public delegate void onZoomInEventHandler(float diff, Vector2 touchAPos, Vector2 touchBPos);
	public event onZoomInEventHandler onZoom;

	// for input blocked
	private bool _isBlocked = false;
	// for Unity UI signal abort in case player is making a gesture
	[SerializeField]
	private float	_gestureIsRunningDeltaTime = 0.03f;
	[SerializeField]
	private float	_gestureIsRunningDeltaDistance = 6.0f;

	private float	_gestureCurrentTime;
	private Vector2 _gestureInitialPoint = Vector2.zero;

	private bool	_nativeInputIsActive = true;
	public bool		NativeInputIsActive
	{
		get { return _nativeInputIsActive; }
	}

	protected override void onAwake()
	{
		float dpi = Screen.dpi;
		if(dpi > 0.0f)
		{
			_ppc = dpi/2.54f;
		}
		else
		{
			_ppc = DEFAULT_PPC;
		}
		_verticalDistancePixels = _ppc * s_verticalSwipeDistance;
		_horizontalDistancePixels = _ppc * s_horizontalSwipeDistance;
	}

	#region Gesture event registration/unregistration
	public void ResetEvents()
	{
		//clear input events
		onTapEvent = null;
		onSwipeEventLeft = null;
		onSwipeEventRight = null;
		onSwipeEventUp = null;
		onSwipeEventDown = null;
		onPressEvent = null;
		onPressingEvent = null;
		onReleaseEvent = null;
		onZoom = null;
	}
	#endregion

	void onFingerDown( Vector2 fingerPos,int fingerId = -1)
	{
        _lastFingerId = fingerId;
		_fingerLastPos = fingerPos;
		_movedThisSwipe = false;
		_startDragTime = Time.realtimeSinceStartup;
		_currentDragMovement.Set(0.0f, 0.0f);
		_dragTimerReset = false;

		if (onTapEvent != null)
			onTapEvent (fingerPos);

		if (onPressEvent != null)
			onPressEvent (fingerPos);

		_gestureCurrentTime = 0.0f;
		_gestureInitialPoint = fingerPos;
#if UNITY_EDITOR
		if (debug)
		{
			CustomLog.Log ("Press");
		}
#endif
	}

    void onFingerUp( Vector2 fingerPos ,int fingerId = -1)
	{
		if(!_movedThisSwipe && !_dragTimerReset)
		{
			checkSwipe (0.5f); //swipe for people that swipe really short and fast
		}

		if(onReleaseEvent != null)
			onReleaseEvent();

		_gestureCurrentTime = 0.0f;
		if (!_nativeInputIsActive)
		{
			_nativeInputIsActive = true;
		}

#if UNITY_EDITOR
		if (debug)
		{
			CustomLog.Log ("Release");
		}
#endif
	}

	
    void onFingerMove( Vector2 fingerPos ,int fingerId = -1)
	{
		if (onPressingEvent != null)
			onPressingEvent(fingerPos);

		if (_movedThisSwipe || fingerId != _lastFingerId)
		{
			return; //don't use the same swipe for 2 movements
		}

		Vector2 delta = fingerPos - _fingerLastPos;
		_fingerLastPos = fingerPos;
		float currentTime = Time.realtimeSinceStartup;
		float deltaTime = currentTime - _startDragTime;

		if(deltaTime > s_dragTimerReset)
		{
			//the drag is reset
			_currentDragMovement = delta;
			_startDragTime = currentTime;
			_dragTimerReset = true;
		}
		else
		{
			_currentDragMovement += delta;
		}

		checkSwipe (1.0f);

		_gestureCurrentTime += Time.deltaTime;
		float magnitude = Vector2.Distance(_gestureInitialPoint, fingerPos);
		if (_gestureCurrentTime > _gestureIsRunningDeltaTime ||
			magnitude > _gestureIsRunningDeltaDistance)
		{
			if (_nativeInputIsActive)
			{
				_nativeInputIsActive = false;
			}			
		}
	}
	
	void checkSwipe (float swipeDistanceMultiplier)
	{
		float distanteToCheck;
		float swipeDistanceX = Mathf.Abs (_currentDragMovement.x);
		float swipeDistanceY = Mathf.Abs (_currentDragMovement.y);
		
		float swipeDistance = 0.0f;
		
        swipeDirection moveDir = swipeDirection.RIGHT;
		
		if(swipeDistanceX > swipeDistanceY) //horizontal swipes are easy to do, so reduce the distance for the comparison
		{
			distanteToCheck = _horizontalDistancePixels;
			swipeDistance = swipeDistanceX;
			if(_currentDragMovement.x < 0.0f)
			{
                moveDir = swipeDirection.LEFT;
			}
			else
			{
                moveDir = swipeDirection.RIGHT;
			}
		}
		else
		{
			distanteToCheck = _verticalDistancePixels;
			swipeDistance = swipeDistanceY;
			if(_currentDragMovement.y < 0.0f)
			{
                moveDir = swipeDirection.DOWN;
			}
			else
			{
                moveDir = swipeDirection.UP;
			}
		}
		
		distanteToCheck *= swipeDistanceMultiplier;

		if( swipeDistance >= distanteToCheck)
		{
			Swipe(moveDir);

			_currentDragMovement.Set(0.0f, 0.0f);
		}
	}
	
	private void Swipe(swipeDirection direction)
	{
		bool moved = false;
		//Process direction and velocity
		switch (direction) 
		{
            case swipeDirection.LEFT:
			if(onSwipeEventLeft != null)
			{
				moved = onSwipeEventLeft();
			}
			break;
            case swipeDirection.RIGHT:
			if(onSwipeEventRight != null)
			{
				moved = onSwipeEventRight();
			}
			break;
            case swipeDirection.UP:
			if(onSwipeEventUp != null)
			{
				moved = onSwipeEventUp();
			}
			break;
            case swipeDirection.DOWN:
			if(onSwipeEventDown != null)
			{
				moved = onSwipeEventDown();
			}
			break;
		}

		if(moved)
		{
			_movedThisSwipe = true;
			if(onReleaseEvent != null)
			{
				onReleaseEvent();
			}

			#if UNITY_EDITOR
			if(debug)
			{
				CustomLog.Log ("swipe dir "+direction);
			}
			#endif
		}
		
	}

	/// <summary>
	/// Handles the zoom in / out gesture
	/// From: https://unity3d.com/es/learn/tutorials/modules/beginner/platform-specific/pinch-zoom
	/// </summary>
	private bool CheckPinchZoom(out float diff)
	{
		// Store both touches.
		Touch touchZero = Input.GetTouch(0);
		Touch touchOne = Input.GetTouch(1);

		// Find the position in the previous frame of each touch.
		Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
		Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

		// Find the magnitude of the vector (the distance) between the touches in each frame.
		float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
		float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

		// Find the difference in the distances between each frame.
		diff = prevTouchDeltaMag - touchDeltaMag;

		return Mathf.Abs(diff) > pinchMinumumDiff;
	}

	void Update()
    {
		if (_isBlocked)
		{
			return;
		}

#if UNITY_EDITOR

		if (!_zooming)
		{
			// left click logic
			if (Input.GetMouseButtonDown(0))
			{
				//touch begin
				_pressed = true;
				onFingerDown(Input.mousePosition);
				return;
			}

			if (Input.GetMouseButtonUp(0))
			{
				if (_pressed)
				{
					_pressed = false;
					onFingerUp(Input.mousePosition);
					return;
				}
			}

			if (_pressed)
			{
				onFingerMove(Input.mousePosition);
				//CustomLog.Log("x: " +Input.mousePosition.x + " y: "+Input.mousePosition.y);
				return;
			}

			// middle click logic
			if (Input.GetMouseButtonDown(2))
			{
				if (onZoom != null)
				{
					float diff = 1.0f;
					onZoom(diff, Input.mousePosition, Input.mousePosition);
					_zooming = true;
				}
				return;
			}

			// right click logic
			if (Input.GetMouseButtonDown(1))
			{
				if (onZoom != null)
				{
					float diff = -1.0f;
					onZoom(diff, Input.mousePosition, Input.mousePosition);
					_zooming = true;
				}
				return;
			}
		}
		else
		{
			// middle click logic
			if (Input.GetMouseButton(2))
			{
				if (onZoom != null)
				{
					float diff = 1.0f;
					onZoom(diff, Input.mousePosition, Input.mousePosition);
				}
				return;
			}

			// right click logic
			if (Input.GetMouseButton(1))
			{
				if (onZoom != null)
				{
					float diff = -1.0f;
					onZoom(diff, Input.mousePosition, Input.mousePosition);
				}
				return;
			}
			_zooming = false;
		}

#else
		int touchCount = Input.touchCount;
		if(touchCount == 0)
		{
			if(_zooming)
			{
				_zooming = false;
				_gestureCurrentTime = 0.0f;
				if (!_nativeInputIsActive)
				{
					_nativeInputIsActive = true;
				}
			}
			return;
		}

		switch (touchCount)
		{
			case 1:
			if(_zooming)
			{
				return;
			}
			Touch touch = Input.GetTouch(0);
            switch(touch.phase)
            {
                case TouchPhase.Began:
                    onFingerDown(touch.position,touch.fingerId);
                    break;
                case TouchPhase.Ended:
                    onFingerUp(touch.position,touch.fingerId);
                    break;
                case TouchPhase.Moved:
				case TouchPhase.Stationary:
                    onFingerMove(touch.position,touch.fingerId);
                    break;
                    
            }
			break;
			case 2:
			if (onZoom != null)
			{
				float diff = 0.0f;
				if(CheckPinchZoom(out diff))
				{
					Touch touchA = Input.GetTouch(0);
					Touch touchB = Input.GetTouch(1);
					onZoom(diff, touchA.position, touchB.position);
					_zooming = true;

					_gestureCurrentTime += Time.deltaTime;
					if (_gestureCurrentTime > _gestureIsRunningDeltaTime)
					{
						_nativeInputIsActive = false;
					}
				}
			}
			break;
			default:
				// Not supporting more than 2 fingers in screen.
				break;
		}        
#endif
	}

	#region Input Control
	public void ActivateInput()
	{
		_isBlocked = false;
		_nativeInputIsActive = true;
		_eventSystemObject.SetActive(true);
	}

	public void DeactivateInput(bool justGestures = false)
	{
		_isBlocked = true;
		if (!justGestures)
		{
			_eventSystemObject.SetActive(false);
		}
		_zooming = false;
#if UNITY_EDITOR
		_pressed = false;
#endif
	}

	public bool IsInputActive()
	{
		return _eventSystemObject.activeInHierarchy && !_isBlocked;
	}
	#endregion
}
