using UnityEngine;

public class ResourceExtractionBehavior : MonoBehaviour
{
    [SerializeField] PlayerBehavior owner;
    [SerializeField] ResourceBehavior resource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AddResourceToIncome();
        owner.updateResourceCounters();
    }

    // Update is called once per frame
    void OnDestroy()
    {
        RemoveResourceFromIncome();
    }
    public void AddResourceToIncome()
    {
        for(int i = 0; i<owner.resourceTypes.Length; i++)
        {
            if (owner.resourceTypes[i] == resource)
            {
                owner.resourceIncomes[i] += resource.Volume;
                break;
            }
        }
    }
    public void RemoveResourceFromIncome()
    {
        for (int i = 0; i < owner.resourceTypes.Length; i++)
        {
            if (owner.resourceTypes[i] == resource)
            {
                owner.resourceIncomes[i] -= resource.Volume;
                break;
            }
        }
    }
}
