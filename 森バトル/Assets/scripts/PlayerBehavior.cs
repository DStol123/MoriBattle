using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PlayerBehavior : MonoBehaviour
{
    private bool isTurn;
    [SerializeField] private GameManager gameManager;
    [SerializeField] public ResourceBehavior[] resourceTypes;
    [SerializeField] private int[] resourcePools;
    [SerializeField] public int[] resourceIncomes;
    public TextMeshProUGUI resourceCounters;
    [SerializeField] private bool isAlive;
    [SerializeField] private Color color;
    [SerializeField] private string userName;
    int[] previousResourcePools;

    [SerializeField] public List<MovementBehavior> allOwnedMovementBehaviors;
    [SerializeField] public List<FogOfWarManager> allFogOfWarManagers;
    private CombatBehavior selectedArmy;
    private TrainingGroundsBehavior selectedTrainingGrounds;
    public CameraControl playerCamera;
    private GameObject[] selectedObjects;
    [SerializeField] private GameObject[] possibleArmies;
    public GameObject unitUI;
    public GameObject constructButton;
    public GameObject trainingUI;
    public GameObject constructionMenu;

    public GameObject trainingGround;
    public GameObject mine;
    public GameObject lumberMill;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        constructionMenu.SetActive(false);
        unitUI.SetActive(false);
        trainingUI.SetActive(false);
    }
    void Start()
    {
        updateResourceCounters();
    }
    // Update is called once per frame
    void Update()
    {  
        if(resourcePools != previousResourcePools)
        {
            Debug.Log($"Resource Pools updated: Wood: {resourcePools[0]} Stone: {resourcePools[1]} Copper: {resourcePools[2]} Iron: {resourcePools[3]}");
        }
        HandleInput();
        if(Input.GetKeyDown(KeyCode.Return) && selectedTrainingGrounds != null)
        {
            selectedTrainingGrounds.makeArmy(0);
        }
        previousResourcePools = resourcePools;
    }
    public void updateResourceCounters()
    {
        resourceCounters.text = "木材: " + resourcePools[0] + " +" + resourceIncomes[0] +
        "  石: " + resourcePools[1] + " +" + resourceIncomes[1] +
            "  銅: " + resourcePools[2] + " +" + resourceIncomes[2] +
            "  鉄: " + resourcePools[3] + " +" + resourceIncomes[3];
    }
    public void Select(CombatBehavior army)
    {
        //軍をセレクトする
        //プレーヤークラスにあるのは、他のセレクトされてるものをディセレクトするため。
        if (army.Owner == this)
        {
            if (selectedArmy != null)
            {
                selectedArmy.IsSelected = false;
            }
            if (selectedTrainingGrounds != null)
            {
                selectedTrainingGrounds.Building.IsSelected = false;
                selectedTrainingGrounds = null;
            }
            selectedArmy = army;
            selectedArmy.IsSelected = true;
        }
    }
    public void Select(TrainingGroundsBehavior building)
    {
        //訓練場をセレクトする
        if (building.Building.Owner == this)
        {
            if (selectedTrainingGrounds != null)
            {
                selectedTrainingGrounds.Building.IsSelected = false;
            }
            if (selectedArmy != null)
            {
                selectedArmy.IsSelected = false;
                selectedArmy = null;
            }
            selectedTrainingGrounds = building;
            selectedTrainingGrounds.Building.IsSelected = true;
        }
        Debug.Log("Selected Training Grounds");
    }
    public void HandleInput()
    {
        if(!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Ray cast");
                Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(inputRay, out hit))
                {
                    Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
                    Debug.Log("CombatBehavior Found: " + hit.collider.GameObject().GetComponent<CombatBehavior>() != null);
                    Debug.Log("TrainingGroundsBehavior Found: " + hit.collider.GameObject().GetComponent<CombatBehavior>() != null);
                    if (hit.collider.GetComponent<CombatBehavior>() != null)
                    {
                        Select(hit.collider.GameObject().GetComponent<CombatBehavior>());
                        unitUI.SetActive(true);
                        if(hit.collider.GetComponent<EngineerBehavior>() != null)
                        {
                            constructButton.SetActive(true);
                        }
                        else
                        {
                            constructButton.SetActive(false);
                        }
                    }
                    else { unitUI.SetActive(false); }
                    if (hit.collider.GetComponent<TrainingGroundsBehavior>() != null)
                    {
                        Select(hit.collider.GameObject().GetComponent<TrainingGroundsBehavior>());
                        trainingUI.SetActive(true);
                    }
                    else { trainingUI.SetActive(false); }
                }
                else
                {
                    Deselect();
                }
            }
            if (Input.GetMouseButtonDown(1) && selectedArmy != null && selectedArmy.AttackMode)
            {
                Debug.Log("Ray cast");
                Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(inputRay, out hit))
                {
                    Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
                    Debug.Log("CombatBehavior Found: " + hit.collider.GameObject().GetComponent<CombatBehavior>() != null);
                    Debug.Log("TrainingGroundsBehavior Found: " + hit.collider.GameObject().GetComponent<CombatBehavior>() != null);
                    if (hit.collider.GetComponent<MovementBehavior>() != null
                        && hit.collider.GetComponent<MovementBehavior>()
                        .currentCell.CheckIfNeighbor(selectedArmy.Movement.currentCell))
                    {
                        CombatBehavior target = hit.collider.GetComponent<CombatBehavior>();
                        selectedArmy.Attack(selectedArmy.BaseAttackDamage, target);
                        selectedArmy.AttackMode = false;
                    }
                }
            }
        }
    }
    public void AddResources()
    {
        for (int i = 0; i < resourcePools.Length; i++)
        {
            resourcePools[i] += resourceIncomes[i];
        }
        updateResourceCounters();
    }
    public void TurnOnAttackMode()
    {
        if(isTurn)
        {
            selectedArmy.AttackMode = true;
            selectedArmy.Movement.MoveMode = false;
        }
        if (selectedArmy.GetComponent<EngineerBehavior>() != null)
        {
            selectedArmy.GetComponent<EngineerBehavior>().PlaceMode = false;
        }
    }
    public void TurnOnMoveMode()
    {
        if(isTurn)
        {
            selectedArmy.AttackMode = false;
            selectedArmy.Movement.MoveMode = true;
        }
        if (selectedArmy.GetComponent<EngineerBehavior>() != null)
        {
            selectedArmy.GetComponent<EngineerBehavior>().PlaceMode = false;
        }
    }
    public void TogglePlaceMode()
    {
        if(isTurn)
        {
            selectedArmy.AttackMode = false;
            selectedArmy.Movement.MoveMode = false;
            if (selectedArmy.GetComponent<EngineerBehavior>() != null)
            {
                EngineerBehavior engineer = selectedArmy.GetComponent<EngineerBehavior>();
                if(engineer.PlaceMode)
                {
                    engineer.PlaceMode = false;
                    constructionMenu.SetActive(false);
                }
                else
                {
                    engineer.PlaceMode = true;
                    constructionMenu.SetActive(true);
                }
            }
            
        }
    }
    public void PlaceTrainingGround()
    {
        TrainingGroundsBehavior behavior = trainingGround.GetComponent<TrainingGroundsBehavior>();
        if(isTurn && selectedArmy.GetComponent<EngineerBehavior>() != null)
        {
            if(Affordable(trainingGround))
            {
                selectedArmy.GetComponent<EngineerBehavior>().PlaceTrainingGrounds();
            }
            selectedArmy.GetComponent<EngineerBehavior>().PlaceMode = false;
        }
        constructionMenu.SetActive(false);
    }
    public void PlaceResourceExtraction()
    {
        if (isTurn && selectedArmy.GetComponent<EngineerBehavior>() != null)
        {
            selectedArmy.GetComponent<EngineerBehavior>().PlaceResourceExtractor();
            selectedArmy.GetComponent<EngineerBehavior>().PlaceMode = false;
        }
        constructionMenu.SetActive(false);
    }
    public void Chop()
    {
        if(isTurn && selectedArmy.GetComponent<EngineerBehavior>() != null)
        {
            selectedArmy.GetComponent<EngineerBehavior>().Chop();
        }
        constructionMenu.SetActive(false);
    }
    public void TrainArmy(int index)
    {
        if(isTurn && Affordable(possibleArmies[index]))
        {
            selectedTrainingGrounds.makeArmy(index);
        }
    }
    public void Deselect()
    {
        selectedArmy.AttackMode = false;
        selectedArmy.Movement.MoveMode = false;
        selectedArmy.IsSelected = false;
        constructionMenu.SetActive(false);
        unitUI.SetActive(false);
    }
    public void RefreshArmies()
    {
        //ターンが終わるとこのメソッドが発動する。このプレーヤーの軍の全部はMPをＭａｘに戻す。
        if (allOwnedMovementBehaviors[0] != null)
        {
            for (int i = 0; i < allOwnedMovementBehaviors.Count; i++)
            {
                MovementBehavior currentArmy = allOwnedMovementBehaviors[i];
                currentArmy.UseMovementPoints(-1f * (currentArmy.MaxMovementPoints - currentArmy.CurrentMovementPoints));
            }
        }
    }
    public bool Affordable(GameObject item)
    {
        if(item.GetComponent<TrainingGroundsBehavior>() != null)
        {
            TrainingGroundsBehavior trainingGround = item.GetComponent<TrainingGroundsBehavior>();
            if(trainingGround.WoodCost <= resourcePools[0] &&
                trainingGround.StoneCost <= resourcePools[1] &&
                trainingGround.CopperCost <= resourcePools[2] &&
                trainingGround.IronCost <= resourcePools[3])
            {
                return true;
            }
            else { return false; }
        }
        else if(item.GetComponent<ResourceExtractionBehavior>() != null)
        {
            ResourceExtractionBehavior resourceExtractor = item.GetComponent<ResourceExtractionBehavior>();
            if(resourceExtractor.WoodCost <= resourcePools[0] &&
                resourceExtractor.StoneCost <= resourcePools[1] &&
                resourceExtractor.CopperCost <= resourcePools[2] &&
                resourceExtractor.IronCost <= resourcePools[3])
            {
                return true;
            }
            else { return false; }
        }
        else if(item.GetComponent<MovementBehavior>() != null)
        {
            MovementBehavior army = item.GetComponent <MovementBehavior>();
            if(army.WoodCost <= resourcePools[0] &&
                army.StoneCost <= resourcePools[1] &&
                army.CopperCost <= resourcePools[2] &&
                army.IronCost <= resourcePools[3])
            {
                return true;
            }
            else { return false; }
        }
        else { return false; }
    }
    public void ClearFogOfWar()
    {
        for(int i = 0; i < gameManager.hexGrid.cells.Length; i++)
        {
            gameManager.hexGrid.cells[i].fog.SetActive(true);
        }
        for(int i = 0; i < allFogOfWarManagers.Count; i++)
        {
            allFogOfWarManagers[i].ClearFogOfWar();
        }
    }
    public void RevealMap()
    {
        for(int i = 0; i < gameManager.hexGrid.cells.Length; i++)
        {
            gameManager.hexGrid.cells[i].fog.SetActive(false);
        }
    }
    public Color Color
    {
        get { return color; }
        set { color = value; }
    }
    public string Username
    {
        get { return userName; }
        set { userName = value; }
    }
    public bool IsTurn
    {
        get { return isTurn; }
        set { isTurn = value; }
    }
    public ResourceBehavior[] ResourceTypes
    {
        get { return resourceTypes; }
        set { resourceTypes = value; }
    }
    public int[] ResourcePools
    {
        get { return resourcePools; }
        set { resourcePools = value; }
    }
    public int[] ResourceIncomes
    {
        get { return resourceIncomes; }
        set { resourceIncomes = value; }
    }

    public bool IsAlive
    {
        get { return isAlive; }
        set { isAlive = value; }
    }
    public TrainingGroundsBehavior SelectedTrainingGrounds
    {
        get { return selectedTrainingGrounds; }
        set { selectedTrainingGrounds = value; }
    }
    public GameObject[] PossibleArmies
    {
        get { return possibleArmies; }
        set { possibleArmies = value; }
    }
}
