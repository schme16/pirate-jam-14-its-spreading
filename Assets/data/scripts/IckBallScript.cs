using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IckBallScript : MonoBehaviour
{
	public Rigidbody rb;
	public Brush brush;
	public Brush brushSmaller;
	public Transform prefab;
	public ParticleSystem ps;
	public float timeBetweenRays = 2.25f;
	public Ray ray;
	private float time = 0;
	public float minDistBetweenIckBalls = 5;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		ps = GetComponent<ParticleSystem>();
		FireRay(-1, brush);
	}

	private void Update() { }

	public void FireRay(int direction, Brush b)
	{
		ray = new Ray(transform.position, transform.up * direction);
		PaintTarget.PaintRaycast(ray, b, false, 0.25f);
	}

	/*private void OnParticleCollision(GameObject other)
	{
		if (other.CompareTag("Paintable"))
		{
			int numCollisionEvents = ps.GetCollisionEvents(other, collisionEvents);
			if (numCollisionEvents > 0 && !other.CompareTag("IckBall"))
			{
				ParticleCollisionEvent col = collisionEvents[0];

				Collider[] hitColliders = Physics.OverlapSphere(transform.position, minDistBetweenIckBalls, LayerMask.GetMask("ick-ball"), QueryTriggerInteraction.Collide);
				bool hasHit = false;
				for (int i = 0; i < hitColliders.Length; i++)
				{
					if (hitColliders[i].transform.parent == other.transform)
					{
						hasHit = true;
						Debug.Log(hitColliders[i].name);
						break;
					}
				}


				if (!hasHit)
				{
					//Transform ickBall = Instantiate(prefab, other.transform, true);
					//ickBall.transform.position = col.intersection; /* + (hit.normal * 0.01f)#1#
					PaintTarget.PaintObject(other.GetComponent<PaintTarget>(), col.intersection, col.normal, brush);
				}
			}
		}
	}
	*/

	void TrySpawnIck(Collider other)
	{
		if (other.CompareTag("Paintable"))
		{
			Vector3 point = other.ClosestPoint(transform.position);
			Collider[] hitColliders = Physics.OverlapSphere(point, minDistBetweenIckBalls, LayerMask.GetMask("ick-ball"), QueryTriggerInteraction.Collide);
			bool hasHit = false;
			for (int i = 0; i < hitColliders.Length; i++)
			{
				if (hitColliders[i].transform.parent == other.transform)
				{
					hasHit = true;
					//Debug.Log(hitColliders[i].name);
					break;
				}
			}


			if (!hasHit)
			{
				Transform ickBall = Instantiate(prefab, other.transform, true);
				ickBall.position = point;
				ickBall.parent = other.transform;
				bool ray = Physics.Raycast(point, (transform.position - point).normalized, out var hit, 1f, ~(1 << 7));
				if (ray)
				{
					PaintTarget.PaintObject(other.GetComponent<PaintTarget>(), hit.point, hit.normal, brush);
				}
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("IckBall"))
		{
			TrySpawnIck(other);
		}
	}

	private void OnTriggerStay(Collider other)
	{

		if (!other.CompareTag("IckBall"))
		{
			TrySpawnIck(other);
		}
	}
}