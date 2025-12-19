using UnityEngine;

[System.Serializable]

public class StockInfo 
{
    public string name;
    public enum StockType
    {
        cereal, bigDrink, chipsTube, fruit, fruitLarge
    }

    public StockType typeOfStock;
}
