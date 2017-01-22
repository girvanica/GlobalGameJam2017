using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Difficulty : MonoBehaviour {

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
		PlayerPrefs.SetInt ("Difficulty", 0);
		SceneManager.LoadScene ("Level1");
	}

	void SadButtonClicked(){
		PlayerPrefs.SetInt ("Difficulty", 1);
		SceneManager.LoadScene ("Level1");
	}

	void ExistentialDespairButtonButtonClicked(){
		PlayerPrefs.SetInt ("Difficulty", 2);
		SceneManager.LoadScene ("Level1");
	}
}
