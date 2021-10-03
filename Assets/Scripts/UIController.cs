using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    bool active;
    public AudioClip activateAudioClip;
    public AudioClip deactivateAudioClip;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        GameController.instance.activeUIController = this;
        active = true;
        //GameController.instance.ClearElementHoverText();
        //GameController.instance.ClearInteractionText();
        Cursor.lockState = CursorLockMode.Confined;
        Input.ResetInputAxes();
        if (activateAudioClip)
            GameController.instance.PlayAudioClip(activateAudioClip);
    }

    public void Dismiss()
    {
        gameObject.SetActive(false);
        GameController.instance.activeUIController = null;
        active = false;
        //GameController.instance.ClearElementHoverText();
        Cursor.lockState = CursorLockMode.Locked;
        Input.ResetInputAxes();
        if (deactivateAudioClip)
            GameController.instance.PlayAudioClip(deactivateAudioClip);
    }

    public void ButtonClicked(Button button)
    {
        //Debug.LogFormat("Button clicked! {0}", button);
    }

    public void ButtonPointerEnter(Button button)
    {
        //GameController.instance.SetElementHoverText(button.name);
    }

    public void ButtonPointerExit(Button button)
    {
        //Debug.LogFormat("ButtonPointerExit {0}", button);
        //GameController.instance.ClearElementHoverText();
    }

    private void Update()
    {
        if (GameController.instance.activeUIController != this)
            return;

        if (active && Input.GetMouseButtonDown(1))
            Dismiss();
    }

}