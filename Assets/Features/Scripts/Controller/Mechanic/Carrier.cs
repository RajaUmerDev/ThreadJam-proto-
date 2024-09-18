using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DG.Tweening;
using Sablo.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Carrier : MonoBehaviour, ICarrier
{
    public List<Reel> carrierBrickList;
    public List<Transform> brickPositions;
    [HideInInspector] public List<Vector3> brickCellPositions;

    public int SpawnOrder;

    // public BrickColor carrierColor;
    public Transform nextCarrierPos;
    [ShowInInspector] private const int MaxCapacity = 9;
    [SerializeField] private Transform lostPos;
    public bool isUpFront;
    public bool isFull;
    private Action _moveAllCarrier;

    public IRoller RollerInterface;

    // public ITray TrayInterface;
    [SerializeField] private float brickDelay = 0.15f;
    public int bricksAddCount;
    public bool lastCarrier;

    public bool isMoving = false;

    // public ISlateBuilder SlateBuilderInterface;
    public IlevelManager LevelManagerInterface;

    [Header("Haptic Values")] [SerializeField]
    private float Amplitude = 0.5f, Frquency = 1f, duration = 0.017f;

    private void SetCarrierFillAmount()
    {
        if (carrierBrickList.Count >= MaxCapacity)
        {
            isFull = true;

        }

    }

   


    public void Initialize()
    {
        isUpFront = true;
        //TapController.Instance.curCarrierColor = carrierColor;
    }

    public void MoveToNextCarrierPos()
    {
        transform.DOMove(nextCarrierPos.position, 0.5f).SetEase(Ease.Linear);
    }

    // private void ResetPositionOfALlBricks()
    // {
    //     for (int i = 0; i < carrierBrickList.Count; i++)
    //     {   
    //         var newPos = brickCellPositions[i];
    //         newPos.y += Configs.GameConfig.brickYOffset;
    //         carrierBrickList[i].transform.position = newPos;
    //     }
    // }
    
    // public IEnumerator ResetBrickPos(int from, List<Brick> selectedBricks)
    // {
    //     var curCarrier = TapController.Instance.curCarrierHandler;
    //     var curCarrierPosList = TapController.Instance.curCarrierHandler.GetBrickPositionList();
    //     
    //     var bricksToMove = new List<Brick>(selectedBricks);
    //     //Debug.Break();
    //     var lastBrickIndex = bricksToMove.Count - 1;
    //     var lastBrick = bricksToMove[lastBrickIndex];
    //
    //     foreach (var brick in bricksToMove)
    //     {
    //         var indexOfBrick = carrierBrickList.IndexOf(brick);
    //
    //         brick.MoveToTargetCellPos(indexOfBrick, brick == lastBrick
    //                 ? () =>
    //                 { 
    //                     ActionCallBack(curCarrier, brick);
    //                     TapController.Instance.curCarrierHandler.MoveAside(); 
    //                 } 
    //                 : () =>
    //                 {
    //                     ActionCallBack(curCarrier, brick);
    //                 }
    //             
    //             , curCarrierPosList);
    //
    //         yield return new WaitForSeconds(Configs.GameConfig.delayBetweenNextBrick);
    //     }
    // }

    private void ActionCallBack(ICarrier curCarrier ,Reel brick)
    {
        curCarrier.IncreaseBrickCount();
        brick.transform.SetParent(RollerInterface.GetRollerTransform());
        // var brickMaterial = brick.brickRenderer.material;
        // Texture brickTexture = RollerInterface.GetNewBrickTexture();
        // brickMaterial.mainTexture = brickTexture;
        AudioManager.instance.ThrowSound();
    }
    

    // private IEnumerator ThrowAllBricksToBuilding()
    // {
    //     var allBricksReached = false;
    //     var carrier = TapController.Instance.curCarrierHandler;
    //     var brickCount = carrier.GetCarrierBricks().Count;
    //     var brickList = carrier.GetCarrierBricks();
    //
    //     for (var i = 0; i < brickCount; i++)
    //     {
    //         var targetBrick = Building.Instance.GetCurrentBrick() ??
    //                           throw new ArgumentNullException("Building.Instance.GetCurrentBrick()");
    //         if (targetBrick != null)
    //         {
    //             var brick = brickList[i];
    //             brick.MoveToTarget(targetBrick, () =>
    //             {
    //                 AudioManager.instance.PlaceBrick();
    //                 var particlePos = brick.transform.position;
    //                 particlePos.y -= 0.1f;
    //                 SpawnParticleOnBrickDrop(RollerInterface.GetSpawnParticle,particlePos);
    //                 brick.gameObject.SetActive(false);
    //                 targetBrick.gameObject.SetActive(true);
    //                 var defaultScale = targetBrick.localScale;
    //                 targetBrick.DOPunchScale(defaultScale, 0.2f, 1, 0.2f).SetEase(Ease.OutBounce);
    //                 if (i == brickCount) allBricksReached = true;
    //             });
    //         }
    //
    //         yield return new WaitForSeconds(0.06f);
    //     }
    //
    //     yield return new WaitUntil(() => allBricksReached);
    //     TapController.Instance.IncreaseNumberOfCarriersPass(); // increasing no of carrier
    //     ReInitializeSlateBuilder();
    //     carrier.GetCarrierTransform().DOMove(lostPos.position, 0.5f).SetEase(Ease.Linear).OnStart(() =>  isMoving = true)
    //         .OnComplete(MoveBrickFromPocketToCarrier);
    //     RollerInterface.MoveAllCarrierToNextOne();
    // }

    private void SpawnParticleOnBrickDrop(GameObject particle,Vector3 pos)
    {
        var newParticle = Instantiate(particle);
        newParticle.transform.position = pos;
        Destroy(newParticle,0.34f);
    }

    private void MoveBrickFromPocketToCarrier()
    {   
        //Debug.LogError("MoveBrickFromPocketToCarrier");
        //  var tapInstance = TapController.Instance;
        //  var brickToRem = tapInstance.TrayHandler.FindBricksOfCarrierColor(tapInstance.curCarrierHandler,tapInstance.curCarrierColor);
        //  tapInstance.TrayHandler.RemoveBrickFromPocketByTray(brickToRem,1);
        //
        // var curCarrierCapacity = TapController.Instance.curCarrierHandler.CarrierCapacity();
        // var color = TapController.Instance.curCarrierColor;
        // var brickToRem = new List<Brick>();
        // //Debug.LogError("Car Color: "+color);
        //TrayInterface.MoveBricksToCurCarrier(color, TapController.Instance.curCarrierHandler,brickToRem);
       
    }

    public void SetInitialPositions(Transform brickTransform)
    {
        brickCellPositions.Add(brickTransform.position);
    }

    // private void ReInitializeSlateBuilder()
    // {
    //     if (RollerInterface.IsLastCarrierOfSubLevel())
    //     {   
    //         if(LevelManagerInterface.GetTotalSubLevelCount!=0)
    //             ChangeColorBySubLevel();
    //         if (LevelManagerInterface.GetSubLevel < LevelManagerInterface.GetTotalSubLevelCount-1)
    //         {
    //             DOVirtual.DelayedCall(0.2f, () =>
    //             {   
    //                 LevelManagerInterface.ReloadLevel();
    //                 SlateBuilderInterface.ReInitialize();
    //             }); 
    //         }
    //        
    //     }
    // }

    // private void ChangeColorBySubLevel()
    // {
    //     
    //     switch(LevelManagerInterface.GetSubLevel)
    //     {
    //         case 0:
    //             int firstLevelBricks=9*(LevelManagerInterface.SubLevelList[LevelManagerInterface.GetSubLevel].carrierToSpawn.Count);
    //             Building.Instance.ChangeColorOfBrickAnimationByLevel(0,firstLevelBricks);
    //             break;
    //         case 1:
    //             int lastLevelBricks = 9*(LevelManagerInterface.SubLevelList[(LevelManagerInterface.GetSubLevel)-1].carrierToSpawn.Count);
    //             int secondLevelBricks=9*(LevelManagerInterface.SubLevelList[LevelManagerInterface.GetSubLevel].carrierToSpawn.Count);
    //             RollerInterface.MaterialSumCount = lastLevelBricks + secondLevelBricks;
    //             Building.Instance.ChangeColorOfBrickAnimationByLevel(lastLevelBricks,RollerInterface.MaterialSumCount);
    //             break;
    //         case 2:
    //             // int last_Level_Bricks = 9*(LevelManagerInterface.SubLevelList[(LevelManagerInterface.GetSubLevel)-1].carrierToSpawn.Count);
    //             // int thirdLevelBricks=9*(LevelManagerInterface.SubLevelList[LevelManagerInterface.GetSubLevel].carrierToSpawn.Count);
    //             // int sums = last_Level_Bricks + thirdLevelBricks;
    //             int max = Building.Instance.GetMaterialCount;
    //             Debug.LogError("this is sum in subLevel 3 : " +RollerInterface.MaterialSumCount);
    //             Building.Instance.ChangeColorOfBrickAnimationByLevel(RollerInterface.MaterialSumCount,max);
    //             break;
    //             
    //     }
    // }

     void ICarrier.SetOffMoving()
    {
        isMoving = false;
    }

     int ICarrier.CarrierCapacity()
     {
         var capacity = MaxCapacity - carrierBrickList.Count;
         return capacity;
     }
     


    #region Interface Methods

    void ICarrier.MoveAside()
    {
        // if (isFull && TapController.Instance.curCarrierHandler.GetBrickCount() >= MaxCapacity)
        // {
        //     var curCarrier = TapController.Instance.curCarrierHandler.GetCarrierTransform();
        //     StartCoroutine(ThrowAllBricksToBuilding());
        // }
    }

    Transform ICarrier.GetCarrierTransform()
    {
        return transform;
    }

    public void OnMoveAside(Action fnToPerform)
    {
        _moveAllCarrier = fnToPerform;
    }

    int ICarrier.GetCarrierCount()
    {
        return carrierBrickList.Count;
    }

    int ICarrier.GetMaxSize()
    {
        return MaxCapacity;
    }

    public IEnumerator ResetBrickPos(int @from, List<Reel> selectedBricks)
    {
        throw new NotImplementedException();
    }

    bool ICarrier.IsCarrierMoving()
    {
        return isMoving;
    }

    void ICarrier.IncreaseBrickCount()
    {
        bricksAddCount++;
    }

    int ICarrier.GetBrickCount()
    {
        return bricksAddCount;
    }

    List<Reel> ICarrier.GetCarrierBricks()
    {
        return carrierBrickList;
    }

    List<Vector3> ICarrier.GetBrickPositionList()
    {
        return brickCellPositions;
    }

   

    void ICarrier.AddBricksToCarrier(List<Reel> selectedBricks, Action callBack)
    {
        // for (var i = 0; i < selectedBricks.Count; i++)
        //     if (carrierBrickList.Count < MaxCapacity)
        //         if (!carrierBrickList.Contains(selectedBricks[i]))
        //         {
        //             carrierBrickList.Add(selectedBricks[i]);
        //             selectedBricks[i].isInCar = true;
        //         }
        //            
        //
        // callBack?.Invoke();
        // IsLastCarrierFull();
        // //StartCoroutine(ResetBrickPos(0));
        // SetCarrierFillAmount();
    }

    private void IsLastCarrierFull()
    {
        var carrierList = RollerInterface.GetCarrierList();
        int indexOfLastCarrier = carrierList.Count - 1;
        if (this == carrierList[indexOfLastCarrier] && carrierBrickList.Count == MaxCapacity)
        {
            DOVirtual.DelayedCall(2.56f, () =>
            {
                GameLoop.Instance.GameWin();
            });
        }
    }

    #endregion
}