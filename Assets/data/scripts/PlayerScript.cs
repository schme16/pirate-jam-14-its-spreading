using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using MoreMountains.Tools;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
	public float gravityValue = -9.81f;
	public float playerSpeed = 2.0f;
	public float jumpHeight = 1.0f;
	public float minDistBetweenIckBalls = 5;
	public float rotationSpeed = 1;
	public Transform ickBallPrefab;
	public float timeBetweenRays = 0.25f;
	public LayerMask climbableLayers;
	public CinemachineFreeLook free;
	private float time = 0;
	private float idleVal = 0;
	private float meltVal = -1.35f;
	private float currentVal = 0;
	private Camera cam;
	public bool ascending = false;
	private Action delayedAction;
	private bool canAscend = false;
	public CharacterController controller;
	private Vector3 playerVelocity;
	private bool groundedPlayer;
	public Animator anim;
	private static readonly int ManualControl = Shader.PropertyToID("_ManualControl");


	//UI
	public GameObject uiAscendText;


	// Start is called before the first frame update
	void Start()
	{
		//Set the camera
		cam = Camera.main;

		//Hide the cursor
		Cursor.visible = false;

		//Lock the cursor to the game window
		Cursor.lockState = CursorLockMode.Locked;

		controller = GetComponent<CharacterController>();
	}

	// Physics updates
	void Update()
	{
		float x = Input.GetAxisRaw("Horizontal");
		bool jump = Input.GetButtonDown("Jump");
		bool sneak = Input.GetButtonDown("Sneak");
		bool dash = Input.GetButtonDown("Dash");
		bool climb = Input.GetButtonDown("Climb");
		float y = Input.GetAxisRaw("Vertical");
		canAscend = false;


		//MOVEMENT
		//Don't allow movement if ascending
		if (!ascending)
		{
			currentVal += Time.deltaTime;

			var transform1 = transform;
			var eulerAngles = transform1.eulerAngles;
			eulerAngles = new Vector3(eulerAngles.x, cam.transform.eulerAngles.y, eulerAngles.z);
			transform1.eulerAngles = eulerAngles;

			groundedPlayer = controller.isGrounded;
			if (groundedPlayer && playerVelocity.y < 0)
			{
				playerVelocity.y = 0f;
			}

			//Vector3 move = Time.deltaTime * playerSpeed * (new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"))) ;
			controller.Move(transform1.forward * (y * playerSpeed * Time.deltaTime));

			if (x is < 0 or > 0)
			{
				free.m_XAxis.Value += (x * rotationSpeed * Time.deltaTime);

				//transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
			}


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

				bool ray = Physics.Raycast(transform1.position, -transform1.up, out var hit, 0.05f);
				if (ray)
				{
					if (!hit.transform.CompareTag("IckBall"))
					{
						Collider[] hitColliders = Physics.OverlapSphere(transform1.position, minDistBetweenIckBalls, LayerMask.GetMask("ick-ball"), QueryTriggerInteraction.Collide);
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


			//First see if there's a thing above us
			bool climbRay = Physics.Raycast(transform.position, transform1.up, out var climbHit, 9999, climbableLayers);
			if (climbRay)
			{
				//Fire one more ray just a little above the object to get the "top" object, minus some gaps allowance
				bool climbToTopRay = Physics.Raycast(climbHit.point + (transform1.up * 5f), -transform1.up, out var topHit, 50f, climbableLayers);
				if (climbToTopRay)
				{
					canAscend = true;


					//are we on the ground, and did we ask to climb?
					if (controller.isGrounded && climb)
					{
						ascend(transform1.position, topHit.point);
					}
				}
			}
		}
		else
		{
			currentVal -= Time.deltaTime;
		}


		//Clamp the animation
		currentVal = Mathf.Clamp(currentVal, meltVal, idleVal);

		//Show/hide the ui for ascending 
		uiAscendText.SetActive(canAscend);

		//Sync the current melt state
		anim.SetBool("melt", ascending);
	}

	void ascend(Vector3 from, Vector3 to, bool playEffect = true)
	{
		Debug.Log("Spawn ascend effect at origin");

		//Prevent multiple teleports getting queued
		ascending = true;

		SetTimeout(() =>
		{
			Debug.Log("Spawn ascend effect destination");
			transform.position = to;
			ascending = false;
			delayedAction = null;
		}, 0.5f);
	}

	private void SetTimeout(Action action, float delayInSeconds)
	{
		delayedAction = action;
		Invoke(nameof(ExecuteDelayedAction), delayInSeconds);
	}

	// Named method to execute the anonymous function
	private void ExecuteDelayedAction()
	{
		if (delayedAction != null)
		{
			// This method will be called after the delay, invoking the anonymous function
			delayedAction.Invoke();
		}
	}
}