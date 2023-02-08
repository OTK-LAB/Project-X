using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Transform player;
    private Vector3 startMousePos, startBallPos;
    private bool moveTheBall;
    [Range(0f,1f)]public float maxSpeed;
    [Range(0f,1f)]public float canSpeed;
    [Range(0f,60f)]public float pathSpeed;
    private float velocity, canVelocity;
    private Camera mainCam;
    public Transform path;
    void Start()
    {
        player = transform;
        mainCam = Camera.main;
    }

    
    void Update()
    {
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
    }
    
}
