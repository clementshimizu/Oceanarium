﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public bool rotating, rotateAtStart;
    public float waitTime = 5f;

    [Header("Orbit Speed")]
    public bool randomSpeed;
    public float minSpeed, maxSpeed;
    public float rotateSpeed;

    [Header("Axis to Orbit")]
    public Axes xyOrzAxis;
    public bool randomAxis;
    [Tooltip("True for Local, false for World")]
    public bool localOrWorld;
    Vector3 chosenAxis;

    public enum Axes
    {
        X, Y, Z,
    }

    //set chosen axis at start
    void Start()
    {
        //randomize speed
        if (randomSpeed)
        {
            RandomSpeed(minSpeed, maxSpeed);
        }

        //random axis
        if (randomAxis)
        {
            RandomizeOrbitAxis();
        }
        //set axis to chosen enum from inspector 
        else
        {
            SetAxis(xyOrzAxis);
        }

        //orbit on start
        if (rotateAtStart)
        {
            if (waitTime > 0)
            {
                StartCoroutine(WaitToOrbit());
            }
            else
            {
                SetOrbit(rotateSpeed);
            }
        }
    }

    IEnumerator WaitToOrbit()
    {
        yield return new WaitForSeconds(waitTime);
        rotating = true;
    }

    void Update()
    {
        if (rotating)
        {
            transform.Rotate(chosenAxis, rotateSpeed * Time.deltaTime);
        }
    }

    //random speed 
    public void RandomSpeed(float min, float max)
    {
        rotateSpeed = Random.Range(min, max);
    }

    //called to start orbit
    public void SetOrbit(float speed)
    {
        rotateSpeed = speed;
        rotating = true;
    }

    //randomize Axis
    public void RandomizeOrbitAxis()
    {
        float randomAxes = Random.Range(0, 100);
        if (randomAxes < 33)
        {
            SetAxis(Axes.X);
        }
        else if (randomAxes > 33 && randomAxes < 66)
        {
            SetAxis(Axes.Y);
        }
        else if (randomAxes > 66 && randomAxes < 100)
        {
            SetAxis(Axes.Z);
        }
    }

    //set axis to chosen enum
    public void SetAxis(Axes axis)
    {
        xyOrzAxis = axis;

        if (xyOrzAxis == Axes.X)
        {
            if (localOrWorld)
            {
                chosenAxis = transform.right;
            }
            else
            {
                chosenAxis = Vector3.right;
            }
        }
        else if (xyOrzAxis == Axes.Y)
        {
            if (localOrWorld)
            {
                chosenAxis = transform.up;
            }
            else
            {
                chosenAxis = Vector3.up;
            }
        }
        else if (xyOrzAxis == Axes.Z)
        {
            if (localOrWorld)
            {
                chosenAxis = transform.forward;
            }
            else
            {
                chosenAxis = Vector3.forward;
            }
        }
    }
}