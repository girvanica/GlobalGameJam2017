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
    public Button backButton;
    bool controllerConnected;

    // Use this for initialization
    void Start () {

		difficultyCanvas = difficultyCanvas.GetComponent<Canvas> ();

		calmButton = calmButton.GetComponent<Button> ();
		calmButton.onClick.AddListener (CalmButtonClicked);

		sadButton = sadButton.GetComponent<Button> ();
		sadButton.onClick.AddListener (SadButtonClicked);

		existentialDespairButton = existentialDespairButton.GetComponent<Button> ();
		existentialDespairButton.onClick.AddListener (ExistentialDespairButtonButtonClicked);


        backButton = backButton.GetComponent<Button>();
        backButton.onClick.AddListener(BackButtonClicked);

        string[] names = Input.GetJoystickNames();

        controllerConnected = false;

        for (int x = 0; x < names.Length; x++)
        {
            if (names[x].ToUpper().Contains("XBOX"))
            {
                controllerConnected = true;
                calmButton.Select();
            }
        }

    }

    private void Update()
    {

        if (Input.GetButtonDown("Fire3"))
        {
            BackButtonClicked();
        }
    }

 

	void CalmButtonClicked(){
		PlayerPrefs.SetInt ("Difficulty", 1);
		SceneManager.LoadScene ("Level1");
	}

	void SadButtonClicked(){
		PlayerPrefs.SetInt ("Difficulty", 2);
		SceneManager.LoadScene ("Level1");
	}

	void ExistentialDespairButtonButtonClicked(){
		PlayerPrefs.SetInt ("Difficulty", 4);
		SceneManager.LoadScene ("Level1");
	}

    void BackButtonClicked(){
        SceneManager.LoadScene("Menu");
    }
}
