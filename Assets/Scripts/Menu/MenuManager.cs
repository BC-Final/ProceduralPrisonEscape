using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

	[SerializeField]
	InputField ipInputField;
	[SerializeField]
	InputField portInputField;
	[SerializeField]
	InputField hostPortInputField;

	// Use this for initialization
	void Start () {
		if (PlayerPrefs.GetString("ConnectionIP") != "") {
			ipInputField.text = PlayerPrefs.GetString("ConnectionIP");
		}

		if (PlayerPrefs.GetString("ConnectionPort") != "") {
			portInputField.text = PlayerPrefs.GetString("ConnectionPort");
		}
	}

	// Update is called once per frame
	void Update () {

	}
	public void UIOnPlayShooter () {
		if (hostPortInputField.text == "") {
			PlayerPrefs.SetString("HostPort", "55556");
		} else {
			PlayerPrefs.SetString("HostPort", hostPortInputField.text);
		}
		SceneManager.LoadScene("ShooterScene");
	}
	public void UIOnPlayHacker () {
		if (ipInputField.text == "") {
			PlayerPrefs.SetString("ConnectionIP", "localhost");
		} else {
			PlayerPrefs.SetString("ConnectionIP", ipInputField.text);
		}

		if (portInputField.text == "") {
			PlayerPrefs.SetString("ConnectionPort", "55556");
		} else {
			PlayerPrefs.SetString("ConnectionPort", portInputField.text);
		}
		SceneManager.LoadScene("HackerScene");
	}
}
