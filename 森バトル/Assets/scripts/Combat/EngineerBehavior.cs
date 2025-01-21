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
    public void PlaceTrainingGrounds()
    {
        Debug.Log("Attempting to place training grounds");
        if(movement.currentCell.occupyingBuilding == null && placeMode)
        {
            GameObject buildingA = Instantiate(trainingGround, this.transform.position, Quaternion.Euler(new Vector3(0f, 0f, 0f)));
            TrainingGroundsBehavior trainingGroundsBehavior = buildingA.GetComponent<TrainingGroundsBehavior>();
            trainingGroundsBehavior.Owner = combat.Owner;
            trainingGroundsBehavior.Cell = movement.currentCell;
            trainingGroundsBehavior.GameManager = combat.GameManager;
            movement.currentCell.occupyingBuilding = trainingGroundsBehavior.Building;
            trainingGroundsBehavior.Place();
        }
    }
    public void PlaceResourceExtractor()
    {
        Debug.Log("Attempting to place resource extraction");
        if (movement.currentCell.occupyingBuilding == null && placeMode)
        {
            if(movement.currentCell.occupyingForest != null)
            {
                GameObject buildingA = Instantiate(lumberMill, this.transform.position, Quaternion.Euler(new Vector3(0f, 0f, 0f)));
                ResourceExtractionBehavior resourceExtractionBehavior = buildingA.GetComponent<ResourceExtractionBehavior>();
                resourceExtractionBehavior.Owner = combat.Owner;
                resourceExtractionBehavior.Cell = movement.currentCell;
                movement.currentCell.occupyingBuilding = resourceExtractionBehavior.Building;
                resourceExtractionBehavior.Place();
            }
            else if(movement.currentCell.occupyingResource.resourceName != null)
            {
                GameObject buildingA = Instantiate(mine, this.transform.position, Quaternion.Euler(new Vector3(0f, 0f, 0f)));
                ResourceExtractionBehavior resourceExtractionBehavior = buildingA.GetComponent<ResourceExtractionBehavior>();
                resourceExtractionBehavior.Owner = combat.Owner;
                resourceExtractionBehavior.Cell = movement.currentCell;
                movement.currentCell.occupyingBuilding = resourceExtractionBehavior.Building;
                resourceExtractionBehavior.Place();
            }
        }
    }
    public void Chop()
    {
        if(movement.currentCell.occupyingBuilding == null && movement.currentCell.occupyingForest != null)
        {
            Destroy(movement.currentCell.occupyingForest.gameObject);
            combat.Owner.ResourcePools[0] += 10;
            combat.Owner.updateResourceCounters();
        }
    }
    public bool PlaceMode
    {
        get { return placeMode; }
        set { placeMode = value; }
    }
}
