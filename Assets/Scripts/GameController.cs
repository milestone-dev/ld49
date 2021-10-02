using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public enum Switch
{
    None,
    Test
};

public enum Item
{
    None,
    Axe,
    Key
};

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public UIController activeUIController;

    public List<Switch> switches;
    public List<Item> inventoryItems;

    public Text captionText;

    public AudioSource musicAudioSource;
    public AudioSource transmissionAudioSource;
    public AudioSource clipAudioSource;
    public GameObject interactionCursorActive;

    void Start()
    {
        instance = this;
        captionText.gameObject.SetActive(false);
        interactionCursorActive.SetActive(false);
    }

    // SWITCH MANAGEMENT

    public bool IsSwitchSet(Switch sw)
    {
        return switches.Contains(sw);
    }

    public void SetSwitch(Switch sw)
    {
        if (!switches.Contains(sw))
        {
            switches.Add(sw);
        }
    }

    public void ClearSwitch(Switch sw)
    {
        switches.RemoveAll(obj => obj.Equals(sw));
    }

    // INVENTORY MANAGEMENT

    public bool PlayerHasItem(Item item)
    {
        return inventoryItems.Contains(item);
    }

    public void AddItem(Item item)
    {
        if (!inventoryItems.Contains(item))
        {
            inventoryItems.Add(item);
        }
    }

    public void RemoveItem(Item item)
    {
        inventoryItems.RemoveAll(obj => obj.Equals(item));
    }

    // INTERACTION AND TEXT MANAGEMENT

    public void ActivateInteractionCursor()
    {
        interactionCursorActive.SetActive(true);
    }

    public void DeactivateInteractionCursor()
    {
        interactionCursorActive.SetActive(false);
    }

    public void SetCaptionText(string text)
    {
        captionText.text = text;
        captionText.gameObject.SetActive(true);
    }

    public void ClearCaptionText()
    {
        captionText.gameObject.SetActive(false);
    }

    // AUDIO MANAGEMENT

    public void PlayAudioClip(AudioClip clip)
    {
        clipAudioSource.PlayOneShot(clip);
    }

    public void StopAudio()
    {
        clipAudioSource.Stop();
    }

    public void PlayTransmissionAudioClip(AudioClip clip)
    {
        DuckMusicAudio();
        transmissionAudioSource.PlayOneShot(clip);
    }

    public void StopTransmissionAudio()
    {
        transmissionAudioSource.Stop();
    }

    public void PlayMusicAudioClip(AudioClip clip)
    {
        musicAudioSource.Play();
    }

    public void StopMusicAudio()
    {
        musicAudioSource.Stop();
    }

    public void DuckMusicAudio()
    {
        musicAudioSource.volume = 0.5f;
    }

    public void RestoreMusicAudio()
    {
        musicAudioSource.volume = 1;
    }
}


