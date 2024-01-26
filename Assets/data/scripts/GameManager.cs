using System;
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
	public float startTime = 120;
	public float timer = 0;

	
	//UI
	public TextMeshProUGUI uiIckPercentage;
	public TextMeshProUGUI uiTimer;
	public GameObject uiGameRunningScreen;
	public GameObject uiFinishedScreen;
	
	
	
	// Start is called before the first frame update
	void Start()
	{
		StartGame();
	}

	// Update is called once per frame
	void Update()
	{
		if (timer <= startTime)
		{
			timer += Time.deltaTime;

			var ts = TimeSpan.FromSeconds(startTime - timer);

			Debug.Log(startTime - timer);
			uiTimer.text = $"{ts.TotalMinutes:00}:{ts.Seconds:00}";

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

		else
		{
			timer = startTime;
			FinishGame();
		}
	}


	void StartGame()
	{
		timer = 0;
		uiGameRunningScreen.SetActive(true);
		uiFinishedScreen.SetActive(false);
	}

	void FinishGame()
	{
		uiGameRunningScreen.SetActive(false);
		uiFinishedScreen.SetActive(true);
	}
}