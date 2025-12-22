using UnityEngine;

public class StockObject : MonoBehaviour
{
    public StockInfo info;
    [SerializeField] float moveSpeed = 5f;
    public bool isPlaced;
    public Rigidbody rb;
    public Collider col;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        info = StockInfoController.instance.GetInfo(info.name);
    }

    void Awake()
    {
        //info = StockInfoController.instance.GetInfo(info.name);
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlaced == true)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, moveSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, moveSpeed * Time.deltaTime);
        }
    }

    public void Pickup()
    {
        rb.isKinematic = true;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        isPlaced = false;

        col.enabled = false;
    }

    public void MakePlaced()
    {
        rb.isKinematic = true;

        isPlaced = true;

        col.enabled = false;
    }

    public void Release()
    {
        rb.isKinematic = false;

        col.enabled = true;
    }

    public void PlaceInBox()
    {
        rb.isKinematic = true;
        col.enabled = false;
    }
}
