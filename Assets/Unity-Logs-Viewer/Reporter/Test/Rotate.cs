using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour
{
	Vector3 angle;

	void Start()
	{
		angle = transform.localEulerAngles;
	}

	void Update()
	{
		angle.y += Time.deltaTime * 100;
		transform.localEulerAngles = angle;
	}

}
