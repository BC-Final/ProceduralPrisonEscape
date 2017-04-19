using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecoderWindow : MonoBehaviour {

    private DoorMapIcon _door;
    [SerializeField]
    private Text _text;

    public string Solution;

    List<DecoderNumber> dNumbers;

    private void Awake()
    {
        SecurityCode.members.Clear();
        DecoderNumber.members.Clear();

        dNumbers = new List<DecoderNumber>();
        foreach (DecoderNumber num in GetComponentsInChildren<DecoderNumber>())
        {
            num.Init();
            dNumbers.Add(num);
        }
        foreach (SecurityCode code in GetComponentsInChildren<SecurityCode>())
        {
            code.Init();
            code.SetWindow(this);
        }     

        SecurityCode.RNDAll();
        DecoderNumber.SetSolutionToCurrent();
        SecurityCode.ResetAll();

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_door)
            {
                _door.showWindow = false;
            }
            GameObject.Destroy(this.gameObject);
        }
    }

    public void CheckSolution()
    {
        foreach(DecoderNumber num in dNumbers)
        {
            if (!num.Solved)
            {
                ShowSolution(false);
                return;
            }
        }
        ShowSolution(true);
    }

    public void ShowSolution(bool show)
    {
        if (show)
        {
            _text.text = Solution;
        }else
        {
            _text.text = "";
        }
    }

    public void SetDoor(DoorMapIcon door)
    {
        _door = door;
    }
}
