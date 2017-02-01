using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExtendedPanel : MonoBehaviour {

    [SerializeField]
    private Image baseImage;
    [SerializeField]
    private Image[] extendedImages = new Image[20];


    public void setImage(int i)
    {
        baseImage.sprite = extendedImages[i].sprite;
    }
}
