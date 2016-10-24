using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CustomNetworkManager : NetworkManager {

	public override void OnClientConnect(NetworkConnection conn)
	{
		base.OnClientConnect(conn);
		Debug.Log(conn.ToString() + "Connected");
		Debug.Log(Network.isServer);
		Debug.Log(Network.isClient);
		if (Network.isServer)
		{
			GUILayout.Label("Running as a server");
		}
		else if (Network.isClient)
		{
			Debug.Log("Connected as a client");
		}
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
