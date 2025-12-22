using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StockInfoController : MonoBehaviour
{

    public List<StockInfo> foodInfo, produceInfo;

    public List<StockInfo> allStock = new List<StockInfo>();

    public static StockInfoController instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void Awake()
    {
        instance = this;

        allStock.AddRange(foodInfo);
        allStock.AddRange(produceInfo);

        for(int i = 0; i<allStock.Count; i++)
        {
            if(allStock[i].currnetPrice == 0)
            {
                allStock[i].currnetPrice = allStock[i].price;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public StockInfo GetInfo(string stockName)
    {
        StockInfo infoToReturn = null;

        for(int i = 0; i<allStock.Count; i++)
        {
            if(allStock[i].name == stockName)
            {
                infoToReturn = allStock[i];
            }
        }

        return infoToReturn;
    }

    public void UpdatePrice(string stockName, float newPrice)
    {
        for(int i = 0; i<allStock.Count; i++)
        {
            if(allStock[i].name == stockName)
            {
                allStock[i].currnetPrice = newPrice;
            }
        }

        List<ShelfSpaceController> shelves = new List<ShelfSpaceController>();

        shelves.AddRange(FindObjectsByType<ShelfSpaceController>(FindObjectsSortMode.None)); //순서 상관없이 찾기

        foreach(ShelfSpaceController shelf in shelves)
        {
            if(shelf.info.name == stockName)
            {
                shelf.UpdateDisplayPrice(newPrice);
            }
        }

    }
}
