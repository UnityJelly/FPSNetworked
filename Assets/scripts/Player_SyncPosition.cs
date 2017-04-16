using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

[NetworkSettings (channel = 0, sendInterval = 0.1f)] //0.1
public class Player_SyncPosition : NetworkBehaviour {

[SyncVar (hook = "SyncPositionValues")]
private Vector3 syncPos;

[SerializeField] Transform myTransform;
	private float lerpRate;
	private float normalLerpRate = 300; //30
	private float fasterLerpRate = 500; //50

	private Vector3 lastPos;
	private float threshold = 0.7f; // 0.7 default

	private NetworkClient nClient;
	private int latency;
	private Text latencyText;

	private List<Vector3> syncPosList = new List<Vector3>();
	[SerializeField] private bool useHistoricalLerping = false;
	private float closeEnough = 0.15f; //Range in which historical position (client) has to get before waypoint changes for next position (Next historical waypoint) 0.15f default

	void Start ()
	{
		nClient = GameObject.Find("Network Manager").GetComponent<NetworkManager>().client;
		latencyText = GameObject.Find("Latency Text").GetComponent<Text>();
		lerpRate = normalLerpRate;
	}
	
	// Update is called once per frame
	void Update () 
	{
		LerpPosition();
		ShowLatency();
	}

	void FixedUpdate () 
	{
		TransmitPosition();
	}

	void LerpPosition ()
	{
		if(!isLocalPlayer)
		{
		if(useHistoricalLerping)
			{
				HistoricalLerping();
			}
			else
			{
				OrdinaryLerping();
			}
		}
	}

	[Command]
	void CmdProvidePositionToServer (Vector3 pos)
	{
		syncPos = pos;	
	}

	[ClientCallback]
	void TransmitPosition ()
	{
		if (isLocalPlayer && Vector3.Distance(myTransform.position, lastPos) > threshold)
		{
		CmdProvidePositionToServer(myTransform.position);
		lastPos = myTransform.position;
		}
	}

	[Client]
	void SyncPositionValues (Vector3 latestPos)
	{
		syncPos = latestPos;
		syncPosList.Add(syncPos);
	}

	void ShowLatency ()
	{
		if(isLocalPlayer)
		{
			latency = nClient.GetRTT();
			latencyText.text = latency.ToString();
		}
	}

	void OrdinaryLerping ()
	{
		myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
	}

	void HistoricalLerping ()
	{
		if(syncPosList.Count > 0)
		{
			myTransform.position = Vector3.Lerp(myTransform.position, syncPosList[0], Time.deltaTime * lerpRate);

			if(Vector3.Distance(myTransform.position, syncPosList[0]) < closeEnough)
			{
				syncPosList.RemoveAt(0);
			}

			if(syncPosList.Count > 10) //10
			{
				lerpRate = fasterLerpRate;
			}
			else
			{
				lerpRate = normalLerpRate;	
			}

			Debug.Log(syncPosList.Count.ToString());
		}
	}
}	
