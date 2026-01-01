using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor.Analytics;

//using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Analytics;

public class Customer : MonoBehaviour
{
    float dis;
    public List<NavPoint> points = new List<NavPoint>();
    public float moveSpeed = 3f;
    private float currentWaitTime = .5f;

    public Animator anim;
    
    public int maxBrowsePoints = 5;
    private int browsePointsRemain = 5;
    public float browseTime;

    private UnityEngine.Vector3 queuePoint;

    public FurnitureController currentShelfCase;
    public enum CustomerState
    {
        entering, browsing, queuing, atCheckout, leaving    
    }
    public CustomerState currentState;

    public GameObject shoppingBag;
    private bool hasGrab = false;
    public float waitAfterGrabbing = .5f;

    public List<StockObject> stockInBag = new List<StockObject>();
    NavMeshAgent navMesh;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //points.Clear();
        //points.AddRange(CustomerManager.instance.GetEntryPoints());

        if(points.Count > 0)
        {
            transform.position = points[0].point.position;

            currentWaitTime = points[0].waitTime;
        }

        navMesh = GetComponent<NavMeshAgent>();
        //points.AddRange(CustomerManager.instance.GetExitPoints());

    }

    // Update is called once per frame
    void Update()
    {

        switch (currentState)
        {
            case CustomerState.entering:
                if(points.Count > 0)
                {
                    MoveToPoint();
                }
                else
                {
                    if(StoreController.instance.shelvingCases.Count > 0)
                    {
                        currentState = CustomerState.browsing;

                        browsePointsRemain = UnityEngine.Random.Range(1, maxBrowsePoints + 1);

                        browsePointsRemain = Math.Clamp(browsePointsRemain, 1, StoreController.instance.shelvingCases.Count);

                        GetBrowsePoint();
                    }
                    else
                    {
                        StartLeaving();
                    }

                } 
                break;
            case CustomerState.browsing:
                MoveToPoint();

                if(points.Count == 0)
                {
                    if(hasGrab == false)
                    {
                        if(!GrabStock()) StartLeaving();
                    }
                    else
                    {
                        browsePointsRemain--;

                        if(browsePointsRemain > 0)
                        {
                            GetBrowsePoint();
                        }
                        else
                        {
                            //StartLeaving();
                            if(stockInBag.Count > 0)
                            {
                                Checkout.instance.AddCustomerToQueue(this);
                            
                                currentState = CustomerState.queuing;
                            }
                            else
                            {
                                StartLeaving();
                            }
                        }
                    }


                }
            break;

            case CustomerState.atCheckout:
            break;

            case CustomerState.queuing:
                //transform.position = UnityEngine.Vector3.MoveTowards(transform.position, queuePoint, moveSpeed * Time.deltaTime);
                UnityEngine.Vector3 targetPosition = queuePoint;

                //NavMesh 위 좌표로 보정
                if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
                {
                    navMesh.speed = moveSpeed;
                    navMesh.SetDestination(hit.position);
                }
                dis = UnityEngine.Vector3.Distance(transform.position, queuePoint);
                if(UnityEngine.Vector3.Distance(transform.position, queuePoint) > 0.3f)
                {
                    Debug.Log("is moving yes");
                    anim.SetBool("isMoving", true);   
                }
                else
                {
                    Debug.Log("is moving false");
                    anim.SetBool("isMoving", false);   
                }

            break;

            case CustomerState.leaving:

                if(points.Count > 0)
                {
                    MoveToPoint();
                }
                else
                {
                    Destroy(gameObject);
                } 
            break;
        }
        
    }

    public void MoveToPoint()
    {   
        
        if (!navMesh.isActiveAndEnabled) return;
        if (!navMesh.isOnNavMesh) return;

        if(points.Count > 0)
        {

            bool isMoving = true;
            
            //UnityEngine.Vector3 targetPosition = new UnityEngine.Vector3(points[0].point.position.x, transform.position.y, points[0].point.position.z);

            //transform.position = UnityEngine.Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            
            UnityEngine.Vector3 targetPosition = points[0].point.position;

            // 🔥 NavMesh 위 좌표로 보정
            if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
            {
                navMesh.speed = moveSpeed;
                navMesh.SetDestination(hit.position);

            }

            //transform.LookAt(targetPosition);

            if(UnityEngine.Vector3.Distance(transform.position, targetPosition) < .25f)
            {
                isMoving = false;

                if(currentWaitTime >= 0)
                {
                    currentWaitTime -= Time.deltaTime;

                    if(currentWaitTime <= 0)
                    {
                        StartNextPoint();
                    }
                }
            }
             anim.SetBool("isMoving", isMoving);
        }
        else
        {
            StartNextPoint();
        }

       
    }

    public void StartNextPoint()
    {
        if(points.Count > 0)
        {
            points.RemoveAt(0);

            if(points.Count > 0)
            {
                currentWaitTime = points[0].waitTime;
            }
        }
    }

    public void StartLeaving()
    {
        currentState = CustomerState.leaving;

        points.Clear();
        points.AddRange(CustomerManager.instance.GetExitPoints());

        currentWaitTime = points[0].waitTime;

        Debug.Log("Leaving");
    }

    void GetBrowsePoint()
    {
        points.Clear();

        int selectedShelf = UnityEngine.Random.Range(0, StoreController.instance.shelvingCases.Count);

        points.Add(new NavPoint());

        points[0].point = StoreController.instance.shelvingCases[selectedShelf].standPoint;
        points[0].waitTime = browseTime * UnityEngine.Random.Range(.75f,1.25f);

        currentWaitTime = points[0].waitTime;

        currentShelfCase = StoreController.instance.shelvingCases[selectedShelf];

    }

    public bool GrabStock()
    {
        bool result = false;

        int shelf = UnityEngine.Random.Range(0, currentShelfCase.shelves.Count);

        StockObject stock = currentShelfCase.shelves[shelf].GetStock();

        if(stock != null)
        {
            Debug.Log("grab stock "+stock.name);
            shoppingBag.SetActive(true);
            hasGrab = true;
            stock.transform.SetParent(shoppingBag.transform);
            stockInBag.Add(stock);
            stock.PlaceInBag();

            points.Clear();
            points.Add(new NavPoint());
            points[0].point = currentShelfCase.standPoint;
            points[0].waitTime = waitAfterGrabbing * UnityEngine.Random.Range(.75f,1.25f);
            currentWaitTime = points[0].waitTime;
            result = true;
            return result;
        }
        else
        {
            Debug.Log("no grab");
            return result;
        }
    }

    public void UpdateQueuePint(UnityEngine.Vector3 newPoint)
    {
        queuePoint = newPoint;
        transform.LookAt(queuePoint);   
    }

    public float GetTotalSpend()
    {
        float total = 0;

        foreach(StockObject stock in stockInBag)
        {
            total += stock.info.currnetPrice;
        }

        return total;
    }

    public void SetPoints(List<NavPoint> points)
    {
        this.points = new List<NavPoint>(points); // 깊은 복사 ⭐

        if (points.Count > 0 && navMesh != null && navMesh.isOnNavMesh)
        {
            navMesh.speed = moveSpeed;
            navMesh.SetDestination(points[0].point.position);
        }
    }
}

[System.Serializable]
public class NavPoint
{
    public Transform point;
    public float waitTime;
}
