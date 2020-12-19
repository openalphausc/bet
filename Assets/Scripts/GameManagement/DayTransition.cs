using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class DayTransition : MonoBehaviour
{
    // day transition game objects
    public GameObject day0to1;
    public GameObject day1to2;
    public GameObject day2to3;
    public GameObject day3to4;
    public GameObject day4to5;
    public GameObject day5to6;
    public GameObject day6to7;
    // buttons
    public GameObject NextButton;
    public GameObject FinalDayButton;

    // calendar game objects
    public GameObject calendar1;
    public GameObject calendar2;
    public GameObject calendar3;
    public GameObject calendar4;
    public GameObject calendar5;
    public GameObject calendar6;
    public GameObject calendar7;

    public GameObject OpenBarButton;

    public GameObject BarBackground;

    public GameObject EvilBuilding;

    public GameObject buttonSounds;
    public GameObject Day1LetterSubtitles;
    public GameObject Day1BuildingSubtitles;
    public GameObject Day1CalendarSubtitles;
    public GameObject Day1CalendarSubtitles2;
    public GameObject Day1CalendarSubtitles3;
    public GameObject Day2PhoneSubtitles;
    public GameObject Day3StoolsSubtitles;
    public GameObject Day4GiftSubtitles;
    public GameObject Day5LetterSubtitles;
    public GameObject Day6WreckingBallSubtitles;
    public GameObject Day7DoorSubtitles;


    public void Start()
    {
        // all the other days setting hella stuff to not active
        day0to1.SetActive(false);
        day1to2.SetActive(false);
        day2to3.SetActive(false);
        day3to4.SetActive(false);
        day4to5.SetActive(false);
        day5to6.SetActive(false);
        day6to7.SetActive(false);
        NextButton = GameObject.Find("Next...Button");
        NextButton.SetActive(false);
        calendar1.SetActive(false);
        calendar2.SetActive(false);
        calendar3.SetActive(false);
        calendar4.SetActive(false);
        calendar5.SetActive(false);
        calendar6.SetActive(false);
        calendar7.SetActive(false);
        OpenBarButton = GameObject.Find("OpenBarButton");
        OpenBarButton.SetActive(false);
        FinalDayButton = GameObject.Find("FinalDayButton");
        FinalDayButton.SetActive(false);
        BarBackground = GameObject.Find("Day1BarBackground");
        BarBackground.SetActive(false);
        EvilBuilding = GameObject.Find("Day1BBCBuilding");
        EvilBuilding.SetActive(false);


        // text disabling
        Day1LetterSubtitles = GameObject.Find("Day1LetterSubtitles");
        Day1LetterSubtitles.SetActive(false);
        Day1BuildingSubtitles = GameObject.Find("Day1BuildingSubtitles");
        Day1BuildingSubtitles.SetActive(false);
        Day1CalendarSubtitles = GameObject.Find("Day1CalendarSubtitles");
        Day1CalendarSubtitles.SetActive(false);
        Day1CalendarSubtitles2 = GameObject.Find("Day1CalendarSubtitles2");
        Day1CalendarSubtitles2.SetActive(false);
        Day1CalendarSubtitles3 = GameObject.Find("Day1CalendarSubtitles3");
        Day1CalendarSubtitles3.SetActive(false);
        Day2PhoneSubtitles = GameObject.Find("Day2PhoneSubtitles");
        Day2PhoneSubtitles.SetActive(false);
        Day3StoolsSubtitles = GameObject.Find("Day3StoolsSubtitles");
        Day3StoolsSubtitles.SetActive(false);
        Day4GiftSubtitles = GameObject.Find("Day4GiftSubtitles");
        Day4GiftSubtitles.SetActive(false);
        Day5LetterSubtitles = GameObject.Find("Day5LetterSubtitles");
        Day5LetterSubtitles.SetActive(false);
        Day6WreckingBallSubtitles = GameObject.Find("Day6WreckingBallSubtitles");
        Day6WreckingBallSubtitles.SetActive(false);
        Day7DoorSubtitles = GameObject.Find("Day7DoorSubtitles");
        Day7DoorSubtitles.SetActive(false);

        // first day code
        if (dataStorage.currentDay == 0)
        {
            GameObject.Find("OneStarText").SetActive(false);
            GameObject.Find("TwoStarText").SetActive(false);
            GameObject.Find("ThreeStarText").SetActive(false);
            GameObject.Find("TabsheetText").SetActive(false);
            GameObject.Find("TabsheetBackground").SetActive(false);
            Day1LetterSubtitles.SetActive(true);
            BarBackground.SetActive(true);
            day0to1.SetActive(true);
        }
    }

    public void startDayTransition()
    {
        buttonSounds.GetComponent<AudioSource>().Play();
        if (dataStorage.currentDay != 0)
        {
            GameObject.Find("OneStarText").SetActive(false);
            GameObject.Find("TwoStarText").SetActive(false);
            GameObject.Find("ThreeStarText").SetActive(false);
            GameObject.Find("TabsheetText").SetActive(false);
            GameObject.Find("TabsheetBackground").SetActive(false);
            Day1LetterSubtitles.SetActive(false);
        }
        GameObject.Find("NextDayButton").SetActive(false);
        switch (dataStorage.currentDay-1)
        {
            case -1:
                BarBackground.SetActive(false);
                day0to1.SetActive(false);
                Day1LetterSubtitles.SetActive(false);
                EvilBuilding.SetActive(true);
                Day1BuildingSubtitles.SetActive(true);
                break;
            case 0:
                day1to2.SetActive(true);
                Day2PhoneSubtitles.SetActive(true);
                break;
            case 1:
                day2to3.SetActive(true);
                Day3StoolsSubtitles.SetActive(true);
                break;
            case 2:
                day3to4.SetActive(true);
                Day4GiftSubtitles.SetActive(true);
                break;
            case 3:
                day4to5.SetActive(true);
                Day5LetterSubtitles.SetActive(true);
                break;
            case 4:
                day5to6.SetActive(true);
                Day6WreckingBallSubtitles.SetActive(true);
                break;
            case 5:
                day6to7.SetActive(true);
                Day7DoorSubtitles.SetActive(true);
                break;
            default:
                break;
        }
        NextButton.SetActive(true);
    }
    public void startCalendarTransition()
    {
        buttonSounds.GetComponent<AudioSource>().Play();
        switch (dataStorage.currentDay-1)
        {
            case -1:
                EvilBuilding.SetActive(false);
                GameObject.Find("Day1BuildingSubtitles").SetActive(false);
                calendar1.SetActive(true);
                Day1CalendarSubtitles.SetActive(true);
                Day1CalendarSubtitles2.SetActive(true);
                Day1CalendarSubtitles3.SetActive(true);
                GameObject.Find("Next...Button").SetActive(false);
                GameObject.Find("TransitionBackground").SetActive(false);
                OpenBarButton.SetActive(true);
                break;
            case 0:
                day1to2.SetActive(false);
                Day2PhoneSubtitles.SetActive(false);
                calendar2.SetActive(true);
                GameObject.Find("Next...Button").SetActive(false);
                GameObject.Find("TransitionBackground").SetActive(false);
                OpenBarButton.SetActive(true);
                break;
            case 1:
                day2to3.SetActive(false);
                Day3StoolsSubtitles.SetActive(false);
                calendar3.SetActive(true);
                GameObject.Find("Next...Button").SetActive(false);
                GameObject.Find("TransitionBackground").SetActive(false);
                OpenBarButton.SetActive(true);
                break;
            case 2:
                day3to4.SetActive(false);
                Day4GiftSubtitles.SetActive(false);
                calendar4.SetActive(true);
                GameObject.Find("Next...Button").SetActive(false);
                GameObject.Find("TransitionBackground").SetActive(false);
                OpenBarButton.SetActive(true);
                break;
            case 3:
                day4to5.SetActive(false);
                Day5LetterSubtitles.SetActive(false);
                calendar5.SetActive(true);
                GameObject.Find("Next...Button").SetActive(false);
                GameObject.Find("TransitionBackground").SetActive(false);
                OpenBarButton.SetActive(true);
                break;
            case 4:
                day5to6.SetActive(false);
                Day6WreckingBallSubtitles.SetActive(false);
                calendar6.SetActive(true);
                GameObject.Find("Next...Button").SetActive(false);
                GameObject.Find("TransitionBackground").SetActive(false);
                OpenBarButton.SetActive(true);
                break;
            case 5:
                day6to7.SetActive(false);
                Day7DoorSubtitles.SetActive(false);
                calendar7.SetActive(true);
                GameObject.Find("Next...Button").SetActive(false);
                GameObject.Find("TransitionBackground").SetActive(false);
                FinalDayButton.SetActive(true);
                break;
            default:
                break;
        }
    }
}
