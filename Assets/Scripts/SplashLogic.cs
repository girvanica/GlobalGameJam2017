using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashLogic : MonoBehaviour {

	Text flashingText;
	string textToFlash = "Press any key to experience your existential despair";
	string blankText = "";
	bool isBlinking = true;
	float fadeOutDuration = 5f;

	void Start(){
		//get the Text component
		flashingText = GetComponent<Text>();
		//Call coroutine BlinkText on Start
		StartCoroutine(BlinkText());
		//call function to check if it is time to stop the flashing.
		//StartCoroutine(StopBlinking());
	}

	void Update() {
		if (Time.time > fadeOutDuration) {
			Destroy (gameObject);
		}

		if (Input.anyKey) {

			FadeOut ();
		}
	}

	void FadeOut() {
		Color myColor = flashingText.color;
		float ratio = Time.time / fadeOutDuration;
		myColor.a = Mathf.Lerp (1, 0, ratio);
		flashingText.color = myColor;
	}

	//function to blink the text 
	public IEnumerator BlinkText(){
		//blink it forever. You can set a terminating condition depending upon your requirement. Here you can just set the isBlinking flag to false whenever you want the blinking to be stopped.
		while(isBlinking){
			//set the Text's text to blank
			flashingText.text = blankText;
			//display blank text for 0.5 seconds
			yield return new WaitForSeconds(.5f);
			//display “I AM FLASHING TEXT” for the next 0.5 seconds
			flashingText.text = textToFlash;
			yield return new WaitForSeconds(.5f); 
		}
	}

}
