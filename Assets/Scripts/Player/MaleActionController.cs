using UnityEngine;
using System.Collections;

/// <note>
/// Nick: I realize now the MaleActionController could be merged very well
/// with MaleMechanics. Decided to leave this for later as MaleMechanics
/// is effectively a copy of the old player behaviour (easier testing).
/// </note>

/// <summary>
/// A substitute class to control and test MaleMechanics functionalities.
/// </summary>
public class MaleActionController : MonoBehaviour
{
	private ThirdPersonCharacter thirdPersonChar;

	private bool userWantsToJump = false;
	private bool userWantsToRoll = false;

	private void Awake ()
	{
		thirdPersonChar = GetComponent<ThirdPersonCharacter> ();

		if (thirdPersonChar == null)
			Debug.LogError ("thirdPersonChar is null! Cant play game!");
	}

	private void OnEnable ()
	{
		// Subscribe to TouchInput module.
		TouchInput.OnLaneJump += DoJump;
		TouchInput.OnLaneRoll += DoABarrelRoll;
	}

	private void OnDisable ()
	{
		// Unsubscribe to TouchInput module.
		TouchInput.OnLaneJump -= DoJump;
		TouchInput.OnLaneRoll -= DoABarrelRoll;
	}

	private void FixedUpdate ()
	{
		// pass all parameters to the character control script
		thirdPersonChar.Move (transform.forward, userWantsToRoll, userWantsToJump);

		userWantsToJump = false;
		userWantsToRoll = false;
	}

	private void DoJump ()
	{
		// Little silly, but update needs to be in fixed!
		userWantsToJump = true;
	}

	private void DoABarrelRoll ()
	{
		// Little silly, but update needs to be in fixed!
		userWantsToRoll = true;
	}
}
