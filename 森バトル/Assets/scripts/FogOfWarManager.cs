using System.Linq;
using UnityEngine;

public class FogOfWarManager : MonoBehaviour
{
    public GameManager gameManager;
    public HexCell currentCell;
    [SerializeField] private int sightRange;
    public PlayerBehavior owner;
    public HexCell[] cells;
    public HexCell startingCell;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(owner!=null)
        {
            Initialize();
        }
    }
    private void OnDestroy()
    {
        owner.allFogOfWarManagers.Remove(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ClearFogOfWar()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            if (currentCell != null && currentCell.Distance(cells[i]) <= sightRange)
            {
                cells[i].fog.SetActive(false);
            }
        }
    }
    public void BlanketMap()
    {
        for(int i = 0; i < cells.Length; i++)
        {
            cells[i].fog.SetActive(true);
        }
    }
    public void Initialize()
    {
        owner.allFogOfWarManagers.Add(this);
        cells = gameManager.hexGrid.cells.Append(startingCell).ToArray();
    }
}