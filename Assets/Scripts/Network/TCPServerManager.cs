using UnityEngine;
using System;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class TCPServerManager : MonoBehaviour {

	TcpListener listener;
	TcpClient connectedClient;
	// Use this for initialization
	void Awake () {
		listener = new TcpListener(IPAddress.Any, 55556);
		listener.Start(5);
	}
	
	// Update is called once per frame
	void Update () {
		if (listener.Pending() )
		{
			//Add new ChatClient
			TcpClient connectingClient = listener.AcceptTcpClient();
			connectedClient = connectingClient;
			Console.WriteLine(connectingClient.Client.LocalEndPoint.ToString() + " Connected");
		}
	}
}
