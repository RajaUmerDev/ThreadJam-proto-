using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICarrier
{
    List<Vector3> GetBrickPositionList();
    void AddBricksToCarrier(List<Reel> selectedBricks,Action callBack);

    Transform GetCarrierTransform();

    void MoveAside();

    int GetCarrierCount();

    int GetMaxSize();
    IEnumerator ResetBrickPos(int from,List<Reel> selectedBricks);

     void IncreaseBrickCount();
     int GetBrickCount();

    List<Reel> GetCarrierBricks();

    bool IsCarrierMoving();

    void SetOffMoving();
    int CarrierCapacity();

}