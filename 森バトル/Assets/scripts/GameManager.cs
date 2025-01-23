using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerBehavior[] players;
    [SerializeField]
    private int turnCounter = 1;
    [SerializeField]
    private float ironDensity;
    [SerializeField]
    private float copperDensity;
    [SerializeField]
    private float forestDensity;
    [SerializeField]
    private int playerCount;
    [SerializeField]
    private int maxIronPerChunk;
    [SerializeField]
    private int maxCopperPerChunk;
    [SerializeField]
    private float dirtDensity;
    [SerializeField]
    private float stoneDensity; //For painting the map
    [SerializeField]
    private float rockDensity; //For distributing resources
    [SerializeField]
    private float forestMovementPenalty;
    [SerializeField]
    private float heightMovementPenalty;
    public int chopYield;
    public HexCell startingCell;
    public float waterLevel;
    [SerializeField] private GameObject water;


    public float seedX;
    public float seedY;
    public float seedZ;
    
    public HexGrid hexGrid;

    public ResourceBehavior ironPrefab;
    public ResourceBehavior copperPrefab;
    public ResourceBehavior forestPrefab;
    public ResourceBehavior stonePrefab;

    private void Awake()
    {
        players[0].IsTurn = true;
        activePlayer = players[0];
    }
    private void Start()
    {
        water.transform.position = new Vector3(0f, waterLevel, 0f);
    }
    public PlayerBehavior[] Players
    {
        get { return players; }
    }
    private PlayerBehavior activePlayer;

    public PlayerBehavior ActivePlayer
    {
        get{ return activePlayer; }
        set{ activePlayer = value; }
    }  
    public void rollToNextPlayer()
    {
        //Passes the turn to the next player in line
        activePlayer.AddResources();
        activePlayer.RefreshArmies();
        if(players.Length != 1)
        {
            if (activePlayer == players[players.Length - 1])
            {
                activePlayer.IsTurn = false;
                activePlayer = players[0];
                activePlayer.IsTurn = true;
            }
            else
            {
                for (int i = 0; i > players.Length; i++)
                {
                    if (players[i] == activePlayer)
                    {
                        activePlayer.IsTurn = false;
                        activePlayer = players[i + 1];
                        activePlayer.IsTurn = true;
                        break;
                    }
                }
            }
        }
        if(activePlayer == players[0])
        {
            turnCounter++;
        }
    }
    public float IronDensity
    {
        get { return ironDensity; }
        set { ironDensity = value; }
    }
    public float CopperDensity
    {
        get { return copperDensity; }
        set { copperDensity = value; }
    }
    public float ForestDensity
    {
        get { return forestDensity; }
        set { forestDensity = value; }
    }
    public int MaxIronPerChunk
    {
        get { return maxIronPerChunk; }
        set { maxIronPerChunk = value; }
    }
    public int MaxCopperPerChunk
    {
        get { return maxCopperPerChunk; }
        set { maxCopperPerChunk = value; }
    }
    public float DirtDensity
    {
        get { return dirtDensity; }
        set { dirtDensity = value; }
    }
    public float StoneDensity
    {
        get { return stoneDensity; }
        set { stoneDensity = value; }
    }
    public float RockDensity
    {
        get { return rockDensity; }
        set { rockDensity = value; }
    }
    public float ForestMovementPenalty
    {
        get { return forestMovementPenalty; }
    }
    public float HeightMovementPenalty
    {
        get {return heightMovementPenalty; }
    }
}
