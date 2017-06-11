using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using Gamelogic.Extensions;

public class NetworkPlayer : MonoBehaviour, IShooterNetworked {
	[SerializeField]
	[Tooltip("How often per second should the player be updated")]
	private float _transformUpdateInterval;



	/// <summary>
	/// Reference to the netowork update timer
	/// </summary>
	private Timer _updateTimer;



	/// <summary>
	/// Reference to the shooters health
	/// </summary>
	private PlayerHealth _playerHealth;


	public ShooterNetworkId _id = new ShooterNetworkId();
	public ShooterNetworkId Id { get { return _id; } }



	/// <summary>
	/// Initializes object after hacker connected
	/// </summary>
	public void Initialize() {
		_updateTimer = TimerManager.CreateTimer("Player Update", false)
		.SetDuration(_transformUpdateInterval)
		.SetLoops(-1)
		.AddCallback(() => ShooterPackageSender.SendPackage(new NetworkPacket.GameObjects.Player.sUpdate(Id, transform.position.x, transform.position.z, transform.rotation.eulerAngles.y, PlayerHealth.Instance.CurrentHealth.Value / PlayerHealth.Instance.MaxHealth)))
		.Start();
	}



	/// <summary>
	/// Adds this to network object list
	/// </summary>
	private void Awake() {
		ShooterPackageSender.RegisterNetworkObject(this);
	}



	/// <summary>
	/// Stops update timer and removes this from network object list
	/// </summary>
	private void OnDestroy() {
		if (_updateTimer != null) {
			_updateTimer.Stop();
		}

		_updateTimer = null;

		ShooterPackageSender.UnregisterNetworkedObject(this);
	}



	/// <summary>
	/// Gets references and starts the update timer
	/// </summary>
	private void Start() {
		_playerHealth = FindObjectOfType<PlayerHealth>();
	}
}
