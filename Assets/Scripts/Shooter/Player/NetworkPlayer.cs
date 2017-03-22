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
	private Timers.Timer _updateTimer;



	/// <summary>
	/// Reference to the shooters health
	/// </summary>
	private PlayerHealth _playerHealth;



	/// <summary>
	/// Network Identification
	/// </summary>
	private int _id;



	/// <summary>
	/// Accessor for the Network Id
	/// </summary>
	public int Id {
		get {
			if (_id == 0) {
				_id = IdManager.RequestId();
			}

			return _id;
		}
	}



	/// <summary>
	/// Initializes object after hacker connected
	/// </summary>
	public void Initialize () {
		//TODO Create init package??
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
	private void OnDestroy () {
		_updateTimer.Stop();
		_updateTimer = null;

		ShooterPackageSender.UnregisterNetworkedObject(this);
	}



	/// <summary>
	/// Gets references and starts the update timer
	/// </summary>
	private void Start () {
		_playerHealth = FindObjectOfType<PlayerHealth>();

		_updateTimer = Timers.CreateTimer("Player Online Update")
		.SetTime(_transformUpdateInterval)
		.SetLoop(-1)
		.UseRealTime(true)
		.SetCallback(() => ShooterPackageSender.SendPackage(new NetworkPacket.Update.Player(Id, transform.position.x, transform.position.z, transform.rotation.eulerAngles.y, PlayerHealth.Instance.CurrentHealth.Value / PlayerHealth.Instance.MaxHealth)))
		.Start();
	}
}
