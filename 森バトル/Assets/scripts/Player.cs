using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Player
{
    private string userName;
    private Color color; //The color for each of this player's units and icons.
    //このプレーヤーの軍とアイコンの色
    public Player(string userName, Color color)
    {
        this.userName = userName;
        this.color = color;
    }

    public string UserName
    {
        get { return userName; }
        set { userName = value; }
    }

    public Color Color
    {
        get { return color; }
        set { color = value; }
    }

    
}
