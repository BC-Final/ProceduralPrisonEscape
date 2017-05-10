using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthDisplayShooterUI : MonoBehaviour {
	private Image _image;

	private void Start () {
		_image = GetComponent<Image>();
		PlayerHealth.Instance.CurrentHealth.OnValueChange += healthChanged;
	}

	private void healthChanged () {
		DOTween.To(() => _image.fillAmount, x => _image.fillAmount = x, PlayerHealth.Instance.CurrentHealth.Value / PlayerHealth.Instance.MaxHealth, 0.1f);
	}
}
