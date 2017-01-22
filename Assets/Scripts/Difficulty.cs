using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Difficulty : MonoBehaviour {

	public int difficulty;

	public Canvas difficultyCanvas;
	public Button calmButton;
	public Button sadButton;
	public Button existentialDespairButton;

	// Use this for initialization
	void Start () {

		difficultyCanvas = difficultyCanvas.GetComponent<Canvas> ();

		calmButton = calmButton.GetComponent<Button> ();
		calmButton.onClick.AddListener (CalmButtonClicked);

		sadButton = sadButton.GetComponent<Button> ();
		sadButton.onClick.AddListener (SadButtonClicked);

		existentialDespairButton = existentialDespairButton.GetComponent<Button> ();
		existentialDespairButton.onClick.AddListener (ExistentialDespairButtonButtonClicked);

	}

	void CalmButtonClicked(){
		difficulty = 0;
		SceneManager.LoadScene ("Level1");
	}

	void SadButtonClicked(){
		difficulty = 1;
		SceneManager.LoadScene ("Level1");
	}

	void ExistentialDespairButtonButtonClicked(){
		difficulty = 2;
		SceneManager.LoadScene ("Level1");
	}

	public int GetCurrentDifficulty(){
		return difficulty;
	}
}
