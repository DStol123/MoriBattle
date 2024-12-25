using UnityEngine;
using UnityEngine.EventSystems;

public class CombatBehavior : MonoBehaviour
{
    [SerializeField] private GameObject thisGameObject;
    [SerializeField] private MovementBehavior movement;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerBehavior owner;

    [SerializeField] private bool isAlive;

    [SerializeField] private bool isSelected; //セレクトされてる
    [SerializeField] private bool attackMode;

    [SerializeField] private bool isRanged;
    [SerializeField] private int range;

    [SerializeField] private float startingHp;
    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;

    [SerializeField] private float baseDamage;

    [SerializeField] private float forestAttackPenalty;
    [SerializeField] private float heightAttackPenalty;
    [SerializeField] private float forestDefenceBoost;
    [SerializeField] private float heightDefenceBoost;

    [SerializeField] private float morale;
    [SerializeField] private float maxMorale;

    [SerializeField] private float discipline;
    [SerializeField] private float maxDiscipline;

    [SerializeField] private float armor;
    [SerializeField] private float maxArmor;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void Attack(CombatBehavior target, int damage)
    {
        if(target.Owner != this.Owner)
        {
            target.TakeDamage(damage);
        }
    }
    public float CalculateDamage(CombatBehavior target, float baseDamage)
    {
        if(target.movement.currentCell.occupyingForest != null)
        {
            baseDamage /= target.forestDefenceBoost * forestAttackPenalty;
        }
        baseDamage *= (1 + (target.movement.currentCell.Elevation - movement.currentCell.Elevation))
            / (target.heightDefenceBoost * heightAttackPenalty);
        return baseDamage;
    }

    public void TakeDamage(float amount)
    {
        CurrentHp -= amount; // Decrease CurrentHp
        if (CurrentHp <= 0)
        {
            Destroy(thisGameObject);
        }
    }
    public void Heal(float amount)
    {
        CurrentHp += amount;
    }

    public void Attack(float damage, CombatBehavior unit)
    {
        unit.TakeDamage(CalculateDamage(unit, damage));
    }

    public void FocusOnMe(PlayerBehavior player)
    {
        player.playerCamera.Target.transform.position = this.transform.position;
    }
    
    public PlayerBehavior Owner
    {
        get { return owner; }
        set { owner = value; }
    }
    public float MaxHp
    {
        get => maxHp; // setting maxHp with get comma
        set => maxHp = Mathf.Max(1, value); // set as not to reach zero
    }
    public float CurrentHp
    {
        get => currentHp;
        set => currentHp = Mathf.Clamp(value, 0, MaxHp); // from 0 to MaxHp
    }
    public bool IsAlive
    {
        get { return isAlive; }
    }
    public bool IsSelected
    {
        get { return isSelected; }
        set
        {
            isSelected = value;
            if (value == false)
            {
                attackMode = false;
            }
        }
    }
    public bool AttackMode
    {
        get { return attackMode; }
        set { attackMode = value; }
    }
    public float BaseDamage
    {
        get { return baseDamage; }
        set { baseDamage = value; }
    }
    public MovementBehavior Movement
    {
        get { return movement; }
        set { movement = value; }
    }
}

