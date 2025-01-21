using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour
{
    public HexCoordinates coordinates;

    public HexGridChunk chunk;

    public GameManager gameManager;

    public CombatBehavior occupyingArmy;
    public ResourceBehavior occupyingResource;
    public ResourceBehavior occupyingForest;
    public Building occupyingBuilding;

    public Color stone;
    public Color grass;
    public Color dirt;

    public HexMapEditor hexMapEditor;
    //public BuildingBehavior occupiedByBuilding;

    Color color;

    [SerializeField]
    HexCell[] neighbors;

    public Color Color
    {
        get
        {
            return color;
        }
        set
        {
            if(color == value)
            {
                return;
            }
            color = value;
            Refresh();
        }
    }

    private void Start()
    {
        color = grass;
        float dirtAdder = Random.Range(0f, 1f);
        if (dirtAdder < gameManager.DirtDensity)
        {
            color = dirt;
        }
        else
        {
            float stoneAdder = Random.Range(0f, 1f);
            if (stoneAdder < gameManager.StoneDensity || occupyingResource == gameManager.ironPrefab || occupyingResource == gameManager.copperPrefab)
            {
                color = stone;
            }
        }
        Refresh();
    }


    int elevation = int.MinValue;

    public RectTransform uiRect;

    public int Elevation
    {
        get
        {
            return elevation;
        }
        set
        {
            if(elevation == value)
            {
                return;
            }
            elevation = value;
            Vector3 position = transform.localPosition;
            position.y = value * HexMetrics.elevationStep;
            position.y += (
                HexMetrics.SampleNoise(position) * 2f - 1f) *
                HexMetrics.elevationPerturbStrength;
            transform.localPosition = position;

            Vector3 uiPosition = uiRect.localPosition;
            uiPosition.z = -position.y;
            uiRect.localPosition = uiPosition;

            Refresh();
        }
    }

    public Vector3 Position
    {
        get { return transform.localPosition; }
    }

    

    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }
    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }
    public bool CheckIfNeighbor(HexCell otherCell)
    {
        bool rBool = false;
        for (int i = 0; i < neighbors.Length; i++)
        {
            if (neighbors[i] == otherCell)
            {
                rBool = true;
                break;
            }
        }
        return rBool;
    }

    void Refresh()
    {
        if(chunk)
        {
            chunk.Refresh();
        }
    }

    public HexCell[] Neighbors
    {
        get { return neighbors; }
    }
}
