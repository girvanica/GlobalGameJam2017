using UnityEngine;

public class PauseMenu : MonoBehaviour {

	public Canvas quitMenu;

	// Use this for initialization
	void Start () {
		quitMenu = quitMenu.GetComponent<Canvas> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ExitGame() {
		Application.Quit();
	}
}
