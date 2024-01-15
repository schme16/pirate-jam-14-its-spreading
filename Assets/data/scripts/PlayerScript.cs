using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
	public float movementSpeed = 0.125f;
	public float maxSpeed = 2f;
	public float jumpSpeed = 5;
	public float minDistBetweenIckBalls = 5;
	public ParticleSystem ps;
	public Transform ickBallPrefab;
	public Transform ickBallHolder;
	public float timeBetweenRays = 0.25f;
	private float time = 0;
	private Camera cam;
	private Rigidbody rb;

	// Start is called before the first frame update
	void Start()
	{
		//Set the camera
		cam = Camera.main;

		//Set the rigidbody
		rb = GetComponent<Rigidbody>();

		//Hide the cursor
		Cursor.visible = false;

		//Lock the cursor to the game window
		Cursor.lockState = CursorLockMode.Locked;
	}

	// Physics updates
	void Update()
	{
		//float x = Input.GetAxis("Horizontal");
		bool jump = Input.GetButtonDown("Jump");
		bool sneak = Input.GetButtonDown("Sneak");
		bool dash = Input.GetButtonDown("Dash");
		float y = Input.GetAxis("Vertical");

		/*if (x != 0)
		{
			transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + (Time.deltaTime * (rotationSpeed * -x)), transform.eulerAngles.z);
		}*/

		transform.eulerAngles = new Vector3(transform.eulerAngles.x, cam.transform.eulerAngles.y, transform.eulerAngles.z);

		if (y != 0)
		{
			var maxV = transform.forward * (maxSpeed * y);
			rb.AddForce(maxV - rb.velocity, ForceMode.Force);
		}

		if (jump)
		{
			rb.AddForce(transform.up * (movementSpeed * jumpSpeed), ForceMode.Impulse);
		}

		if (rb.velocity.magnitude > 1)
		{
			//Debug.Log(rb.velocity.magnitude);
		}
		
		
		

		time += Time.deltaTime;

		if (time > timeBetweenRays)
		{
			time = 0;

			bool ray = Physics.Raycast(transform.position, -transform.up, out var hit, 0.05f);
			if (ray)
			{
				Debug.Log(1);
				if (!hit.transform.CompareTag("IckBall"))
				{
					Debug.Log(2);
					Collider[] hitColliders = Physics.OverlapSphere(transform.position, minDistBetweenIckBalls, LayerMask.GetMask("ick-ball"), QueryTriggerInteraction.Collide);
					bool test = hitColliders.Length > 0;
				
					if (!test)
					{
						IckBallScript ickBall = Instantiate(ickBallPrefab).GetComponent<IckBallScript>();
						ickBall.transform.position = hit.point + (hit.normal * 0.01f);
						ickBall.transform.LookAt(transform.position);
						ickBall.transform.parent = hit.transform;
						ickBall.minDistBetweenIckBalls = minDistBetweenIckBalls;
						ickBall.prefab = ickBallPrefab;
					}
				}
			}
		}
		
		
	}

	private void OnParticleCollision(GameObject other)
	{
		/*Collider[] hitColliders = Physics.OverlapSphere(transform.position, minDistBetweenIckBalls, LayerMask.GetMask("ick-ball"), QueryTriggerInteraction.Collide);
		bool hasHit = false;
		foreach (var hitCollider in hitColliders)
		{
			hasHit = true;
			Debug.Log(hitCollider.name);
		}

		if (!hasHit)
		{
			IckBallScript ickBall = Instantiate(ickBallPrefab).GetComponent<IckBallScript>();
			ickBall.transform.position = transform.position;
			ickBall.transform.parent = other.transform;
		}*/
	}
}