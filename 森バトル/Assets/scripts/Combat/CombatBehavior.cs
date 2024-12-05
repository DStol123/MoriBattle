using UnityEngine;
using UnityEngine.EventSystems;

public class CombatBehavior : MonoBehaviour
{
    [SerializeField]
    private static int startingHp;
    [SerializeField]
    private static float startingMovementPoints;
    [SerializeField]
    private PlayerBehavior owner;

    public Combat Combat;
    [SerializeField]
    private float armyHeight; //This will be used to keep the model above the ground


    public HexCell currentCell;
    private HexCell destinationCell;

    private bool isSelected = false;

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
            return Combat.hexGrid.GetCell(hit.point);
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
        destinationCell = Combat.hexGrid.GetCell(position);
    }

    private void MoveToCell(HexCell cell)
    {
        transform.position = new Vector3(cell.Position.x, cell.Position.y + armyHeight, cell.Position.z);
    }

    private void Attack(CombatBehavior target, int damage)
    {
        if(target.Combat.Owner != this.Combat.Owner)
        {
            target.Combat.TakeDamage(damage);
        }
    }

    public HexCell DestinationCell
    {
        get { return destinationCell; }
        set { destinationCell = value; }
    }
}
