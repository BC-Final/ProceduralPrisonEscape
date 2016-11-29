using UnityEngine;
using System.Collections;

public static class IdManager  {
	private static int _availibleId = 1;

	public static int RequestId () {
		return _availibleId++;
	}
}
