using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterNetworkId {
	private static int _availibleId = 1;

	private int _id;

	public ShooterNetworkId() {
		_id = _availibleId++;
	}

	public static implicit operator int(ShooterNetworkId i) {
		return i._id;
	}
}
