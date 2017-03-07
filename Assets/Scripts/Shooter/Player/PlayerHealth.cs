using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerHealth : MonoBehaviour, IDamageable {
	[SerializeField]
	private float _maxHealth;

	private float _currentHealth;

	private Image _healthBar;

	private void Start() {
		_currentHealth = _maxHealth;
		_healthBar = GameObject.FindGameObjectWithTag("healthbar").GetComponent<Image>();
	}

	public void ReceiveDamage(Vector3 pDirection, Vector3 pPoint, float pDamage) {
		//TODO Display damage direction indicator
		_currentHealth = Mathf.Max(0.0f, _currentHealth - pDamage);

		if (_currentHealth == 0.0f) {
			//Debug.Log("You are dead!");
			//Debug.Break();
		}
	
		DOTween.To(() => _healthBar.fillAmount, x => _healthBar.fillAmount = x, _currentHealth / _maxHealth, 0.1f);
		//_healthBar.fillAmount = _currentHealth / _maxHealth;
	}

	public void HealDamage(float pAmount) {
		_currentHealth = Mathf.Min(_maxHealth, _currentHealth + pAmount);
		DOTween.To(() => _healthBar.fillAmount, x => _healthBar.fillAmount = x, _currentHealth / _maxHealth, 0.1f);
		//_healthBar.fillAmount = _currentHealth / _maxHealth;
	}
}
