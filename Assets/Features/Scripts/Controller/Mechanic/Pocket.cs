using System.Collections;
using System.Collections.Generic;
using Sablo.Gameplay;
using UnityEngine;

public class Pocket : BaseGameplayModule
{   
    [SerializeField] private int numberOfObjects = 10;
    [SerializeField] private float spacing = 2.0f;
    public List<Transform> pocketCell;
    public GameObject locator;
    private float yOffset = 7.030809f;
    private float zOffset = -4.48f;
    public override void Initialize()
    {
        ArrangeObjects();
    }

    private void ArrangeObjects()
    {
        for (var i = 0; i < numberOfObjects; i++)
        {
            // Instantiate object
            GameObject obj;
            // if (i == 0)
            //     obj = Instantiate(edgeObj2, transform);
            // else if (i == numberOfObjects - 1)
            //     obj = Instantiate(edgeObj, transform);
            // else
            //     obj = Instantiate(objectPrefab, transform);
            obj = Instantiate(locator, transform);
            var objPos = obj.transform.position;
            // Calculate position
            var xPosition = (i - (numberOfObjects - 1) / 2.0f) * spacing;
            obj.transform.localPosition = new Vector3(xPosition,objPos.y+yOffset, zOffset);
            pocketCell.Add(obj.transform);
            // obj.transform.localRotation = Quaternion.Euler(0,90,0);
            // Optionally, set object's rotation or other properties here
        }
    }
}
