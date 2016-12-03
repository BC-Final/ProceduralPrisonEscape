using UnityEngine;
using System.Collections;

public class NetworkPlayer : MonoBehaviour {
	[SerializeField]
	private float _transformUpdateInterval;
	private Timers.Timer _updateTimer;


	private void Awake() {
		_updateTimer = Timers.CreateTimer()
			.SetTime(_transformUpdateInterval)
			.SetLoop(-1)
			.UseRealTime(true)
			.AddCallback(() => ShooterPackageSender.SendPackage(new CustomCommands.Update.PlayerPositionUpdate(transform.position, transform.rotation.eulerAngles.y)))
			.Play();
	}
}
