using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public enum Switch
{
    None,
    KnowWhereTreasureIsBuried,
    KnowCodeToLighthouse,
    HasUnlockedLighthouse,
    GameWon
};

public enum Item
{
    None,
    Axe,
    Shovel,
    Ruby,
};

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public UIController activeUIController;

    public List<Switch> switches;
    public List<Item> inventoryItems;
    public Item heldItem = Item.None;

    public Text captionText;

    public AudioSource musicAudioSource;
    public AudioSource transmissionAudioSource;
    public AudioSource clipAudioSource;
    public GameObject interactionCursorActive;

    public bool paused;

    void Start()
    {
        instance = this;
        captionText.gameObject.SetActive(false);
        interactionCursorActive.SetActive(false);
        AddItem(Item.None);
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
            Debug.LogFormat("SetSwitch: {0}", sw);
        }
    }

    public void ClearSwitch(Switch sw)
    {
        switches.RemoveAll(obj => obj.Equals(sw));
        Debug.LogFormat("ClearSwitch: {0}", sw);

    }

    // INVENTORY MANAGEMENT

    public void SwitchToNextHeldItem() {
        int itemIndex = inventoryItems.FindIndex(item => item == heldItem);
        if (itemIndex == -1)
            itemIndex = 0;

        if (itemIndex < inventoryItems.Count - 1)
        {
            itemIndex++;
        } else
        {
            itemIndex = 0;
        }
        heldItem = inventoryItems[itemIndex];
    }

    public void ResetHeldItem()
    {
        heldItem = Item.None;
    }

    public bool PlayerIsHoldingItem(Item item)
    {
        return heldItem == item;
    }

    public bool PlayerHasItem(Item item)
    {
        return inventoryItems.Contains(item);
    }

    public void AddItem(Item item)
    {
        if (!inventoryItems.Contains(item))
        {
            inventoryItems.Add(item);
            Debug.LogFormat("AddItem: {0}", item);
        }
    }

    public void RemoveItem(Item item)
    {
        inventoryItems.RemoveAll(obj => obj.Equals(item));
        Debug.LogFormat("RemoveItem: {0}", item);
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


