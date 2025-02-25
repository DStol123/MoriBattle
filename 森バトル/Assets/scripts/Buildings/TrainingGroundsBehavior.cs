using System;
using UnityEngine;

public class TrainingGroundsBehavior : MonoBehaviour
{
    [SerializeField] private string bName;
    [SerializeField] private string description;
    [SerializeField] private PlayerBehavior owner;
    private Building building;
    public HexCell cell;
    [SerializeField] private bool isStartingBuilding;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float heightOffset;
    [SerializeField] private float lateralOffset;
    [SerializeField] private float forwardOffset;

    public AudioSource audioSource;

    [SerializeField] private int woodCost;
    [SerializeField] private int stoneCost;
    [SerializeField] private int copperCost;
    [SerializeField] private int ironCost;

    [SerializeField] private int sightRange;
    public FogOfWarManager fogOfWarManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(isStartingBuilding)
        {
            cell = InitialCell();
            fogOfWarManager.gameManager = gameManager;
            fogOfWarManager.cells = gameManager.hexGrid.cells;
            Place();
            fogOfWarManager.ClearFogOfWar();
            if(cell.occupyingForest != null)
            {
                Destroy(cell.occupyingForest.gameObject);
            }
        }
        else if(!isStartingBuilding)
        {
            owner.ResourcePools[0] -= woodCost;
            owner.ResourcePools[1] -= stoneCost;
            owner.ResourcePools[2] -= copperCost;
            owner.ResourcePools[3] -= ironCost;
        }
        building = new Building(bName, description, owner, cell);
        cell.occupyingBuilding = building;
        fogOfWarManager.ClearFogOfWar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        if(cell.occupyingBuilding!= null)
        {
            cell.occupyingBuilding = null;
        }
        if(owner.SelectedTrainingGrounds != null)
        {
            owner.SelectedTrainingGrounds = null;
        }
    }

    public void makeArmy(int armyIndex)
    {
        if(cell.occupyingArmy == null)
        {
            GameObject army = Instantiate(owner.PossibleArmies[armyIndex], this.transform.position, Quaternion.Euler(new Vector3(0f, 0f, 0f)));
            CombatBehavior combatBehavior = army.GetComponent<CombatBehavior>();
            combatBehavior.Owner = owner;
            combatBehavior.Movement.Owner = owner;
            combatBehavior.Movement.currentCell = cell;
            combatBehavior.Movement.GameManager = gameManager;
            combatBehavior.GameManager = gameManager;
            combatBehavior.Movement.fogOfWarManager.currentCell = cell;
            combatBehavior.Movement.fogOfWarManager.gameManager = gameManager;
            combatBehavior.Movement.fogOfWarManager.owner = owner;
            combatBehavior.Movement.fogOfWarManager.startingCell = gameManager.startingCell;
            combatBehavior.Movement.fogOfWarManager.Initialize();
            combatBehavior.Movement.fogOfWarManager.ClearFogOfWar();
            cell.occupyingArmy = combatBehavior.Movement;
            combatBehavior.Movement.MoveToCell();
            Debug.Log("Army Placed at " + cell.coordinates);
            PlayConstructionSound();
        }
        Debug.Log("Army already at location");
    }

    private HexCell InitialCell()
    {
        float sector = Array.IndexOf(gameManager.Players, owner) * 30f - 17f;
        if (owner == null)
        {
            sector = 9f;
        }
        Vector3 initialPosition = new Vector3(HexMetrics.outerRadius * gameManager.hexGrid.cellCountX / sector, 0f, HexMetrics.outerRadius * gameManager.hexGrid.cellCountZ / 2f);
        HexCell initialCell = gameManager.hexGrid.GetCell(initialPosition);
        if (initialCell.rocky)
        {
            for (int i = 0; i < initialCell.Neighbors.Length; i++)
            {
                if (!initialCell.Neighbors[i].rocky)
                {
                    initialCell = initialCell.Neighbors[i];
                    break;
                }
            }
        }
        return initialCell;
    }
    private void PlayConstructionSound()
    {
        audioSource.clip = gameManager.gameSound.soundEffects[UnityEngine.Random.Range(0, 1)];
        audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        audioSource.volume = gameManager.gameSound.seVolume;
        audioSource.Play();
    }
    public void Place()
    {
        transform.position = new Vector3(cell.Position.x, cell.Position.y + heightOffset, cell.Position.z);
        transform.position += transform.forward * forwardOffset + transform.right * lateralOffset;
        cell.occupyingBuilding = building;
        fogOfWarManager.currentCell = cell;
    }

    public Building Building
    {
        get { return building; }
        set { building = value; }
    }
    public PlayerBehavior Owner
    {
        get { return owner; }
        set { owner = value; }
    }
    public HexCell Cell
    {
        get { return cell; }
        set { cell = value; }
    }
    public GameManager GameManager
    {
        get { return gameManager; }
        set { gameManager = value; }
    }
    public int WoodCost
    {
        get { return woodCost; }
        set { woodCost = value; }
    }
    public int StoneCost
    {
        get { return stoneCost; }
        set { stoneCost = value; }
    }
    public int CopperCost
    {
        get { return copperCost; }
        set { copperCost = value; }
    }
        public int IronCost
    {
        get { return ironCost; }
        set { ironCost = value; }
    }
}