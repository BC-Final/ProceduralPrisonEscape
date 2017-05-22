using System.Net.Sockets;

public interface IShooterNetworked {
	void Initialize ();
	ShooterNetworkId Id { get; }
	//TODO Add this line to only send discovered objects
	//bool Discovered { get; }
}

public interface IHackerNetworked {
	int Id { get; set; }
}