using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// UI implementations of the LevelInventory (Singleton).
/// !! Only collect/spend powerups using the GlobalInventory script !!
/// </summary>
public class LevelInventory : TouchMonoBehaviour
{

	public static LevelInventory Instance {
		get { return instance; }
	}

	private static LevelInventory instance;

	/// <summary>
	/// Object references to the inventory sprite slots.
	/// </summary>
	[SerializeField]
	private Image[] spriteSlots;

	/// <summary>
	/// The powerups that are currently usable by the player.
	/// </summary>
	private Powerup.Power[] pwrInventory;

	private void Awake ()
	{
		if (instance != null) {
			Debug.LogError ("There are multiple instances of " + typeof(LevelInventory).ToString () + " present in the scene!");
			return;
		}

		instance = this;
		DontDestroyOnLoad (gameObject);

		if (spriteSlots.Length == 0)
			Debug.LogError ("Inventory slotsize == 0, Powerups Disabled!");

		pwrInventory = new Powerup.Power[spriteSlots.Length];

		ClearInventory ();
	}

	/// <summary>
	/// Public OnClick() method that triggers when the user presses on of the powerup slots.
	/// </summary>
	/// <param name="nSlot">The slot number that was clicked (0 - 2)</param>
	public void OnSlotClick (int nSlot)
	{
		try {
			// Cancel other touch so you don't acidentally perform a player action.
			TouchInput.CancelTouchOperations ();

			if (pwrInventory [nSlot] != Powerup.Power.none) {
				// Use the powerup
				Debug.Log ("Using powerup: " + pwrInventory [nSlot].ToString ());
				GlobalInventory.Instance.SpendPowerup (pwrInventory [nSlot]);

				pwrInventory [nSlot] = Powerup.Power.none;
				OnInventoryChange ();
			} else
				Debug.Log ("Powerup slot (" + nSlot + ") is empty.");
		} catch (System.Exception e) {
			Debug.LogError (e.Message + ": Trying to use powerup slot that doesn't exist!");
		}

	}

	/// <summary>
	/// Tries to add an item to the LevelInventory. Nothing happens if Inventory is already full.
	/// </summary>
	/// <param name="power">The powerup type to add to the inventory.</param>
	public void AddPowerupToUI (Powerup.Power power)
	{
		for (int i = 0; i < pwrInventory.Length; i++)
			if (pwrInventory [i] == Powerup.Power.none) {
				pwrInventory [i] = power;
				OnInventoryChange ();
				return;
			}

		Debug.Log ("Inventory is already full!");
	}

	/// <summary>
	/// Should be called everytime LevelInventory changes. Updates the Sprite slots.
	/// </summary>
	private void OnInventoryChange ()
	{
		for (int i = 0; i < spriteSlots.Length; i++) {
			spriteSlots [i].sprite = Powerup.GetSprite (pwrInventory [i]);

			// Set sprite alpha to zero or white block will be shown if sprite is null
			if (spriteSlots [i].sprite == null)
				spriteSlots [i].color = new Color (1, 1, 1, 0);
			else
				spriteSlots [i].color = new Color (1, 1, 1, 1);
		}
	}

	/// <summary>
	/// Empty all contents in LevelInventory. (Doesn't affect unused powerups).
	/// </summary>
	private void ClearInventory ()
	{
		for (int i = 0; i < pwrInventory.Length; i++)
			pwrInventory [i] = Powerup.Power.none;

		OnInventoryChange ();
	}

}
