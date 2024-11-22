using UnityEngine;
using UnityEngine.EventSystems;

public class CombatBehavior : MonoBehaviour
{
    [SerializeField]
    private static int startingHp;
    [SerializeField]
    private static float startingMovementPoints;

    private Combat Combat = new Combat(startingHp, startingMovementPoints); //Max Health of 100 and 3 movement points
    [SerializeField]
    private float armyHeight; //This will be used to keep the model above the ground


    public HexCell currentCell;
    void Start()
    {
        MoveToCell();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
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
        currentCell = Combat.hexGrid.GetCell(position);
    }

    private void MoveToCell(HexCell cell)
    {
        transform.position = new Vector3(cell.Position.x, cell.Position.y + armyHeight, cell.Position.z);
    }
}
