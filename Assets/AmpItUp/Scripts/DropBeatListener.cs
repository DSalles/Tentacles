using UnityEngine;
using System.Collections;
//using SonicBloom.Koreo;
//using ParticlePlayground;

public class DropBeatListener : MonoBehaviour {
	//public Transform MusicVisualizerSprite;

	public Transform LHAttract;
	public Transform RHAttract;
	public Transform MiddleAttract;

	//public PlaygroundParticlesC PC_0;
	//public PlaygroundParticlesC PC_1;
	//public PlaygroundParticlesC PC_2;
	//public PlaygroundParticlesC PC_3;
	//public PlaygroundParticlesC PC_4;

	//public PlaygroundParticlesC BetweenhandsSystem1;	
	//public PlaygroundParticlesC BetweenhandsSystem2;

	//public PlaygroundParticlesC OneShotExplosion;
	//public PlaygroundParticlesC OneShotSnareExplosion;
	//public PlaygroundParticlesC OneShotSuccessExplosion;
	
//	PlaygroundParticlesC[] PlaygroundParticles;
	private Vector3 InitScale;
	// Use this for initialization
	void Start () {
		//PlaygroundParticles = new PlaygroundParticlesC[5]{PC_0,PC_1, PC_2,PC_3,PC_4};
		//Koreographer.Instance.RegisterForEventsWithTime("DropBeats", DropBeatWithTime);
		//Koreographer.Instance.RegisterForEvents ("DropBeats", DropBeatOncePer);
		//Koreographer.Instance.RegisterForEvents ("SnareBeats", DropSnareOncePer);
	//	InitScale = MusicVisualizerSprite.localScale;
		//MusicVisualizerSprite.GetComponent<SpriteRenderer> ().gameObject.SetActive (false);
	}
	
	public Transform ScaleObject;

	//void DropBeatOncePer (KoreographyEvent koreoEvent)
	//{
	//	if (koreoEvent.HasColorPayload ())
	//	{
	//		if (koreoEvent.GetColorValue() == Color.cyan)
	//		{
	//		//	BetweenhandsSystem1.enabled = true;
	//		//	BetweenhandsSystem2.enabled = true;
	//			MusicVisualizerSprite.GetComponent<SpriteRenderer> ().gameObject.SetActive (true);
	//		}
	//		else if(koreoEvent.GetColorValue() == Color.red)
	//		{
	//		//	BetweenhandsSystem1.enabled = false;
	//		//	BetweenhandsSystem2.enabled = false;
	//			MusicVisualizerSprite.GetComponent<SpriteRenderer> ().gameObject.SetActive (false);
	//		}
	//	}
	//	if (koreoEvent.HasIntPayload ()) 
	//	{
	//	//	var LHExplode = Instantiate (OneShotExplosion);
	//		//LHExplode.GetComponent<FTIE01_ParticleScaler> ().scaleSize = 0.5f;
	//	//	LHExplode.transform.position = LHAttract.transform.position;
	//	//	var RHExplode = Instantiate (OneShotExplosion);
	//		//RHExplode.GetComponent<FTIE01_ParticleScaler> ().scaleSize = 0.5f;
	//	//	RHExplode.transform.position = RHAttract.transform.position;
	//	//	var MidExplode = Instantiate (OneShotExplosion);
	//		//MidExplode.transform.position = MiddleAttract.transform.position;
	//	}
	//}

	//void DropSnareOncePer (KoreographyEvent koreoEvent)
	//{
	//	if (koreoEvent.HasIntPayload ()) 
	//	{
	//		var MidSnareExplode = Instantiate (OneShotSnareExplosion);
	//		MidSnareExplode.transform.position = MiddleAttract.transform.position;
	//	}
	//}
	public IEnumerator ParticleBlast(Vector3 xplosionPlace)
	{
	
		StartCoroutine ("Blast", xplosionPlace);
		yield return new WaitForSeconds (1);
		StopCoroutine ("Blast");
	}
//	IEnumerator Blast(Vector3 xplosionPlace)
//	{
	//	var MidSnareExplode = Instantiate (OneShotSuccessExplosion);
	//	MidSnareExplode.transform.position = xplosionPlace;
	
		float t = 0;
		//while (true) {
		
		//	foreach (PlaygroundParticlesC PC in PlaygroundParticles)
		//		if (PC != null)
		//			PC.scale = Mathf.Lerp(1,.29f, t);
		//	t +=Time.deltaTime;
		//	yield return null;
		//}

	//}
	//void DropBeatWithTime (KoreographyEvent koreoEvent, int sampleTime, int sampleDelta, DeltaSlice deltaSlice)
	//{
	//	if (!koreoEvent.HasCurvePayload())
	//		return;

	//	float curveVal = koreoEvent.GetValueOfCurveAtTime (sampleTime);
	//	//	MusicVisualizerSprite.GetComponent<SpriteRenderer> ().gameObject.SetActive (true);
	//	MusicVisualizerSprite.localScale = new Vector3 (1+koreoEvent.GetValueOfCurveAtTime (sampleTime),1+koreoEvent.GetValueOfCurveAtTime (sampleTime),1+koreoEvent.GetValueOfCurveAtTime (sampleTime));
	//	if(ScaleObject)ScaleObject.localScale = new Vector3 (curveVal, curveVal, curveVal);
	//	foreach( PlaygroundParticlesC PC in PlaygroundParticles)if(PC!=null) PC.scale = curveVal;
	//}
}
