using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using Unity.VisualScripting;
using UnityEngine;

public class IckBallScript : MonoBehaviour
{
	public Rigidbody rb;
	public Brush brush;
	public Brush brushSmaller;
	public Transform prefab;
	public float timeBetweenRays = 2.25f;
	public Ray ray;
	private float time = 0;
	public float minDistBetweenIckBalls;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();

		/*Collider[] hitColliders = Physics.OverlapSphere(transform.position, minDistBetweenIckBalls, LayerMask.GetMask("ick-ball"), QueryTriggerInteraction.Collide);
		if (hitColliders.Length > 0)
		{
			bool test = false;
			foreach (var hc in hitColliders)
			{
				if (hc.transform.parent == transform.parent)
				{
					test = true;
				}
			}

			if (test)
			{
				DestroyImmediate(gameObject);
			}
		}
		else
		{
			FireRay(-1, brush);
		}*/
		FireRay(-1, brush);
	}

	private void Update()
	{
		time += Time.deltaTime;
		
		if (time > timeBetweenRays)
		{
			time = 0;
			RaycastHit hit;

			if (Physics.Raycast(transform.position, transform.up, out hit, 0.025f))
			{
				Debug.DrawRay(transform.position, transform.up * hit.distance, Color.yellow, 5f);

				if (!hit.transform.CompareTag("IckBall"))
				{
					Collider[] hitColliders = Physics.OverlapSphere(hit.transform.position, minDistBetweenIckBalls, LayerMask.GetMask("ick-ball"), QueryTriggerInteraction.Collide);
					bool test = false;
					if (hitColliders.Length > 0)
					{
						Debug.Log(3);
						foreach (var hc in hitColliders)
						{
							if (hc.transform.parent == transform.parent)
							{
								test = true;
							}
						}
					}

					if (!test)
					{
						Debug.Log(4);
						IckBallScript ickBall = Instantiate(prefab).GetComponent<IckBallScript>();
						ickBall.transform.position = hit.point;
						ickBall.transform.parent = hit.transform;
						ickBall.minDistBetweenIckBalls = minDistBetweenIckBalls;
						ickBall.prefab = prefab;
					}
				}
			}
			else
			{
				Debug.DrawRay(transform.position, transform.up * 0.025f, Color.white, 1f);
				Debug.Log("Did not Hit");
			}
		}
	}

	public void FireRay(int direction, Brush b)
	{
		ray = new Ray(transform.position, transform.up * direction);
		PaintTarget.PaintRaycast(ray, b, false, 0.25f);
	}


	/*private void OnTriggerEnter(Collider other)
	{
		if (!other.transform.CompareTag("IckBall"))
		{
			IckBallScript ickBall = Instantiate(prefab).GetComponent<IckBallScript>();
			ickBall.transform.position = transform.position;
			ickBall.transform.parent = other.transform;
			ickBall.minDistBetweenIckBalls = minDistBetweenIckBalls;
			ickBall.prefab = prefab;
			Debug.Log(other.name);
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		Debug.Log(222);
		/*IckBallScript ickBall = Instantiate(gameObject).GetComponent<IckBallScript>();
		ickBall.transform.position = transform.position;
		ickBall.transform.parent = other.transform;
		ickBall.minDistBetweenIckBalls = minDistBetweenIckBalls;#1#
	}*/
}