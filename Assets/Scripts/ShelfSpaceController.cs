using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ShelfSpaceController : MonoBehaviour
{
    public StockInfo info;
    //public int amountOnShelf;
    public List<StockObject> objectsOnShelf;

    public List<Transform> bigDrinkPoints;
    public List<Transform> cerealPoints;
    public List<Transform> tubeChipsPoints;
    public List<Transform> fruitPoints;
    public List<Transform> bigFruitoints;

    [SerializeField] TMP_Text shelfLabel;

    public void PlaceStock(StockObject objectToPlace)
    {
        bool preventPlacing = true;
        
        //Debug.Log(objectToPlace.info.name);

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

                switch (info.typeOfStock)
                {
                    case StockInfo.StockType.bigDrink:
                        if(objectsOnShelf.Count >= bigDrinkPoints.Count)
                        {
                            preventPlacing = true;
                        }
                        break;
                    case StockInfo.StockType.cereal:
                        if(objectsOnShelf.Count >= cerealPoints.Count)
                        {
                            preventPlacing = true;
                        }
                        break;
                    case StockInfo.StockType.fruit:
                        if(objectsOnShelf.Count >= fruitPoints.Count)
                        {
                            preventPlacing = true;
                        }
                        break;
                    case StockInfo.StockType.fruitLarge:
                        if(objectsOnShelf.Count >= bigDrinkPoints.Count)
                        {
                            preventPlacing = true;
                        }
                        break;
                    case StockInfo.StockType.chipsTube:
                        if(objectsOnShelf.Count >= tubeChipsPoints.Count)
                        {
                            preventPlacing = true;
                        }
                        break;
                }
            }
        }

        if(preventPlacing == false)
        {
            objectToPlace.transform.SetParent(transform);
            objectToPlace.MakePlaced();

            if(info == null)
            {
                return;
            }

            switch (info.typeOfStock)
                {
                    case StockInfo.StockType.bigDrink:
                        objectToPlace.transform.SetParent(bigDrinkPoints[objectsOnShelf.Count]);
                        break;
                    case StockInfo.StockType.cereal:
                        objectToPlace.transform.SetParent(cerealPoints[objectsOnShelf.Count]);
                        break;
                    case StockInfo.StockType.fruit:
                        objectToPlace.transform.SetParent(fruitPoints[objectsOnShelf.Count]);
                        break;
                    case StockInfo.StockType.fruitLarge:
                        objectToPlace.transform.SetParent(bigFruitoints[objectsOnShelf.Count]);
                        break;
                    case StockInfo.StockType.chipsTube:
                        objectToPlace.transform.SetParent(tubeChipsPoints[objectsOnShelf.Count]);
                        break;
                }    

            objectsOnShelf.Add(objectToPlace);

        }

        UpdateDisplayPrice(info.currnetPrice);
        //shelfLabel.text = info.price.ToString();

    }

    public StockObject GetStock()
    {
        StockObject objectToReturn = null;

        if(objectsOnShelf.Count > 0)
        {
            objectToReturn = objectsOnShelf[objectsOnShelf.Count - 1];
            objectsOnShelf.RemoveAt(objectsOnShelf.Count - 1);
        }

        if(objectsOnShelf.Count == 0)
        {
            shelfLabel.text = string.Empty;
        }

        return objectToReturn;
    }

    public void StartPriceUpdate()
    {
        if(objectsOnShelf.Count > 0)
        {
            UIController.instance.OpenUpdatePrice(info);
        }
    }

    public void UpdateDisplayPrice(float price)
    {

        if(objectsOnShelf.Count > 0)
        {
            info.currnetPrice = price;

            shelfLabel.text = price.ToString();
        }

        
    }
}
