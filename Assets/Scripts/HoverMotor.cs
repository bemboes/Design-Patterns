using UnityEngine;
using System.Collections;

public class HoverMotor : MonoBehaviour
{

	public float speed = 90f;
	public float turnSpeed = 5f;
	public float hoverForce = 65f;
	public float hoverHeight = 3.5f;
	private float powerInput;
	private float turnInput;
	private Rigidbody bulletRigidbody;


	void Awake ()
	{
		bulletRigidbody = GetComponent <Rigidbody> ();
	}

	void Update ()
	{
		powerInput = Input.GetAxis ("Vertical");
		turnInput = Input.GetAxis ("Horizontal");
	}

	void FixedUpdate ()
	{
		Ray ray = new Ray (transform.position, -transform.up);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, hoverHeight)) {
			float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
			Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
			bulletRigidbody.AddForce (appliedHoverForce, ForceMode.Acceleration);
		}

		bulletRigidbody.AddRelativeForce (0f, 0f, powerInput * speed);
		bulletRigidbody.AddRelativeTorque (0f, turnInput * turnSpeed, 0f);

	}
}