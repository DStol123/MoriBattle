using UnityEngine;

public class OpacityModifier : MonoBehaviour
{
    [SerializeField] private GameObject opaqueForest;
    [SerializeField] private GameObject transluscentForest;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        transluscentForest.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetOpacity(bool opacity)
    {
        if(opacity)
        {
            transluscentForest.SetActive(false);
            opaqueForest.SetActive(true);
        }
        else
        {
            transluscentForest.SetActive(true);
            opaqueForest.SetActive(false);
        }
    }
}
