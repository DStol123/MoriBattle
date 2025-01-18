using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBehavior : MonoBehaviour
{
    private bool isTurn;
    [SerializeField] private GameManager gameManager;
    [SerializeField] public ResourceBehavior[] resourceTypes;
    [SerializeField] private float[] resourcePools;
    [SerializeField] public float[] resourceIncomes;
    [SerializeField] private bool isAlive;
    [SerializeField] private Color color;
    [SerializeField] private string userName;
    [SerializeField] public List<MovementBehavior> allOwnedMovementBehaviors;
    private CombatBehavior selectedArmy;
    private TrainingGroundsBehavior selectedTrainingGrounds;
    public CameraControl playerCamera;
    private GameObject[] selectedObjects;
    [SerializeField] private GameObject[] possibleArmies;
    public GameObject unitUI;
    public GameObject constructButton;
    public GameObject trainingUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        unitUI.SetActive(false);
        trainingUI.SetActive(false);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        
        if(Input.GetKeyDown(KeyCode.Return) && selectedTrainingGrounds != null)
        {
            selectedTrainingGrounds.makeArmy(0);
        }
        
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
    }

    public void TurnOnAttackMode()
    {
        selectedArmy.AttackMode = true;
        selectedArmy.Movement.MoveMode = false;
        if (selectedArmy.GetComponent<EngineerBehavior>() != null)
        {
            selectedArmy.GetComponent<EngineerBehavior>().PlaceMode = false;
        }
    }
    public void TurnOnMoveMode()
    {
        selectedArmy.AttackMode = false;
        selectedArmy.Movement.MoveMode = true;
        if (selectedArmy.GetComponent<EngineerBehavior>() != null)
        {
            selectedArmy.GetComponent<EngineerBehavior>().PlaceMode = false;
        }
    }
    public void TurnOnPlaceMode()
    {
        selectedArmy.AttackMode = false;
        selectedArmy.Movement.MoveMode = false;
        if(selectedArmy.GetComponent<EngineerBehavior>() != null)
        {
            selectedArmy.GetComponent<EngineerBehavior>().PlaceMode = true;
        }
    }
    public void PlaceTrainingGround()
    {
        if(selectedArmy.GetComponent<EngineerBehavior>() != null)
        {
            selectedArmy.GetComponent<EngineerBehavior>().PlaceTrainingGrounds();
        }
    }
    public void TrainArmy(int index)
    {
        selectedTrainingGrounds.makeArmy(index);
    }
    public void Deselect()
    {
        selectedArmy.AttackMode = false;
        selectedArmy.Movement.MoveMode = false;
        selectedArmy.IsSelected = false;
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
    public float[] ResourcePools
    {
        get { return resourcePools; }
        set { resourcePools = value; }
    }
    public float[] ResourceIncomes
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
