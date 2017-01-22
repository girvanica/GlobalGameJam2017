using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public Canvas quitMenu;
    public Canvas howToPlay;
    public Button startText;
	public Button exitText;
    UnityEngine.UI.Button yesButton;
    UnityEngine.UI.Button noButton;
    UnityEngine.UI.Button okButton;
    
    bool controllerConnected;

    void Start()
    {
        quitMenu = quitMenu.GetComponent<Canvas>();
        howToPlay = howToPlay.GetComponent<Canvas>();
        startText = startText.GetComponent<Button>();
        exitText = exitText.GetComponent<Button>();
        quitMenu.enabled = false;
        howToPlay.enabled = false;

        string[] names = Input.GetJoystickNames();

        controllerConnected = false;

        for (int x = 0; x < names.Length; x++)
        {
            if (names[x].ToUpper().Contains("XBOX")){        
                controllerConnected = true;
                startText.enabled = true;
                startText.Select();
            }
        }
    }
   
	public void ExitPress() {
        quitMenu.enabled = true;
       howToPlay.enabled = false;
        startText.enabled = false;
		exitText.enabled = false;
        if (controllerConnected && Input.GetButtonDown("Jump"))
        {           
            yesButton = GameObject.Find("Yes").GetComponent<UnityEngine.UI.Button>();
            noButton = GameObject.Find("No").GetComponent<UnityEngine.UI.Button>();
            yesButton.Select();
        }
    }

	public void NoPress() {
		quitMenu.enabled = false;
        howToPlay.enabled = false;
        startText.enabled = true;
		exitText.enabled = true;
        if (controllerConnected)
        {
            startText.Select();
        }

    }

	public void StartGame() {
		SceneManager.LoadScene ("Difficulty"); 
	}

	public void ExitGame() {
		Application.Quit();
	}
    public void HowToPlay()
    {
        howToPlay.enabled = true;
        if (controllerConnected && Input.GetButtonDown("Jump"))
        {            
            okButton = GameObject.Find("Ok").GetComponent<UnityEngine.UI.Button>();
            okButton.Select();
        }
    }
}
