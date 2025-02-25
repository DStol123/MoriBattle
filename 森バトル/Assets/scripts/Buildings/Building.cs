using UnityEngine;

public class Building
{
    private string name;
    private string description;
    private bool isSelected;
    private PlayerBehavior owner;
    private bool isAlive;
    private HexCell cell;

    public Building(string name, string description, PlayerBehavior owner, HexCell cell)
    {
        this.name = name;
        this.description = description;
        this.owner = owner;
        isSelected = false;
        isAlive = true;
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    public string Description
    {
        get { return description; }
        set { description = value; }
    }
    public bool IsSelected
    {
        get { return isSelected; }
        set { isSelected = value; }
    }
    public bool IsAlive
    {
        get { return isAlive; }
        set { isAlive = value; }
    }
    public PlayerBehavior Owner
    {
        get { return owner; }
        set { owner = value; }
    }
    public HexCell Cell
    {
        get { return cell; }
        set { cell = value; }
    }
}