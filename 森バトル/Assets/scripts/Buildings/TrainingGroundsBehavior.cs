using UnityEngine;

public class TrainingGroundsBehavior : MonoBehaviour
{
    [SerializeField] private string bName;
    [SerializeField] private string description;
    [SerializeField] private PlayerBehavior owner;
    private Building building;
    [SerializeField] private HexCell cell;
    [SerializeField] private bool isStartingBuilding;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float heightOffset;
    [SerializeField] private float lateralOffset;
    [SerializeField] private float forwardOffset;

    [SerializeField] private int woodCost;
    [SerializeField] private int stoneCost;
    [SerializeField] private int copperCost;
    [SerializeField] private int ironCost;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(isStartingBuilding)
        {
            cell = InitialCell();
        }
        else if(!isStartingBuilding)
        {
            owner.ResourcePools[0] -= woodCost;
            owner.ResourcePools[1] -= stoneCost;
            owner.ResourcePools[2] -= copperCost;
            owner.ResourcePools[3] -= ironCost;
        }
        Place();
        building = new Building(bName, description, owner, cell);
        cell.occupyingBuilding = building;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        if(cell.occupyingBuilding!= null)
        {
            cell.occupyingBuilding = null;
        }
        if(owner.SelectedTrainingGrounds != null)
        {
            owner.SelectedTrainingGrounds = null;
        }
    }

    public void makeArmy(int armyIndex)
    {
        if(cell.occupyingArmy == null)
        {
            GameObject army = Instantiate(owner.PossibleArmies[armyIndex], this.transform.position, Quaternion.Euler(new Vector3(0f, 0f, 0f)));
            CombatBehavior combatBehavior = army.GetComponent<CombatBehavior>();
            combatBehavior.Owner = owner;
            combatBehavior.Movement.Owner = owner;
            combatBehavior.Movement.currentCell = cell;
            combatBehavior.Movement.GameManager = gameManager;
            combatBehavior.GameManager = gameManager;
            cell.occupyingArmy = combatBehavior;
            combatBehavior.Movement.MoveToCell();
            Debug.Log("Army Placed at " + cell.coordinates);
        }
        Debug.Log("Army already at location");
    }

    private HexCell InitialCell()
    {
        Vector3 initialPosition = new Vector3(HexMetrics.outerRadius * gameManager.hexGrid.cellCountX / 8.5f, 0f, HexMetrics.outerRadius * gameManager.hexGrid.cellCountZ / 1.5f);
        HexCell initialCell = gameManager.hexGrid.GetCell(initialPosition);
        if (initialCell.occupyingArmy != null)
        {
            for (int i = 0; i < initialCell.Neighbors.Length; i++)
            {
                if (initialCell.Neighbors[i].occupyingArmy == null)
                {
                    initialCell = initialCell.Neighbors[i];
                    break;
                }
            }
        }
        return initialCell;
    }

    public void Place()
    {
        transform.position = new Vector3(cell.Position.x, cell.Position.y + heightOffset, cell.Position.z);
        transform.position += transform.forward * forwardOffset + transform.right * lateralOffset;
        cell.occupyingBuilding = building;
    }

    public Building Building
    {
        get { return building; }
        set { building = value; }
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
    public GameManager GameManager
    {
        get { return gameManager; }
        set { gameManager = value; }
    }
    public int WoodCost
    {
        get { return woodCost; }
        set { woodCost = value; }
    }
    public int StoneCost
    {
        get { return stoneCost; }
        set { stoneCost = value; }
    }
    public int CopperCost
    {
        get { return copperCost; }
        set { copperCost = value; }
    }
        public int IronCost
    {
        get { return ironCost; }
        set { ironCost = value; }
    }
}

