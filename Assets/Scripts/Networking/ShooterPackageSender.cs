using System;
using System.IO;
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
	public static void RegisterNetworkObject(IShooterNetworked pObject) {
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
	public static void UnregisterNetworkedObject(IShooterNetworked pObject) {
		_networkObjects.Remove(pObject);
	}

	/// <summary>
	/// Initializes the server
	/// </summary>
	public void Awake() {
		int port = PlayerPrefs.GetInt("HostPort", 55556);
		Debug.Log("Hosted on port : " + port);
		_listener = new TcpListener(IPAddress.Any, port);
		_listener.Start(5);
	}

	/// <summary>
	/// Checks for connecting clients and deletes bad ones
	/// </summary>
	public void Update() {
		CheckForNewClients();
	}

	/// <summary>
	/// 
	/// </summary>
	private void CheckForNewClients() {
		if (_listener.Pending()) {
			TcpClient connectingClient = _listener.AcceptTcpClient();

			if (_client == null || !_client.Connected) {
				_client = connectingClient;
				Debug.Log(connectingClient.Client.LocalEndPoint.ToString() + " Connected");
				ClientInitialize(connectingClient);
			} else {
				sendPackage(new NetworkPacket.Messages.DisconnectRequest(), connectingClient);
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
	private void ClientInitialize(TcpClient pClient) {
		//ShooterNetworkWindow.Instance.gameObject.SetActive(true);
		FindObjectOfType<ShooterMinimapCamera>().SendUpdate();

		foreach (IShooterNetworked n in _networkObjects) {
			n.Initialize();
		}

		_clientInitialized = true;

		//ShooterNetworkWindow.Instance.gameObject.SetActive(false);

		SendPackage(new NetworkPacket.Messages.CreationEnd());
	}

	public static T GetNetworkedObject<T>(int pId) where T : class, IShooterNetworked {
		IShooterNetworked temp = _networkObjects.Find(x => x.Id == pId);
		return temp is T ? temp as T : default(T);
	}

	/// <summary>
	/// Sends a package to all known clients
	/// </summary>
	/// <param name="package">Sendable data</param>
	public static void SendPackage(NetworkPacket.AbstractPacket pPacket) {
		sendPackage(pPacket, _client);
	}

	/// <summary>
	/// Sends a package to all known clients
	/// </summary>
	/// <param name="package">Sendable data</param>
	private static void sendPackage(NetworkPacket.AbstractPacket pPacket, TcpClient pClient) {
		if (pClient != null && pClient.Connected) {
			Instance.StartCoroutine(Instance.sendPacketAsync(pPacket, pClient));
			//Instance.StartCoroutine(Instance.SerializePacket(pPacket, CopyStream(pClient.GetStream())));
			
		}
	}

	private IEnumerator sendPacketAsync(NetworkPacket.AbstractPacket pPacket, TcpClient pClient) {
		try {
			Stream stream = pClient.GetStream();
			

			_formatter.Serialize(stream, pPacket);

			
		} catch (Exception e) {
			Debug.LogError("Failed to serialize. Reason: " + e.Message);
		}

		yield return null;
	}
	private IEnumerator SerializePacket(NetworkPacket.AbstractPacket pPacket, Stream stream)
	{
		try
		{
			Stream tempStream = CopyStream(stream);
			tempStream.Position = 0;
			_formatter.Serialize(tempStream, pPacket);
			long byteCount = tempStream.Position;
			Debug.Log("bytecount : " + byteCount);
		}
		catch (Exception e)
		{
			Debug.LogError("Failed to serialize. Reason: " + e.Message);
		}

		yield return null;
	}

	private static Stream CopyStream(Stream inputStream)
	{
		Stream outputStream = new MemoryStream();
		byte[] buffer = new byte[4096];
		int read;
		while ((read = inputStream.Read(buffer, 0, buffer.Length)) > 0)
		{
			outputStream.Write(buffer, 0, read);
		}
		return outputStream;
	}

	private void OnApplicationQuit () {
		SendPackage(new NetworkPacket.Messages.DisconnectRequest());
		disconnectClient();
	}

	#if UNITY_EDITOR
	private void OnDestroy () {
		SendPackage(new NetworkPacket.Messages.DisconnectRequest());
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

