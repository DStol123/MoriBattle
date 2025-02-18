using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool paused;

    [SerializeField]
    private PlayerBehavior[] players;
    private int alivePlayers;
    [SerializeField]
    private PlayerBehavior activePlayer;
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
    public GameSoundManager gameSound;
    [SerializeField] private GameObject water;
    [SerializeField] private GameObject pauseMenu;


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
        
    }
    private void Start()
    {
        players[0].IsTurn = true;
        SwitchCamera();
        activePlayer = players[0];
        alivePlayers = players.Length;
        water.transform.position = new Vector3(0f, waterLevel, 0f);
        activePlayer.ClearFogOfWar();
        pauseMenu.SetActive(false);
        paused = false;
    }
    private void Update()
    {
        if(activePlayer != null)
        {
            transform.position = activePlayer.transform.position;
            if(!activePlayer.IsTurn)
            {
                activePlayer.IsTurn = true;
                Debug.Log("Set Active Player's Turn Status");
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(paused)
            {
                pauseMenu.SetActive(false);
                paused = false;
            }
            else
            {
                pauseMenu.SetActive(true);
                paused = true;
            }
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
    public PlayerBehavior[] Players
    {
        get { return players; }
    }
    public PlayerBehavior ActivePlayer
    {
        get{ return activePlayer; }
        set{ activePlayer = value; }
    }  
    public void rollToNextPlayer()
    {
        //Passes the turn to the next player in line
        Debug.Log("Rolling to next player. Current Active Player: " + activePlayer);
        Debug.Log("Adding Resource to Pools");
        activePlayer.AddResources();
        Debug.Log("Resting armies");
        activePlayer.RefreshArmies();
        activePlayer.turnOffUI();
        if(players.Length != 1)
        {
            Debug.Log("Number of players is more than 1");
            if (activePlayer == players[players.Length - 1])
            {
                Debug.Log("Active player is last");
                activePlayer.IsTurn = false;
                activePlayer = players[0];
                activePlayer.IsTurn = true;
            }
            else
            {
                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i] == activePlayer)
                    {
                        Debug.Log("Player is " + i + " in list of players");
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
        Debug.Log("New Active Player: " + activePlayer);
        activePlayer.IsTurn = true;
        activePlayer.ClearFogOfWar();
        activePlayer.updateResourceCounters();
        SwitchCamera();
        Debug.Log("Camera Switched");
    }
    public void SwitchCamera()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].IsTurn)
            {
                players[i].gameCamera.SetActive(true);
            }
            else
            {
                players[i].gameCamera.SetActive(false);
            }
        }
    }
    public void EndGame()
    {

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
    public int AlivePlayers
    {
        get { return alivePlayers; }
        set { alivePlayers = value; }
    }
}
