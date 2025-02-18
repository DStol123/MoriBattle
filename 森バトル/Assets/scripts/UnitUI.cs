using UnityEngine;

public class UnitUI : MonoBehaviour
{
    public GameManager gameManager;
    private GameObject mainCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
        }
        mainCamera = gameManager.ActivePlayer.gameCamera;
    }

    // Update is called once per frame
    void Update()
    {
        if(mainCamera != gameManager.ActivePlayer.gameCamera)
        {
            mainCamera = gameManager.ActivePlayer.gameCamera;
        }
        transform.rotation = mainCamera.transform.rotation;
    }
}
