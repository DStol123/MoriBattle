using UnityEngine;

public class Resource
{
    public GameManager gameManager;

    private string name; //For in game tooltip over deposits
    private string description; //For in game tooltip over deposits
    private PlayerBehavior owner; //To decide who gets the resource income from a deposit

    private float volumePerTurn; //How much income this deposit provides

    //Constructor
    public Resource(string name, string description, PlayerBehavior owner, float volumePerTurn)
    {
        this.name = name;
        this.description = description;
        this.owner = owner;
        this.volumePerTurn = volumePerTurn;
    }

    public PlayerBehavior Owner
    {
        get { return owner; }
        set
        {
            for (int i = 0; i < gameManager.Players.Length; i++)
            {
                if (value == gameManager.Players[i])
                {
                    owner = value;
                    break;
                }
            }
        }
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public string Description
    { 
        get { return description; }
        set {  description = value; }
    }

    public float VolumePerTurn
    {
        get { return volumePerTurn; }
        set { volumePerTurn = value; }
    }

    //Adds this resource's income to the player's collection of resource incomes
    public void AddResourceToPool()
    {
        for (int i = 0; i < owner.ResourcePools.Length; i++)
        {
            if (owner.ResourceTypes[i].name == name)
            {
                owner.ResourceIncomes[i] += volumePerTurn;
            }
        }
    }
}
