using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

public class ShooterButton : MonoBehaviour, IInteractable, IDamageable{

    //IDamageable Implementation
    public GameObject GameObject { get { return gameObject; } }
    private Faction _faction = Faction.Neutral;
    public Faction Faction { get { return _faction; } }

    virtual public void ReceiveDamage(Transform pSource, Vector3 pHitPoint, float pDamage, float pForce)
    {
        if(_canBeTriggered)
        {
            TriggerTimer();
        }
    }

    //IDamageable Implementation end


    [SerializeField]
    protected float _triggeredDuration;
    [SerializeField]
    protected float _delayBetweenTriggering;
    protected float _timer;

    public ObservedValue<bool> Triggered = new ObservedValue<bool>(false);
    protected bool _canBeTriggered;

    [Header("Colors")]
    [SerializeField]
    protected Color offColor;
    [SerializeField]
    protected Color notActiveColor;
    [SerializeField]
    protected Color activeColor;
    [SerializeField]
    protected Color solvedColor;
    protected Color _currentColor;


    virtual public void Interact()
    {
        if (_canBeTriggered)
        {
            TriggerTimer();
        }
    }
	
	// Update is called once per frame
	protected void Update () {
        if(_timer > 0)
        {
            _timer -= Time.deltaTime;
            if(_timer <= _delayBetweenTriggering)
            {
                Triggered.Value = false;
                SetColor(offColor);
            }
            if(_timer <= 0)
            {
                _timer = 0;    
            }
        }
        if (_timer == 0)
        {
            _canBeTriggered = true;
            SetColor(notActiveColor);
        }

    }

    virtual public void SetColor(Color color)
    {
        if(color != _currentColor)
        {
            _currentColor = color;
            GetComponent<Renderer>().material.color = _currentColor;
        }

    }

    public void OnSolved()
    {
        _canBeTriggered = false;
        SetColor(solvedColor);
        this.enabled = false;
    }

    protected void TriggerTimer()
    {
        _canBeTriggered = false;
        _timer = _triggeredDuration + _delayBetweenTriggering;
        SetColor(activeColor);
        Triggered.Value = true;
        
    }
}
