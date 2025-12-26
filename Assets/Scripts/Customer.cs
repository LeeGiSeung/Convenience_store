using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class Customer : MonoBehaviour
{

    public List<NavPoint> points = new List<NavPoint>();
    public float moveSpeed = 3f;
    private float currentWaitTime = .5f;

    public Animator anim;
    
    public enum CustomerState
    {
        entering, browsing, queuing, atCheckout, leaving    
    }
    public CustomerState currentState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        points.Clear();
        points.AddRange(CustomerManager.instance.GetEntryPoints());

        if(points.Count > 0)
        {
            transform.position = points[0].point.position;

            currentWaitTime = points[0].waitTime;
        }
        
        //points.AddRange(CustomerManager.instance.GetExitPoints());

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(points.Count > 0)
        {
            MoveToPoint();
        }
        */
        switch (currentState)
        {
            case CustomerState.entering:
                if(points.Count > 0)
                {
                    MoveToPoint();
                }
                else
                {
                    StartLeaving();
                } 
                break;
            case CustomerState.browsing:
            
            break;

            case CustomerState.atCheckout:
            break;

            case CustomerState.queuing:
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

        bool isMoving = true;
        
        UnityEngine.Vector3 targetPosition = new UnityEngine.Vector3(points[0].point.position.x, transform.position.y, points[0].point.position.z);

        transform.position = UnityEngine.Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        transform.LookAt(targetPosition);
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
    }
}

[System.Serializable]
public class NavPoint
{
    public Transform point;
    public float waitTime;
}
