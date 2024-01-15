using UnityEngine;

public class TriggerPainter : MonoBehaviour
{
	public Brush brush;
	public bool RandomChannel = false;

	private void Start() { }

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log(111);
		HandleCollision(other);
	}

	private void OnTriggerStay(Collider other)
	{
		Debug.Log(2222);
		HandleCollision(other);
	}

	private void HandleCollision(Collider collision)
	{
		var contact = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

		PaintTarget paintTarget = collision.GetComponent<PaintTarget>();
		if (paintTarget != null)
		{
			if (RandomChannel) brush.splatChannel = Random.Range(0, 4);
			PaintTarget.PaintObject(paintTarget, contact, contact, brush);
		}
	}
}