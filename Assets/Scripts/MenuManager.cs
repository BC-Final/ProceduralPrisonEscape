﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
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
		SceneManager.LoadScene("HackerScene");
	}
}
