using System.Net.Sockets;

public interface IShooterNetworked {
	void Initialize (TcpClient pClient);
	int Id { get; }
}

public interface IHackerNetworked {
	void Initialize ();
	int Id { get; set; }
}
