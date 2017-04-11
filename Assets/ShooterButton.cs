using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

public class ShooterButton : MonoBehaviour, IInteractable, IDamageable{

    //IDamageable Implementation
    public GameObject GameObject { get { return gameObject; } }
    private Faction _faction = Faction.Neutral;
    public Faction Faction { get { return _faction; } }

    public void ReceiveDamage(Transform pSource, Vector3 pHitPoint, float pDamage, float pForce)
    {
        TriggerTimer();
    }

    //IDamageable Implementation end


    [SerializeField]
    private float _triggeredDuration;
    private float _timer;

    public ObservedValue<bool> Triggered = new ObservedValue<bool>(false);

    public void Interact()
    {
        TriggerTimer();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(_timer > 0)
        {
            _timer -= Time.deltaTime;
            if(_timer <= 0)
            {
                _timer = 0;
                Triggered.Value = false;
            }
        }

	}

    public void SetColor(Color color)
    {
        GetComponent<Renderer>().material.color = color;
    }

    private void TriggerTimer()
    {
        _timer = _triggeredDuration;
        Triggered.Value = true;
    }
}
