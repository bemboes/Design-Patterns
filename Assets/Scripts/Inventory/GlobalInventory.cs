using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <note>
/// Explanation of Inventory components:
/// ------------------------------------
/// The Inventory is split into two seperate components: LevelInventory and GlobalInventory.
/// Essentially it works as follows: No changes are made to the GlobalInventory till either a
/// powerup is picked up or spent. The LevelInventory only holds the slots, graphics and interactions.
/// </note>

/// <summary>
/// Backend part of the in-game inventory.
/// Handles all GlobalInventory interaction.
/// </summary>
public class GlobalInventory : MonoBehaviour
{
    public static GlobalInventory Instance
    {
        get { return instance; }
    }

    private static GlobalInventory instance;

    /// Small test for collecting powerups
    //private IEnumerator TestPowerupCollect()
    //{
    //    for (;;)
    //    {
    //        yield return new WaitForSeconds(4.0f);
    //        CollectPowerup(Powerup.Power.pwrBoost);
    //    }
    //}

    private void Awake ()
    {
        if (instance != null)
        {
            Debug.LogError("There are multiple instances of " + typeof(GlobalInventory).ToString() + " present in the scene!");
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        //StartCoroutine(TestPowerupCollect());
	}

    /// <summary>
    /// Returns the amount of a particular powerup.
    /// </summary>
    public int GetPowerupAmount(Powerup.Power ptype)
    {
        string powerupName = ptype.ToString();
        if (PlayerPrefs.HasKey(powerupName))
        {
            return PlayerPrefs.GetInt(powerupName, 0);
        }
        else
        {
            Debug.LogWarning("Powerup type: " + powerupName + " does not exist in the PlayerPrefs");
            PlayerPrefs.SetInt(powerupName, 0);
        }

        return 0;
    }

    /// <summary>
    /// Collect a powerup (anywhere in-game) and add it to the GlobalInventory (and LevelInventory).
    /// </summary>
    public void CollectPowerup(Powerup.Power ptype)
    {
        // Pass the powerup to the level inventory.
        LevelInventory.Instance.AddPowerupToUI(ptype);

        string powerupName = ptype.ToString();
        if (PlayerPrefs.HasKey(powerupName))
        {
            int powerupCount = PlayerPrefs.GetInt(powerupName);
            PlayerPrefs.SetInt(powerupName, powerupCount + 1);
        }
        else
        {
            PlayerPrefs.SetInt(powerupName, 1);
        }

        // Only save collecting powerups after finishing level??
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Spend a powerup from the GlobalInventory. (Called when user clicks a powerup slot).
    /// </summary>
    /// <param name="ptype"></param>
    public void SpendPowerup(Powerup.Power ptype)
    {
        string powerupName = ptype.ToString();
        if (PlayerPrefs.HasKey(powerupName))
        {
            int powerupCount = PlayerPrefs.GetInt(powerupName);
            if (powerupCount < 1)
                Debug.LogError("Trying to spend " + powerupName + " powerup, but the user doesn't have any!");
            else
                PlayerPrefs.SetInt(powerupName, powerupCount - 1);
        }
        else
        {
            Debug.LogError("Trying to spend " + powerupName + " powerup, but it doesn't exist in PlayerPrefs!");
        }
        
        // Do we want to allow retry with all three powerups if one was already spent??
        PlayerPrefs.Save();
    }
}