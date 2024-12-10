using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBehavior : MonoBehaviour
{
    private bool isTurn;
    private Resource[] resourceTypes;
    private float[] resourcePools;
    private bool isAlive;
    private Color color;
    private string userName;
    private CombatBehavior selectedArmy;
    public CameraControl playerCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    public bool IsAlive
    {
        get { return isAlive; }
        set { isAlive = value; }
    }

}
