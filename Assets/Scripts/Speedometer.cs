﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Speedometer : MonoBehaviour
{
    public Rigidbody target;

    public TextMeshProUGUI speedTextMeshPro; 

    public float maxSpeed = 0.0f;

    public float minSpeedArrowAngle;
    public float maxSpeedArrowAngle;

    [Header("UI")]
    public Text speedLabel;
    public RectTransform arrow;

    private float speed = 0.0f;

    private void Update()
    {
        speed = target.velocity.magnitude * 3.6f;

        if (speedTextMeshPro != null)
            speedTextMeshPro.text = ((int)speed) + " km/h";
        if (arrow != null)
            arrow.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(minSpeedArrowAngle, maxSpeedArrowAngle, speed / maxSpeed));
    }
}