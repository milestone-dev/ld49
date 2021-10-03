using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneStep
{
    public string text;
    public float duration;
    public AudioClip clip;

    public bool waitUntilDone;

    public Transform transform;
    public Vector3 scaleByVector = Vector3.zero;
    public Vector3 moveByVector = Vector3.zero;
    public Vector3 rotateToVector = Vector3.zero;

    public string animationProperty;
    public bool animationValue;

    public InteractableObject interactableObject;
    public bool interactableObjectInteractionEnabled;

    public GameObject removeObject;

    public static CutsceneStep Wait(float duration)
    {
        var action = new CutsceneStep();
        action.duration = duration;
        return action;
    }

    public static CutsceneStep DisplayText(string text, float duration = 0)
    {
        var action = new CutsceneStep();
        action.text = text;
        action.duration = duration;
        return action;
    }

    public static CutsceneStep PlayAudio(AudioClip clip, bool waitUntilDone = false)
    {
        var action = new CutsceneStep();
        action.clip = clip;
        action.waitUntilDone = waitUntilDone;
        if (waitUntilDone)
            action.duration = clip.length;
        return action;
    }

    public static CutsceneStep Transmission(string text, AudioClip clip)
    {
        var action = new CutsceneStep();
        action.text = text;
        action.clip = clip;
        action.duration = clip.length;
        return action;
    }

    public static CutsceneStep ScaleBy(Transform transform, Vector3 scaleByVector, float duration, bool waitUntilDone = false)
    {
        var action = new CutsceneStep();
        action.transform = transform;
        action.scaleByVector = scaleByVector;
        action.duration = duration;
        return action;
    }

    public static CutsceneStep MoveBy(Transform transform, Vector3 moveByVector, float duration, bool waitUntilDone = false)
    {
        var action = new CutsceneStep();
        action.transform = transform;
        action.moveByVector = moveByVector;
        action.duration = duration;
        return action;
    }

    public static CutsceneStep RotateTo(Transform transform, Vector3 rotateToVector, float duration, bool waitUntilDone = false)
    {
        var action = new CutsceneStep();
        action.transform = transform;
        action.rotateToVector = rotateToVector;
        action.duration = duration;
        return action;
    }

    public static CutsceneStep EnableInteractableObject(InteractableObject obj)
    {
        var action = new CutsceneStep();
        action.interactableObject = obj;
        action.interactableObjectInteractionEnabled = true;
        return action;
    }

    public static CutsceneStep DisableInteractableObject(InteractableObject obj)
    {
        var action = new CutsceneStep();
        action.interactableObject = obj;
        action.interactableObjectInteractionEnabled = false;
        return action;
    }

    public static CutsceneStep RemoveObject(GameObject obj)
    {
        var action = new CutsceneStep();
        action.removeObject = obj;
        return action;
    }
}