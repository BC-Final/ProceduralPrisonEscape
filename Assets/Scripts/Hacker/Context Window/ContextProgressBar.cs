using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ContextProgressBar : MonoBehaviour {
	[SerializeField]
	private Text _hackedLabel;

	[SerializeField]
	private Image _progressBarImage;

	public void SetProgress (float pProgress) {
		_progressBarImage.fillAmount = pProgress;
		gameObject.SetActive(pProgress > 0.0f);
		_hackedLabel.gameObject.SetActive(pProgress == 1.0f);
	}

	//HACK Is this a hacky solution?
	public void Start () {
		_progressBarImage.fillAmount = 0.0f;
		gameObject.SetActive(false);
		_hackedLabel.gameObject.SetActive(false);
	}
}
