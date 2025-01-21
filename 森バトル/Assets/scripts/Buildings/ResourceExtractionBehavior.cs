using UnityEngine;
using UnityEngine.UIElements;

public class ResourceExtractionBehavior : MonoBehaviour
{
    [SerializeField] PlayerBehavior owner;
    [SerializeField] private string bName;
    [SerializeField] private string description;
    [SerializeField] private float heightOffset;
    [SerializeField] private float forwardOffset;
    [SerializeField] private float lateralOffset;
    private Building building;
    [SerializeField] ResourceBehavior resource;
    [SerializeField] HexCell cell;
    [SerializeField] int woodCost;
    [SerializeField] int stoneCost;
    [SerializeField] int copperCost;
    [SerializeField] int ironCost;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log($"Before: Wood: {owner.ResourcePools[0]}");
        owner.ResourcePools[0] -= woodCost;
        Debug.Log("Removed " + woodCost + " wood from pool");
        Debug.Log($"After: Wood: {owner.ResourcePools[0]}");
        Debug.Log($"Before: Stone: {owner.ResourcePools[1]}");
        owner.ResourcePools[1] -= stoneCost;
        Debug.Log("Removed " + stoneCost + " stone from pool");
        Debug.Log($"After: Stone: {owner.ResourcePools[1]}");
        Debug.Log($"Before: Copper: {owner.ResourcePools[2]}");
        owner.ResourcePools[2] -= copperCost;
        Debug.Log("Removed " + copperCost + " copper from pool");
        Debug.Log($"After: Copper: {owner.ResourcePools[2]}");
        Debug.Log($"Before: Iron: {owner.ResourcePools[3]}");
        owner.ResourcePools[3] -= ironCost;
        Debug.Log("Removed " + ironCost + " iron from pool");
        Debug.Log($"After: Iron: {owner.ResourcePools[3]}");
        Debug.Log($"Wood: {owner.ResourcePools[0]} Stone: {owner.ResourcePools[1]} Copper: {owner.ResourcePools[2]} Iron: {owner.ResourcePools[3]}");
        DetermineResource();
        AddResourceToIncome();
        owner.updateResourceCounters();
        Place();
        building = new Building(bName, description, owner, cell);
        cell.occupyingBuilding = building;
    }

    // Update is called once per frame
    void OnDestroy()
    {
        RemoveResourceFromIncome();
    }
    public void DetermineResource()
    {
        if(cell.occupyingForest != null)
        {
            resource = owner.ResourceTypes[0];
        }
        else
        {
            resource = cell.occupyingResource;
        }
    }
    public void AddResourceToIncome()
    {
        for(int i = 0; i<owner.resourceTypes.Length; i++)
        {
            if (owner.resourceTypes[i].resourceName == resource.resourceName)
            {
                owner.resourceIncomes[i] += resource.Volume;
                break;
            }
        }
    }
    public void RemoveResourceFromIncome()
    {
        for (int i = 0; i < owner.resourceTypes.Length; i++)
        {
            if (owner.resourceTypes[i] == resource)
            {
                owner.resourceIncomes[i] -= resource.Volume;
                break;
            }
        }
    }
    public void Place()
    {
        if(cell != null)
        {
            transform.position = new Vector3(cell.Position.x, cell.Position.y + heightOffset, cell.Position.z);
            transform.position += transform.forward * forwardOffset + transform.right * lateralOffset;
            cell.occupyingBuilding = building;
        }
    }

    public PlayerBehavior Owner
    {
        get { return owner; }
        set { owner = value; }
    }
    public Building Building
    {
        get { return building; }
        set { building = value; }
    }
    public HexCell Cell
    {
        get { return cell; }
        set { cell = value; }
    }
}
