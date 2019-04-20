using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RateTest : MonoBehaviour {

	public Text InfoText;

	public void RateButtonClicked()
	{
		// call a native review dialog
		bool dialogShown = RabaGames.RateInsideAppiOS.DisplayReviewDialog ();

		// show info about the result
		InfoText.text = "Result: " + dialogShown;
	}
}
