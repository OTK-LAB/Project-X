using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    private Transform player;
    private Vector3 startMousePos, startBallPos, startTouchPos, endTouchPos;
    private bool moveTheBall;
    [Range(0f,1f)]public float maxSpeed;
    [Range(0f,1f)]public float canSpeed;
    [Range(0f,60f)]public float pathSpeed;
    private float velocity, canVelocity;
    private Camera mainCam;
    public Transform path;
    private Rigidbody rb;
    private Collider _collider;
    public Slider _slider;
    private float sliderMin;
    private float sliderMax;
    private float sliderSpeed;


    void Start()
    {
        player = transform;
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //rb.isKinematic = _collider.isTrigger = false;
            rb.velocity = new Vector3(0f,7f,0f);
            Debug.Log("JUMPED");
        }

        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startTouchPos = Input.GetTouch(0).position;
        }

        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            endTouchPos = Input.GetTouch(0).position;
        }

        if(endTouchPos.y > startTouchPos.y && rb.velocity.y == 0)
        {
            rb.isKinematic = _collider.isTrigger = false;
            rb.velocity = new Vector3(0f,7f,0f);
            startTouchPos = Vector3.zero;
            endTouchPos = Vector3.zero;
            Debug.Log("JUMPED");
        }
        
        if(Input.GetMouseButtonDown(0) && MenuManager.MenuManagerInstance.GameState)
        {
            moveTheBall = true;

            Plane newPlan = new Plane(Vector3.up, 0f);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (newPlan.Raycast(ray,out var distance))
            {
                startMousePos = ray.GetPoint(distance);
                startBallPos = player.position;
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            moveTheBall = false;
        }

        if(moveTheBall)
        {
            Plane newPlan = new Plane(Vector3.up, 0f);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (newPlan.Raycast(ray,out var distance))
            {
                Vector3 mouseNewPos = ray.GetPoint(distance);
                Vector3 MouseNewPos = mouseNewPos - startMousePos;
                Vector3 DesireBallPos = MouseNewPos + startBallPos;

                DesireBallPos.x = Mathf.Clamp(DesireBallPos.x,-1.4f,1.4f);

                player.position = new Vector3(Mathf.SmoothDamp(player.position.x, DesireBallPos.x, ref velocity, maxSpeed), player.position.y, player.position.z);
            }  
        }

        if(MenuManager.MenuManagerInstance.GameState)
        {
        var pathNewPos = path.position;
        path.position = new Vector3(pathNewPos.x, pathNewPos.y, Mathf.MoveTowards(pathNewPos.z, -1000f, pathSpeed * Time.deltaTime));
        }
    }
    private void LateUpdate()
    {
        var CameraNewPos = mainCam.transform.position;

        mainCam.transform.position = new Vector3(Mathf.SmoothDamp(CameraNewPos.x, player.transform.position.x, ref canVelocity, canSpeed), CameraNewPos.y, CameraNewPos.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Obstacle"))
        {
            gameObject.SetActive(false);
            MenuManager.MenuManagerInstance.GameState = false;
        }
        if(other.CompareTag("Sticks"))
        {
            other.gameObject.SetActive(false);
        }
        if(other.CompareTag("Faster"))
        {
            pathSpeed += 2;
        }
        if(other.CompareTag("Slower"))
        {
            pathSpeed -= 2;
        }
        if(other.CompareTag("EndingLine"))
        {
            pathSpeed = 0;
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if(other.collider.CompareTag("Path"))
        {
            //rb.isKinematic = _collider.isTrigger = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
      
    }
}
