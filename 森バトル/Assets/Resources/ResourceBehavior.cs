using UnityEngine;

public class ResourceBehavior : MonoBehaviour
{
    [SerializeField]
    public Player owner; //Which player controls this resource
    public Resource resourceType;
    public HexCell cell; //Which cell/tile is this resource sitting on
    public float height; //Adjusts placement so the model isn't buried
    public string resourceName;
    public string description;
    [SerializeField]
    private float volume; //How many units per turn does this deposit give
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resourceType = new Resource(resourceName, description, owner, volume);
        transform.position = new Vector3(cell.Position.x, cell.Position.y + height, cell.Position.z);
    }

    // Update is called once per frame
    void Update()
    {

    }
}