using UnityEngine;
using UnityEngine.EventSystems;

public class CombatBehavior : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float startingHp;
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;
    [SerializeField] private float maxMovementPoints;
    [SerializeField] private float startingMovementPoints;
    [SerializeField] private float currentMovementPoints;
    [SerializeField] private PlayerBehavior owner;
    [SerializeField] private float armyHeight; //This will be used to keep the model above the ground
    [SerializeField] private float forwardOffset;
    [SerializeField] private float lateralOffset;//This will be used to keep the model in the center of its cell
    [SerializeField] private float forestAttackPenalty;
    [SerializeField] private float heightAttackPenalty;
    [SerializeField] private float forestDefenceBoost;
    [SerializeField] private float heightDefenceBoost;
    [SerializeField] private float morale;


    public HexCell currentCell;
    private HexCell destinationCell;

    [SerializeField]private bool isSelected = true;

    public static HexGrid hexGrid;

    public bool IsSelected
    {
        get { return isSelected; }
        set { isSelected = value; }
    }
    void Start()
    {
        if(currentCell != null)
        {
            MoveToCell();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && isSelected)
        {
            if(ChangeCell() != null)
            {
                HexCell cell2 = ChangeCell();
                MoveToCell(cell2);
            }
        }
    }


    private HexCell ChangeCell()
    {
        HexCell newCell = null;
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(inputRay, out hit) && hit.collider.GetComponent<CombatBehavior>() == null)
        {
            if(currentCell == null)
            {
                newCell = CombatBehavior.hexGrid.GetCell(hit.point);
            }
            else
            {
                for (int i = 0; i < currentCell.Neighbors.Length; i++)
                {
                    if (CombatBehavior.hexGrid.GetCell(hit.point) == currentCell.Neighbors[i])
                    {
                        newCell = CombatBehavior.hexGrid.GetCell(hit.point);
                        break;
                    }
                }
            }
            
        }
        if(newCell == null)
        {
            Debug.Log("Not a neighbor.");
        }
        return newCell;
    }

    private float calculatePointCost(HexCell nextCell)
    {
        float pointCost = 1f;
        if(nextCell.occupyingForest!=null)
        {
            pointCost *= gameManager.ForestMovementPenalty;
        }
        if(nextCell.Elevation - currentCell.Elevation >= 0.05f)
        {
            pointCost *= gameManager.HeightMovementPenalty * (1f + nextCell.Elevation - currentCell.Elevation);
        }
        return pointCost;
    }

    private void MoveToCell()
    {
        transform.position = new Vector3(currentCell.Position.x, currentCell.Position.y + armyHeight, currentCell.Position.z);
        transform.position += transform.forward * forwardOffset + transform.right * lateralOffset;
    }

    private void SelectDestination(Vector3 position)
    {
        destinationCell = CombatBehavior.hexGrid.GetCell(position);
    }

    private void MoveToCell(HexCell cell)
    {
        if (currentCell == null || calculatePointCost(cell) <= currentMovementPoints)
        {
            if (currentCell != null)
            {
                float moveCost = calculatePointCost(cell);
                RotateArmy(cell);
                Debug.Log(moveCost);
                UseMovementPoints(moveCost);
            }
            transform.position = new Vector3(cell.Position.x, cell.Position.y + armyHeight, cell.Position.z);
            transform.position += transform.forward * forwardOffset + transform.right * lateralOffset;
            currentCell = cell;
        }
    }

    private void Attack(CombatBehavior target, int damage)
    {
        if(target.Owner != this.Owner)
        {
            target.TakeDamage(damage);
        }
    }

    public void RotateArmy(HexCell hexCell)
    {
        int sides = 0;
        for(int i = 0; i < currentCell.Neighbors.Length; i++)
        {
            if (hexCell == currentCell.Neighbors[i]) { sides = i - 1; break; }
        }
        transform.rotation = Quaternion.Euler(new Vector3(0f, sides * 60f + 90f, 0f));
    }

    public float CalculateDamage(CombatBehavior target, float baseDamage)
    {
        if(target.currentCell.occupyingForest != null)
        {
            baseDamage /= target.forestDefenceBoost * forestAttackPenalty;
        }
        baseDamage *= (1 + (target.currentCell.Elevation - currentCell.Elevation))
            / (target.heightDefenceBoost * heightAttackPenalty);
        return baseDamage;
    }

    public void TakeDamage(float amount)
    {
        CurrentHp -= amount; // Decrease CurrentHp
        if (CurrentHp <= 0)
        {
            Debug.Log("The Character was dead!"); //message
        }
    }

    public bool UseMovementPoints(float amount)
    {
        if (CurrentMovementPoints >= amount)
        {
            CurrentMovementPoints -= amount;
            return true;
        }
        else
        {
            Debug.Log("No points to move the Characters");
            return false;
        }
    }

    public void Heal(float amount)
    {
        CurrentHp += amount;
    }

    public void Attack(float damage, CombatBehavior unit)
    {
        unit.TakeDamage(CalculateDamage(unit, damage));
    }

    public HexCell DestinationCell
    {
        get { return destinationCell; }
        set { destinationCell = value; }
    }

    public PlayerBehavior Owner
    {
        get { return owner; }
        set { owner = value; }
    }

    public float MaxHp
    {
        get => maxHp; // setting maxHp with get comma
        set => maxHp = Mathf.Max(1, value); // set as not to reach zero
    }

    public float CurrentHp
    {
        get => currentHp;
        set => currentHp = Mathf.Clamp(value, 0, MaxHp); // from 0 to MaxHp
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
}
