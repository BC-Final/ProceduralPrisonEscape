using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerControl : NetworkBehaviour {

	// Use this for initialization
	void Awake () {
		if (isClient)
		{
			Debug.Log("is Client");
		}
		if (isServer)
		{
			Debug.Log("is Server");
		}
		Debug.Log("initilize");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
