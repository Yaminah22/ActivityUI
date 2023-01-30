using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StarScript : MonoBehaviour
{
    //Creating Fade aimation for central star appearing and disappearing
    public static StarScript Instance 
    { 
        set; 
        get; 
    }
    public Image starResult; //star that will fade in and fade out
    private bool isInTransition;
    private float transition;
    private bool isShowing;
    private float duration;
    public Animator fadeImageAnimator; //animator for movement across the screen of central star
    public Animator fox; //For playing fox animation

    //For displaying level end animation
    public GameObject completeLevelUI;
    public GameObject topbar;
    public GameObject body;

    // Logic for stars filling in star bar
    public Animator[] star;
    //For keeping track of star to be filled
    int count = 0;

    //Sounds
    public AudioSource starSound;

    void Awake()
    {
        Instance = this;
    }

    // Fade IN OUT BASE function
    public void Fade(bool showing, float duration)
    {
        isShowing = showing;
        isInTransition = true;
        this.duration = duration;
        transition = (isShowing) ? 0 : 1;
        starSound.Play();
        StopAllCoroutines();
        StartCoroutine(animating(showing)); //A coroutine for showing animation
    }
    // Star Movement Function
    IEnumerator animating(bool showing)
    {
        if (showing == true)
        {
            fox.SetBool("changeFromIdleState", true);
            //start fox movement
           
            yield return new WaitForSeconds(0.7f);
            //wait for fade-in animation to end
            fadeImageAnimator.SetBool("isStationery", false);
            //movement animation starts
       
            yield return new WaitForSeconds(0.7f);// wait for movement to end
            Fade(false, 0.01f);// fade-out animation
            
            ProgressStars(count);// Set stars in bar according to progress
            fadeImageAnimator.SetBool("isStationery", true);// reset star position
            
            fox.SetBool("changeFromIdleState", false); //Stop Fox movement or switch for to default movement
            if (count == 4)
            {
                levelEnd();
            }
        }
    }
    // Stars Bar Function
    void ProgressStars(int progress_score)
    {
        
        switch (progress_score)
        {
            case 1:
                star[0].SetBool("filled", true);
                break;
            case 2:
                star[1].SetBool("filled", true);
                break;
            case 3:
                star[2].SetBool("filled", true);
                break;
            case 4:
                star[3].SetBool("filled", true);
                break;
            default: 
                Debug.LogWarning("Progress Star Out of Range!");
                break;
        }

    }

    //function to display level complete screen
    private void levelEnd()
    {
        topbar.SetActive(false);
        body.SetActive(false);
        completeLevelUI.SetActive(true);
    }

    //Function which updates the progress
    public void progress()
    {
        Fade(true, 0.5f); //calling fade-in animation
        count += 1;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            progress();
            //Just simply remove this code and call this function from your own logic
        }
        
        // fade animation creation script
        if (!isInTransition) return;
        transition += (isShowing) ? Time.deltaTime * (1 / duration) : -Time.deltaTime * (1 / duration);
        starResult.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, transition);
        if (transition > 1 || transition < 0) isInTransition = false;
    }

}
