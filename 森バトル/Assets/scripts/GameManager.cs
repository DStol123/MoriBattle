using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Player[] players;

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

    public ResourceBehavior ironPrefab;
    public ResourceBehavior copperPrefab;
    public ResourceBehavior forestPrefab;
    public ResourceBehavior stonePrefab;

    private void Awake()
    {
        players = new Player[playerCount];
    }

    public Player[] Players
    {
        get { return players; }
    }

    private Player activePlayer;

    public Player ActivePlayer
    {
        get{ return activePlayer; }
        set{ activePlayer = value; }
    }
    
    public void rollToNextPlayer()
    {
    //Passes the turn to the next player in line
        if(activePlayer == players[players.Length - 1])
        {
            activePlayer = players[0];
        }
        else
        {
            for(int i = 0; i > players.Length; i++)
            {
                if (players[i] == activePlayer) { activePlayer = players[i + 1]; break; }
            }
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
}
