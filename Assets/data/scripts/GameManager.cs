using System;
using Cinemachine;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	//This is the current state the game is in, e.g. "main menu" "playing" "game over" etc 
	public string state;
	public string lastState;
	public float startTime = 120;
	public float timer = 0;
	public float timerOverride = 0;
	public Transform player;
	public Transform ick;
	public CinemachineFreeLook cam;
	private Vector3 startPos;
	private Quaternion startRot;

	//UI
	public TextMeshProUGUI uiTimer;
	public TextMeshProUGUI uiCurrentIckScore;
	public TextMeshProUGUI uiBestIckScoreTimer;
	public GameObject uiGameRunningScreen;
	public GameObject uiMainMenu;
	public GameObject uiFinishedScreen;


	// Start is called before the first frame update
	void Start()
	{
		ResetUI();


		SetState("main menu");
	}

	// Update is called once per frame
	void Update()
	{
		//Runs when state changes
		if (state != lastState)
		{
			lastState = state;

			ResetUI();
			switch (state)
			{
				case "main menu":
					MainMenu();
					break;

				case "start":
					StartGame();
					break;

				case "playing":
					break;

				case "finish":
					FinishGame();
					break;

				case "nope":
					Nope();

					break;
			}
		}


		//Runs each frame
		switch (state)
		{
			case "main menu":
				break;

			case "start":

				if (timer <= startTime)
				{
					timer += Time.deltaTime;

					var ts = TimeSpan.FromSeconds(startTime - timer);

					uiTimer.text = $"{ts.TotalMinutes:00}:{ts.Seconds:00}";
				}

				else
				{
					timer = startTime;
					SetState("finish");
				}

				break;

			case "finish":
				if (Input.GetButtonDown("Restart"))
				{
					SetState("start");
				}

				break;

			case "nope":
				break;
		}
	}

	void ResetUI()
	{
		cam.enabled = false;

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		uiMainMenu.gameObject.SetActive(false);
		uiGameRunningScreen.gameObject.SetActive(false);
		uiFinishedScreen.gameObject.SetActive(false);
	}

	void MainMenu()
	{
		//Hide the cursor
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.Confined;


		uiMainMenu.gameObject.SetActive(true);
	}

	void StartGame()
	{
		ick.gameObject.SetActive(false);
		ick.localPosition = Vector3.zero;
		ick.localRotation = Quaternion.identity;
		ick.gameObject.SetActive(true);

		cam.enabled = true;
		PaintTarget.ClearAllPaint();
		if (timerOverride > 0)
		{
			timer = timerOverride;
			timerOverride = 0;
		}
		else
		{
			timer = 0;
			uiGameRunningScreen.SetActive(true);
		}
	}

	void FinishGame()
	{
		//Hide the cursor
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.Confined;


		PaintTarget.TallyScore(true);
		float pb = PlayerPrefs.GetFloat("highScore");
		if (pb < PaintTarget.scores.x)
		{
			pb = PaintTarget.scores.x;
			PlayerPrefs.SetFloat("highScore", pb);
			PlayerPrefs.Save();
		}

		uiCurrentIckScore.text = $"{PaintTarget.scores.x:F2}%";
		uiBestIckScoreTimer.text = $"{pb:F2}%".ToString();

		uiFinishedScreen.SetActive(true);
	}


	void Nope()
	{
		SetState("start");
		timerOverride = startTime - 0.01f;
	}

	public void SetState(string _state)
	{
		state = _state;
	}
}