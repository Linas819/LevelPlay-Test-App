using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    Levels gameLevelData;
    public int currentLevelProgression;
    public GameObject spawnedDot, canvas, menu;
    public List<GameObject> point;
    Line scriptLine;
    // Start is called before the first frame update
    void Start()
    {
        ReadData(); //reads data from Json file
        scriptLine = GameObject.FindGameObjectWithTag("GameController").GetComponent<Line>(); //connect variable "script" with the LineRenderer's script
    }

    public void PlayGame(int BTN_Pressed) //menu
    {
        currentLevelProgression = 0; //sets the progression to 0 at the start of every level
        scriptLine.positions = new List<Transform>(); //creates a new list for Transform, in the LineRenderer's Line script, at the start of every level
        point = new List<GameObject>(); //creates a new list for the points GameObject at the start of every level
        switch (BTN_Pressed)
        {
            //cases choose the level
            case 0:
            case 1:
            case 2:
            case 3:
                CreateLevel(BTN_Pressed);
                break;
            default:
                Application.Quit();
                break;
        }
        menu.SetActive(false); //once the level starts, the menus UI is deactivated
    }

    //reading json file
    void ReadData()
    {
        string filePath = Application.persistentDataPath + "/level_data.json"; // the location of the json file in build
        Debug.Log(Application.persistentDataPath);
        string jsonString = File.ReadAllText(filePath); // reading the json file
        gameLevelData = JsonUtility.FromJson<Levels>(jsonString); //attaching the Json file's data to a Levels object vriable
    }
    [System.Serializable]
    public class LevelData
    {
        public string[] level_data; // the coordinates of each level from the Json file
    }
    [System.Serializable]
    public class Levels
    {
        public LevelData[] levels; //the levels taken from the Json file that contain LevelData strings for coordinates
    }
    public void CreateLevel(int currentLevel)
    {
        int currentPoint = 0; //variable used to track the current point ebing added in a level
        for (int i = 0; i < gameLevelData.levels[currentLevel].level_data.Length; i = i + 2)
        {
            point.Add(Instantiate(spawnedDot, new Vector2(float.Parse(gameLevelData.levels[currentLevel].level_data[i]), float.Parse(gameLevelData.levels[currentLevel].level_data[i + 1])*-1), Quaternion.identity));
            scriptLine.PositionsList(point[currentPoint].transform); //points's position added in the LineRenderer's Line script
            point[currentPoint].GetComponentInChildren<Text>().text = (currentPoint+1).ToString(); //seting the numeric value of the boind to a text gameobject
            point[currentPoint].transform.SetParent(canvas.transform, false); //transforming the current point game object, to be the child of the canvas game object.
            currentPoint++;//setting up the new point's index 
        }
    }
    public void CurrentLevelLProgression(int i) // adds to how many points are clicked and draws the line
    {
        currentLevelProgression = currentLevelProgression + i;
        scriptLine.LineSetUp(currentLevelProgression);
    }
    public void EndLevel() //destroys the cirrent points and reactivates the menu UI
    {
        for (int i = 0; i < point.Count; i++)
        {
            Destroy(point[i]);
        }
        menu.SetActive(true);
    }
}
