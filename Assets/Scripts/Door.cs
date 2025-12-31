using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{

    public static Door instance;

    public const string Player = "Player";

    public bool canMoveShopScene = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && Door.instance.canMoveShopScene)
        {
            LoadShopScene();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.name == Player) //들어온게 Player면 변경 제공
        {
            canMoveShopScene = true;
            // e 키를 누르면 다음 스테이지 이동 가능
            UIController.instance.ePressText.gameObject.SetActive(true);
            Debug.Log("Enter");
    
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.name == Player) //들어온게 Player면 변경 제공
        {
            canMoveShopScene = false;
            // e 키를 누르면 다음 스테이지 이동 가능
            UIController.instance.ePressText.gameObject.SetActive(false);
            Debug.Log("Exit");
        }
    }

    public void LoadShopScene()
    {
        SceneManager.LoadScene(1);
    }
}
