using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ReelsSet : MonoBehaviour
{
   // public int totalReels=3;
   public Reel[] reels = new Reel[3];

   // [Button]
   // public void SetOffAllReels()
   // {
   //    foreach (var reel in reels)
   //    {
   //       reel.gameObject.SetActive(false);
   //    }
   // }

   public void Initialize(int totalReels)
   {
      for (int i = 0; i < totalReels; i++)
      {
         reels[i].gameObject.SetActive(true);
      }
   }
}
