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

public class TCPMBTesterServer : MonoBehaviour
{
	private DoorManager _doorManager;
	[SerializeField]
	public Camera _camera;
	float _playerPosUpdateTimer = 0;
	float _playerPosUpdateInterval = 0.5f;
	//Player struct with basic player information
	private struct Player
	{
		public Player(string pName, int pScore)
		{
			this.name = pName;
			this.score = pScore;
		}
		public string name;
		public int score;
	}
	//Help Response
	private static string helpResponse = "\n!Add [Playername] [Score]\t: Adds a player to the highscore \n" +
											"!Get [Playername]	\t: Gets the highest score of the player \n" +
											"!List \t: Gets a list of the players with the highest score \n";

	//Networking Variables
	private static Dictionary<string, string> commandDictionary = new Dictionary<string, string>();
	private static List<Player> highscore = new List<Player>();
	private static List<TcpClient> conncectedClients = new List<TcpClient>();
	private static List<TcpClient> clientsToBeDeleted = new List<TcpClient>();
	private static BinaryFormatter formatter = new BinaryFormatter();
	private static ShooterPackageReader _reader;
	private static CustomCommands.AbstractPackage response;
	private TcpListener _listener;
	[SerializeField]
	private Transform _player;

	public void Start()
	{
		Application.runInBackground = true;
		//Commands
		commandDictionary.Add("!Add [Playername] [Score]", "Adds a player to the highscore");
		commandDictionary.Add("!Get [Playername]", "Gets the highest score of the player");
		commandDictionary.Add("!List", "Gets a list of the players and their highest score");

		_doorManager = GameObject.FindObjectOfType<DoorManager>();
		_doorManager.SetSender(this);
		_reader = GameObject.FindObjectOfType<ShooterPackageReader>();
		_listener = new TcpListener(IPAddress.Any, 55556);
		_listener.Start(5);
	}

	public void Update()
	{
		if (_listener.Pending())
		{
			//Add new Client
			TcpClient connectingClient = _listener.AcceptTcpClient();
			conncectedClients.Add(connectingClient);
			Debug.Log(connectingClient.Client.LocalEndPoint.ToString() + " Connected");
			ClientInitialize(connectingClient);

		}

		foreach (TcpClient client in conncectedClients)
		{
			_playerPosUpdateTimer += Time.deltaTime;
			if(_playerPosUpdateTimer > _playerPosUpdateInterval)
			{
				_playerPosUpdateTimer -= _playerPosUpdateInterval;
				SendResponse(new CustomCommands.PlayerPositionUpdate(_player.position), client);
			}
			//Check if client has anything to say before blocking else go to next client
			if (client.Available == 0)
			{
				continue;
			}

		}

		//delete all clients that need to be deleted
		foreach (TcpClient client in clientsToBeDeleted)
		{
			conncectedClients.Remove(client);
			DisconnectClient(client);
		}
		clientsToBeDeleted.Clear();


	}

	//Player Management

	private void ClientInitialize(TcpClient pClient)
	{
		SendResponse(new CustomCommands.SendMinimapUpdate(GetMinimapData()), pClient);

		_reader.SetClient(pClient);
		List<Door> doors = _doorManager.GetDoorList();
		foreach(Door d in doors)
		{
			Debug.Log(d.transform.rotation.y);
			SendResponse(new CustomCommands.DoorUpdate(d.Id, d.transform.position.x, d.transform.position.z, d.transform.rotation.eulerAngles.y, d.GetDoorState().ToString()), pClient);
		}
	}

	public void SendDoorUpdate(Door door)
	{
		foreach(TcpClient client in conncectedClients)
		{
		SendResponse(new CustomCommands.DoorChangeState(door.Id, door.GetDoorState().ToString()), client);
		}
	}
	/// <summary>
	/// Sorts Players by their highscore.
	/// </summary>
	/// <returns>Returns highscore list in string format</returns>
	//private static Dictionary<string, int> ReturnHighscore()
	//{
	//	string stringList = "\nHighscore\n";
	//
	//	List<Player> sortedList = new List<Player>();
	//	List<Player> tempList = new List<Player>(highscore);
	//	Dictionary<string, int> playerHighscore = new Dictionary<string, int>();
	//
	//	for (int i = 0; i < tempList.Count; i++)
	//	{
	//		Player p = new Player(tempList[i].name, GetPlayer(tempList[i].name));
	//		if (!sortedList.Contains(p))
	//		{
	//			sortedList.Add(p);
	//		}
	//	}
	//	//while (tempList.Count > 0)
	//	//{
	//	//	int highestScore = 0;
	//	//	int highestIndex = 0;
	//	//	for (int i = 0; i < tempList.Count; i++)
	//	//	{
	//	//		Console.WriteLine(tempList[i].score + " > " + highestScore + " ?");
	//	//		if (tempList[i].score > highestScore)
	//	//		{
	//	//			Console.WriteLine(tempList[i].score + " is now highest");
	//	//			highestScore = tempList[i].score;
	//	//			highestIndex = i;
	//	//		}
	//	//	}
	//	//	sortedList.Add(tempList[highestIndex]);
	//	//	tempList.RemoveAt(highestIndex);
	//	//}
	//	for (int i = 0; i < sortedList.Count; i++)
	//	{
	//		stringList += sortedList[i].name + "\t\t\t\t" + sortedList[i].score;
	//		stringList += "\n";
	//		playerHighscore.Add(sortedList[i].name, sortedList[i].score);
	//	}
	//	return playerHighscore;
	//}

	//Adds player to player List
	//private static void AddPLayer(string pName, int pScore) { highscore.Add(new Player(pName, pScore)); }

	/// <summary>
	/// Creates a list of all entries with "pName". 
	/// Then returns the entry with the highest score of player "pName"
	/// </summary>
	/// <param name="pName">Name of player you want to get the highest score of</param>
	/// <returns>Player "pName"s highest score</returns>
	//private static int GetPlayer(string pName)
	//{
	//	List<int> tempPlayerIndexList = new List<int>();
	//	for (int i = 0; i < highscore.Count; i++)
	//	{
	//		if (highscore[i].name == pName)
	//		{
	//			tempPlayerIndexList.Add(i);
	//		}
	//	}
	//	int highestScore = 0;
	//	for (int i = 0; i < tempPlayerIndexList.Count; i++)
	//	{
	//		Console.WriteLine(highscore[i].score + " > " + highestScore + " ?");
	//		if (highscore[tempPlayerIndexList[i]].score > highestScore)
	//		{
	//			Console.WriteLine("new highest : " + highscore[tempPlayerIndexList[i]].score);
	//			highestScore = highscore[tempPlayerIndexList[i]].score;
	//
	//		}
	//	}
	//	return highestScore;
	//}

	//Message Management

	//Processes the request and returns a response
	private CustomCommands.AbstractPackage GetResponse(CustomCommands.AbstractPackage request, TcpClient client)
	{
		if(request is CustomCommands.MinimapUpdateRequest)
		{
			return new CustomCommands.SendMinimapUpdate(GetMinimapData());
		}
		if(request is CustomCommands.PlayerPositionUpdateRequest)
		{
			return new CustomCommands.PlayerPositionUpdate(_player.position);
		}
		return new CustomCommands.NotImplementedMessage();
	}

	////Read incoming bytes and returns them when finished
	//private static byte[] ReadBytes(int pByteCount, TcpClient client)
	//{
	//	byte[] bytes = new byte[pByteCount];
	//	int bytesRead = 0;
	//	int totalBytesRead = 0;
	//
	//	try
	//	{
	//		//while not finished reading continiue reading. If GetStream().Read() returns 0 there are no bytes to read.
	//		while (totalBytesRead != pByteCount && (bytesRead = client.GetStream().Read(bytes, totalBytesRead, pByteCount - totalBytesRead)) > 0)
	//		{
	//			Debug.Log("Total Bytes Read : " + totalBytesRead);
	//			totalBytesRead += bytesRead;
	//		}
	//	}
	//	catch
	//	{
	//		//If sending failes disconnect client
	//		clientsToBeDeleted.Add(client);
	//		return null;
	//	}
	//	return (totalBytesRead == pByteCount) ? bytes : null;
	//}

	//First gets Message size in bytes then gets Message in bytes itself.
	//private static byte[] ReceiveMessage(TcpClient client)
	//{
	//	int byteCountToRead = BitConverter.ToInt32(ReadBytes(8, client), 0);
	//	return ReadBytes(byteCountToRead, client);
	//}
	//
	////Starts listening to Message with ReceiveMessage(). When finished listening returns message.
	//private static string ReceiveString(Encoding pEncoding, TcpClient client)
	//{
	//	return pEncoding.GetString(ReceiveMessage(client));
	//}
	//
	////Starts sending Message size then sends Message itself
	//private static void SendMessage(byte[] pMessage, TcpClient client)
	//{
	//	try
	//	{
	//		client.GetStream().Write(BitConverter.GetBytes(pMessage.Length), 0, 4);
	//		client.GetStream().Write(pMessage, 0, pMessage.Length);
	//	}
	//	catch
	//	{
	//		clientsToBeDeleted.Add(client);
	//	}
	//}
	//
	////Starts sending a string Message 
	//private static void SendString(string pMessage, Encoding pEncoding, TcpClient client)
	//{
	//	SendMessage(pEncoding.GetBytes(pMessage), client);
	//}

	//Starts sending a response
	private static void SendResponse(CustomCommands.AbstractPackage response, TcpClient client)
	{
		try
		{
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(client.GetStream(), response);
		}
		catch (SerializationException e)
		{
			Console.WriteLine("Failed to serialize. Reason: " + e.Message);
			clientsToBeDeleted.Add(client);
			throw;
		}
	}
	//Connection Management

	//Mainly disconnecting Clients
	private static void OnProcessExit(object sender, EventArgs e)
	{
		Console.WriteLine("Im outta here");
		DisconnectAllClients();
	}
	private static void DisconnectClient(TcpClient client)
	{
		Console.WriteLine("Client Disconnected");
		conncectedClients.Remove(client);
		client.GetStream().Close();
		client.Close();
	}
	private static void DisconnectAllClients()
	{
		foreach (TcpClient client in conncectedClients)
		{
			DisconnectClient(client);
			//client.GetStream().Close();
			//client.Close();
		}
	}

	//Internal Methods
	private byte[] GetMinimapData()
	{
		RenderTexture rt = new RenderTexture(512, 512, 24);

		_camera.targetTexture = rt;
		_camera.Render();

		RenderTexture.active = rt;
		Texture2D tex2d = new Texture2D(512, 512, TextureFormat.RGB24, false);
		tex2d.ReadPixels(new Rect(0, 0, 512, 512), 0, 0);

		RenderTexture.active = null;
		_camera.targetTexture = null;

		byte[] bytes;
		bytes = tex2d.EncodeToPNG();

		return bytes;
	}
}

