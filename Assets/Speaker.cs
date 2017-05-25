using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour, IShooterNetworked {
	private ShooterNetworkId _id = new ShooterNetworkId();
	public ShooterNetworkId Id { get { return _id; } }

	public void Initialize() { }

	private void Awake() {
		ShooterPackageSender.RegisterNetworkObject(this);
	}

	private void OnDestroy() {
		ShooterPackageSender.UnregisterNetworkedObject(this);
	}
}
