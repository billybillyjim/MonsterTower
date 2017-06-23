using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesiribilityModifier{

    private string type;
    private float value;

    public DesiribilityModifier()
    {

    }
    public DesiribilityModifier(string s, float f)
    {
        type = s;
        value = f;
    }
	public string getType()
    {
        return type;
    }
    public float getValue()
    {
        return value;
    }
}
