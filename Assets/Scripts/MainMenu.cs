using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public Canvas quitMenu;
	public Button startText;
	public Button exitText;
    UnityEngine.UI.Button yesButton;
    UnityEngine.UI.Button noButton;
    bool controllerConnected;

    void Start()
    {
        quitMenu = quitMenu.GetComponent<Canvas>();
        startText = startText.GetComponent<Button>();
        exitText = exitText.GetComponent<Button>();
        quitMenu.enabled = false;

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
		startText.enabled = false;
		exitText.enabled = false;
        if (controllerConnected && Input.GetButtonDown("Jump"))
        {
            quitMenu.gameObject.SetActive(true);
            quitMenu.enabled = true;
            yesButton = GameObject.Find("Yes").GetComponent<UnityEngine.UI.Button>();
            noButton = GameObject.Find("No").GetComponent<UnityEngine.UI.Button>();
            yesButton.Select();
        }
    }

	public void NoPress() {
		quitMenu.enabled = false;
		startText.enabled = true;
		exitText.enabled = true;
	}

	public void StartGame() {
		SceneManager.LoadScene ("Level1"); 
	}

	public void ExitGame() {
		Application.Quit();
	}
}
