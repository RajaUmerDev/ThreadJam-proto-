using System.Collections;
using System.Collections.Generic;
using Sablo.Gameplay;
using UnityEngine;

public class Cupboard : BaseGameplayModule
{
   [SerializeField] private GameObject wrack;
   [SerializeField] private LevelData curLevelData;
   [SerializeField] private List<Box> _boxList;
   [SerializeField] private List<Vector3> cellPositions;
   
   private int _rows;
   private int _columns;
   public IlevelManager LevelHandler { get; set; }

   public override void Initialize()
   {
      SpawnWrack();
      SetData();
      SpawnData(transform, curLevelData);
      SpawnReelSets();
   }

   private void SpawnWrack()
   {
      Instantiate(wrack, transform);
   }

   private void SetData()
   {
      curLevelData = LevelHandler.GetCurrentLevel;
   }

   private void SpawnReelSets()
   {
       for (var index = 0; index < curLevelData.BoxDataList.Count; index++)
       {
           var box = curLevelData.BoxDataList[index];
           for (int i = 0; i < box.reelDataList.Count; i++)
           {
               var myReel = Instantiate(box.reelDataList[i].reelsSet, transform);
               myReel.transform.localPosition = cellPositions[index];
               myReel.Initialize(box.reelDataList[i].count);
               Vector3 celPos = cellPositions[index];
               celPos.z += 0.9f;
               cellPositions[index] = celPos;
           }
          
       }
   }
   
   private void SpawnData(Transform parentObject, LevelData level)
        {
            _rows = level.Column;
            _columns = level.Row;
            var grid = level.Grid;
            var tileSpacing = 2.72f;
            var totalWidth = (_rows - 1) * tileSpacing;
            var totalHeight = (_columns - 1) * tileSpacing;
            var gridCenterOffset = new Vector3(totalWidth / 2, 0, totalHeight / 2);

            for (int row = 0; row < _rows; row++)
            {
                for (int col = 0; col < _columns; col++)
                {
                    // var cellData = grid[row, col];
                    var tilePosition = new Vector3(row * tileSpacing, (level.Column - 3.35f - col) * tileSpacing,2 ) - gridCenterOffset  /*+parentObject.transform.position*/;
                    switch (grid[row, col].tileType)
                    {
                        case TileType.Empty:
                            Debug.Log("No Stack here");
                            // SpawnReelSets(emptySet, transform, false, tilePosition);
                            break;
                        case TileType.Stack:
                            cellPositions.Add(tilePosition);
                            // _gridPositions.Add(stack.transform);
                            break;
                    }
                    
                }
            }
        }

    
    
}
