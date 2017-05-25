using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerButton : ShooterButton{

	override public void ReceiveDamage(Transform pSource, Vector3 pHitPoint, float pDamage, float pForce)
    {
        
    }

    override public void Interact()
    {
        
    }

    public override void SetColor(Color color)
    {
        if (color != CurrentColor)
        {
            CurrentColor = color;
            GetComponent<Renderer>().material.color = CurrentColor;
			Terminal.OnHackerButtonColorChanged();
		}
    }
}
