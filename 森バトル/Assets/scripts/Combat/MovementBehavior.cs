using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MovementBehavior : MonoBehaviour
{
    public Image mpBar;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CombatBehavior combat;
    [SerializeField] private PlayerBehavior owner;

    [SerializeField] private bool isStartingArmy;

    [SerializeField] private float maxMovementPoints;
    [SerializeField] private float startingMovementPoints;
    [SerializeField] private float currentMovementPoints;

    [SerializeField] private float armyHeight; //This will be used to keep the model above the ground
    [SerializeField] private float forwardOffset;
    [SerializeField] private float lateralOffset;//This will be used to keep the model in the center of its cell

    [SerializeField] private bool moveMode;

    public HexCell currentCell;
    private HexCell destinationCell;
    void Start()
    {
        owner.allOwnedMovementBehaviors.Add(this);
        Debug.Log(owner.allOwnedMovementBehaviors);
        if (isStartingArmy)
        {
            currentCell = InitialCell();
            MoveToCell();
            owner.playerCamera.Target.transform.position = transform.position;
            owner.playerCamera.FollowTarget();
        }
    }

    private void OnDestroy()
    {
        owner.allOwnedMovementBehaviors.Remove(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (
            moveMode &&
            Input.GetMouseButtonDown(1) &&
            !EventSystem.current.IsPointerOverGameObject() &&
            combat.IsSelected &&
            owner.IsTurn && ChangeCell() != null
            )
        {

            HexCell cell2 = ChangeCell();
            MoveToCell(cell2);

        }
    }

    private HexCell InitialCell()
    {
        float sector = 10f;
        if (owner == null)
        {
            sector = 9f;
        }
        Vector3 initialPosition = new Vector3(HexMetrics.outerRadius * gameManager.hexGrid.cellCountX / sector, 0f, HexMetrics.outerRadius * gameManager.hexGrid.cellCountZ / 2f);
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

    private HexCell ChangeCell()
    {
        HexCell newCell = null;
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit) && hit.collider.GetComponent<CombatBehavior>() == null)
        {
            if (currentCell == null)
            {
                newCell = gameManager.hexGrid.GetCell(hit.point);
            }
            else
            {
                for (int i = 0; i < currentCell.Neighbors.Length; i++)
                {
                    if (gameManager.hexGrid.GetCell(hit.point) == currentCell.Neighbors[i])
                    {
                        newCell = gameManager.hexGrid.GetCell(hit.point);
                        break;
                    }
                }
            }

        }
        if (newCell == null)
        {
            Debug.Log("Not a neighbor.");
        }
        return newCell;
    }

    private float calculatePointCost(HexCell nextCell)
    {
        float pointCost = 1f;
        if (Mathf.Abs(nextCell.Elevation - currentCell.Elevation) >= 0.009f)
        {
            pointCost = 100f;
            Debug.Log(pointCost);
            return pointCost;
        }  
        if (nextCell.occupyingForest != null)
        {
            pointCost *= gameManager.ForestMovementPenalty;
        }
        if (nextCell.Elevation - currentCell.Elevation >= 0.005f)
        {
            pointCost *= gameManager.HeightMovementPenalty * (1f + (nextCell.Elevation - currentCell.Elevation) * 10f);
        }
        Debug.Log(pointCost);
        return pointCost;
    }

    public void MoveToCell()
    {
        transform.position = new Vector3(currentCell.Position.x, currentCell.Position.y + armyHeight, currentCell.Position.z);
        transform.position += transform.forward * forwardOffset + transform.right * lateralOffset;
        currentCell.occupyingArmy = combat;
    }

    private void SelectDestination(Vector3 position)
    {
        destinationCell = gameManager.hexGrid.GetCell(position);
    }

    public void MoveToCell(HexCell cell)
    {
        if ((currentCell == null || calculatePointCost(cell) <= currentMovementPoints) && cell.occupyingArmy == null)
        {
            if (currentCell != null)
            {
                currentCell.occupyingArmy = null;
                float moveCost = calculatePointCost(cell);
                RotateArmy(cell);
                Debug.Log(moveCost);
                UseMovementPoints(moveCost);
            }
            transform.position = new Vector3(cell.Position.x, cell.Position.y + armyHeight, cell.Position.z);
            transform.position += transform.forward * forwardOffset + transform.right * lateralOffset;
            currentCell = cell;
            cell.occupyingArmy = combat;
        }
    }

    public void RotateArmy(HexCell hexCell)
    {
        int sides = 0;
        for (int i = 0; i < currentCell.Neighbors.Length; i++)
        {
            if (hexCell == currentCell.Neighbors[i]) { sides = i - 1; break; }
        }
        transform.rotation = Quaternion.Euler(new Vector3(0f, sides * 60f + 90f, 0f));
    }

    public bool UseMovementPoints(float amount)
    {
        Debug.Log("Used " + amount + " MP");
        if (currentMovementPoints >= amount)
        {
            currentMovementPoints -= amount;
            if(mpBar != null)
            {
                mpBar.fillAmount = currentMovementPoints / maxMovementPoints;
            }
            return true;
        }
        else
        {
            Debug.Log("No points to move the Characters");
            return false;
        }
    }

    public HexCell DestinationCell
    {
        get { return destinationCell; }
        set { destinationCell = value; }
    }
    public float MaxMovementPoints
    {
        get => maxMovementPoints;
        set => maxMovementPoints = Mathf.Max(0, value); // not to reach minus
    }
    public float CurrentMovementPoints
    {
        get => currentMovementPoints;
        set => currentMovementPoints = Mathf.Clamp(value, 0, MaxMovementPoints); // from 0 to MaxMovementPoints
    }
    public PlayerBehavior Owner
    {
        get { return owner; }
        set { owner = value; }
    }
    public bool MoveMode
    {
        get { return moveMode; }
        set { moveMode = value; }
    }
}
