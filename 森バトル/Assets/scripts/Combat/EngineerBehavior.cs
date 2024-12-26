using UnityEngine;

public class EngineerBehavior : MonoBehaviour
{
    [SerializeField] private MovementBehavior movement;
    [SerializeField] private CombatBehavior combat;

    [SerializeField] private GameObject trainingGround;
    [SerializeField] private GameObject mine;
    [SerializeField] private GameObject lumberMill;

    [SerializeField] private bool placeMode;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaceTrainingGrounds()
    {
        Debug.Log("Attempting to place training grounds");
        if(movement.currentCell.occupyingBuilding == null)
        {
            GameObject buildingA = Instantiate(trainingGround, this.transform.position, Quaternion.Euler(new Vector3(0f, 0f, 0f)));
            TrainingGroundsBehavior trainingGroundsBehavior = buildingA.GetComponent<TrainingGroundsBehavior>();
            trainingGroundsBehavior.Owner = combat.Owner;
            trainingGroundsBehavior.Cell = movement.currentCell;
            movement.currentCell.occupyingBuilding = trainingGroundsBehavior.Building;
            trainingGroundsBehavior.Place();
        }
    }

    public bool PlaceMode
    {
        get { return placeMode; }
        set { placeMode = value; }
    }
}
