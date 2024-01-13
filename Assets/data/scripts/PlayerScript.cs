using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
	public float movementSpeed = 0.4f;
	public float jumpSpeed = 1;

	private Camera cam;
	private Rigidbody rb;

	// Start is called before the first frame update
	void Start()
	{
		cam = Camera.main;
		rb = GetComponent<Rigidbody>();
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	// Update is called once per frame
	void Update()
	{
		//float x = Input.GetAxis("Horizontal");
		bool jump = Input.GetButtonDown("Jump");
		float y = Input.GetAxis("Vertical");

		/*if (x != 0)
		{
			transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + (Time.deltaTime * (rotationSpeed * -x)), transform.eulerAngles.z);
		}*/

		transform.eulerAngles = new Vector3(transform.eulerAngles.x, cam.transform.eulerAngles.y, transform.eulerAngles.z);
		
		if (y != 0)
		{
			rb.AddForce(transform.forward * (movementSpeed * y), ForceMode.VelocityChange);
		}
		
		if (jump)
		{
			rb.AddForce(transform.up * (movementSpeed * jumpSpeed), ForceMode.Impulse);
		}
	}
}