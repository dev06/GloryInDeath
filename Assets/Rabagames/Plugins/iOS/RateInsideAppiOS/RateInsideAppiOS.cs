using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

namespace RabaGames
{
	public class RateInsideAppiOS : MonoBehaviour {

		[DllImport ("__Internal")]
		private static extern bool _DisplayReviewController();

		/// <summary>
		/// Displays the review (rate) dialog pop up.
		/// </summary>
		/// <returns><c>true</c>, if a native iOS method was called successfully, <c>false</c> otherwise.</returns>
		public static bool DisplayReviewDialog()
		{
			if (_DisplayReviewController ()) {
				return true;
			} else {
				return false;
			}
		}
	}
}
