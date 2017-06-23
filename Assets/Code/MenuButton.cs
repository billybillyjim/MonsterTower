using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour {

    public Button button;
    public string hotKey;
    public string buttonName;

    public void setButtonName(string s)
    {
        buttonName = s;
    }
    public string getButtonName()
    {
        return buttonName;
    }

}
