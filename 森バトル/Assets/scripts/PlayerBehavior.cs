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
    public CameraControl playerCamera;
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
    }

    public void SelectArmy(CombatBehavior army)
    {
        if(army.Owner == this)
        {
            if(selectedArmy != null)
            {
                selectedArmy.IsSelected = false;
            }
            selectedArmy = army;
            selectedArmy.IsSelected = true;
        }
    }

    public void HandleInput()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("Ray cast");
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(inputRay, out hit))
            {
                Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
                Debug.Log("CombatBehavior Found: " + hit.collider.GameObject().GetComponent<CombatBehavior>() != null);
                if (hit.collider.GetComponent<CombatBehavior>() != null)
                {
                    SelectArmy(hit.collider.GameObject().GetComponent<CombatBehavior>());
                }
            }
        }
    }

    public void AddResources()
    {
        for(int i = 0; i < resourcePools.Length; i++)
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

}
