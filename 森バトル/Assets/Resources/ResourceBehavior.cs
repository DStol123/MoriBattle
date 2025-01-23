using UnityEngine;

public class ResourceBehavior : MonoBehaviour
{
    [SerializeField]
    public HexCell cell; //Which cell/tile is this resource sitting on
    public float height; //Adjusts placement so the model isn't buried
    public string resourceName;
    public string description;
    [SerializeField]
    private int volume; //How many units per turn does this deposit give
    public OpacityModifier opacityModifier;
    public MineAttacher mineAttacher;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new Vector3(cell.Position.x, cell.Position.y + height, cell.Position.z);
    }
    public int Volume
    {
        get { return volume; }
        set { volume = value; }
    }
}