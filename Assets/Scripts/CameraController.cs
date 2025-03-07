using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    private Rigidbody playerRB;
    public Vector3 Offset;
    public float speed;
   
    void Start()
    {
        playerRB = player.GetComponent<Rigidbody>();
    }

   void LateUpdate()
{
    Vector3 playerTargetPosition = player.position + player.TransformVector(Offset) + player.transform.forward * (-5f);
    transform.position = Vector3.Lerp(transform.position, playerTargetPosition, speed * Time.deltaTime);
    transform.LookAt(player);
}
}