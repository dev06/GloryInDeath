using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace RabaGames
{
	public class RateInsideAppiOS : MonoBehaviour
	{

#if UNITY_IOS
		[DllImport ("__Internal")]
		private static extern bool _DisplayReviewController ();

		/// <summary>
		/// Displays the review (rate) dialog pop up.
		/// </summary>
		/// <returns><c>true</c>, if a native iOS method was called successfully, <c>false</c> otherwise.</returns>
		public static bool DisplayReviewDialog ()
		{
			if (_DisplayReviewController ())
			{
				return true;
			}
			else
			{
				return false;
			}
		}
#endif
	}

}