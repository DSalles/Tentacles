using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using KinectLink;

public class SessionWindow : EditorWindow {
//	[MenuItem("BLi/Session Control Window")]
//	static void Open () {
//		SessionWindow.GetWindow<SessionWindow>().Show();
//	}

//	public static SessionWindow Instance;

//	bool recording;
//	bool playing;

//	void OnEnable () {
//		Instance = this;
//		Repaint();
//	}

//	void CommandPalette () {
//		GUILayout.BeginHorizontal();
//		if (KinectBodiesReceiver.Instance.playingSession) {
//			//StopPlaybackButton();
//			GUILayout.Label("Muted", EditorStyles.miniLabel);
//		} else {
//			foreach (var pair in KinectBodiesReceiver.Instance.streamStateTable) {
//				if (pair.Value == StreamState.Streaming) {
//					if (GUILayout.Button("Snapshot", EditorStyles.miniButton))
//						KinectBodiesReceiver.Instance.CaptureFullFrame("Misc", KinectBodiesReceiver.GetFormattedTimestamp(), pair.Key);
//					StartRecordingButton(pair.Key);
//				} else if (pair.Value == StreamState.Recording) {
//					if (GUILayout.Button("Snapshot", EditorStyles.miniButton))
//						KinectBodiesReceiver.Instance.CaptureFullFrame("Misc", KinectBodiesReceiver.GetFormattedTimestamp(), pair.Key);
//					StopSessionButton(pair.Key);
//				}
//			}
//		}

//		GUILayout.EndHorizontal();

//		/*
//		if (KinectBodiesReceiver.Instance.CurrentFrame.forwarderState == ForwarderState.Streaming) {
//			if (GUILayout.Button("Capture Full Frame")) {
//				KinectBodiesReceiver.Instance.CaptureFullFrame("Misc", KinectBodiesReceiver.GetFormattedTimestamp());
//			}

//			if (!KinectBodiesReceiver.Instance.playingSession) {
//				PlaySessionButton();
//				StartRecordingButton();
//			} else {
//				GUILayout.Label("Playing session: " + KinectBodiesReceiver.Instance.sessionFilename);
//				StopPlaybackButton();
//			}
//		} else if (KinectBodiesReceiver.Instance.CurrentFrame.forwarderState == ForwarderState.Recording) {
//			if (GUILayout.Button("Capture Full Frame")) {
//				KinectBodiesReceiver.Instance.CaptureFullFrame("", KinectBodiesReceiver.GetFormattedTimestamp());
//			}
//			StopRecordingButton();
//		}
//		*/
//	}

//	void PlaySessionButton () {
//		if (GUILayout.Button("Play Session", EditorStyles.miniButton)) {
//			string filePath = EditorUtility.OpenFilePanel("Open session...", KinectBodiesReceiver.Instance.sessionDirectory, "bsd");
//			if (filePath != "") {
//				KinectBodiesReceiver.Instance.sessionDirectory = Path.GetDirectoryName(filePath);
//				KinectBodiesReceiver.Instance.sessionFilename = Path.GetFileName(filePath);
//				KinectBodiesReceiver.Instance.StartPlayback();
//			}
//		}
//	}

//	void StopPlaybackButton () {
//		if (GUILayout.Button("Stop Playback", EditorStyles.miniButton)) {
//			KinectBodiesReceiver.Instance.StopPlayback();
//		}
//	}

//	void StartRecordingButton (byte kinectId = 0xFF) {
//		EditorGUI.BeginDisabledGroup(KinectBodiesReceiver.Instance.lastPacketTime == 0);
//		if (GUILayout.Button("Start Session", EditorStyles.miniButton)) {
//			KinectBodiesReceiver.Instance.StartSession("Misc", KinectBodiesReceiver.GetFormattedTimestamp(), 0, kinectId);
//		}
//		EditorGUI.EndDisabledGroup();
//	}

//	void StopSessionButton (byte kinectId) {
//		if (GUILayout.Button("Stop Session", EditorStyles.miniButton)) {
//			KinectBodiesReceiver.Instance.StopSession(kinectId);
//		}
//	}

//	//TODO: Show data from multiple kinect sources
//	void StatusPalette () {
//		GUILayout.BeginVertical();
//		{
//			foreach (var pair in KinectBodiesReceiver.Instance.streamStateTable) {
//				GUILayout.Label("Kinect ID " + pair.Key.ToString() + " : " + pair.Value.ToString(), EditorStyles.miniLabel);
//			}
//		}
//		GUILayout.EndVertical();
//	}

//	void PlaybackPalette () {
//		if (!KinectBodiesReceiver.Instance.playingSession) {
//			GUILayout.BeginHorizontal();
//			{
//				PlaySessionButton();
//				EditorGUI.BeginDisabledGroup(true);
//				{
//					EditorGUILayout.Slider(KinectBodiesReceiver.Instance.playbackSessionTime, 0, KinectBodiesReceiver.Instance.playbackSessionDuration, GUILayout.Width(250));
//				}
//				EditorGUI.EndDisabledGroup();
//			}
//			GUILayout.EndHorizontal();
//		} else {
//			GUILayout.BeginHorizontal();
//			{
//				StopPlaybackButton();
//				EditorGUI.BeginDisabledGroup(true);
//				{
//					EditorGUILayout.Slider(KinectBodiesReceiver.Instance.playbackSessionTime, 0, KinectBodiesReceiver.Instance.playbackSessionDuration, GUILayout.Width(250));
//				}
//				EditorGUI.EndDisabledGroup();
//			}
//			GUILayout.EndHorizontal();
//		}
//	}

//	void OnGUI () {
//		if (!Application.isPlaying || KinectBodiesReceiver.Instance == null)
//			return;

//		PlaybackPalette();

//		GUILayout.BeginHorizontal();
//		{
//			StatusPalette();
//			CommandPalette();
//		}
//		GUILayout.EndHorizontal();



//	}

//	void Update () {
//		Repaint();
//	}


	/*
	void OldOnGUI(){
		if (!Application.isPlaying || KinectBodiesReceiver.Instance == null)
			return;

		if (recordingDelayEnabled) {
			float countdown = recordingStartTime - Time.realtimeSinceStartup;
			GUILayout.Label(countdown.ToString());
			if(countdown <= 0){
				KinectBodiesReceiver.Instance.StartRecording(KinectBodiesReceiver.Instance.sessionFilename);
				EditorApplication.Beep();
				recordingDelayEnabled = false;
			}

			Repaint();

			return;
		}
		CommandPalette();

		GUILayout.Label (Path.Combine (KinectBodiesReceiver.Instance.sessionDirectory, KinectBodiesReceiver.Instance.sessionFilename));

		EditorGUI.BeginDisabledGroup (KinectBodiesReceiver.Instance.recordingSession || KinectBodiesReceiver.Instance.playingSession);
		GUILayout.BeginHorizontal ();
		if (GUILayout.Button ("New Session")) {
			string filePath = EditorUtility.SaveFilePanel("Save session as...", KinectBodiesReceiver.Instance.sessionDirectory, "session", "bsd");
			if(filePath != ""){
				KinectBodiesReceiver.Instance.sessionDirectory = Path.GetDirectoryName(filePath);
				KinectBodiesReceiver.Instance.sessionFilename = Path.GetFileName(filePath);
			}
		}

		if (GUILayout.Button ("Open Session")) {
			string filePath = EditorUtility.OpenFilePanel("Open session...", KinectBodiesReceiver.Instance.sessionDirectory, "bsd");
			if(filePath != ""){
				KinectBodiesReceiver.Instance.sessionDirectory = Path.GetDirectoryName(filePath);
				KinectBodiesReceiver.Instance.sessionFilename = Path.GetFileName(filePath);
			}
		}
		GUILayout.EndHorizontal ();
		EditorGUI.EndDisabledGroup ();

		EditorGUI.BeginDisabledGroup (KinectBodiesReceiver.Instance.sessionFilename == "");
		{
			//record buttons
			EditorGUI.BeginDisabledGroup(KinectBodiesReceiver.Instance.playingSession);
				{
				if(!KinectBodiesReceiver.Instance.recordingSession){
					GUILayout.BeginHorizontal();
					if(GUILayout.Button("Start Recording")){
						recordingDelayEnabled = true;
						recordingStartTime = Time.realtimeSinceStartup + recordingDelay;
					}

					recordingDelay = (float)EditorGUILayout.IntSlider("", Mathf.RoundToInt(recordingDelay), 0, 10);

					GUILayout.EndHorizontal();
				}
				else{
					if(GUILayout.Button("Stop Recording")){
						KinectBodiesReceiver.Instance.StopRecording();
					}
				}
			}
			EditorGUI.EndDisabledGroup();

			//playback buttons
			EditorGUI.BeginDisabledGroup(KinectBodiesReceiver.Instance.recordingSession);
			{
				if(!KinectBodiesReceiver.Instance.playingSession){
					if (GUILayout.Button ("Start Playback")) {
						KinectBodiesReceiver.Instance.StartPlayback();
					}
				}
				else{
					if(GUILayout.Button("Stop Playback")){
						KinectBodiesReceiver.Instance.StopPlayback();
					}
				}
			}
			EditorGUI.EndDisabledGroup();
		}
		EditorGUI.EndDisabledGroup ();

		Repaint ();
	}
	 */
}
