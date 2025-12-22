using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    [SerializeField] public GameObject updatePricePanel;
    [SerializeField] public TMP_Text basePriceText, currentPriceText;
    [SerializeField] public TMP_InputField priceInputField;

    private StockInfo activeStockInfo;
    private void Awake()
    {
        instance = this;
    }

    

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OpenUpdatePrice(StockInfo stockToUpdate)
    {
        updatePricePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;

        basePriceText.text = stockToUpdate.price.ToString();
        currentPriceText.text = stockToUpdate.currnetPrice.ToString();

        activeStockInfo = stockToUpdate;

        priceInputField.text = stockToUpdate.currnetPrice.ToString();
    }

    public void CloseUpdatePrice()
    {
        updatePricePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ApplyPriceUpdate()
    {
        
            activeStockInfo.currnetPrice = float.Parse(priceInputField.text); //string -> float

            currentPriceText.text = activeStockInfo.currnetPrice.ToString();

            StockInfoController.instance.UpdatePrice(activeStockInfo.name, activeStockInfo.currnetPrice);

            CloseUpdatePrice();
        

    }
}
