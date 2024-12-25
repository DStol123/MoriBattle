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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (isStartingBuilding)
        {
            cell = InitialCell();
        }
        building = new Building(bName, description, owner, cell);
        cell.occupyingBuilding = building;
        Place();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        cell.occupyingBuilding = null;
        owner.SelectedTrainingGrounds = null;
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

    private void Place()
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
}
