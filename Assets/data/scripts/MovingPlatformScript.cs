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

		_t.localPosition = Vector3.Lerp(pos, startPos, timer/duration);
		if (timer < 0 || timer > duration)
		{
			Debug.Log(0);
			direction = !direction;
		}

		if (direction)
		{
			Debug.Log(1);
			timer += Time.deltaTime;
		}
		else
		{
			Debug.Log(2);
			timer -= Time.deltaTime;
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(transform.position + pos, 0.1f);
	}
}