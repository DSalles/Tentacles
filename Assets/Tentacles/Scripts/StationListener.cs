using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using SonicBloom.Koreo;
using System;

public class StationListener : MonoBehaviour
{
    public StationState currentState = StationState.IDLE;
    // TODO: Organize and clean up all variable declarations
    // Organize by access (public vs private, etc)
    public KinectMotionListener _KinectListenerStation;
    public string StationName;
    //public StationStats _StationStats;
    public AudioSource SuccessSound;
    public AudioSource FailSound;
    public bool AutoPlay;

    public Transform ScoreTimerObject;

    public MasterListener _MasterListener;
    //public Session currentSession;
    public int StationIndex;

    public Rect StationRect;

    // Notifications
    UnityEngine.UI.Text TextObject;

    private KinectBodiesReceiver _KinectBodiesReceiver;

    private string[] StateTextPrompts;
    //private const int HowManyGestures = 12; // TODO: Does this have to be a constant or can it be derived through some other means? Seems like a bug waiting to happen...
    //private float IdealScale =52.5f;


    private GameObject Tutorial;
    private Animator StandByForAIU;
    public Transform ThisWelcomeScreen;


    public bool PlayerPresent;
    private GameObject IdleUIBkgd;

    private Transform ThisFinalScore;
    private Transform ThisGoodbyeScreen;


    public int StationWidth;
    public    float FractionOfIdealHeight;

    public int ScreenHeight;
    float HitTime;

    public ScoreTimer _ScoreTimerScript;

    private GameObject Canvas;



    public void ChangeState(StationState newState)
    {
        debug("-- State Change from: " + currentState.ToString() + "to: " + newState.ToString());

        switch (newState)
        {
            case StationState.IDLE:                
                Idle();               
                break;
            case StationState.WELCOME:
                Welcome();               
                break;
            case StationState.PLAYING:
                Play();
                break;
            case StationState.PLAYERMISSING:
                PlayerMissing();
                break;
            case StationState.FINAL_SCORE:
                FinalScore();
                break;
            case StationState.GOODBYE:
                GoodBye();
                    break;
        }

        currentState = newState;
        TextObject.text = StateTextPrompts[(int)currentState];
  
    }
    private float PlayerMissingTimeLimit;
    private void PlayerMissing()
    {
      PlayerMissingTimeLimit = 5;
      foreach(TentacleBehavior tentacle in _KinectListenerStation.tentacles) tentacle.Ungrow(PlayerMissingTimeLimit);
    }

   
    // Use this for initialization
    void Start()
    {
        StateTextPrompts = new string[] { "Join Game!", "Welcome!", "Play!", "PlayerMissing!", "Final Score", "Good-Bye!" };

        StationWidth = Screen.width / NumberOfStations;
       

        ScreenHeight = Screen.height;
        FractionOfIdealHeight = (float)ScreenHeight / IDEALHEIGHT;
        StationRect = new Rect((StationIndex-1) * (StationWidth), 0, StationWidth, ScreenHeight);

        Canvas = GameObject.Find("Canvas");
        InitializeSceneObjects();
        InitializeUI();
        
        _KinectBodiesReceiver = KinectBodiesReceiver.Instance;

        // Kick things off
        if (PlayerPresent)
        {
            ChangeState(StationState.WELCOME); //  (skip Idle)
        }
        else
        {
            ChangeState(StationState.IDLE); // IDLE
        }
       

    }

    // Initializes all UI elements
    void InitializeUI()
    {
        debug("InitializeUI()");
        // Idle mode

        try
        {
        IdleUIBkgd= Instantiate(GameObject.Find("IdleBkgUI"));
        IdleUIBkgd.gameObject.transform.localScale *= FractionOfIdealHeight;
        IdleUIBkgd.gameObject.transform.SetParent(Canvas.transform);
        IdleUIBkgd.gameObject.transform.position = new Vector3(StationRect.xMin + StationRect.width / 2, ScreenHeight/2, 0);
        }
        catch { debug("no Idle object"); }

       // // Gameplay notifications

        try
        {
            TextObject = Instantiate(GameObject.Find("Text").GetComponent<UnityEngine.UI.Text>());
            TextObject.gameObject.transform.localScale *= FractionOfIdealHeight;
            TextObject.gameObject.transform.SetParent(Canvas.transform);
            TextObject.text = "Join Game!";
            TextObject.gameObject.transform.position = new Vector3(StationRect.xMin+ StationRect.width/2, ScreenHeight*0.75f, 0);
        }
        catch { debug("no text object"); }
        //	DropBonusText.gameObject.transform.position = Place (MasterListener.Placement.HighMiddleHalf);          
        
    }


    void InitializeSceneObjects()
    {
  
    }

 

    // Cleans up previous session data / UI elements
    public void Cleanup()
    {
       foreach (TentacleBehavior tentacle in _KinectListenerStation.tentacles) tentacle.gameObject.SetActive(false);
        //	debug ("Cleanup()");

    }

    public void Idle()
    {
        debug("Idle()");
        Cleanup();
        IdleUIBkgd.SetActive(true);
       
  
    }

    // Welcome state
    public void Welcome()
    {
        StartNewPlay = false;
        debug ("REached Welcome()");
        // Hide Idle mode UI
        IdleUIBkgd.SetActive(false);
  
        foreach (TentacleBehavior tentacle in _KinectListenerStation.tentacles) tentacle.gameObject.SetActive(true);
        foreach (TentacleBehavior tentacle in _KinectListenerStation.tentacles) tentacle.TentacleStartOver();

        StartCoroutine(CountdowntoPlay());
    }
    

    private IEnumerator CountdowntoPlay()
    {        
        bool someTentaclesStillGrowing = true;
        while (someTentaclesStillGrowing == true)
        {
            
            bool newbool = true;

            foreach (TentacleBehavior tentacle in _KinectListenerStation.tentacles)
            {
                if (!tentacle.Growing)
                {
                    newbool = false;
                    someTentaclesStillGrowing = newbool;
                    print("Finished wwelcome corioutine");
                    continue;
                }
            }
            yield return null;
        }
       
        ChangeState(StationState.PLAYING);
        
    }

    // Play state
    public void Play()
    {
        if (StartNewPlay) return;
        StartNewPlay = true; // only start once
        debug("PlayPlay()");
        TimeLeftToPlay = 50;
    }

    private IEnumerator CountdowntoFinalScore()
    {
        print("Game over man");
        while (currentState == StationState.PLAYING)
        {
            yield return new WaitForSeconds(10);
            ChangeState(StationState.FINAL_SCORE);
        }
    }
   
    void Update()
    {
        if (currentState == StationState.PLAYING)
        {
            TimeLeftToPlay -= Time.deltaTime;
            if (TimeLeftToPlay <= 0) ChangeState(StationState.FINAL_SCORE);
        }
        else if(currentState == StationState.PLAYERMISSING)
        {
            PlayerMissingTimeLimit -= Time.deltaTime;
            if (PlayerMissingTimeLimit <= 0) ChangeState(StationState.GOODBYE);
        }

        if (Input.GetKeyDown(KeyCode.W)) ChangeState(StationState.WELCOME);
        else if (Input.GetKeyDown(KeyCode.I)) ChangeState(StationState.IDLE);
        else if (Input.GetKeyDown(KeyCode.P)) ChangeState(StationState.PLAYING);
        else if (Input.GetKeyDown(KeyCode.M)) ChangeState(StationState.PLAYERMISSING);
        else if (Input.GetKeyDown(KeyCode.F)) ChangeState(StationState.FINAL_SCORE);
        else if (Input.GetKeyDown(KeyCode.G)) ChangeState(StationState.GOODBYE);
        else if (Input.GetKeyDown(KeyCode.T)) _KinectListenerStation.tentacles[0].TentacleStartOver();

      //  if (currentState == StationState.WELCOME) foreach (TentacleBehavior tentacle in _KinectListenerStation.tentacles) tentacle.UpdateTentacle();
    }

    
    float PreviousDropBonus;
   // private bool PlayerDetected;
   // private float AvgHeight = 1.75f;
    private const int NumberOfStations = 3;
    private  const float IDEALHEIGHT = 1920F;

    public bool StartNewPlay { get; private set; }
    public float TimeLeftToPlay { get; private set; }

    void AddToStationScore(float dropBonus)
    {
        if (PreviousDropBonus + 5 < dropBonus)
        {
            PreviousDropBonus = dropBonus;
            //	_StationStats.Score +=5;
            //	_ScoreTimerScript.score = _StationStats.Score;
        }
    }
  
  

 
 
    // Final score screen
    // Called from ScoreTimer.cs when ScoreTimer outro animation completes
    public void FinalScore()
    {
        StartCoroutine(CountDownToGoodBye());
    }

    private IEnumerator CountDownToGoodBye()
    {
        yield return new WaitForSeconds(2);
        ChangeState(StationState.GOODBYE);
    }

    // Goodbye screen
    // Called from FinalScore.cs when FinalScore outro animation completes
    public void GoodBye()
    {
        foreach (TentacleBehavior tentacle in _KinectListenerStation.tentacles) tentacle.gameObject.SetActive(true);
        foreach (TentacleBehavior tentacle in _KinectListenerStation.tentacles) tentacle.TentacleStartOver();
        StartCoroutine(CountdowntoIdle());
    }

    private IEnumerator CountdowntoIdle()
    {
        yield return new WaitForSeconds(3);
        ChangeState(StationState.IDLE);
    }

    // Game Over screen
    // Called from GoodBye.cs when GoodBye outro animation completes

    void debug(string str)
    {
        Debug.Log(str + " " + StationIndex);
    }


    
    internal void PlayerIn(bool playerPresent)
    {
        if (PlayerPresent == playerPresent) return;   // If no change in player presence don't do anything
         PlayerPresent = playerPresent;

        if (playerPresent) //player newly present
        {
            if (currentState == StationState.IDLE) ChangeState(StationState.WELCOME);
            else if (currentState == StationState.PLAYERMISSING) ChangeState(StationState.PLAYING);         
        }
        else // player newly missing
        //if (currentState != StationState.IDLE && currentState != StationState.PLAYING) 
        //{
        //    ChangeState(StationState.IDLE);
        //}
         if (currentState == StationState.PLAYING)
        {
            ChangeState(StationState.PLAYERMISSING);
        }
    }
}


public enum StationState : int
{

    IDLE = 0,
    WELCOME ,
    PLAYING ,
    PLAYERMISSING,
    FINAL_SCORE,
    GOODBYE

  
}
