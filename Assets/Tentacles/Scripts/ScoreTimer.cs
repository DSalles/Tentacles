using UnityEngine;
using System.Collections;

public class ScoreTimer : MonoBehaviour {
    [Tooltip("Is the score/timer UI visible?")]
    public bool isVisible;
    [Tooltip("What is the player's overall score")]
    public int score;
    [Tooltip("How many seconds are left remaing?")]
    public int secondsLeft;
    [Tooltip("Animator that controls animations for this UI")]
    public Animator animator;

	// Update is called once per frame
	void Update () {
      //  animator.SetBool("isVisible", isVisible);
	}
	public void MakeVisible()
	{
		animator.SetBool ("isVisible", isVisible);
	}
}
