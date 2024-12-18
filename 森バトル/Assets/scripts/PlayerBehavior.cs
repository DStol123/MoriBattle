using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBehavior : MonoBehaviour
{
    private bool isTurn;
    [SerializeField] private Resource[] resourceTypes;
    [SerializeField] private float[] resourcePools;
    [SerializeField] private float[] resourceIncomes;
    [SerializeField] private bool isAlive;
    [SerializeField] private Color color;
    [SerializeField] private string userName;
    private CombatBehavior selectedArmy;
    private TrainingGroundsBehavior selectedTrainingGrounds;
    public CameraControl playerCamera;
    private GameObject[] selectedObjects;
    [SerializeField] private GameObject[] possibleArmies;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {

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
                    }
                    if (hit.collider.GetComponent<TrainingGroundsBehavior>() != null)
                    {
                        Select(hit.collider.GameObject().GetComponent<TrainingGroundsBehavior>());
                    }
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
                    if (hit.collider.GetComponent<CombatBehavior>() != null
                        && hit.collider.GetComponent<CombatBehavior>()
                        .currentCell.CheckIfNeighbor(selectedArmy.currentCell))
                    {
                        CombatBehavior target = hit.collider.GetComponent<CombatBehavior>();
                        selectedArmy.Attack(selectedArmy.BaseDamage, target);
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.A) && selectedArmy != null)
            {
                selectedArmy.AttackMode = true;
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
    public Resource[] ResourceTypes
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
