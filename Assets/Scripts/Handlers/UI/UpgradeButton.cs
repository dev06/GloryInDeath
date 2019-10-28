using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UpgradeButton : MonoBehaviour
{
    public float cost;
    public TextMeshProUGUI costText;
    private SimpleButtonHandler button;
    private bool _isPurchased;

    void Start ()
    {
        button = GetComponent<SimpleButtonHandler> ();
        Check ();
    }

    public void Check ()
    {
        button = GetComponent<SimpleButtonHandler> ();
        if (button.buttonID == ButtonID.B_UPG_DAMAGE)
        {
            costText.text = PlayerPrefs.HasKey ("UPG_DAMAGE") ? "-" : cost.ToString ();
            _isPurchased = PlayerPrefs.HasKey ("UPG_DAMAGE");
        }

        if (button.buttonID == ButtonID.B_UPG_CRITHIT)
        {
            costText.text = PlayerPrefs.HasKey ("UPG_CRITHIT") ? "-" : cost.ToString ();
            _isPurchased = PlayerPrefs.HasKey ("UPG_CRITHIT");
        }

        if (button.buttonID == ButtonID.B_UPG_HEALTH)
        {
            costText.text = PlayerPrefs.HasKey ("UPG_HEALTH") ? "-" : cost.ToString ();
            _isPurchased = PlayerPrefs.HasKey ("UPG_HEALTH");
        }
    }

    public bool CanPurchase ()
    {
        bool b = GameController.Instance.CurrentGold >= cost;
        if (_isPurchased)
        {
            return false;
        }
        if (b)
        {
            GameController.Instance.CurrentGold -= (int) cost;
            PlayerPrefs.SetInt ("GOLD", GameController.Instance.CurrentGold);
        }
        return b;
    }

}