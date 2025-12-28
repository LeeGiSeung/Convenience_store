using TMPro;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UIElements;

public class BuyStockFrameController : MonoBehaviour
{
    public StockInfo info;
    public TMP_Text nameText, pricText, amountInBoxText, boxPriceText, buttonText;
    public StockBoxController boxToSpawn;

    public float boxCost;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateFrameInfo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateFrameInfo()
    {
        info = StockInfoController.instance.GetInfo(info.name);

        nameText.text = info.name;
        pricText.text = info.price.ToString();

        int boxAmount = boxToSpawn.GetStockAmount(info.typeOfStock);
        amountInBoxText.text = boxAmount.ToString() + " per box";

        boxCost = boxAmount * info.price;
        boxPriceText.text = "Box Price : "+boxCost.ToString();

        buttonText.text = "PAY : "+ boxCost.ToString();

    }

    public void BuyBox()
    {
        if(StoreController.instance.CheckMoneyAvailable(boxCost) == true)
        {
            StoreController.instance.SpendMoney(boxCost);

            Instantiate(boxToSpawn, StoreController.instance.stockSpawnPoint.position, Quaternion.identity).SetupBox(info);
        }
    }
}
