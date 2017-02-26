using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FactionManager : MonoBehaviour {

    private List<Faction> factions = new List<Faction>();
    public Text humanText;
    public Text zombieText;
    public Text witchText;
    public Text demonText;
    public bool testing = true;

	// Use this for initialization
	void Start () {
        factions.Add(new Faction("Humans", 0, 0, 50));
        factions.Add(new Faction("Zombies", 0, 0, 0));
        factions.Add(new Faction("Witches", 0, 0, 0));
        factions.Add(new Faction("Demons", 0, -50, -50));
	}
	
    void Update()
    {
        if (!testing)
        {
            updateText();
        }
       
    }

    private void updateText()
    {
        humanText.text = "Human Faction Prestige: " + factions[0].getPrestige() + "\nHuman Faction Relationship: " + factions[0].getRelationship();
        zombieText.text = "Zombie Faction Prestige: " + factions[1].getPrestige() + "\nZombie Faction Relationship: " + factions[1].getRelationship();
        witchText.text = "Witch Faction Prestige: " + factions[2].getPrestige() + "\nWitch Faction Relationship: " + factions[2].getRelationship();
        demonText.text = "Demon Faction Prestige: " + factions[3].getPrestige() + "\nDemon Faction Relationship: " + factions[3].getRelationship();
    }

    public void trendFactionsTowardNeutral()
    {
        foreach(Faction f in factions)
        {
            if(f.getRelationship() > f.getNeutral())
            {
                f.setRelationship(f.getRelationship() - .01f);
            }
            else if(f.getRelationship() < f.getNeutral())
            {
                f.setRelationship(f.getRelationship() + .01f);
            }
            else
            {

            }
            
        }
    }

	public void addPrestige(int i, float f)
    {
        factions[i].addPrestige(f);
    }
    public void addRelations(int i, float f)
    {
        factions[i].addRelations(f);
    }

}
