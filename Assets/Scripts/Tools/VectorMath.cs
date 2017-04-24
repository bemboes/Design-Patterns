using UnityEngine;
using System.Collections;

public static class VectorMath
{
    public enum ScreenRegion
    {
        Left,
        Right
    }

    public static bool IsInScreenRegion(this Vector2 pos, ScreenRegion region)
    {
        switch (region)
        {
            case ScreenRegion.Left:
                if (pos.x <= (Screen.width / 2f))
                    return true;
                break;
            case ScreenRegion.Right:
                if (pos.x >= (Screen.width / 2f))
                    return true;
                break;
        }

        return false;
    }


    /// <summary>
    /// Calculates the angle between two Vector2's in 180 or 360 degrees. (Clockwise direction)
    /// </summary>
    public static float AcuteAngle(Vector2 fromVector2, Vector2 toVector2, bool degreeIn360)
    {
        float ang = Vector2.Angle(fromVector2, toVector2);
        Vector3 cross = Vector3.Cross(fromVector2, toVector2);

        if (degreeIn360 && cross.z > 0)
            ang = 360 - ang;

        return ang;
    }

    /// <summary>
    /// Converts centimeters into Pixels using the Destination DPI.
    /// </summary>
    public static float ConvertCmToPixel(float cm)
    {
        /// Screen DPI may be incorrect!: http://forum.unity3d.com/threads/screen-dpi-on-android.414014/
        if (Screen.dpi == 0)
        {
            Debug.LogError("Device DPI could not be determined!");
        }
        float nPixels = (cm / 2.54f) * Screen.dpi;
        return nPixels;
    }

    /// <summary>
    /// Converts pixels into centimeters using the Destination DPI.
    /// </summary>
    public static float ConvertPixelToCm(float px)
    {
        /// Screen DPI may be incorrect!: http://forum.unity3d.com/threads/screen-dpi-on-android.414014/
        if (Screen.dpi == 0)
        {
            Debug.LogError("Device DPI could not be determined!");
        }
        float cm = (px / Screen.dpi) * 2.54f;
        return cm;
    }
}
