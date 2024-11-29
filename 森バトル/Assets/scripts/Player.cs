using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Player
{
    private string userName;
    private Color color; //The color for each of this player's units and icons.
    //このプレーヤーの軍とアイコンの色

    private Resource[] resourceTypes;
    private float[] resourcePools;

    //private Dictionary<Resource, float> resources = resourceTypes.Zip(resourcePools, (key, value) => new { key, value }).ToDictionary(pair => pair.key, pair => pair.value);

    private bool isAlive;

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

    public Resource[] ResourceTypes
    {
        get { return resourceTypes; }
        set { resourceTypes = value; }
    }

    public float[] ResourcePools
    {
        get { return resourcePools; }
        set { resourcePools = value; }
    }

    public bool IsAlive
    {
        get { return isAlive; }
        set { isAlive = value; }
    }
}
