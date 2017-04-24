using UnityEngine;
using UnityEngine.UI;

/// <note>
/// I needed a good way to interact with the inventory. Created minimal interaction.
/// Current implementation uses a Power enum to denote the powerup type.
/// </note>

/// <summary>
/// Abstract Base class for in-game powerups.
/// Contains collision methods and static Inventory communication.
/// </summary>
public abstract class Powerup : MonoBehaviour
{
    /// <summary>
    /// <para>Contains all powerup types in the game. Also used by PlayerPrefs.</para>
    /// <para>Dont initialize a powerup with Power.none, this is used for the UI only.</para>
    /// </summary>
    public enum Power
    {
        none,
        pwrBoost,
        pwrSlowdown,
        pwrWallBreak
    }

    [SerializeField]
    protected Power type;

    /// <summary>
    /// Global definition for collecting powerup.
    /// Overwrite this method if needed.
    /// </summary>
    public void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Player")
            GlobalInventory.Instance.CollectPowerup(type);
    }

    /// <summary>
    /// Returns the correct sprite to the caller (User by inventoryUI).
    /// </summary>
    /// <param name="power">the Type of powerup we want the sprite of.</param>
    public static Sprite GetSprite(Power power)
    {
        switch(power)
        {
            case Power.pwrBoost:
                return Resources.Load<Sprite>("pwr-boost");
            case Power.pwrSlowdown:
                return Resources.Load<Sprite>("pwr-slowdown");
            case Power.pwrWallBreak:
                return Resources.Load<Sprite>("pwr-wallbreak");
            default:
                return null;
        }
    }

    private void Awake()
    {
        if (type == Power.none)
            Debug.LogError("Cannot initialize a powerup in Power.none state!");
    }
}

/// <summary>
/// Example implementation of powerup.
/// </summary>
public sealed class WallBreak : Powerup
{
}