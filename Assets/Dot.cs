using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dot : MonoBehaviour
{
    //Variables
    public Sprite clickedSprite;
    public SpriteRenderer spriteRender;
    bool Clicked = false;
    public Text number;
    GameController gameController;
    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }
    public void ChangeImage()
    {
        if (Clicked == false) //Checking if the point has not been clicked
        {
            if (gameController.currentLevelProgression == float.Parse(number.text)-1) //Checking if the correct point in the order is pressed
            {
                Clicked = true;
                spriteRender.sprite = clickedSprite; //Changing the image of the point
                StartCoroutine(FadeOut(2f, number)); //starting the fadeout animation with a set time
                gameController.CurrentLevelLProgression(1); //Adds to the level progression
                Destroy(number, 3); //Destroying the no longer nesecery Text object in the Point prefab
            }
        }
    }
    //Fade out animation
    public IEnumerator FadeOut(float t, Text i)
    {
        while (i.color.a > 0.0f) //while the opacity is stil visible
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t)); //sets a new color value, with the same parameters but a lower opacity
            yield return null;
        }
    }
}
