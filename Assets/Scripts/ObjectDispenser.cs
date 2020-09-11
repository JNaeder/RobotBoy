using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDispenser : MonoBehaviour
{

    public GameObject[] possibleObjects;
    public Transform spawnLocation;
    public Transform spawnedObjectParent;
    [Range(.01f, 10f)]
    public float spawnRate;
    public bool isDispensing;

    float _currentTime;
    float _newSpawnRate;

    private void Start()
    {
        if (isDispensing)
        {
            _currentTime = -spawnRate;
        }
    }


    private void Update()
    {
        SetSpawnRate();

        if (isDispensing) {
            RunDispenser();
        }
    }

    private void SetSpawnRate()
    {
        _newSpawnRate = (1 / spawnRate) * 5;
    }

    void SpawnObject() {
        
        Instantiate(ChooseRandomObject(), spawnLocation.position, Quaternion.identity, spawnedObjectParent);

    }

    GameObject ChooseRandomObject() {
        int randNum = UnityEngine.Random.Range(0, possibleObjects.Length);
        GameObject randomObject = possibleObjects[randNum];
        return randomObject;
    }

    void RunDispenser() {
        if (Time.time > _currentTime + _newSpawnRate) {
            SpawnObject();
            _currentTime = Time.time;

        }

    }

    public void ChangeDispenseState(bool newState) {
        isDispensing = newState;

    }
}
