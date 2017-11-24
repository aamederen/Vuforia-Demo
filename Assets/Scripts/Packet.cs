using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Packet : MonoBehaviour {

	public const float THRESHOLD = 0.001f;

	public float Speed = 0.2f;
	
	private List<Vector3> path = new List<Vector3>();
	
	public void SetPath(Vector3[] points) {
		path.Clear();
		foreach (var point in points) {
			path.Add(point);
		}
	}

	void Update () {
		Debug.Log("path.count = " + path.Count);
		// do nothing if there are no points in the path for me to follow
		if (path.Count == 0) {
			Destroy(this.gameObject);
			return;
		}

		var nextPoint = path[0];

		if (Vector3.Distance(transform.position, nextPoint) < THRESHOLD) {
			Debug.Log("Checkpoint! path.count = " + path.Count);
			path.RemoveAt(0);
			nextPoint = path.Count == 0 ? new Vector3(0,0,0) : path[0];
		}

		if(nextPoint != null) {
			transform.position = Vector3.MoveTowards(transform.position, 
																							 nextPoint,
																							 Speed * Time.deltaTime);
		}
	}
}
