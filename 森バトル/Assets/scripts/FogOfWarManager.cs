using UnityEngine;

public class FogOfWarManager : MonoBehaviour
{
    public GameManager gameManager;
    public HexCell currentCell;
    [SerializeField] private int sightRange;
    public PlayerBehavior owner;
    public HexCell[] cells;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cells = gameManager.hexGrid.cells;
        owner.allFogOfWarManagers.Add(this);
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
}
