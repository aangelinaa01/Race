using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CarEngine : MonoBehaviour
{
    public Transform Path; 
    public float maxSteerAngle = 90f;
    public float maxMotorTorque = 300f;
    public float currentSpeed;
    public float maxSpeed = 3000f;
    public WheelCollider WheelFL;
    public WheelCollider WheelFR;
    public Vector3 centerOfMass;

    [Header ("Sensors")]
    public float sensorLength = 3f;
    public Vector3 frontSensorPosition = new Vector3(0, 0.2f, 0.5f);
    public float frontSideSensorPosition = 0.3f;
    public float frontSensorAngle = 30f;

    private List<Transform> nodes;
    private int currentNode = 0;

   private bool avoiding = false; 


    void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = centerOfMass;
        Transform [] PathTransforms = GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < PathTransforms.Length; i++) {
            if (PathTransforms[i] != Path.transform){
                nodes.Add(PathTransforms[i]);
            }
        }
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        ApplySteer();
        Drive();
        CheckWaypointDistance();
        Sensors();
    }
    private void ApplySteer(){
        if(avoiding) return;
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        WheelFL.steerAngle = newSteer;
        WheelFR.steerAngle = newSteer;
        }
    private void Drive(){
        float currentSpeed = 2 * Mathf.PI * WheelFL.radius * WheelFL.rpm * 60 / 1000;
        if(currentSpeed < maxSpeed) {
            WheelFL.motorTorque = maxMotorTorque;
            WheelFR.motorTorque = maxMotorTorque;
        } else {
            WheelFL.motorTorque = 0;
            WheelFR.motorTorque = 0;
        }
    }
    private void CheckWaypointDistance() {
    float distance = Vector3.Distance(transform.position, nodes[currentNode].position);
    if (distance < 1f) { // Уменьшаем пороговое расстояние для перехода к следующему узлу
        if (currentNode == nodes.Count - 1) {
            currentNode = 0;
        } else {
            currentNode++;
        }
    }
}
    private void Sensors(){
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position;
        sensorStartPos += transform.forward * frontSensorPosition.z;
        sensorStartPos += transform.up * frontSensorPosition.y;
        float avoidMultiplayer = 0; 
        avoiding = false;

        
        //front center sensor
        if(avoidMultiplayer == 0){
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength)){
            if(!hit.collider.CompareTag("Terrain")) {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                if(hit.normal.x < 0){
                    avoidMultiplayer = -1;
                } else {
                    avoidMultiplayer = +1;
                }
            }
        }
        }
        

        //front right sensor
        sensorStartPos += transform.right * frontSideSensorPosition;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength)){
            if(!hit.collider.CompareTag("Terrain")) {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplayer -= 1f;
            }
        }
        
        //front right angle sensor
    
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength)){
            if(!hit.collider.CompareTag("Terrain")) {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplayer -= 0.5f;
            }
        }
        
        //front left sensor
        sensorStartPos -= transform.right* frontSideSensorPosition * 2;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength)){
             if(!hit.collider.CompareTag("Terrain")) {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplayer += 1f;
            } 
        }
       
         //front left angle sensor
    
        else if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength)){
            if(!hit.collider.CompareTag("Terrain")) {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                avoidMultiplayer += 0.5f;
            }
        }
        if (avoiding) {
        WheelFL.steerAngle = maxSteerAngle * avoidMultiplayer;
        WheelFR.steerAngle = maxSteerAngle * avoidMultiplayer;
    }
    }
}
