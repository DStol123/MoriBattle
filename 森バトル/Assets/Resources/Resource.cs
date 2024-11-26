using UnityEngine;

public class Resource
{
    public GameManager gameManager;

    private string name;
    private string description;
    private Player owner;

    public Resource(string name, string description, Player owner)
    {
        this.name = name;
        this.description = description;
        this.owner = owner;
    }

    public Player Owner
    {
        get {return owner; }
        set
        {
            for(int i = 0; i < gameManager.Players.Length; i++)
            {
                if(value == gameManager.Players[i])
                {
                    owner = value;
                    break;
                }
            }
        }
    }
}
