using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GetComponent<GameController>();
    }
    private void EndCutscene()
    {
        gameController.RestoreMusicAudio();
        Debug.Log("EndCutscene");
    }

    IEnumerator ScaleTransform(Transform transform, Vector3 upScale, float duration)
    {
        Vector3 initialScale = transform.localScale;
        for (float time = 0; time < duration; time += Time.deltaTime)
        {
            float progress = time / duration;
            transform.localScale = Vector3.Lerp(initialScale, upScale, progress);
            yield return null;
        }
    }

    IEnumerator PositionTransform(Transform transform, Vector3 position, float duration)
    {
        Vector3 initialPosition = transform.localPosition;
        for (float time = 0; time < duration; time += Time.deltaTime)
        {
            float progress = time / duration;
            transform.localPosition = Vector3.Lerp(initialPosition, initialPosition + position, progress);
            yield return null;
        }
    }

    private void StartCutscene(List<CutsceneStep> steps)
    {
        StartCoroutine(ExecuteCutscene(steps));
    }

    private IEnumerator ExecuteCutscene(List<CutsceneStep> steps)
    {
        bool skippable = false; // TODO figure out
        Debug.Log(steps);
        foreach (CutsceneStep step in steps)
        {
            if (step.clip)
            {
                gameController.DuckMusicAudio();
                gameController.PlayAudioClip(step.clip);
            }

            if (step.text != null)
                gameController.SetCaptionText(step.text);

            if (step.transform != null)
            {
                if (step.scaleByVector != Vector3.zero)
                    StartCoroutine(ScaleTransform(step.transform, step.scaleByVector, step.duration));
                if (step.moveByVector != Vector3.zero)
                    StartCoroutine(PositionTransform(step.transform, step.moveByVector, step.duration));
            }

            if (step.interactableObject)
            {
                step.interactableObject.interactionEnabled = step.interactableObjectInteractionEnabled;
            }

            if (step.duration > 0)
            {
                float entryDuration = step.duration;
                float remainingTimeDuration = entryDuration;
                while (remainingTimeDuration > 0)
                {
                    remainingTimeDuration -= Time.deltaTime;
                    if (Input.GetMouseButtonUp(0) && skippable)
                    {
                        if (entryDuration - remainingTimeDuration > 0.25)
                        {
                            EndCutscene();
                            yield break;
                        }
                    }
                    // Yield to return out of the Ienumeration
                    yield return 0;
                }
            }
            gameController.ClearCaptionText();
        }
        EndCutscene();
        yield break;
    }

    public void InteractWithPlaceholderObject(InteractableObject obj)
    {
        StartCutscene(new List<CutsceneStep>()
        {
            CutsceneStep.DisplayText("It works", 4f),
            CutsceneStep.Wait(1f),
            CutsceneStep.DisplayText("Sort of.", 2f)
        });
    }
}
