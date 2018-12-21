using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DicesController : MonoBehaviour {

    public delegate void DicesRolledHandler(object sender, int firstDiceValue, int secondDiceValue);
    public event DicesRolledHandler DicesRolled;

    // Array of dice sides sprites to load from Resources folder
    private Sprite[] diceSides;

    // Reference to sprite renderer to change sprites
    private Image diceRenderer1;
    private Image diceRenderer2;

    // Use this for initialization
    private void Start () {

        // Assign Renderer component
        diceRenderer1 = this.transform.GetChild(0).gameObject.GetComponent<Image>();
        diceRenderer2 = this.transform.GetChild(1).gameObject.GetComponent<Image>();

        // Load dice sides sprites to array from DiceSides subfolder of Resources folder
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");
	}

    public void Roll()
    {
        StartCoroutine("RollTheDices");
    }

    // Coroutine that rolls the dice
    private IEnumerator RollTheDices()
    {
        // Variable to contain random dice side number.
        // It needs to be assigned. Let it be 0 initially
        int randomDiceSide1 = 0;
        int randomDiceSide2 = 0;

        // Final side or value that dice reads in the end of coroutine
        int dice1Value = 0;
        int dice2Value = 0;

        // Loop to switch dice sides ramdomly
        // before final side appears. 20 itterations here.
        for (int i = 0; i <= 20; i++)
        {
            // Pick up random value from 0 to 5 (All inclusive)
            randomDiceSide1 = Random.Range(0, 5);
            randomDiceSide2 = Random.Range(0, 5);

            // Set sprite to upper face of dice from array according to random value
            diceRenderer1.sprite = diceSides[randomDiceSide1];
            diceRenderer2.sprite = diceSides[randomDiceSide2];

            // Pause before next itteration
            yield return new WaitForSeconds(0.05f);
        }

        // Assigning final side so you can use this value later in your game
        // for player movement for example
        dice1Value = randomDiceSide1 + 1;
        dice2Value = randomDiceSide2 + 1;

        // Show final dice value in Console

        DicesRolled(this, dice1Value, dice2Value);
    }
}
