using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UpgradeContainer : MonoBehaviour
{
    public Transform panel;
    public TextMeshProUGUI moneyLeftText;
    public UpgradeButton[] upgradeButtons;
    private bool _panelActivated;

    void OnEnable ()
    {
        EventManager.OnButtonClick += OnButtonClick;
    }
    void OnDisable ()
    {
        EventManager.OnButtonClick -= OnButtonClick;
    }

    void Start ()
    {
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            upgradeButtons[i].Check ();
        }
    }

    void Update ()
    {
        moneyLeftText.text = GameController.Instance.CurrentGold.ToString ();
    }

    void OnButtonClick (ButtonID id, SimpleButtonHandler handler)
    {
        switch (id)
        {
            case ButtonID.B_UPG_CRITHIT:
                {
                    if (upgradeButtons[0].CanPurchase ())
                    {
                        EventManager.OnGameEvent (EventID.CHR_UPG_CRITHIT);
                        for (int i = 0; i < upgradeButtons.Length; i++)
                        {
                            upgradeButtons[i].Check ();
                        }
                    }

                    break;
                }

            case ButtonID.B_UPG_DAMAGE:
                {
                    if (upgradeButtons[1].CanPurchase ())
                    {
                        EventManager.OnGameEvent (EventID.CHR_UPG_DAMAGE);
                        for (int i = 0; i < upgradeButtons.Length; i++)
                        {
                            upgradeButtons[i].Check ();
                        }
                    }

                    break;
                }

            case ButtonID.B_UPG_HEALTH:
                {
                    if (upgradeButtons[2].CanPurchase ())
                    {
                        EventManager.OnGameEvent (EventID.CHR_UPG_HEALTH);
                        for (int i = 0; i < upgradeButtons.Length; i++)
                        {
                            upgradeButtons[i].Check ();
                        }
                    }
                    break;
                }

            case ButtonID.B_UPG_ACTIVATE:
                {
                    StopCoroutine ("ITogglePanel");
                    StartCoroutine ("ITogglePanel", !_panelActivated);
                    break;
                }
        }
    }

    IEnumerator ITogglePanel (bool b)
    {
        float _target = 0f;
        RectTransform _rt = panel.GetComponent<RectTransform> ();
        if (b)
        {
            _panelActivated = true;
            _target = -250f;
            while (_rt.anchoredPosition.x > _target)
            {

                _rt.anchoredPosition = Vector3.Lerp (_rt.anchoredPosition, new Vector3 (_target, 0f, 0f), Time.deltaTime * 10f);
                yield return null;
            }
        }
        else
        {
            _panelActivated = false;
            _target = 0f;
            while (_rt.anchoredPosition.x < _target)
            {

                _rt.anchoredPosition = Vector3.Lerp (_rt.anchoredPosition, new Vector3 (_target, 0f, 0f), Time.deltaTime * 10f);
                yield return null;
            }
        }
    }
}