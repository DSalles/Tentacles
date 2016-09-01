
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
//using SonicBloom.Koreo;
//using Assets;


public class MasterListener : MonoBehaviour {
  


	//public AudioPlayer _AudioPlayer;
	public Dictionary<int, StationListener> StationListeners = new Dictionary<int, StationListener>();
	public int TheOnlyActiveStationForNow;
	public GestureNums ActiveGesture;

	public KeyCode PlayKey;
	public KeyCode PauseKey;
	public KeyCode DropKey;
	public KeyCode DropKey2;
	public KeyCode EndKey;
	public KeyCode ResetKey;
	public KeyCode ResetKey2;
	public KeyCode VerseKey;

	private float HitTime;
	private bool HitTimeOver = false;
	private List<Placement> PlacementList;
	private Placement LastPlacement;
	private Dictionary<KeyCode, int> KeyCodeStation = new Dictionary<KeyCode, int>() { { KeyCode.Alpha1, 0 }, { KeyCode.Alpha2, 1 }, { KeyCode.Alpha3, 2 }, { KeyCode.Alpha4, 3 }, { KeyCode.Alpha5, 4 }, { KeyCode.Alpha6, 5 }, { KeyCode.Alpha7, 6 } };
	
    
    private int stationsReady = 0;

    internal int GetTeamScore()
    {
        int teamScore = 0;
        foreach(KeyValuePair<int,StationListener> kvp in StationListeners)
        {
         //   teamScore += kvp.Value._StationStats.Score;
        }
        return teamScore;
    }

    void Start()
    {

        Application.targetFrameRate = 60;

    }

	void Update ()
	{

		//HOW TO PLAY SOUND
		if (Input.GetKeyUp (KeyCode.S)) {
			KinectBodiesReceiver.Instance.PlaySound("success",4);
		}

		//ALSO SOUND :D
		if (Input.GetKeyUp (KeyCode.I))
			KinectBodiesReceiver.Instance.PlayIdentification (4);
        

		// Update each station with remaining song time, adjusted for gap skipping over drop beginning to middle of drop
		foreach(KeyValuePair<int, StationListener> ki in StationListeners) {
            
			
		//	ki.Value.UpdateTime(_AudioPlayer.SecondsLeft);
          
        }


		// Key press game jumping logic

		if (Input.anyKeyDown) { // for making individual stations ready - assigns stations user names

			foreach(KeyValuePair<KeyCode,int> ki in KeyCodeStation) {
				if(Input.GetKeyDown (ki.Key)&& StationListeners.ContainsKey(ki.Value)) {
	

					StationListener station = StationListeners[ki.Value];
				station.ChangeState(StationState.WELCOME);

//					}
				}
			}
			
			// For playing, pausing and jumping to different  points in game progression
			if (Input.GetKeyDown(PlayKey) && (StationListeners.Count > 0)) {
				Debug.Log ("1 pressed");

				foreach (KeyValuePair<int, StationListener> kvp in StationListeners) {
					StationListener station = kvp.Value;
					if (station.AutoPlay) {
						station.ChangeState(StationState.PLAYING); 
					}
				}

				
			} else if (Input.GetKeyDown(PauseKey)) {
			//	_AudioPlayer.Pause();
			} else if(Input.GetKeyDown(EndKey)) {
				//_AudioPlayer.SkipToSampleTime(7329750);
			} else if(Input.GetKeyDown (DropKey) || Input.GetKey(DropKey2)) {
				//_AudioPlayer.SkipToSampleTime(3240000);
			}
			else if(Input.GetKeyDown (ResetKey)|| Input.GetKey(ResetKey2))
			{
				//_AudioPlayer.SkipToSampleTime(0);
			}else if(Input.GetKeyDown(VerseKey)) {
			//	_AudioPlayer.SkipToSampleTime(1350000);
			} else if(Input.GetKeyDown (ResetKey)|| Input.GetKey(ResetKey2)){
				//_AudioPlayer.SkipToSampleTime(0);
			} else if(Input.GetKeyDown(VerseKey)){
				//_AudioPlayer.SkipToSampleTime(1350000);
			} else if(Input.GetKeyDown (ResetKey)|| Input.GetKey(ResetKey2)) {
			//	_AudioPlayer.SkipToSampleTime(0);
			} else if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.S)) {
				// TODO: start game stations
				Debug.Log("Starting game stations...");
				foreach (StationListener station in StationListeners.Values) {
					//station.
					Debug.Log("-- " + station.StationIndex);
				}
			}	
		}
	}

	// Awaits notification from stations they are ready to play and starts the game
	public void readyToPlay(StationListener station) {
		//stationsReady++;
		//if (stationsReady == StationListeners.Count) {
          
  //          StartGameStations();
  //          stationsReady = 0;
  //      }

	}

    /*
	 *	Private methods 
	 */
    IEnumerator WaitToPlay()
    {
      yield return new WaitForSeconds(4);

    }
	private void StartGameStations() {
		Debug.Log ("StartGameStations()");
		foreach (KeyValuePair<int, StationListener> kvp in StationListeners) {
			StationListener station = kvp.Value;
			// only play if we're in welcome state...
			Debug.Log ("-- Starting station " + station.StationIndex);
			if (station.AutoPlay || (int)station.currentState == 1) {
				station.ChangeState(StationState.PLAYING); 
			}
			station.Play();
		}
        StartCoroutine("WaitToPlay");

	}

	// starts the gesture icon sprite anim based on the int value from koreo
/*	private void GestureBeat(KoreographyEvent koreoEvent)
	{
       //if (koreoEvent.HasTextPayload()) print(koreoEvent.GetTextValue());
		// timing of key scene events
		if (koreoEvent.GetColorValue () == Color.black) {
			for (int i = 0; i < 8; i++) {
				if (StationListeners.ContainsKey (i))
					StationListeners [i].Drop ();
				_AudioPlayer.SkipToSampleTime(3898255);
			}
		} else if (koreoEvent.GetColorValue () == Color.red) {
			for (int i = 0; i < 8; i++) {
				if (StationListeners.ContainsKey (i))
					StationListeners [i].DropEnd ();
			}
		} else if (koreoEvent.GetColorValue () == Color.green) {
			for (int i = 0; i < 8; i++) {
				if (StationListeners.ContainsKey (i))
					StationListeners [i].FinalScoreLeadUp ();
			}
		}
		
		// triggers gestures based on koreo notes
		switch( koreoEvent.GetTextValue() )
		{
	

		case "C" :
			
			ActiveGesture = GestureNums.TwoHandsDown;
			for(int i = 0; i < 8; i++){if(StationListeners.ContainsKey (i))StationListeners[i].TwoHandsDown.SetTrigger("Start");  }  
			LastPlacement = Placement.HighMiddleHalf;
			break;

		case "C#/Db" :

			ActiveGesture = GestureNums.UpLeft;
			for(int i = 0; i < 8; i++){if(StationListeners.ContainsKey (i))StationListeners[i].UpLeft.SetTrigger("Start");  }  
			LastPlacement = Placement.HighLeftQuad;
			break;
			
		case "D" : 
			ActiveGesture = GestureNums.UpRight;
			for(int i = 0; i < 8; i++){if(StationListeners.ContainsKey (i))StationListeners[i].UpRight.SetTrigger("Start");  } 
			LastPlacement = Placement.HighRightQuad;
			break;
			
		case "D#/Eb" :
			ActiveGesture = GestureNums.StraightLeft;
			for(int i = 0; i < 8; i++){if(StationListeners.ContainsKey (i))StationListeners[i].StraightLeft.SetTrigger("Start");  } 
			LastPlacement = Placement.LowLeftQuad; // not
			break;
			
		case "E" :
			ActiveGesture = GestureNums.StraightRight;
			for(int i = 0; i < 8; i++){if(StationListeners.ContainsKey (i))StationListeners[i].StraighRight.SetTrigger("Start");  } 
			LastPlacement = Placement.LowRightQuad; // not
			break;
			
		case "F" :
			ActiveGesture = GestureNums.CurlRight;
			for(int i = 0; i < 8; i++){if(StationListeners.ContainsKey (i))StationListeners[i].CurlRight.SetTrigger("Start");  } 
			LastPlacement = Placement.HighLeftQuad;
			break;
			
		case "F#/Gb" :
			ActiveGesture = GestureNums.CurlLeft;
			for(int i = 0; i < 8; i++){if(StationListeners.ContainsKey (i))StationListeners[i].CurlLeft.SetTrigger("Start");  } 
			LastPlacement = Placement.HighRightQuad;
			break;
			
		case "G" :
			ActiveGesture = GestureNums.BigWaveRight;
			for(int i = 0; i < 8; i++){if(StationListeners.ContainsKey (i))StationListeners[i].BigWaveRight.SetTrigger("Start");  } 
			LastPlacement = Placement.LowLeftQuad;
			break;
			
		case "G#/Ab" :
				ActiveGesture = GestureNums.BigWaveLeft;
			for(int i = 0; i <8; i++){if(StationListeners.ContainsKey (i))StationListeners[i].BigWaveLeft.SetTrigger("Start");  } 
			LastPlacement = Placement.LowRightQuad;
			break;
			
		case "A" :
			ActiveGesture = GestureNums.TwoHandsUp;
			for(int i = 0; i < 8; i++){if(StationListeners.ContainsKey (i))StationListeners[i].TwoHandsUp.SetTrigger("Start");  } 
			LastPlacement = Placement.HighMiddleHalf;
			break;
			
		case "A#/Bb" :
			ActiveGesture = GestureNums.TwoHandsWaveRight;
			for(int i = 0; i < 8; i++){if(StationListeners.ContainsKey (i))StationListeners[i].TwoHandsWaveRight.SetTrigger("Start");  } 
			LastPlacement = Placement.LowMiddleHalf;
			break;

		case "B" :
			ActiveGesture = GestureNums.TwoHandsWaveLeft;
			for(int i = 0; i < 8; i++){if(StationListeners.ContainsKey (i))StationListeners[i].TwoHandsWaveLeft.SetTrigger("Start");  } 
			LastPlacement = Placement.LowMiddleHalf;
			break;
		case "StartTutorialIntro" :
			for(int i = 0; i < 8; i++){if(StationListeners.ContainsKey (i))StationListeners[i].StartTutorialIntro();}
			break;
		case "EndTutorialIntro" :
			for(int i = 0; i < 8; i++){if(StationListeners.ContainsKey (i))StationListeners[i].EndTutorialIntro();}
			break;
		case "StartTutorialInstructions01" :
			for(int i = 0; i < 8; i++){if(StationListeners.ContainsKey (i))StationListeners[i].StartTutorialInstructions01();}
			break;
		case "EndTutorialInstructions01" :
			for(int i = 0; i < 8; i++){if(StationListeners.ContainsKey (i))StationListeners[i].EndTutorialInstructions01();}
			break;
		case "StartTutorialInstructions02pt1" :
			for(int i = 0; i < 8; i++){if(StationListeners.ContainsKey (i))StationListeners[i].StartTutorialInstructions02pt1();}
			break;
		case "EndTutorialInstructions02pt1" :
			for(int i = 0; i < 8; i++){if(StationListeners.ContainsKey (i))StationListeners[i].EndTutorialInstructions02pt1();}
			break;
		case "StartTutorialInstructions02pt2" :
			for(int i = 0; i < 8; i++){if(StationListeners.ContainsKey (i))StationListeners[i].StartTutorialInstructions02pt2();}
			break;
		case "EndTutorialInstructions02pt2" :
			for(int i = 0; i < 8; i++){if(StationListeners.ContainsKey (i))StationListeners[i].EndTutorialInstructions02pt2();}
			break;
		case "StartTutorialOutro" :
			for(int i = 0; i < 8; i++){if(StationListeners.ContainsKey (i))StationListeners[i].StartTutorialOutro();}
			break;
		case "EndTutorialOutro" :
			for(int i = 0; i < 8; i++){if(StationListeners.ContainsKey (i))StationListeners[i].EndTutorialOutro();}
			break;
		case "GetReady" :
			for(int i = 0; i < 8; i++){if(StationListeners.ContainsKey (i)){

					StationListeners[i].GetReady.SetTrigger("Start");
				} } 
			break;

		case "ScoreTimerStart":

               
			for(int i = 0; i < 8; i++){if(StationListeners.ContainsKey (i))
				{
					StationListeners[i].ScoreTimerObject.gameObject.SetActive (true);
					StationListeners[i]._ScoreTimerScript.isVisible = true;	} } 
			break;
		default:
			break;
		}       
	}*/
	//ScoreTimerObject.gameObject.SetActive (true);
	//_ScoreTimerScript.MakeVisible ();
	
	internal void AddStation(int stationNum,StationListener stationListener) {
		//print ("Addstation");
		StationListeners.Add(stationNum, stationListener);
    }

	
	internal void RemoveStation(int stationNum) {
		StationListeners.Remove(stationNum);
	}


	/*
	 *	Helpers
	 */

	// Returns StationListener based on <0..6> id
	public StationListener getStationListenerById (int stationId) {
		return StationListeners[stationId];
	}

	public void Shuffle<Placement> (List<Placement> list) {
		int n = list.Count;
		while (n > 1) {
			n--;
			int k = UnityEngine.Random.Range(0, n + 1);
			Placement value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}


	public enum GestureNums : int {
		UpLeft,
		UpRight,
		StraightLeft,
		StraightRight,
		CurlRight,
		CurlLeft,
		BigWaveLeft,
		BigWaveRight,
		TwoHandsWaveLeft,
		TwoHandsWaveRight,
		TwoHandsUp,
		TwoHandsDown,
		PowerUp,
		PowerDown
	}

	public enum StationStates : int {   
		//Inactive,
		Idle,
		Welcome,
		Playing,
		FinalScore,
		GoodBye
	}
	
	public enum Placement {
		HighLeftQuad,
		HighRightQuad,
		LowLeftQuad,
		LowRightQuad,
		HighMiddleHalf,
		LowMiddleHalf,
		Center,
		CenterLeft,
		CenterRight
	}
}


