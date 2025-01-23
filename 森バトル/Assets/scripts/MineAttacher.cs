using UnityEngine;

public class MineAttacher : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject mineAttacher;
    public Vector3 attachmentPoint;
    void Start()
    {
        attachmentPoint = mineAttacher.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Attach(GameObject gameObject)
    {
        gameObject.transform.position = attachmentPoint;
    }    
}
