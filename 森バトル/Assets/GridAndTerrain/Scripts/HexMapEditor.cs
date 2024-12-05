
//This script handles everything one can do to change the map. Including changing the texture and height of a cell.
using UnityEngine;
using UnityEngine.EventSystems;


public class HexMapEditor : MonoBehaviour
{
    public Color[] colors;
    public HexGrid hexGrid;
    [SerializeField]private bool touchEnabled;
    private Color activeColor;

    void Awake()
    {
        SelectColor(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && touchEnabled)
        {
            HandleInput();
        }
    }

    //Edits cell selection according to where the mouse clicks.
    void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            EditCell(hexGrid.GetCell(hit.point));
        }
    }

    int activeElevation;
    //Changes the height of a cell to the input value.
    public void SetElevation(float elevation)
    {
        activeElevation = (int)elevation;
    }

    //Changes the cell's color and elevation to selected values.
    void EditCell(HexCell cell)
    {
        cell.Color = activeColor;
        cell.Elevation = activeElevation;
    }

    //Changes active color brush to the input value.
    public void SelectColor (int index)
    {
        activeColor = colors[index];
    }

}
