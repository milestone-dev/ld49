using System.Collections;
using System.Collections.Generic;
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
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateInteractionCrosshair()
    {

    }

    public void DeactivateInteractionCrosshair()
    {

    }
}


