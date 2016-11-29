using UnityEngine;
using System.Collections;

public static class IdManager  {
	private static int _availibleId = 0;

	public static int RequestId () {
		return _availibleId++;
	}
}
