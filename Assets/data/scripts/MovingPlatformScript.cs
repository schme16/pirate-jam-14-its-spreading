using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{
	public Vector3 pos;
	public float duration;
	public Transform _t;
	public Vector3 startPos;
	public bool direction;
	public float timer;

	void Start()
	{
		timer = duration;
		startPos = _t.localPosition;
	}

	// Update is called once per frame
	void Update()
	{
		_t.localPosition = Vector3.Lerp(pos, startPos, timer / duration);
		if (timer < 0 || timer > duration)
		{
			direction = !direction;
		}

		if (direction)
		{
			timer += Time.deltaTime;
		}
		else
		{
			timer -= Time.deltaTime;
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(transform.position + pos, 0.1f);
	}
}