using UnityEngine;
using System.Collections;

public static class Helper  {
	public static T ParseEnum<T> (string value) {
		return (T)DoorState.Parse(typeof(T), value, true);
	}
}
