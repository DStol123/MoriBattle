using UnityEngine;

public class TrainingGroundsBehavior : MonoBehaviour
{
    [SerializeField] private string bName;
    [SerializeField] private string description;
    [SerializeField] private PlayerBehavior owner;
    private Building building;
    [SerializeField] private HexCell cell;
    [SerializeField] private GameObject[] possibleArmies;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        building = new Building(bName, description, owner, cell);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
