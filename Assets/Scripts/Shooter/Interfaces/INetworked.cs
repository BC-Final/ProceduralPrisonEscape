using System.Net.Sockets;

public interface INetworked {
	void Initialize ();
	int Id { get; }
	//TODO Add this line to only send discovered objects
	//bool Discovered { get; }
}