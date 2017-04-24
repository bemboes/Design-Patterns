using UnityEngine;

/// <devnote>
/// Let's see. There are different moments when the input detection should occur.
/// - Start of touch initiates a hold.
/// - Letting go of touch initiates a click
/// - Swipe should occur irregardless of letting go.
///     - Cannot swipe twice in a single gesture
/// - Can rely on less delegates and events
/// </devnote>

/// <summary>
/// Contains all input data related to a single press (finger or mouse).
/// </summary>
public class FingerData
{

    public enum SwipeDirection
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3,
    }

    public static float minSwipeLengthCM = 0.5f;
    public static float minLongSwipeLengthCM = 1.0f;

    public bool Enabled = true;

    public bool IsSwiping
    {
        get { return GetSwipeLengthInCM() > minSwipeLengthCM; }
    }

    /// <summary>
    /// Ignore the hold gesture if too much displacement was detected.
    /// </summary>
    public bool CancelHoldGesture
    {
        get { return GetSwipeLengthInCM() > minLongSwipeLengthCM; }
    }

    /// <summary>
    /// Returns whether the gesture was initialized with InitGesture().
    /// Prevents multiple gestures in a single touch operation.
    /// </summary>
    public bool IsValidGesture
    {
        get;
        private set;
    }

    public Vector2 StartPoint
    {
        get;
        private set;
    }
    public Vector2 EndPoint
    {
        get;
        private set;
    }
    public float RealTimeStamp
    {
        get;
        private set;
    }

    public FingerData()
    {
        Reset();
    }

    public void Reset()
    {
        Enabled = true;
        StartPoint = Vector2.zero;
        EndPoint = Vector2.zero;
        RealTimeStamp = 0;
        IsValidGesture = false;
    }

    public void InitGesture(Vector2 pos)
    {
        Enabled = true;
        StartPoint = pos;
        EndPoint = pos;
        RealTimeStamp = Time.realtimeSinceStartup;
        IsValidGesture = true;
    }

    public void UpdateGesture(Vector2 pos)
    {
        EndPoint = pos;
    }

    public float GetTimeHeld()
    {
        return Time.realtimeSinceStartup - RealTimeStamp;
    }

    public float GetSwipeLengthInCM()
    {
        return VectorMath.ConvertPixelToCm((EndPoint - StartPoint).magnitude);
    }

    public SwipeDirection GetSwipe()
    {
        // Calculates the closest SwipeDirection by looking at the swipeAngle.
        float swipeAngle = VectorMath.AcuteAngle(Vector2.up, (EndPoint - StartPoint).normalized, true);
        SwipeDirection swipeDir = (SwipeDirection)(Mathf.RoundToInt(swipeAngle / 90f) % 4);
        return swipeDir;
    }
}
