using UnityEngine;

public class Resource
{
    public GameManager gameManager;

    private string name;
    private string description;
    private Player owner;

    private float volumePerTurn;

    public Resource(string name, string description, Player owner, float volumePerTurn)
    {
        this.name = name;
        this.description = description;
        this.owner = owner;
        this.volumePerTurn = volumePerTurn;
    }

    public Player Owner
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

    public void AddResourceToPool()
    {
        for (int i = 0; i < owner.ResourcePools.Length; i++)
        {
            if (owner.ResourceTypes[i].name == name)
            {
                owner.ResourcePools[i] += volumePerTurn;
            }
        }
    }
}
