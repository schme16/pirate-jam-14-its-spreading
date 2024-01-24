using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	//This is the current state the game is in, e.g. "main menu" "playing" "game over" etc 
	public string state;
	private float tallyCheckTimer;
	public float tallyCheckInterval;
	public TextMeshProUGUI uiIckPercentage;

	// Start is called before the first frame update
	void Start() { }

	// Update is called once per frame
	void Update()
	{
		if (tallyCheckTimer > tallyCheckInterval)
		{
			PaintTarget.TallyScore(true);
			tallyCheckTimer = 0;
			uiIckPercentage.text = $"Ick: {PaintTarget.scores.x:F2}%";
		}
		else
		{
			tallyCheckTimer += Time.deltaTime;
		}
	}
}