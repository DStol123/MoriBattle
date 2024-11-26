using UnityEngine;

public class Player
{
    private string userName;
    private Color color;

    private bool isAlive;

    public bool IsAlive
    {
        get { return isAlive; }
        set { isAlive = value; }
    }
}
