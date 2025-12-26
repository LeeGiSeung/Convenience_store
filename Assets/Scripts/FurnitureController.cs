using System.Collections.Generic;
using UnityEngine;

public class FurnitureController : MonoBehaviour
{

    public GameObject mainObject, placingObject;
    public Collider col;

    public float price;

    public Transform standPoint;
    public List<ShelfSpaceController> shelves;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(shelves.Count > 0)
        {
            StoreController.instance.shelvingCases.Add(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakePlaceable()
    {
        mainObject.SetActive(false);
        placingObject.SetActive(true);
        col.enabled = false;
    }

    public void PlaceFurniturare()
    {
        mainObject.SetActive(true);
        placingObject.SetActive(false);
        col.enabled = true;
    }
}
