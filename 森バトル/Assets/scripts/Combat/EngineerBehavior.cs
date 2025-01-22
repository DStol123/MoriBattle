using UnityEngine;

public class EngineerBehavior : MonoBehaviour
{
    [SerializeField] private MovementBehavior movement;
    [SerializeField] private CombatBehavior combat;

    

    [SerializeField] private bool placeMode;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlaceTrainingGrounds()
    {
        Debug.Log("Attempting to place training grounds");
        if(movement.currentCell.occupyingBuilding == null && placeMode)
        {
            GameObject buildingA = Instantiate(movement.Owner.trainingGround, this.transform.position, Quaternion.Euler(new Vector3(0f, 0f, 0f)));
            TrainingGroundsBehavior trainingGroundsBehavior = buildingA.GetComponent<TrainingGroundsBehavior>();
            trainingGroundsBehavior.Owner = combat.Owner;
            trainingGroundsBehavior.fogOfWarManager.owner = combat.Owner;
            trainingGroundsBehavior.fogOfWarManager.gameManager = combat.GameManager;
            trainingGroundsBehavior.fogOfWarManager.currentCell = movement.currentCell;
            trainingGroundsBehavior.fogOfWarManager.ClearFogOfWar();
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
            if(movement.currentCell.occupyingForest != null && movement.Owner.Affordable(movement.Owner.lumberMill))
            {
                GameObject buildingA = Instantiate(movement.Owner.lumberMill, this.transform.position, Quaternion.Euler(new Vector3(0f, 0f, 0f)));
                ResourceExtractionBehavior resourceExtractionBehavior = buildingA.GetComponent<ResourceExtractionBehavior>();
                resourceExtractionBehavior.Owner = combat.Owner;
                resourceExtractionBehavior.Cell = movement.currentCell;
                movement.currentCell.occupyingBuilding = resourceExtractionBehavior.Building;
                resourceExtractionBehavior.fogOfWarManager.gameManager = combat.GameManager;
                resourceExtractionBehavior.fogOfWarManager.cells = combat.GameManager.hexGrid.cells;
                resourceExtractionBehavior.fogOfWarManager.owner = combat.Owner;
                resourceExtractionBehavior.fogOfWarManager.currentCell = movement.currentCell;
                resourceExtractionBehavior.Place();
                resourceExtractionBehavior.fogOfWarManager.ClearFogOfWar();
                movement.Owner.constructionMenu.SetActive(false);
                placeMode = false;
            }
            else if(movement.currentCell.occupyingResource.resourceName != null && movement.Owner.Affordable(movement.Owner.mine))
            {
                GameObject buildingA = Instantiate(movement.Owner.mine, this.transform.position, Quaternion.Euler(new Vector3(0f, 0f, 0f)));
                ResourceExtractionBehavior resourceExtractionBehavior = buildingA.GetComponent<ResourceExtractionBehavior>();
                resourceExtractionBehavior.Owner = combat.Owner;
                resourceExtractionBehavior.Cell = movement.currentCell;
                movement.currentCell.occupyingBuilding = resourceExtractionBehavior.Building;
                resourceExtractionBehavior.fogOfWarManager.gameManager = combat.GameManager;
                resourceExtractionBehavior.fogOfWarManager.cells = combat.GameManager.hexGrid.cells;
                resourceExtractionBehavior.fogOfWarManager.owner = combat.Owner;
                resourceExtractionBehavior.fogOfWarManager.currentCell = movement.currentCell;
                resourceExtractionBehavior.Place();
                resourceExtractionBehavior.fogOfWarManager.ClearFogOfWar();
                movement.Owner.constructionMenu.SetActive(false);
                placeMode = false;
            }
        }
    }
    public void Chop()
    {
        if(movement.currentCell.occupyingBuilding == null && movement.currentCell.occupyingForest != null)
        {
            Destroy(movement.currentCell.occupyingForest.gameObject);
            combat.Owner.ResourcePools[0] += movement.GameManager.chopYield;
            combat.Owner.updateResourceCounters();
        }
    }
    public bool PlaceMode
    {
        get { return placeMode; }
        set { placeMode = value; }
    }
}
