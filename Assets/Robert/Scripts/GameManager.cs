using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

public class GameManager : MonoBehaviour {

	List<HackerDoor> doors;
	NetworkPlayer connectedClient;
	TcpListener listener;
	
	void Awake () {
		//doors = new List<Door>();
		//Door[] doorArray = GetComponents<Door>();
		//for(int i = 0; i<doorArray.Length; i++)
		//{
		//	doorArray[i].Id = i;
		//	doorArray[i].position = doorArray[i].transform.position;
		//	doors.Add(doorArray[i]);
		//}
		//listener = new TcpListener(IPAddress.Any, 55556);
		//listener.Start(5);
	}
	void OnPlayerConnected(NetworkPlayer player)
	{
	}


	void Update () {
		//if (listener.Pending())
		//{
		//	//Add new ChatClient
		//	TcpClient connectingClient = listener.AcceptTcpClient();
		//	connectedClient = connectingClient;
		//	Debug.Log(connectingClient.Client.LocalEndPoint.ToString() + " Connected");
		//}
	}

	public List<HackerDoor> GetDoorList()
	{
		return doors;
	}
}
