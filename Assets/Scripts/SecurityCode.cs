using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecurityCode : MonoBehaviour {

    static public List<SecurityCode> members = new List<SecurityCode>();

    [SerializeField]
    List<DecoderNumber> decNumber;

    private int valueNumber = 0;
    private Image image;

    private DecoderWindow _window;
    private int[] values = { 0, 1, 2, 4};
    private int valuteIndex = 0;
    public int Value {
        get { return valueNumber; }
        set { valueNumber = value;
            SetColor(value);
            foreach (DecoderNumber n in decNumber)
            {
                n.Recalculate();
            }
        }
    }

    public void SetWindow(DecoderWindow window)
    {
        _window = window;
    }

    public void Init()
    {
        members.Add(this);
        if (decNumber != null)
        {
            foreach (DecoderNumber n in decNumber)
            {
                n.SubscribeCodeField(this);
            }
        }
        image = GetComponent<Image>();
    }

    public void OnClick()
    {
        //Debug.Log("Click");
        ++valuteIndex;
        if (valuteIndex > 3)
        {
            valuteIndex = 0;
        }
        Value = values[valuteIndex];
        _window.CheckSolution();
    }

    private void SetColor(int i)
    {
        if(image == null)
        {
            image = GetComponent<Image>();
        }

        if(i == 0)
        {
            image.color = Color.yellow;
        }
        else if(i == 1)
        {
            image.color = Color.red;
        }else if(i == 2)
        {
            image.color = Color.blue;
        }else if (i == 4)
        {
            image.color = Color.green;
        }
    }

    private void ResetValue()
    {
        Value = values[0];
    }
    private void Randomize()
    {
        valuteIndex = Random.Range(0,4);
        Value = values[valuteIndex];
    }



    static public void RNDAll()
    {
        foreach(SecurityCode sc in members)
        {
            sc.Randomize();
        }
    }

    static public void ResetAll()
    {
        foreach (SecurityCode sc in members)
        {
            sc.ResetValue();
        }
    }
}
