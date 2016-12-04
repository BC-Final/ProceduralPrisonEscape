using System.Net.Sockets;

public interface INetworked {
	void Initialize (TcpClient pClient);
	int Id { get; }
}
