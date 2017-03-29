using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecoderNumber : MonoBehaviour {

    static public List<DecoderNumber> members = new List<DecoderNumber>();

    [SerializeField]
    Texture2D solvedTexture;

    Texture2D wrongTexture;
    Image image;

    [SerializeField]
    int targetValue;
    [SerializeField]
    int valueNumber;
    public int Value
    {
        get { return valueNumber; }
        set { valueNumber = value; }
    }
    List<SecurityCode> codes = new List<SecurityCode>();

    float timer;
    float interval = 0.5f;

    private void Start()
    {
        members.Add(this);
        wrongTexture = new Texture2D(100, 100, TextureFormat.ARGB32, false);
        SetImage(Value);
    }

    private void Update()
    {
        

        //timer += Time.deltaTime;
        //if(timer > interval)
        //{
        //    timer -= interval;
        //    SetImage(Value);
        //}
    }

    public void SubscribeCodeField(SecurityCode code)
    {
        codes.Add(code);
    }

    public void Recalculate()
    {
        int tempValue = 0;
        foreach(SecurityCode i in codes)
        {
            tempValue += i.Value;
        }
        Value = tempValue;
        SetImage(Value);
    }

    public void SetTargetToCurrent()
    {
        targetValue = Value;
        Recalculate();
    }

    public void SetImage(int value)
    {
        if (image == null)
        {
            image = transform.GetComponent<Image>();
        }

        if (value == targetValue)
        {
            image.sprite = Sprite.Create(solvedTexture, new Rect(0, 0, solvedTexture.width, solvedTexture.height), new Vector2(0.5f, 0.5f));
        }else{
            Texture2D rTex = CreateRandomTexture();
            image.sprite= Sprite.Create(rTex, new Rect(0, 0, rTex.width, rTex.height), new Vector2(0.5f, 0.5f));
        }
    }

    public Texture2D CreateRandomTexture()
    {
        
        for(int i=0; i<100; i+=10)
        {
            for(int j=0; j<100; j++)
            {
                int ran = Random.Range(0, 2);
                if(ran == 1) {
                    for (int o = i; o < i + 10; o++)
                    {
                        wrongTexture.SetPixel(o, j+i, Color.black);
                    }
                }else{
                    for (int o = i; o < i + 10; o++)
                    {
                        wrongTexture.SetPixel(o, j+o, Color.white);
                    }
                }
            }
        }
        wrongTexture.Apply();
        return wrongTexture;
    }

    static public void SetSolutionToCurrent()
    {
        foreach (DecoderNumber dn in members)
        {
            dn.SetTargetToCurrent();
        }
    }
}
