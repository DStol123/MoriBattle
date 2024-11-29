using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    [SerializeField]
    int cellCountX, cellCountZ;

    public int chunkCountX = 5, chunkCountZ = 3;

    public HexCell cellPrefab;

    public HexGridChunk chunkPrefab;
    HexGridChunk[] chunks;

    HexCell[] cells;

    public Text cellLabelPrefab;

    public Color defaultColor = Color.white;

    public Texture2D noiseSource;

    public GameManager gameManager;

    void Awake()
    {
        Debug.Log(cellPrefab != null ? "cellPrefab is assigned!" : "cellPrefab is null!");
        HexMetrics.noiseSource = noiseSource; 

        cellCountX = chunkCountX * HexMetrics.chunkSizeX;
        cellCountZ = chunkCountZ * HexMetrics.chunkSizeZ;

        cells = new HexCell[cellCountZ * cellCountX];

        CreateChunks();
        CreateCells();
        Combat.hexGrid = this;
    }


    void CreateChunks()
    {
        chunks = new HexGridChunk[chunkCountX * chunkCountZ];

        for (int z = 0, i = 0; z < chunkCountZ; z++)
            for (int x = 0; x < chunkCountX; x++)
            {
                HexGridChunk chunk = chunks[i++] = Instantiate(chunkPrefab);
                chunk.transform.SetParent(transform);
            }
    }

    void CreateCells()
    {
        for (int z = 0, i = 0; z < cellCountZ; z++)
        {
            for (int x = 0; x < cellCountX; x++)
            {
                CreateCell(x, z, i++);
            }
        }
    }

    private void OnEnable()
    {
        HexMetrics.noiseSource = noiseSource;
    }

    void CreateCell(int x, int z, int i)
    {
        Debug.Log("Creating cell at index: " + i);
        Vector3 position;
        position.x = (x + z * 0.5f - z/2) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        Debug.Log(cellPrefab != null ? "cellPrefab is assigned!" : "cellPrefab is null!");
        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        Debug.Log($"Cell instantiated at index {i}, position {position}");
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.Color = defaultColor;

        if(x > 0)
        {
            cell.SetNeighbor(HexDirection.W, cells[i - 1]);
        }
        if(z > 0)
        {
            if((z&1)==0)
            {
                cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX]);
                if(x > 0)
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX - 1]);
                }
            }
            else
            {
                cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX]);
                if(x < cellCountX - 1)
                {
                    cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX + 1]);
                }
            }
        }

        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();

        cell.uiRect = label.rectTransform;

        cell.Elevation = 0;

        AddCellToChunk(x, z, cell);



        float ironAdder = Random.Range(0f, 1f);
        float copperAdder = Random.Range(0f, 1f);
        float forestAdder = Random.Range(0f, 1f);
        float rockAdder = Random.Range(0f, 1f);

        if(ironAdder < gameManager.IronDensity && cell.occupyingResource == null)
        {
            ResourceBehavior iron = Instantiate(gameManager.ironPrefab);
            cell.occupyingResource = gameManager.ironPrefab;
            iron.cell = cell;
        }
        if(copperAdder < gameManager.CopperDensity && cell.occupyingResource == null)
        {
            ResourceBehavior copper = Instantiate(gameManager.copperPrefab);
            cell.occupyingResource = gameManager.copperPrefab;
            copper.cell = cell;
        }
        if(rockAdder < gameManager.RockDensity && cell.occupyingResource == null)
        {
            ResourceBehavior stone = Instantiate(gameManager.stonePrefab);
            cell.occupyingResource = gameManager.stonePrefab;
            stone.cell = cell;
        }
        if (forestAdder < gameManager.ForestDensity && cell.occupyingForest == null && cell.occupyingResource == null)
        {
            ResourceBehavior forest = Instantiate(gameManager.forestPrefab);
            cell.occupyingForest = gameManager.forestPrefab;
            forest.cell = cell;
        }

    }

    void AddCellToChunk(int x, int z, HexCell cell)
    {
        int chunkX = x / HexMetrics.chunkSizeX;
        int chunkZ = z / HexMetrics.chunkSizeZ;
        HexGridChunk chunk = chunks[chunkX + chunkZ * chunkCountX];

        int localX = x - chunkX * HexMetrics.chunkSizeX;
        int localZ = z - chunkZ * HexMetrics.chunkSizeZ;
        chunk.AddCell(localX + localZ * HexMetrics.chunkSizeX, cell);
    }

        
    //Returns a specific cell based on the the input coordinates.
    public HexCell GetCell(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * cellCountX + coordinates.Z / 2;
        return cells[index];
    }

}
