using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool {

    string toolName;
    float cost;
    int width;
    int height;
    int minPop;
    int maxPop;
    int position;
    Transform button;

    public Tool()
    {

    }

    public Tool(string n, float c, int w, int h, int min, int max, int pos)
    {
        toolName = n;
        cost = c;
        width = w;
        height = h;
        minPop = min;
        maxPop = max;
        position = pos;
    }

    public string getName()
    {
        return toolName;
    }
    public float getCost()
    {
        return cost;
    }
    public int getWidth()
    {
        return width;
    }
    public int getHeight()
    {
        return height;
    }
    public int getMaxPop()
    {
        return maxPop;
    }
    public int getMinPop()
    {
        return minPop;
    }
    public int getPosition()
    {
        return position;
    }
    public void setButton(Transform B)
    {
        button = B;
    }
    public Transform getButton()
    {
        return button;
    }
}
