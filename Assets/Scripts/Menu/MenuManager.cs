using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
	private const string IPV4REGEX = "((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)";

	[SerializeField]
	private InputField _ipInputField;
	[SerializeField]
	private InputField _portInputField;
	[SerializeField]
	private InputField _hostPortInputField;

	FMOD.Studio.EventInstance evnt;

	private void Start () {
		_ipInputField.text = PlayerPrefs.GetString("ConnectionIP", "127.0.0.1");

		if (!new Regex(IPV4REGEX).IsMatch(_ipInputField.text)) {
			_ipInputField.text = "127.0.0.1";
			PlayerPrefs.SetString("ConnectionIP", "127.0.0.1");
		}

		_portInputField.text = PlayerPrefs.GetInt("ConnectionPort", 55556).ToString();
		_hostPortInputField.text = PlayerPrefs.GetInt("HostPort", 55556).ToString();


		FMOD.Studio.EventDescription desc = FMODUnity.RuntimeManager.GetEventDescription("snapshot:/snapkilltobi");
		desc.createInstance(out evnt);
	}

	public void UIOnPlayShooter () {
		int port = 55556;

		FMODUnity.RuntimeManager.CreateInstance("event:/PE_menu/PE_menu_buttonclick").start();

		if (!int.TryParse(_hostPortInputField.text, out port)) {
			Debug.LogWarning("Invalid Host Port.");
		} else {
			PlayerPrefs.SetInt("HostPort", port);
			SceneManager.LoadScene("ShooterScene");
		}
	}
	public void UIOnPlayHacker () {
		FMODUnity.RuntimeManager.CreateInstance("event:/PE_menu/PE_menu_buttonclick").start();

		if (new Regex(IPV4REGEX).IsMatch(_ipInputField.text)) {
			PlayerPrefs.SetString("ConnectionIP", _ipInputField.text);

			int port = 55556;

			if (!int.TryParse(_hostPortInputField.text, out port)) {
				Debug.LogWarning("Invalid Connection Port.");
			} else {
				PlayerPrefs.SetInt("ConnectionPort", port);
				SceneManager.LoadScene("HackerScene");
			}
		} else {
			Debug.LogWarning("Invalid Connection IP-Adress.");
		}
	}

	public void SetTobias (bool pState) {
		if (pState) {
			evnt.start();
		} else {
			evnt.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		}
	}
}
