using System.Net.Sockets;

public interface INetworked {
	void Initialize ();
	int Id { get; }
}