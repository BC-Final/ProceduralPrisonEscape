using System;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;

public class PackageReader : MonoBehaviour {

	Vector2 _minimapScale;
	private DoorManager _doorManager;
	private TcpChatClient _networkManager;
	private ChatBoxScreen _chatBoxScreen;
	[SerializeField]
	private Transform _minimap;
	private Texture _minimapTexture;
	private TcpClient _client;
	private NetworkStream _stream;
	private BinaryFormatter _formatter;
	private MinimapPlayer _player;
	[SerializeField]
	private GameObject _playerPrefab;
	[SerializeField]
	private GameObject _minimapDoorPrefab;

	// Use this for initialization
	void Start () {
		_minimapScale = new Vector2(4, 4);
		_networkManager = FindObjectOfType<TcpChatClient>();
		_chatBoxScreen = _networkManager.GetChatBoxScreen();
		_client = _networkManager.GetClient();
		_stream = _networkManager.GetStream();
		_formatter = _networkManager.GetFormatter();
		_doorManager = GameObject.FindObjectOfType<DoorManager>();
	}
	
	// Update is called once per frame
	void Update () {
		try
		{
			if (_client.Available != 0)
			{
				CustomCommands.AbstractPackage response = _formatter.Deserialize(_stream) as CustomCommands.AbstractPackage;

				ReadResponse(response);
			}
		}
		catch (Exception e)
		{
			Debug.Log("Error" + e.ToString());
		}
	}
	//Message Management

	//Read incoming bytes and returns them when finished
	private byte[] ReadBytes(int pByteCount)
	{
		byte[] bytes = new byte[pByteCount];
		int bytesRead = 0;
		int totalBytesRead = 0;

		try
		{
			while (totalBytesRead != pByteCount && (bytesRead = _stream.Read(bytes, totalBytesRead, pByteCount - totalBytesRead)) > 0)
			{
				totalBytesRead += bytesRead;
			}
		}
		catch
		{
			Console.WriteLine("Something went wrong");
		}

		return (totalBytesRead == pByteCount) ? bytes : null;
	}

	//First gets Message size in bytes then gets Message in bytes itself.
	private byte[] ReceiveMessage()
	{
		int byteCountToRead = BitConverter.ToInt32(ReadBytes(8), 0);
		return ReadBytes(byteCountToRead);
	}

	//Starts listening to Message with ReceiveMessage(). When finished listening returns message.
	private string ReceiveString(Encoding pEncoding)
	{
		return pEncoding.GetString(ReceiveMessage());
	}

	//Starts sending Message size then sends Message itself
	private void SendMessage(byte[] pMessage)
	{
		_stream.Write(BitConverter.GetBytes(pMessage.Length), 0, 4);
		_stream.Write(pMessage, 0, pMessage.Length);
	}

	//Starts sending a string Message 
	private void SendString(string pMessage, Encoding pEncoding)
	{
		SendMessage(pEncoding.GetBytes(pMessage));
	}

	/// <summary>
	/// Sends a custom request to the server
	/// </summary>
	/// <param name="req">Request type</param>
	private void SendRequest(CustomCommands.AbstractPackage req)
	{
		try
		{
			BinaryFormatter formatter = new BinaryFormatter();
			Debug.Log(req.ToString());
			formatter.Serialize(_stream, req);
		}
		catch (SerializationException e)
		{
			Console.WriteLine("Failed to serialize. Reason: " + e.Message);
			throw;
		}

	}
	private void ReadResponse(CustomCommands.AbstractPackage response)
	{
		if (response is CustomCommands.NotImplementedMessage)
		{
			CustomCommands.NotImplementedMessage NIM = response as CustomCommands.NotImplementedMessage;
			_chatBoxScreen.AddChatLine(NIM.message);
		}
		if (response is CustomCommands.SendMinimapUpdate)
		{
			CustomCommands.SendMinimapUpdate MU = response as CustomCommands.SendMinimapUpdate;
			Texture2D tex = new Texture2D(2, 2);
			tex.LoadImage(MU.bytes);
			_minimap.GetComponent<Renderer>().material.mainTexture = tex;
			Debug.Log("Minimap Updated");
			_chatBoxScreen.AddChatLine("Minimap Update");
		}
		if (response is CustomCommands.PlayerPositionUpdate)
		{
			Debug.Log("Updating Player Position");
			CustomCommands.PlayerPositionUpdate PPU = response as CustomCommands.PlayerPositionUpdate;
			if(_player == null){
				GameObject gameObject = (GameObject)Instantiate(_playerPrefab, new Vector3(PPU.x,0,PPU.z), Quaternion.identity);
				_player = gameObject.GetComponent<MinimapPlayer>();
			}else
			{
				_player.SetNewPos(new Vector3(PPU.x/_minimapScale.x, 0, PPU.z/_minimapScale.y));
			}
		}
		if (response is CustomCommands.DoorUpdate)
		{
			Debug.Log("Updating Door");
			CustomCommands.DoorUpdate DU = response as CustomCommands.DoorUpdate;
			Debug.Log("Update for Door : " + DU.ID);
			if (!_doorManager.DoorAlreadyExists(DU.ID))
			{
				Debug.Log("-Door does not exists");
				Debug.Log(DU.rotationY);
				GameObject gameObject = (GameObject)Instantiate(_minimapDoorPrefab, new Vector3(DU.x/_minimapScale.x, 0, DU.z/_minimapScale.y), Quaternion.EulerRotation(0, Mathf.Deg2Rad*DU.rotationY, 0));
				Door door = gameObject.GetComponent<Door>();
				door.Id = DU.ID;
				
				_doorManager.AddDoor(door);
			}else
			{
				Debug.Log("-Door exists");
				_doorManager.UpdateDoor(DU);
			}
		}
	}

	//Exit Methods
	private void OnProcessExit(object sender, EventArgs e)
	{
		SendRequest(new CustomCommands.NotImplementedMessage());
	}

	private void OnApplicationQuit()
	{
		SendRequest(new CustomCommands.NotImplementedMessage());
	}
}



