using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

	InputField inputField;

	// Use this for initialization
	void Start () {
		inputField = GameObject.FindObjectOfType<IpInputField>().GetComponent<InputField>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void UIOnPlayShooter()
	{
		//SceneManager.LoadScene("NetworkedShooterTestScene");
		SceneManager.LoadScene("PresentationScene");
	}
	public void UIOnPlayHacker()
	{
		if (inputField.text == "")
		{
			PlayerPrefs.SetString("ConnectionIP", "localhost");
		}
		else
		{
			PlayerPrefs.SetString("ConnectionIP", inputField.text);
		}
		SceneManager.LoadScene("HackerScene");
	}
}
