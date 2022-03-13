using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Obsolete("DEPRECATED IN BUILD")]
public class generateMaze : MonoBehaviour
{
    /// <summary>
    /// 
    /// 
    /// DEPRECATED
    /// 
    /// 
    /// </summary>

    


    public InputField inputMazePath;

    public Camera cam;

    public GameObject mazeHolder;

    public static Vector2 mazeSize = new Vector2(0,0);

    public List<GameObject> mazePeices;

    public static List<List<int>> mazeData = new List<List<int>>(0);

    public static List<List<GameObject>> physicalMaze = new List<List<GameObject>>();

    public agentHandler ppoAgentHandler;

    public float scale;
    public void generate()
    {
        // grab path from typed text in input field
        string path = inputMazePath.text;

        // retreive file and create string containing all data
        string tmpMazeData = System.IO.File.ReadAllText(@path);

        // parse the data into an intermidiary array
        List<int> rawMazeData = new List<int>();
        string placeHolder = "";
        for (int i = 0; i < tmpMazeData.Length; i++)
        {
            if (tmpMazeData[i] != ' ')
            {
                placeHolder += tmpMazeData[i];
            }
            else
            {
                rawMazeData.Add(int.Parse(placeHolder));
                placeHolder = "";
            }

            if (i == tmpMazeData.Length - 1)
            {
                rawMazeData.Add(int.Parse(placeHolder));
                placeHolder = "";
            }
        }

        // store the size of the maze in mazeSize and remove it from the data
        mazeSize.x = rawMazeData[0];
        rawMazeData.RemoveAt(0);
        mazeSize.y = rawMazeData[0];
        rawMazeData.RemoveAt(0);

        for (int i = 0; i < mazeSize.x; i++)
        {
            mazeData.Add(new List<int>());
            physicalMaze.Add(new List<GameObject>());
        }

        int itr = 0;
        for (int i = 0; i < rawMazeData.Count; i++)
        {
            mazeData[itr].Add(rawMazeData[i]);
            physicalMaze[itr].Add(new GameObject());
            if (itr == mazeSize.x - 1)
            {
                itr = 0;
            }
            else
            {
                itr++;
            }
        }

        // generate physical maze and scale it all correctly depending on the size of the maze
        float verticalExtents = cam.orthographicSize;
        float horizontalExtents = verticalExtents * Screen.width /Screen.height;
        
        // generate the scale so it looks nice
    
        scale = verticalExtents / mazeSize.y;

        //scale = (verticalExtents / (mazeSize.x * mazeSize.y)) * 5f ;

        GameObject spawner = Instantiate(new GameObject(), mazeHolder.transform.position, mazeHolder.transform.rotation);

        spawner.transform.position = new Vector3(mazeHolder.transform.position.x - ((scale * mazeSize.x) / 2) , mazeHolder.transform.position.y + ((scale * mazeSize.y) / 2),0);

        for (int i = 0; i < mazeSize.x; i++)
        {
            for (int j = 0; j < mazeSize.y; j++)
            {
                switch (mazeData[i][j])
                {
                    case 0:
                        physicalMaze[i][j] = Instantiate(mazePeices[0], spawner.transform.position, spawner.transform.rotation, mazeHolder.transform);
                        physicalMaze[i][j].transform.localScale = new Vector3(scale, scale,1);
                        break;
                    case 1:
                        physicalMaze[i][j] = Instantiate(mazePeices[1], spawner.transform.position, spawner.transform.rotation, mazeHolder.transform);
                        physicalMaze[i][j].transform.localScale = new Vector3(scale, scale,1);
                        break;
                    case 2:
                        physicalMaze[i][j] = Instantiate(mazePeices[2], spawner.transform.position, spawner.transform.rotation, mazeHolder.transform);
                        physicalMaze[i][j].transform.localScale = new Vector3(scale, scale,1);
                        break;
                    case 3:
                        physicalMaze[i][j] = Instantiate(mazePeices[3], spawner.transform.position, spawner.transform.rotation, mazeHolder.transform);
                        physicalMaze[i][j].transform.localScale = new Vector3(scale, scale,1);
                        break;
                    default:
                        break;
                
                }
                spawner.transform.position = new Vector3(spawner.transform.position.x, spawner.transform.position.y - (scale * 1.25f),0);

            }
            spawner.transform.position = new Vector3(spawner.transform.position.x + (scale * 1.25f), mazeHolder.transform.position.y + ((scale * mazeSize.y) / 2),0);
        }

        ppoAgentHandler.GenerateStartAndEndLocations();
        
    }
    
   
}
//  D:\downloads\MazeFilesForLab9\Lab9TerrainFile1.txt