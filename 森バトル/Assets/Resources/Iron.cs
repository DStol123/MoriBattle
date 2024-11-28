using UnityEngine;

public class Iron : MonoBehaviour
{
    public Player owner;
    public Resource resourceType;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resourceType = new Resource("鉄", "強いやつ", owner, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
