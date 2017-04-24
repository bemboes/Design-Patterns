using UnityEngine;
using System.Collections;

/// <summary>
/// Simple global class for global declaration of LanePositions.
/// Needed something for LaneSwitch. What is a good design for determining player positions?
/// This implementation is a little broken. More than two lanes not yet supported.
/// Some sort of level class should be created before this can be made sensibly.
/// </summary>
public class LaneGeometry : MonoBehaviour
{

    public float LowLanePos
    {
        get { return lowLaneHeight; }
    }

    public float HighLanePos
    {
        get { return highLaneHeight; }
    }

    public static LaneGeometry Instance
    {
        get { return instance; }
    }

    public enum SwitchDirection
    {
        Up,
        Down
    }

    public enum PlayerLane
    {
        Low = 0,
        High = 1
    }

    public PlayerLane playerLane;

    // Make sure this number matches the amount of PlayerLane values!
    private int nLanes = 2;

    [SerializeField]
    private float lowLaneHeight;
    [SerializeField]
    private float highLaneHeight;
    
    private static LaneGeometry instance;

    private void Awake ()
    {
        if (instance != null)
        {
            Debug.LogError("There are multiple instances of " + typeof(LaneGeometry).ToString() + " present in the scene!");
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.left * 100 + Vector3.up * lowLaneHeight, Vector3.right * 100 + Vector3.up * lowLaneHeight);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(Vector3.left * 100 + Vector3.up * highLaneHeight, Vector3.right * 100 + Vector3.up * highLaneHeight);
    }
    #endif

    public bool ChangeLane(SwitchDirection dir, ref float targetHeight)
    {
        if (dir == SwitchDirection.Up)
        {
            if ((int)playerLane + 1 < nLanes)
            {
                playerLane += 1;
                targetHeight = highLaneHeight;
                return true;
            }
        }

        if (dir == SwitchDirection.Down)
        {
            if ((int)playerLane - 1 >= 0)
            {
                playerLane -= 1;
                targetHeight = lowLaneHeight;
                return true;
            }
        }

        return false;
    }
}
