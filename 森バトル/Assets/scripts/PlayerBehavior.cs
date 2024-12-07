using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private bool isTurn;
    private Resource[] resourceTypes;
    private float[] resourcePools;
    private bool isAlive;
    private Color color;
    private string userName;
    private List<CombatBehavior> units;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Color Color
    {
        get { return color; }
        set { color = value; }
    }

    public string Username
    {
        get { return userName; }
        set { userName = value; }
    }

    public bool IsTurn
    {
        get { return isTurn; }
        set { isTurn = value; }
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

    public void SelectArmy(CombatBehavior army)
    {

    }
}
