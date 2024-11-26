using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Player[] players;

    public Player[] Players
    {
        get { return players; }
    }

    private Player activePlayer;

    public Player getActivePlayer()
    {
        return activePlayer;
    }
    
    public void rollToNextPlayer()
    {
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
}
