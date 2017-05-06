using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterDebugMenu : MonoBehaviour {
	[SerializeField]
	private bool _show;

	private bool _display = false;

	private Rect _windowRect = new Rect(20, 40, 120, 50);
	private Vector2 _scrollPos = new Vector2(0, 0);

	private void OnGUI() {
		if (_show) {
			if (GUILayout.Button("Debug Menu")) {
				_display = !_display;
			}

			if (_display) {
				_windowRect = GUILayout.Window(0, _windowRect, DebugWindow, "Debug Menu");
			}
		}
	}

	private void DebugWindow(int windowID) {
		if (GUILayout.Button("Fill Ammo")) {
			FindObjectOfType<Phaser>().MagazineContent = FindObjectOfType<Phaser>().MagazineCapacity;
			FindObjectOfType<Phaser>().ReserveAmmo = FindObjectOfType<Phaser>().MaxReserveAmmo;

			FindObjectOfType<Machinegun>().MagazineContent = FindObjectOfType<Machinegun>().MagazineCapacity;
			FindObjectOfType<Machinegun>().ReserveAmmo = FindObjectOfType<Machinegun>().MaxReserveAmmo;

			FindObjectOfType<Mininglaser>().MagazineContent = FindObjectOfType<Mininglaser>().MagazineCapacity;
			FindObjectOfType<Mininglaser>().ReserveAmmo = FindObjectOfType<Mininglaser>().MaxReserveAmmo;

			FindObjectOfType<GrenadeThrow>().NoOfGrenades = FindObjectOfType<GrenadeThrow>().MaxGrenades;
		}

		if (GUILayout.Button("Toggle Alarm")) {
			ShooterAlarmManager.Instance.AlarmIsOn = !ShooterAlarmManager.Instance.AlarmIsOn;
		}

		if (GUILayout.Button("Reload Scene")) {
			int scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
			UnityEngine.SceneManagement.SceneManager.LoadScene(scene, UnityEngine.SceneManagement.LoadSceneMode.Single);
		}
	}
}
