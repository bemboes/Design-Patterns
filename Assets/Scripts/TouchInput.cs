//#define VERBOSE_LOGGING
using UnityEngine;

/// <note>
/// Subscribe to the public event using your preferred method in the Awake/Start method.
/// - void Awake() { TouchInput.OnLaneJump += YourMethod(); }
/// - void Awake() { TouchInput.OnHold += YourMethod(); }
/// </note>

/// <summary>
/// TouchInput class (Singleton) handles game-related input.
/// Derive from TouchMonoBehaviour for interacting with UI Elements.
/// </summary>
public class TouchInput : MonoBehaviour
{
    /// <summary>
    /// The amount of fingers the component keeps track of.
    /// (PC = 3, Android = 5 to 8)
    /// </summary>
    public const uint MAX_FINGERS = 8;

    /// <summary>
    /// Holds all relevant touch information about each finger-press.
    /// </summary>
    private FingerData[] fingers = new FingerData[MAX_FINGERS];

    public delegate void TouchGesture();
    public delegate void TouchPosition(Vector2 startPos, Vector2 endPos);

    /// <summary>
    /// This event executes when the user jumps within a lane.
    /// </summary>
    public static event TouchGesture OnLaneJump
    {
        add    { onLaneJump -= value;
                 onLaneJump += value; }
        remove { onLaneJump -= value; }
    }

    /// <summary>
    /// This event executes when the user rolls within a lane.
    /// </summary>
    public static event TouchGesture OnLaneRoll
    {
        add    { onLaneRoll -= value;
                 onLaneRoll += value; }
        remove { onLaneRoll -= value; }
    }

    /// <summary>
    /// This event executes when the shoots a bullet.
    /// </summary>
    public static event TouchGesture OnShoot
    {
        add    { onShoot -= value;
                 onShoot += value; }
        remove { onShoot -= value; }
    }

    /// <summary>
    /// This event executes when the user holds screen for any amount of time.
    /// </summary>
    public static event TouchPosition OnHold
    {
        add    { onHold -= value;
                 onHold += value; }
        remove { onHold -= value; }
    }

    /// <summary>
    /// Swap jump/roll & shoot controls by switching sides of the screen.
    /// Default: Jump/roll Leftside, Shoot rightside.
    /// </summary>
    public static bool UseReverseControls
    {
        get
        {
            return PlayerPrefs.GetInt("ReverseControls", 0) > 0 ? true : false;
        }
        set
        {
            PlayerPrefs.SetInt("ReverseControls", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public static TouchInput Instance
    {
        get { return instance; }
    }
    private static TouchInput instance;

    // Event backing fields (for explicit add/remove fields).
    private static event TouchGesture onLaneJump;
    private static event TouchGesture onLaneRoll;
    private static event TouchGesture onShoot;
    private static event TouchPosition onHold;

    /// <summary>
    /// KeyCode for rolling action when using the keyboard.
    /// </summary>
    [SerializeField]
    private KeyCode keyCodeRollAction = KeyCode.LeftControl;

    /// <summary>
    /// KeyCode for jump action when using the keyboard.
    /// </summary>
    [SerializeField]
    private KeyCode keyCodeJumpAction = KeyCode.Space;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There are multiple instances of " + typeof(TouchInput).ToString() + " present in the scene!");
            return;
        }

        instance = this;

        for (int i = 0; i < fingers.Length; i++)
            fingers[i] = new FingerData();

        InitInputDevice();
    }

    private void Update()
    {
        #if UNITY_EDITOR || UNITY_STANDALONE
                ProcessMouse();
                ProcessKeyBoard();
        #elif UNITY_ANDROID || UNITY_IOS
                ProcessTouch();
        #else
        #error No input method was written for this platform!
        #endif
    }

    /// <summary>
    /// Cancel all current on-going touch operations.
    /// </summary>
    public static void CancelTouchOperations()
    {
        // Fingers are only enabled at the beginning and ending phase of a touch.
        // So if a finger is disabled while currently in motion, it will not be processed.
        for (int i = 0; i < instance.fingers.Length; i++)
        {
            instance.fingers[i].Enabled = false;
        }
    }

    private void InitInputDevice()
    {
        // Disable simulation, we use either mouse or touchinput.
        Input.simulateMouseWithTouches = false;

        #if UNITY_EDITOR || UNITY_STANDALONE

        if (!Input.mousePresent)
            Debug.LogError("No inputdevice: A mouse is required to play on this platform!");

        #elif UNITY_ANDROID || UNITY_IOS

        if (!Input.touchSupported)
            Debug.LogError("Touch input is not supported on this system!");

        if (!Input.multiTouchEnabled)
            Debug.LogWarning("Multi-touch is not supported on this system!");

        if (Input.touchPressureSupported)
            Debug.Log("Pressure-touch is supported on this system.");

        #endif
    }

    /// <summary>
    /// Process touch-input (this only works on devices with touchscreens).
    /// </summary>
    private void ProcessTouch()
    {
        foreach (Touch touch in Input.touches)
        {
            // Skip input above max allowed fingers.
            if (touch.fingerId > MAX_FINGERS - 1)
                continue;

            // A finger touched the screen (this frame)
            if (touch.phase == TouchPhase.Began)
            {
                fingers[touch.fingerId].InitGesture(Input.mousePosition);
            }

            // A finger moved on the screen
            if (touch.phase == TouchPhase.Moved /* || touch.phase == TouchPhase.Stationary */)
            {
                fingers[touch.fingerId].UpdateGesture(touch.position);
                ProcessGesture(fingers[touch.fingerId], true);
            }

            // A finger was lifted from the screen. (Final phase of touch)
            if (touch.phase == TouchPhase.Ended)
            {
                ProcessGesture(fingers[touch.fingerId], false);
                fingers[touch.fingerId].Reset();
                return;
            }

            // The system cancelled tracking for the touch
            // (Happens when more touches are applied than the system can handle).
            // The exact number varies from platform-to-platform, device-to-device.
            if (touch.phase == TouchPhase.Canceled)
            {
                Debug.LogWarning("User touched too many places at once... (" + Input.touchCount + " places)");
            }
        }
    }

    /// <summary>
    /// Process mouse-input (this only works on devices with a mouse).
    /// </summary>
    private void ProcessMouse()
    {
        foreach (int buttonIndex in new int[] { 0, 1, 2 })
        {
            // Skip input above max allowed buttons.
            if (buttonIndex > MAX_FINGERS - 1)
                continue;

            // A finger touched the screen (this frame)
            if (Input.GetMouseButtonDown(buttonIndex))
            {
                fingers[buttonIndex].InitGesture(Input.mousePosition);
            }

            // A finger moved on the screen (user is holding down)
            if (Input.GetMouseButton(buttonIndex))
            {
                fingers[buttonIndex].UpdateGesture(Input.mousePosition);
                ProcessGesture(fingers[buttonIndex], true);
            }

            // A finger was lifted from the screen (this frame)
            if (Input.GetMouseButtonUp(buttonIndex))
            {
                ProcessGesture(fingers[buttonIndex], false);
                fingers[buttonIndex].Reset();
            }
        }
    }

    /// <summary>
    /// Process keyboard-input (this only works on devices with a keyboard).
    /// </summary>
    private void ProcessKeyBoard()
    {
        if(Input.GetKeyDown(keyCodeRollAction))
        {
            ExecuteEvent(onLaneRoll);
        }

        if (Input.GetKeyDown(keyCodeJumpAction))
        {
            ExecuteEvent(onLaneJump);
        }
    }

    /// <summary>
    /// Process a gesture using start and ending positions of finger or mouse.
    /// </summary>
    private void ProcessGesture(FingerData finger, bool isHoldingScreen)
    {

        if(!finger.Enabled)
        {
            #if VERBOSE_LOGGING
            Debug.Log("Operation was cancelled by UI.");
            #endif
            return;
        }

        // Only allow gestures that weren't previously processed in the same touch.
        if (!finger.IsValidGesture)
            return;

        if (finger.IsSwiping && (finger.StartPoint.IsInScreenRegion(VectorMath.ScreenRegion.Left) ^ UseReverseControls) )
        {
            switch (finger.GetSwipe())
            {
                case FingerData.SwipeDirection.Up:
                    ExecuteEvent(onLaneJump);
                    break;
                case FingerData.SwipeDirection.Down:
                    ExecuteEvent(onLaneRoll);
                    break;
                case FingerData.SwipeDirection.Left:
                    ExecuteEvent(onLaneJump);
                    break;
                case FingerData.SwipeDirection.Right:
                    ExecuteEvent(onLaneRoll);
                    break;
                default:
                    Debug.LogError("SwipeDirection reached the default case!");
                    break;
            }
            finger.Reset();
        }
        else if(!finger.CancelHoldGesture)
        {
            if (finger.StartPoint.IsInScreenRegion(VectorMath.ScreenRegion.Right) ^ UseReverseControls)
            {
                ExecuteEvent(onShoot);
                finger.Reset();
            }

            if (!isHoldingScreen)
            {
                ExecuteEvent(onHold, finger.StartPoint, finger.EndPoint);
                finger.Reset();
            }
        }

        #if VERBOSE_LOGGING
        Debug.Log(
                "A gesture was processed." +
                "     id = " + fingerId +
                ",     t = " + timeHeld.ToString("00.0000") +
                ",     SwipeDirection: " + (isSwiping ? swipeDir.ToString() : "None") +
                "\nStartPos: " + startPos.ToString() + ",  EndPos: " + endPos.ToString()
                );
        #endif
    }

    private void ExecuteEvent(TouchGesture touchEvent)
    {
        if (touchEvent != null)
        {
            touchEvent();
        }
        else
        {
            #if VERBOSE_LOGGING
            Debug.LogWarning("Cannot execute event: " + typeof(TouchGesture).ToString() + " has no subscribers!");
            #endif
        }
    }

    private void ExecuteEvent(TouchPosition touchEvent, Vector2 startPos, Vector2 endPos)
    {
        if (touchEvent != null)
        {
            touchEvent(startPos, endPos);
        }
        else
        {
            #if VERBOSE_LOGGING
            Debug.LogWarning("Cannot execute event: " + typeof(TouchPosition).ToString() + " has no subscribers!");
            #endif
        }
    }
}
