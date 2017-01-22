using UnityEngine;
using System.Collections;

public class MinimizePanel : MonoBehaviour {

	void OnMouseExit()
    {
        hidePanel();
    }

    public void showPanel()
    {
        gameObject.SetActive(true);
    }
    public void hidePanel()
    {
        gameObject.SetActive(false);
    }
}
