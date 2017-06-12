using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
	private const string IPV4REGEX = "((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)";

	[Header("Container")]
	[SerializeField]
	private GameObject _mainMenuContainer;

	[SerializeField]
	private GameObject _shooterMenuContainer;

	[SerializeField]
	private GameObject _hackerMenuContainer;


	[Header("Play Buttons")]
	[SerializeField]
	private Button _playShooterButton;
	[SerializeField]
	private Button _playHackerButton;

	[Header("Start Buttons")]
	[SerializeField]
	private Button _startShooterLevel0;
	[SerializeField]
	private Button _startShooterLevel1;
	[SerializeField]
	private Button _startHacker;


	[Header("Input Fields")]
	[SerializeField]
	private InputField _hostPortInput;
	[SerializeField]
	private InputField _clientIpInput;
	[SerializeField]
	private InputField _clientPortInput;


	[Header("Back Buttons")]
	[SerializeField]
	private Button _shooterMenuBackButton;

	[SerializeField]
	private Button _hackerMenuBackButton;


	//FMOD.Studio.EventInstance evnt;

	private void Start () {
		_clientIpInput.text = PlayerPrefs.GetString("ConnectionIP", "127.0.0.1");

		if (!new Regex(IPV4REGEX).IsMatch(_clientIpInput.text)) {
			_clientIpInput.text = "127.0.0.1";
			PlayerPrefs.SetString("ConnectionIP", "127.0.0.1");
		}

		_clientPortInput.text = PlayerPrefs.GetInt("ConnectionPort", 55556).ToString();
		_hostPortInput.text = PlayerPrefs.GetInt("HostPort", 55556).ToString();


		_playHackerButton.onClick.AddListener(OnPlayHacker_Click);
		_playShooterButton.onClick.AddListener(OnPlayShooter_Click);

		_startShooterLevel0.onClick.AddListener(delegate { OnStartShooter_Click(1); });
		_startShooterLevel1.onClick.AddListener(delegate { OnStartShooter_Click(2); });
		_startHacker.onClick.AddListener(OnStartHacker_Click);

		_hackerMenuBackButton.onClick.AddListener(OnBack_Click);
		_shooterMenuBackButton.onClick.AddListener(OnBack_Click);
	}

	private void OnPlayShooter_Click() {
		_mainMenuContainer.SetActive(false);
		_shooterMenuContainer.SetActive(true);
	}

	private void OnPlayHacker_Click() {
		_mainMenuContainer.SetActive(false);
		_hackerMenuContainer.SetActive(true);
	}

	private void OnBack_Click() {
		_hackerMenuContainer.SetActive(false);
		_shooterMenuContainer.SetActive(false);
		_mainMenuContainer.SetActive(true);
	}

	private void OnStartHacker_Click() {
		if (new Regex(IPV4REGEX).IsMatch(_clientIpInput.text)) {
			PlayerPrefs.SetString("ConnectionIP", _clientIpInput.text);

			int port = 55556;

			if (int.TryParse(_clientPortInput.text, out port)) {
				PlayerPrefs.SetInt("ConnectionPort", port);
				SceneManager.LoadScene("HackerSceneNew");
			}
		}
	}

	private void OnStartShooter_Click(int pLevelId) {
		int port = 55556;

		if (int.TryParse(_hostPortInput.text, out port)) {
			PlayerPrefs.SetInt("HostPort", port);
			SceneManager.LoadScene(pLevelId);
		}
	}
}
