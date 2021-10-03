using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    private GameController gameController;

    public GameObject safeRuby;

    public Transform respawnPoint;

    bool isRunningCutscene;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GetComponent<GameController>();
        instance = this;
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

    IEnumerator RotationTransform(Transform transform, Vector3 rotation, float duration)
    {
        Quaternion initialQuaternion = transform.localRotation;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        for (float time = 0; time < duration; time += Time.deltaTime)
        {
            float progress = time / duration;
            transform.localRotation = Quaternion.Lerp(initialQuaternion, targetRotation, progress);
            yield return null;
        }
    }

    private void StartCutscene(List<CutsceneStep> steps)
    {
        if (isRunningCutscene)
        {
            Debug.Log("Attempting to start cutscene before the previous one finished");
            return;
        }
        isRunningCutscene = true;
        StartCoroutine(ExecuteCutscene(steps));
    }

    private void EndCutscene()
    {
        isRunningCutscene = false;
        gameController.RestoreMusicAudio();
        Debug.Log("EndCutscene");
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
                if (step.rotateToVector != Vector3.zero)
                    StartCoroutine(RotationTransform(step.transform, step.rotateToVector, step.duration));
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

    public void InteractWithVoid(InteractableObject obj)
    {
        //Debug.Log("Fall out of earth");
        PlayerController.instance.MoveTo(respawnPoint.position);
    }

    public void InteractWithTree(InteractableObject obj)
    {
        if (GameController.instance.PlayerIsHoldingItem(Item.Axe))
        {
            PlayerController.instance.PutBackHeldItem();
            StartCoroutine(RotationTransform(obj.transform, new Vector3(60, 90, 6), 1));
        } else
        {
            StartCutscene(new List<CutsceneStep>()
            {
                CutsceneStep.RotateTo(obj.transform, new Vector3(10, 90, 6), 0.2f, true),
                CutsceneStep.RotateTo(obj.transform, new Vector3(0, 90, 6), 0.2f, true),
            });
        }
    }

    public void InteractWithDD1(InteractableObject obj)
    {
        StartCutscene(new List<CutsceneStep>()
        {
                CutsceneStep.DisplayText("Data Disc 1", 2f),
        });
    }

    public void InteractWithDD2(InteractableObject obj)
    {
        StartCutscene(new List<CutsceneStep>()
        {
                CutsceneStep.DisplayText("Data Disc 2", 2f),
        });
    }

    public void InteractWithDD3(InteractableObject obj)
    {
        StartCutscene(new List<CutsceneStep>()
        {
                CutsceneStep.DisplayText("Data Disc 3", 2f),
        });
    }

    public void InteractWithDD4(InteractableObject obj)
    {
        StartCutscene(new List<CutsceneStep>()
        {
                CutsceneStep.DisplayText("Data Disc 4", 2f),
        });
    }

    public void InteractWithDD5(InteractableObject obj)
    {
        StartCutscene(new List<CutsceneStep>()
        {
                CutsceneStep.DisplayText("Data Disc 5", 2f),
        });
    }

    public void InteractWithDD6(InteractableObject obj)
    {
        StartCutscene(new List<CutsceneStep>()
        {
                CutsceneStep.DisplayText("Data Disc 6", 2f),
        });
    }

    public void InteractWithDD7(InteractableObject obj)
    {
        StartCutscene(new List<CutsceneStep>()
        {
                CutsceneStep.DisplayText("Data Disc 7", 2f),
        });
    }

    public void InteractWithAxe(InteractableObject obj)
    {
        Destroy(obj.gameObject);
        GameController.instance.AddItem(Item.Axe);
        PlayerController.instance.SwitchToItem(Item.Axe);
    }

    public void InteractWithShovel(InteractableObject obj)
    {
        Destroy(obj.gameObject);
        GameController.instance.AddItem(Item.Shovel);
        PlayerController.instance.SwitchToItem(Item.Shovel);
    }


    public void InteractWithTreasureRock(InteractableObject obj)
    {
        if (GameController.instance.IsSwitchSet(Switch.KnowWhereTreasureIsBuried) && GameController.instance.PlayerIsHoldingItem(Item.Shovel))
        {
            PlayerController.instance.PutBackHeldItem();
            Destroy(obj.gameObject);
        }
    }

    public void InteractWithRuby(InteractableObject obj)
    {
        GameController.instance.AddItem(Item.Ruby);
        Destroy(obj.gameObject);
    }


    public void InteractWithPirateBook(InteractableObject obj)
    {
        if (!GameController.instance.IsSwitchSet(Switch.KnowWhereTreasureIsBuried))
            GameController.instance.SetSwitch(Switch.KnowWhereTreasureIsBuried);
    }

    public void InteractWithAlchemistSafe(InteractableObject obj)
    {
        if (GameController.instance.PlayerIsHoldingItem(Item.Ruby))
        {
            GameController.instance.RemoveItem(Item.Ruby);
            PlayerController.instance.PutBackHeldItem();
            safeRuby.SetActive(true);
            StartCutscene(new List<CutsceneStep>()
            {
                CutsceneStep.MoveBy(obj.transform, new Vector3(-0.2f, 0, 0), 1f),
            });
        }
    }

    public void InteractWithAlchemistScroll(InteractableObject obj)
    {
        if (!GameController.instance.IsSwitchSet(Switch.KnowCodeToLighthouse))
            GameController.instance.SetSwitch(Switch.KnowCodeToLighthouse);
    }

    public void InteractWithLighthouse(InteractableObject obj)
    {
        if (gameController.IsSwitchSet(Switch.HasUnlockedLighthouse))
        {
            StartCutscene(new List<CutsceneStep>()
            {
                CutsceneStep.DisplayText("Scientist: Who are you?", 4f),
                CutsceneStep.Wait(2f),
                CutsceneStep.DisplayText("You win!", 10f),
            });
        } else
        {
            StartCutscene(new List<CutsceneStep>()
            {
                CutsceneStep.DisplayText("You knock but nothing happens. The door is locked", 4f),
            });
        }
    }

}
