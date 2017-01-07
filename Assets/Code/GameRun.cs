using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameRun : MonoBehaviour {

    [SerializeField]
    private TowerMap tower;

    private Building[,] towerMap;

    public static float cash;
    public Text cashtext;


	// Use this for initialization
	void Start () {
        cash = 100000;
	}
	void Update()
    {
        cashtext.text = "$" + cash;
    }
    public static void chargeMoney(float f)
    {
        cash -= f;
    }
    public void addMoney(float f)
    {
        cash += f;
    }

}
