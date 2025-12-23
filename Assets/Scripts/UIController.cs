using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    [SerializeField] public GameObject updatePricePanel;
    [SerializeField] public TMP_Text basePriceText, currentPriceText;
    [SerializeField] public TMP_InputField priceInputField;
    
    public GameObject buyMenuScreen;
    public TMP_Text moneyText;
    
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
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            OpenCloseBuyMenu();
        }
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

    public void UpdateMoney(float currentMoney){
        moneyText.text = currentMoney.ToString();
    }

    public void OpenCloseBuyMenu()
    {
        if(buyMenuScreen.activeSelf == false)
        {
            buyMenuScreen.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            buyMenuScreen.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
