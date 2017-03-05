using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToolTip : MonoBehaviour{

    [SerializeField]
    private GameObject ToolTipBox;
    [SerializeField]
    private Text toolTipText;
    private string tooltip;

    public void setText(string s)
    {
        toolTipText.text = s;
    }
    
}
