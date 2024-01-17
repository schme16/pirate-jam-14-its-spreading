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


	public CharacterController controller;
	private Vector3 playerVelocity;
	private bool groundedPlayer;
	private float playerSpeed = 2.0f;
	private float jumpHeight = 1.0f;
	private float gravityValue = -9.81f;


	// Start is called before the first frame update
	void Start()
	{
		//Set the camera
		cam = Camera.main;

		//Set the rigidbody
		rb = GetComponent<Rigidbody>();
		controller = GetComponent<CharacterController>();

		//Hide the cursor
		Cursor.visible = false;

		//Lock the cursor to the game window
		Cursor.lockState = CursorLockMode.Locked;
	}

	// Physics updates
	void Update()
	{
		float x = Input.GetAxisRaw("Horizontal");
		bool jump = Input.GetButtonDown("Jump");
		bool sneak = Input.GetButtonDown("Sneak");
		bool dash = Input.GetButtonDown("Dash");
		float y = Input.GetAxisRaw("Vertical");

		/*if (x != 0)
		{
			transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + (Time.deltaTime * (rotationSpeed * -x)), transform.eulerAngles.z);
		}*/

		transform.eulerAngles = new Vector3(transform.eulerAngles.x, cam.transform.eulerAngles.y, transform.eulerAngles.z);

		/*if (y != 0)
		{
			var maxV = transform.forward * (maxSpeed * y);
			rb.AddForce(maxV - rb.velocity, ForceMode.Force);
		}

		if (jump && !isJumping)
		{
			rb.AddForce(transform.up * (movementSpeed * jumpSpeed), ForceMode.Impulse);
		}

		if (rb.velocity.magnitude > 1)
		{
			//Debug.Log(rb.velocity.magnitude);
		}*/

		
		
		
		
		groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        //Vector3 move = Time.deltaTime * playerSpeed * (new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"))) ;
        controller.Move(transform.forward * (y * playerSpeed * Time.deltaTime));

        /*if (move != Vector3.zero)
        {
            //gameObject.transform.forward = move;
        }*/

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
		
		
		

		time += Time.deltaTime;

		if (time > timeBetweenRays)
		{
			time = 0;

			bool ray = Physics.Raycast(transform.position, -transform.up, out var hit, 0.05f);
			if (ray)
			{
				if (!hit.transform.CompareTag("IckBall"))
				{
					Collider[] hitColliders = Physics.OverlapSphere(transform.position, minDistBetweenIckBalls, LayerMask.GetMask("ick-ball"), QueryTriggerInteraction.Collide);
					bool hasHit = false;
					for (int i = 0; i < hitColliders.Length; i++)
					{
						if (hitColliders[i].transform.parent == hit.transform)
						{
							hasHit = true;
							break;
						}
					}


					if (!hasHit)
					{
						Transform ickBall = Instantiate(ickBallPrefab, hit.transform, true);
						ickBall.transform.position = hit.point; /* + (hit.normal * 0.01f)*/
						;
						//ickBall.transform.LookAt(transform.position);
					}
				}
			}
		}
	}
}