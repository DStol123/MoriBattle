using UnityEngine;

public class Combat
{
    private int maxHp;
    private int currentHp;
    private float maxMovementPoints;
    private float currentMovementPoints;

    public static HexGrid hexGrid;

    public int MaxHp
    {
        get => maxHp; // setting maxHp with get comma
        set => maxHp = Mathf.Max(1, value); // set as not to reach minus
    }

    public int CurrentHp
    {
        get => currentHp;
        set => currentHp = Mathf.Clamp(value, 0, MaxHp); // from 0 to MaxHp
    }

    public float MaxMovementPoints
    {
        get => maxMovementPoints;
        set => maxMovementPoints = Mathf.Max(0, value); // not to reach minus
    }

    public float CurrentMovementPoints
    {
        get => currentMovementPoints;
        set => currentMovementPoints = Mathf.Clamp(value, 0, MaxMovementPoints); // from 0 to MaxMovementPoints
    }

    // Constructor
    public Combat(int hp, float movementPoints)
    {
        MaxHp = hp;
        CurrentHp = hp;
        MaxMovementPoints = movementPoints;
        CurrentMovementPoints = movementPoints;
    }

    // TakingDamage
    public void TakeDamage(int amount)
    {
        CurrentHp -= amount; // Decrease CurrentHp
        if (CurrentHp <= 0)
        {
            Debug.Log("The Character was dead!"); //message
        }
    }

    // Using MovementPoints
    public bool UseMovementPoints(float amount)
    {
        if (CurrentMovementPoints >= amount)
        {
            CurrentMovementPoints -= amount;
            return true;
        }
        else
        {
            Debug.Log("No points to move the Characters");
            return false;
        }
    }

    // Healing
    public void Heal(int amount)
    {
        CurrentHp += amount;
    }
}