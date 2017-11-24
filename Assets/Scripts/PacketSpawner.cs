using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketSpawner : MonoBehaviour {

	public Transform PacketObject = null;
	public float TimeToCreatePacket = 0.5f;
	public float PacketSpeedMin = 1.0f;
	public float PacketSpeedMax = 1.5f;

	private GameObject PacketsContainer = null;
	private List<Vector3[]> Paths = new List<Vector3[]>();

	private float LastPacketCreatedAt = 0.0f;
	private int NextPath = 0;

	// Use this for initialization
	void Start () {
		PacketsContainer = GameObject.Find("Packets");
		
		var linesContainer = GameObject.Find("Lines");
		var lineRendererComponents = linesContainer.GetComponentsInChildren<LineRenderer>();

		foreach (LineRenderer lineRenderer in lineRendererComponents) {
			int pointCount = lineRenderer.positionCount;
			Vector3[] points = new Vector3[pointCount];
			lineRenderer.GetPositions(points);
			for(int i = 0; i < points.Length; i++)
			{
				Debug.Log("Point before: " + points[i]);
				points[i] = lineRenderer.transform.TransformPoint(points[i]);
				Debug.Log("Point after: " + points[i]);
			}
			Paths.Add(points);
		}

		LastPacketCreatedAt = Time.time;

		Debug.Log("Initialized PacketSpawner");
	}
	
	// Update is called once per frame
	void Update () {
		if (PacketObject == null)
		{
			return;
		}

		float now = Time.time;
		if (now > LastPacketCreatedAt + TimeToCreatePacket)
		{
			var path = Paths[NextPath];

			var packetObject = Instantiate(PacketObject, path[0], Quaternion.identity);
			var packet = packetObject.GetComponent<Packet>();

			packetObject.transform.parent = PacketsContainer.transform;
			packet.SetPath(path);
			packet.Speed = Random.Range(PacketSpeedMin, PacketSpeedMax);
			NextPath = (NextPath + 1) % Paths.Count;
			LastPacketCreatedAt = now;	
		}
	}
}
