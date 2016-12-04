using UnityEngine;
using System.Collections;
using System.Net.Sockets;

public class NetworkPlayer : MonoBehaviour, INetworked {
	[SerializeField]
	private float _transformUpdateInterval;
	private Timers.Timer _updateTimer;

	private int _id;
	public int Id {
		get {
			if (_id == 0) {
				_id = IdManager.RequestId();
			}

			return _id;
		}
	}

	public void Initialize (TcpClient pClient) {
		//TODO Create init package??
	}

	private void Awake() {
		ShooterPackageSender.RegisterNetworkObject(this);
		_updateTimer = Timers.CreateTimer()
			.SetTime(_transformUpdateInterval)
			.SetLoop(-1)
			.UseRealTime(true)
			.AddCallback(() => ShooterPackageSender.SendPackage(new CustomCommands.Update.PlayerPositionUpdate(transform.position, transform.rotation.eulerAngles.y)))
			.Play();
	}

	private void OnDestroy () {
		_updateTimer.Pause();
		_updateTimer = null;
		ShooterPackageSender.UnregisterNetworkedObject(this);
	}
}
