using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    private GameController gameController;

    public GameObject safeRuby;

    public Transform respawnPoint;

    public UIController bookUIController;
    public UIController scrollUIController;
    public UIController keypadUIController;

    bool keypadProgress;
    string lastKeypadInput;

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
        if (isRunningCutscene)
            return;

        StartCutscene(new List<CutsceneStep>()
        {
            CutsceneStep.Transmission("Day 28. This is most confusing. This chunk of land appeared this morning, approximately around 8 - not that time carries any meaning here. Compared to the other ones it bears no unique features hinting at its origin. I will collect some soil samples for further analysis.", Resources.Load<AudioClip>("Audio/d01")),
        });
    }

    public void InteractWithDD2(InteractableObject obj)
    {
        if (isRunningCutscene)
            return;

        StartCutscene(new List<CutsceneStep>()
        {
            CutsceneStep.Transmission("Day 53: I am unable to explain the endless nature of this waterfall, but it's marvellous. I have found myself zone out, sitting beside it for hours. Based on the contents of the message in the bottle, this island seems to be from the late 17th century. In order not to tamper with the state of this island further, I left the message as I found it.", Resources.Load<AudioClip>("Audio/d02")),
        });
    }

    public void InteractWithDD3(InteractableObject obj)
    {
        if (isRunningCutscene)
            return;

        StartCutscene(new List<CutsceneStep>()
        {
                CutsceneStep.Transmission("Day 33: I originally struggled to reach this chunk of land. Only after felling a few trees and securing robust planks of wood, I managed to reach this part of land. This will make it easier for me to explore further. The wind protection and axe suggest this chunk must have been displaced from deep within the woods. A hiker perhaps?", Resources.Load<AudioClip>("Audio/d03")),
        });
    }

    public void InteractWithDD4(InteractableObject obj)
    {
        if (isRunningCutscene)
            return;

        StartCutscene(new List<CutsceneStep>()
        {
            CutsceneStep.Transmission("Day 87: Last sleep cycle - it doesn't make sense to call it 'night', I dreamt someone came here to rescue me. I woke up with renewed energy, and have spent the last few hours running a new series of tests, attempting to yet again reverse the trend of chunks of land appearing.", Resources.Load<AudioClip>("Audio/d04a")),
            CutsceneStep.Transmission("My leading hypothesis is that my actions reversed the polarity of a quantum link, starting an avalanche of time intersections that seems to transport chunks of land from various parts of the 'normal' timeline. ", Resources.Load<AudioClip>("Audio/d04b")),

        });
    }

    public void InteractWithDD5(InteractableObject obj)
    {
        if (isRunningCutscene)
            return;

        StartCutscene(new List<CutsceneStep>()
        {
                CutsceneStep.Transmission("Day 99: The architecture of this tower must place this chunk of land in the early 13th century. The tower itself is simplistic and featureless, almost as if the builders didn't have time finish construction. Based on my somewhat limited knowledge of medieval history and  architecture, I am assuming its origin point is somewhere in eastern Europe.", Resources.Load<AudioClip>("Audio/d06a")),
                CutsceneStep.Transmission("The barrels within are sealed shut, and I dare not break them open. The panel in the center of the room is most intriguing. My attempts to open it has so far failed. The socket-like hole seems to be shaped for some type of keystone. But what?", Resources.Load<AudioClip>("Audio/d06b")),

        });
    }

    public void InteractWithDD6(InteractableObject obj)
    {
        if (isRunningCutscene)
            return;

        StartCutscene(new List<CutsceneStep>()
        {
                CutsceneStep.Transmission("Day 286: Every day, there are more chunks of land appearing. Just more, and more. I don't know what to do anymore. The loneliness is overwhelming. All my attempts at reversing this has proven futile. If I only had someone to discuss my theories with. Then perhaps...", Resources.Load<AudioClip>("Audio/d06a")),
                CutsceneStep.Transmission("While meeting another human remains my deepest wish, I also need to protect myself in case of an outsider entering this... place… with malicious intent. I think I will use the output from my recent radiometric dating analysis to ensure my security.", Resources.Load<AudioClip>("Audio/d06b")),

        });
    }

    public void InteractWithDD7(InteractableObject obj)
    {
        if (isRunningCutscene)
            return;

        StartCutscene(new List<CutsceneStep>()
        {
                CutsceneStep.Transmission("Day 1 since the incident. This is Doctor Dina Adelson. I am starting a new audio notebook to document this experiment. After numerous failed attempts at establishing a quantum link, I must have finally succeeded.", Resources.Load<AudioClip>("Audio/d07a")),
                CutsceneStep.Transmission("After the flash, everything in a 20 meter radius, myself included, seem to have been displaced to some type of dimension, because this is surely not the Norwegian coast. I will begin measure time using my atomic clock, for however long my generator lasts.", Resources.Load<AudioClip>("Audio/d07b")),
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
        bookUIController.Activate();
        if (!GameController.instance.IsSwitchSet(Switch.KnowWhereTreasureIsBuried))
            GameController.instance.SetSwitch(Switch.KnowWhereTreasureIsBuried);
    }

    public void InteractWithAlchemistSafe(InteractableObject obj)
    {
        if (isRunningCutscene)
            return;

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

    public void InteractWithLighthouseDoor(InteractableObject obj)
    {
        StartCutscene(new List<CutsceneStep>()
        {
            CutsceneStep.DisplayText("Rattle rattle", 0.5f),
        });
    }

    public void InteractWithLighthouseKeypad(InteractableObject obj)
    {
        keypadUIController.Activate();
    }

    public void InteractWithKeypadButton(Button button)
    {
        //1264
        string input = button.name;
        //Debug.LogFormat("Input {0} last={1} progress={2}", input, lastKeypadInput, keypadProgress);
        if (input == "1")
        {
            keypadProgress = true;
            lastKeypadInput = input;
            return;
        }

        if (keypadProgress && lastKeypadInput == "1" && input == "2")
        {
            keypadProgress = true;
            lastKeypadInput = input;
            return;
        }

        if (keypadProgress && lastKeypadInput == "2" && input == "6")
        {
            keypadProgress = true;
            lastKeypadInput = input;
            return;
        }

        if (keypadProgress && lastKeypadInput == "6" && input == "4")
        {
            keypadProgress = true;
            gameController.SetSwitch(Switch.HasUnlockedLighthouse);
            keypadUIController.Dismiss();

            Invoke("StartOutroCutscene", 0.5f);
            return;
        }

        keypadProgress = false;
    }

    void StartOutroCutscene()
    {
        StartCutscene(new List<CutsceneStep>()
        {
            CutsceneStep.DisplayText("Who are you?", 2f),
        });
    }

}
