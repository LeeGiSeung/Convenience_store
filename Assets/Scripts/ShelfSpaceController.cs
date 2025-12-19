using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShelfSpaceController : MonoBehaviour
{
    public StockInfo info;
    //public int amountOnShelf;
    public List<StockObject> objectsOnShelf;

    public void PlaceStock(StockObject objectToPlace)
    {
        bool preventPlacing = true;
        
        Debug.Log(objectToPlace.info.name);

        if(objectsOnShelf.Count == 0)
        {
            info = objectToPlace.info;
            preventPlacing = false;
        }
        else
        {
            if(info.name == objectToPlace.info.name)
            {
                preventPlacing = false;
            }
        }

        if(preventPlacing == false)
        {
            objectToPlace.transform.SetParent(transform);
            objectToPlace.MakePlaced();

            objectsOnShelf.Add(objectToPlace);
        }

    }

    public StockObject GetStock()
    {
        StockObject objectToReturn = null;

        if(objectsOnShelf.Count > 0)
        {
            objectToReturn = objectsOnShelf[objectsOnShelf.Count - 1];
            objectsOnShelf.RemoveAt(objectsOnShelf.Count - 1);
        }

        return objectToReturn;
    }
}
