using UnityEngine;
using System.Collections;

public class LoadGame : MonoBehaviour {

    public GameRun game;

	public void Load()
    {
       game.setMoney(PlayerPrefs.GetFloat("Player Money"));
    }
}
