using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
// using Sablo.Analytics;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameLoop : MonoBehaviour
{
    [SerializeField] private GameUi myGameUi;
    [ShowInInspector] public static GameLoop Instance;
    [SerializeField] public GameObject mainCamera;
    [SerializeField] private Vector3 newPos;
    [SerializeField] private Vector3 newPos2;
    private Action onLevelContinue;
    private bool isVideoPrepared=false;
    public bool isUiActive;
    public IlevelManager LevelHandler { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        // myGameUi.tutorialPlayer.prepareCompleted += OnVideoPrepared;
        // // // Start preparing the video
        // myGameUi.tutorialPlayer.Prepare();
        

        //AddCoins(20000);
    }

    public bool onceFail;
  
    public void GameFail()
    {
        //LionAnalyticEvents.OnLevelFail(LevelHandler.GetCurrentLevelIndex()+1,LevelHandler.LevelAttempt,"");
//         if (onceFail) return;
//         onceFail = true;
// //LionAnalyticEvents.OnLevelFail(LevelHandler.GetCurrentLevelIndex()+1,LevelHandler.LevelAttempt,"");
//         LionAnalyticEvents.MissionFailed(false, "GamePlay", LevelHandler.GetCurrentLevelIndex() + 1,
//             LevelHandler.LevelAttempt); //Level Failed
//         AnalyticsEventsSort._instance.LevelFailed(LevelHandler.GetCurrentLevelIndex() + 1, LevelHandler.LevelAttempt);
       
        ManagePlayOnButton();
        LevelHandler.CurrentRunningLevel = 0;
        LevelHandler.LevelAttempt += 1;
        myGameUi.TurnOnFailPanel();
    }

    public void FailEvent()
    {
        if (onceFail) return;
        onceFail = true;

        // LionAnalyticEvents.MissionFailed(false, "GamePlay", LevelHandler.GetCurrentLevelIndex() + 1,
        //     LevelHandler.LevelAttempt); //Level Failed
        // AnalyticsEventsSort._instance.LevelFailed(LevelHandler.GetCurrentLevelIndex() + 1, LevelHandler.LevelAttempt);
    }
    
    public bool onceCompleted;
    
    public void GameWin()
    {    
        if (onceCompleted) return;
        onceCompleted = true;
        // LionAnalyticEvents.MissionCompleted(false, "GamePlay", LevelHandler.GetCurrentLevelIndex() + 1,
        //     LevelHandler.LevelAttempt); //Level Completed
        // LionAnalyticEvents.MissionStepCompleted(false, "GamePlay", LevelHandler.GetCurrentLevelIndex() + 1,
        //     LevelHandler.GetSubLevel + 1, LevelHandler.LevelAttempt); //missionStep
        //
        // AnalyticsEventsSort._instance.LevelCompleted(LevelHandler.GetCurrentLevelIndex() + 1,
        //     LevelHandler.LevelAttempt);
        //
        // AnalyticsEventsSort._instance.LevelStepCompleted(LevelHandler.GetCurrentLevelIndex() + 1,
        //     LevelHandler.GetSubLevel + 1, LevelHandler.LevelAttempt); 
        
        
        
        int totalSubLevels = LevelHandler.GetTotalSubLevelCount-1;
        if (totalSubLevels > -1 && LevelHandler.GetSubLevel == totalSubLevels)
        {
            Debug.LogError("tick last fill");
            myGameUi.levelNumberObjects[totalSubLevels].tickList[LevelHandler.GetSubLevel].gameObject.SetActive(true);
        }
        //LionAnalyticEvents.OnLevelFail(LevelHandler.GetCurrentLevelIndex()+1,LevelHandler.LevelAttempt,"");
        // LionAnalyticEvents.MissionCompleted(false, "GamePlay", LevelHandler.GetCurrentLevelIndex() + 1,
        //      LevelHandler.LevelAttempt); //Level Completed
        LevelHandler.SubLevelPref = 0;
        myGameUi.LevelManagerHandler.IncrementLevel();  
        LevelHandler.CurrentRunningLevel = 0;
        LevelHandler.LevelAttempt = 1;
        myGameUi.TurnOnWinPanel();
        // MoveCamera.Instance.TriggerFunctionUsingButton();
        // var buildInstance=Building.Instance;
        // if(buildInstance.isColorizeAble && (LevelHandler.GetTotalSubLevelCount<1))
        //     buildInstance.ChangeColorOfBrickAnimation();
        // AudioManager.instance.ConfettiSfx();
    }

    public void ResetWinFailBool()
    {
        onceCompleted = false;
        onceFail = false;
    }

    public void AddCoins(int val)
    {
        int startCoins = LevelHandler.Coins;
        int totalCoins = startCoins + val;
        myGameUi.DisableNextButton();
        // Animate both UI elements simultaneously
        var coinsAnimation = DOTween.Sequence();

        coinsAnimation.Append(DOTween.To(
            x =>
            {
                int displayedCoins = Mathf.FloorToInt(x);
                myGameUi.coinsVal.text = displayedCoins.ToString();
                myGameUi.coinsVal2.text = displayedCoins.ToString();
              
            },
            startCoins,
            totalCoins,
            1
        ).OnComplete(() =>
        {
            // Update the actual coin count after the animation is complete
            LevelHandler.Coins += val;
            myGameUi.EnableNextButton();
            // Ensure the final UI update is accurate
            myGameUi.coinsVal.text = LevelHandler.Coins.ToString();
            myGameUi.coinsVal2.text = LevelHandler.Coins.ToString();
            
            // Manage button state or any other necessary updates
            ManagePlayOnButton();
        }).SetDelay(2f).OnStart(()=> StartCoroutine(CoinsSound())));

        
       
         IEnumerator CoinsSound()
        {
            for (int i = 1; i <= 25; i++)
            {
                yield return new WaitForSeconds(0.05f);
                // AudioManager.instance.CoinsAdd();
            }
        }
    }

    
    public void DecreaseCoins(int val)
    {   
        if(LevelHandler.Coins>=50)
            LevelHandler.Coins -= val;
        myGameUi.coinsVal.text = LevelHandler.Coins.ToString();
        myGameUi.coinsVal2.text = LevelHandler.Coins.ToString();
        ManagePlayOnButton();
    }

    public void RefreshCoins()
    {
        myGameUi.coinsVal.text = LevelHandler.Coins.ToString();
        myGameUi.coinsVal2.text = LevelHandler.Coins.ToString();
    }
    
    public void ManagePlayOnButton()
    {
        if (LevelHandler.Coins < 100)
        {
            myGameUi.DisablePlayOnButton();
        }
        else
        {
            myGameUi.EnablePlayOnButton();
        }
    }

    public void MakeStackOutOfPocket()
    {
        onLevelContinue?.Invoke();
    }
    
    

    public void ChangeCameraPosAccToRowCount(int noOfRows)
    {
        var camPos = mainCamera.transform.position;
        camPos.z = noOfRows == 4 ? newPos.z : newPos2.z;
        if (noOfRows == 2)
            camPos.y = newPos2.y;
        mainCamera.transform.position = camPos;
    }

    public void OnContinueLevel(Action actionToPerform)
    {
        onLevelContinue = actionToPerform;
    }

    public void UpdateLevelNo(int value)
    {
        myGameUi.levelNoText.text = "Level "+value.ToString();
    }
    
    public void UpdateSubLevelNo(int totalSubLevels,int subLevelNo)
    {
        if (totalSubLevels > -1)
        {
            myGameUi.levelNumberObjects[totalSubLevels].parentLevelNumber.gameObject.SetActive(true);
            for (int i = 0; i <= subLevelNo; i++)
            {   
                if (i == subLevelNo)
                {
                    myGameUi.levelNumberObjects[totalSubLevels].fillers[i].sprite = myGameUi.selectedFill;
                    myGameUi.levelNumberObjects[totalSubLevels].fillers[i].SetNativeSize();
                    
                }
                else
                {
                    myGameUi.levelNumberObjects[totalSubLevels].fillers[i].sprite = myGameUi.fill;
                    myGameUi.levelNumberObjects[totalSubLevels].fillers[i].SetNativeSize();
                }
                myGameUi.levelNumberObjects[totalSubLevels].fillers[i].gameObject.SetActive(true);
            } 
            for (int i = 0; i < subLevelNo; i++)
            {
                myGameUi.levelNumberObjects[totalSubLevels].tickList[i].gameObject.SetActive(true);
            }
        }
        else
        {
            var levelTextTransform = myGameUi.levelNoText.transform;
            var newPos = levelTextTransform.position;
            newPos.y -= 20f;
            levelTextTransform.position = newPos;
        }
    }


    public void ActivateTutorialPanel(int curLevelNo)
    {
        switch (curLevelNo)
        {
            case 6:
                SetTutorialData(0);
                break;
            case 10:
                SetTutorialData(1);
                break;
            case 14:
                SetTutorialData(2);
                break;
        }
    }
    
    // void OnVideoPrepared(VideoPlayer vp)
    // {
    //     Debug.Log("Video is prepared");
    //     //myGameUi.tutorialPlayer.Play();
    //     isVideoPrepared = true;
    // }

    private void SetTutorialData(int index)
    {
        myGameUi.tutorialHeadingTxt.text = myGameUi.tutorialData[index].tutorialHeading;
        myGameUi.tutorialDescription.text = myGameUi.tutorialData[index].tutorialDescription;

       myGameUi.tutorialData[index].tutorialClip.SetActive(true);
        //myGameUi.tutorialPlayer.Prepare();

        // Wait until the video is prepared
        // yield return new WaitUntil(() => myGameUi.tutorialPlayer.isPrepared);
        
        //myGameUi.tutorialPlayer.Play();
        myGameUi.EnableTutorialPanel();
    }

    public void RunPocketFillTutorial()
    {
        if (FillTutorial == 0)
        {
            myGameUi.EnableFillTutorial();
            FillTutorial = 1;
        }
    }
    
    public void ExitPocketFillTutorial()
    {
        if (FillTutorial == 1)
        {   
            myGameUi.DisableFillTutorial();
        }
    }
    
    private int FillTutorial
    {
        get => PlayerPrefs.GetInt("PocketFill", 0);
        set => PlayerPrefs.SetInt("PocketFill", value);
    }
}


