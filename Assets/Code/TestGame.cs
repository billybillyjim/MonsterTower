using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TestGame : MonoBehaviour {

    public GameRun game;
    public TowerMap tower;
    public EventManager eventManager;
    public FactionManager factionManager;

    public Text humanPop;
    public Text zombiePop;
    public Text witchPop;
    public Text demonPop;

    void Update()
    {
        humanPop.text = tower.getHumanPop().ToString();
        zombiePop.text = tower.getZombiePop().ToString();
        witchPop.text = tower.getWitchPop().ToString();
        demonPop.text = tower.getDemonPop().ToString();
    }
	public void increasePopulation(int i)
    {
        tower.setTotalPopulation(tower.getPopulation() + i);
    }
    public void increaseHumanPopulation(int i)
    {
        tower.addHumanPop(i);
    }
    public void increaseZombiePopulation(int i)
    {
        tower.addZombiePop(i);
    }
    public void increaseWitchPopulation(int i)
    {
        tower.addWitchPop(i);
    }
    public void increaseDemonPopulation(int i)
    {
        tower.addDemonPop(i);
    }
    
}
