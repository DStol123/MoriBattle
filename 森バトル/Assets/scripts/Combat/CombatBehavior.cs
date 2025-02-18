using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CombatBehavior : MonoBehaviour
{
    public Image healthBar;
    [SerializeField] private bool leader;

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

    [SerializeField] private float baseAttackDamage;
    [SerializeField] private float baseDefenceDamage;

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

    public float CalculateAttackDamage(CombatBehavior target, float baseDamage)
    {
        if(target.movement.currentCell.occupyingForest != null)
        {
            baseDamage /= target.forestDefenceBoost * forestAttackPenalty;
        }
        float elevationDifference = target.movement.currentCell.Elevation - movement.currentCell.Elevation;
        if(elevationDifference > 0)
        {
            baseDamage /= (1 + elevationDifference) / (target.heightDefenceBoost * heightAttackPenalty);
        }
        return baseDamage;
    }
    public float CalculateDefenceDamage(CombatBehavior target, float baseDamage)
    {
        if(movement.currentCell.occupyingForest != null)
        {
            baseDamage *= forestDefenceBoost * target.forestAttackPenalty;
        }
        float elevationDifference = movement.currentCell.Elevation - target.movement.currentCell.Elevation;
        if(elevationDifference > 0)
        {
            baseDamage *= (1 + elevationDifference) * heightDefenceBoost * target.heightAttackPenalty;
        }
        return baseDamage;
    }
    public void TakeDamage(float amount)
    {
        CurrentHp -= amount; // Decrease CurrentHp
        if(healthBar != null)
        {
            healthBar.fillAmount = CurrentHp / maxHp;
        }
        if (CurrentHp <= 0)
        {
            Destroy(thisGameObject);
        }
    }
    private void OnDestroy()
    {
        if(leader)
        {
            owner.IsAlive = false;
            gameManager.AlivePlayers--;
            if(gameManager.AlivePlayers == 1)
            {
                gameManager.EndGame();
            }
        }
    }
    public void Attack(float damage, CombatBehavior unit)
    {
        HexCell targetCell = unit.movement.currentCell;
        if (movement.CurrentMovementPoints >= movement.CalculatePointCost(targetCell))
        {
            damage = Random.Range(damage - damage * 0.1f, damage + damage * 0.1f);
            float returnDamage = unit.BaseDefenceDamage;
            returnDamage = Random.Range(returnDamage - returnDamage * 0.1f, returnDamage + returnDamage * 0.1f);
            unit.TakeDamage(CalculateAttackDamage(unit, damage));
            TakeDamage(CalculateDefenceDamage(this, unit.baseDefenceDamage));
            if (unit.currentHp <= 0f && !targetCell.impassable)
            {
                movement.MoveToCell(targetCell);
            }
            movement.UseMovementPoints(movement.CurrentMovementPoints);
        }
    }
    public void Attack(ResourceExtractionBehavior target)
    {
        if(target.Owner != owner)
        {
            Destroy(target.gameObject);
        }
    }
    public void Attack(TrainingGroundsBehavior target)
    {
        if (target.Owner != owner)
        {
            Destroy(target.gameObject);
        }
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
    public float BaseAttackDamage
    {
        get { return baseAttackDamage; }
        set { baseAttackDamage = value; }
    }
    public float BaseDefenceDamage
    {
        get { return baseDefenceDamage; }
        set { baseDefenceDamage = value; }
    }
    public MovementBehavior Movement
    {
        get { return movement; }
        set { movement = value; }
    }
    public GameManager GameManager
    {
        get { return gameManager; }
        set { gameManager = value; }
    }
    public bool Leader
    {
        get { return leader; }
    }
}

