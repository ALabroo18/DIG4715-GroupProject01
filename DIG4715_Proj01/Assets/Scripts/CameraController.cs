using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Player;
    public float dampTime = 0.4f; // delay effect to follow player
    private Vector3 cameraPos;
    private Vector3 velocity = Vector3.zero;

    
    void Start()
    {
        
    }

    void Update()
    {
        cameraPos = new Vector3(Player.position.x,Player.position.y,-10f);// position from camera to player with x,y no (z=-10f)
        transform.position = Vector3.SmoothDamp(gameObject.transform.position,cameraPos, ref velocity, dampTime);
    }
}
