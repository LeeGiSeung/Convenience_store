using System.ComponentModel;
using UnityEditor.PackageManager;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] InputActionReference moveAction;
    [SerializeField] float moveSpeed = 10;
    [SerializeField] CharacterController characterController;
    [SerializeField] float ySpeed = 0;

    [SerializeField] InputActionReference jumpAction;
    [SerializeField] InputActionReference lookAcction;
    [SerializeField] float jumpForce;

    private float horiRot, vertRot;
    [SerializeField] Camera theCam;
    [SerializeField] float lookSpeed;
    
    [SerializeField] LayerMask whatIsStock;
    [SerializeField] LayerMask whatIsShelf;
    [SerializeField] float interactionRange;

    [SerializeField] Transform holdPoint;

    private StockObject heldPickup;
    [SerializeField] float throwForce;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 lookinput = lookAcction.action.ReadValue<Vector2>();
        horiRot += lookinput.x * Time.deltaTime * lookSpeed;
        horiRot %= 360f;
        transform.rotation = Quaternion.Euler(0f,horiRot,0f);

        vertRot -= lookinput.y * Time.deltaTime * lookSpeed;
        vertRot = Mathf.Clamp(vertRot, -80f, 80f);
        theCam.transform.localRotation = Quaternion.Euler(vertRot,0f,0f);


        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();

        Vector3 vertMove = transform.forward * moveInput.y; //y값이 아니라 앞뒤임
        Vector3 horiMove = transform.right * moveInput.x;

        Vector3 moveAmount = vertMove + horiMove;
        moveAmount = moveAmount.normalized;
        moveAmount *= moveSpeed;

        ySpeed = ySpeed + (Physics.gravity.y * Time.deltaTime);

        if (characterController.isGrounded)
        {
            ySpeed = -2f;

            if (jumpAction.action.WasPressedThisFrame())
            {
                ySpeed = jumpForce;
            }
        }

        moveAmount.y = ySpeed;

        characterController.Move(moveAmount * Time.deltaTime);

        //check for pickup
        Ray ray = theCam.ViewportPointToRay(new Vector3(0.5f,0.5f,0f));
        
        RaycastHit hit;
        // if(Physics.Raycast(ray, out hit, interactionRange, whatIsStock))
        // {
        //     Debug.Log("I see a Pickup");
        // }
        // else
        // {
        //     Debug.Log("I no Pickup");
        // }
        if(heldPickup == null)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if(Physics.Raycast(ray, out hit, interactionRange, whatIsStock))
                {
                    Debug.Log("I see a Pickup");

                    // heldPickup = hit.collider.gameObject;

                    heldPickup = hit.collider.GetComponent<StockObject>();
                    heldPickup.transform.SetParent(holdPoint);
                    heldPickup.Pickup();
                }
            }

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                if(Physics.Raycast(ray, out hit, interactionRange, whatIsShelf))
                {
                    heldPickup = hit.collider.GetComponent<ShelfSpaceController>().GetStock();

                    if(heldPickup != null)
                    {
                        heldPickup.transform.SetParent(holdPoint);
                        heldPickup.Pickup();
                    }
                }
            }
        }
        else
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if(Physics.Raycast(ray, out hit, interactionRange, whatIsShelf))
                {
                    // heldPickup.transform.position = hit.transform.position;
                    // heldPickup.transform.rotation = hit.transform.rotation;

                    // heldPickup.transform.SetParent(null);
                    // heldPickup = null;

                    // heldPickup.MakePlaced();
                    // heldPickup.transform.SetParent(hit.transform);
                    // heldPickup = null;

                    hit.collider.GetComponent<ShelfSpaceController>().PlaceStock(heldPickup);
                    if(heldPickup.isPlaced == true)
                    {
                        heldPickup = null;
                    }
                }
            }

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                heldPickup.Release();
                heldPickup.rb.AddForce(theCam.transform.forward * throwForce, ForceMode.Impulse);

                heldPickup.transform.SetParent(null);
                heldPickup = null;
            }
        }

    }
}
