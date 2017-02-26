using UnityEngine;
using System.Collections;

public class SaveGame : MonoBehaviour {

    public GameRun game;

    public void Save()
    {
        PlayerPrefs.SetFloat("Player Money", game.getMoney());
    }
}
