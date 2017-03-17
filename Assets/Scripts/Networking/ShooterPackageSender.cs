using System;
using System.Net.Sockets;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;
using Gamelogic.Extensions;

public class ShooterPackageSender : Singleton<ShooterPackageSender> {
	//Networking Variables
	private static List<IShooterNetworked> _networkObjects = new List<IShooterNetworked>();

	private static TcpClient _client;
	private static BinaryFormatter _formatter = new BinaryFormatter();

	public static TcpClient Client {
		get { return _client; }
	}

	public static BinaryFormatter Formatter {
		get { return _formatter; }
	}

	private TcpListener _listener;
	private static bool _clientInitialized = false;

	/// <summary>
	/// Adds a networked object to the reference list
	/// </summary>
	/// <param name="pObject"></param>
	public static void RegisterNetworkObject (IShooterNetworked pObject) {
		_networkObjects.Add(pObject);

		//FIX Hacky fix for a initializing rtuntime objects over the network
		if (_clientInitialized) {
			pObject.Initialize();
		}
	}
	
	/// <summary>
	/// Removes a networked object from the reference list
	/// </summary>
	/// <param name="pObject"></param>
	public static void UnregisterNetworkedObject (IShooterNetworked pObject) {
		_networkObjects.Remove(pObject);
	}

	/// <summary>
	/// Initializes the server
	/// </summary>
	public void Awake () {
		int port = PlayerPrefs.GetInt("HostPort", 55556);
		Debug.Log("Hosted on port : " + port);
		_listener = new TcpListener(IPAddress.Any, port);
		_listener.Start(5);
	}

	/// <summary>
	/// Checks for connecting clients and deletes bad ones
	/// </summary>
	public void Update () {
		CheckForNewClients();
	}

	/// <summary>
	/// 
	/// </summary>
	private void CheckForNewClients () {
		if (_listener.Pending()) {
			TcpClient connectingClient = _listener.AcceptTcpClient();

			if (_client == null || !_client.Connected) {
				_client = connectingClient;
				Debug.Log(connectingClient.Client.LocalEndPoint.ToString() + " Connected");
				ClientInitialize(connectingClient);
			} else {
				sendPackage(new NetworkPacket.Message.DisconnectRequest(), connectingClient);
				connectingClient.GetStream().Close();
				connectingClient.Close();
				Debug.Log("Connecting client refused.");
			}
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="pClient"></param>
	private void ClientInitialize (TcpClient pClient) {
		//ShooterNetworkWindow.Instance.gameObject.SetActive(true);
		FindObjectOfType<ShooterMinimapCamera>().SendUpdate();

		foreach (IShooterNetworked n in _networkObjects) {
			n.Initialize();
		}

		_clientInitialized = true;

		//ShooterNetworkWindow.Instance.gameObject.SetActive(false);

		//SendPackage(new CustomCommands.Creation.OnCreationEnd());
	}

	public static T GetNetworkedObject<T> (int pId) where T : class, IShooterNetworked {
		IShooterNetworked temp = _networkObjects.Find(x => x.Id == pId);
		return temp is T ? temp as T : default(T);
	}

	/// <summary>
	/// Sends a package to all known clients
	/// </summary>
	/// <param name="package">Sendable data</param>
	public static void SendPackage (NetworkPacket.AbstractPacket pPacket) {
		sendPackage(pPacket, _client);
	}

	/// <summary>
	/// Sends a package to all known clients
	/// </summary>
	/// <param name="package">Sendable data</param>
	private static void sendPackage (NetworkPacket.AbstractPacket pPacket, TcpClient pClient) {
		if (pClient != null && pClient.Connected) {
			try {
				_formatter.Serialize(pClient.GetStream(), pPacket);
			} catch (Exception e) {
				Debug.LogError("Failed to serialize. Reason: " + e.Message);
			}
		}
	}

	private void OnApplicationQuit () {
		SendPackage(new NetworkPacket.Message.DisconnectRequest());
		disconnectClient();
	}

	#if UNITY_EDITOR
	private void OnDestroy () {
		SendPackage(new NetworkPacket.Message.DisconnectRequest());
		disconnectClient();
	}
#endif

	private static void disconnectClient () {
		if (_client != null) {
			_client.GetStream().Close();
			_client.Close();
			_client = null;
		}
	}
}

