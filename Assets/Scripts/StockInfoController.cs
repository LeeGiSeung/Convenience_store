using System.Collections.Generic;
using UnityEngine;

public class StockInfoController : MonoBehaviour
{

    public List<StockInfo> foodInfo, produceInfo;

    public List<StockInfo> allStock = new List<StockInfo>();

    public static StockInfoController instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;

        allStock.AddRange(foodInfo);
        allStock.AddRange(produceInfo);
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
}
