using UnityEngine;
using UnityEngine.EventSystems;

public class CombatBehavior : MonoBehaviour
{
    [SerializeField] private int startingHp;
    [SerializeField] private int maxHp;
    [SerializeField] private int currentHp;
    [SerializeField] private float maxMovementPoints;
    [SerializeField] private float startingMovementPoints;
    [SerializeField] private float currentMovementPoints;
    [SerializeField] private PlayerBehavior owner;
    [SerializeField] private float armyHeight; //This will be used to keep the model above the ground


    public HexCell currentCell;
    private HexCell destinationCell;

    private bool isSelected = false;

    public static HexGrid hexGrid;

    public bool IsSelected
    {
        get { return isSelected; }
        set { isSelected = value; }
    }
    void Start()
    {
        MoveToCell();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && isSelected)
        {
            if(ChangeCell() != null)
            {
                currentCell = ChangeCell();
                MoveToCell();
            }
        }
    }


    private HexCell ChangeCell()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(inputRay, out hit))
        {
            return CombatBehavior.hexGrid.GetCell(hit.point);
        }
        else
        {
            return null;
        }
    }

    private void MoveToCell()
    {
        transform.position = new Vector3(currentCell.Position.x, currentCell.Position.y + armyHeight, currentCell.Position.z);
    }

    private void SelectDestination(Vector3 position)
    {
        destinationCell = CombatBehavior.hexGrid.GetCell(position);
    }

    private void MoveToCell(HexCell cell)
    {
        transform.position = new Vector3(cell.Position.x, cell.Position.y + armyHeight, cell.Position.z);
    }

    private void Attack(CombatBehavior target, int damage)
    {
        if(target.Owner != this.Owner)
        {
            target.TakeDamage(damage);
        }
    }

    public void TakeDamage(int amount)
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

    public void Heal(int amount)
    {
        CurrentHp += amount;
    }

    public void Attack(int damage, CombatBehavior unit)
    {
        unit.TakeDamage(damage);
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

    public int MaxHp
    {
        get => maxHp; // setting maxHp with get comma
        set => maxHp = Mathf.Max(1, value); // set as not to reach zero
    }

    public int CurrentHp
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
