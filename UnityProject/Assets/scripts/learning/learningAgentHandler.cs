using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class learningAgentHandler : MonoBehaviour
{
    public learningMazeGeneration mazeData;
    public GameObject _agentPrefab;

    public Text comptimedata;
    public Text compratedata;

    private List<GameObject> agents;
    private List<Vector2Int> start = new List<Vector2Int>(4);
    private List<Vector2Int> end = new List<Vector2Int>(4);

    // created for debug, not in build
    public void createAgent()
    {
        GameObject tmp = Instantiate(_agentPrefab, learningMazeGeneration.physicalMaze[0][start[0].x][start[0].y].transform.position, learningMazeGeneration.physicalMaze[0][start[0].x][start[0].y].transform.rotation);
        tmp.GetComponent<learningMlagentsAgentController>()._start = start;
        tmp.GetComponent<learningMlagentsAgentController>()._end = end;

    }

    // creates a given amount of agents, and scales them accordingly 
    public void createAgent(int _num)
    {
        GenerateStartAndEndLocations();
        for (int i = 0; i < _num; i++)
        {
            GameObject tmp = Instantiate(_agentPrefab, transform.position, transform.rotation);
            tmp.GetComponent<learningMlagentsAgentController>()._start = start;
            tmp.GetComponent<learningMlagentsAgentController>()._end = end;
        }
    }

    // gets the start and end positions
    public void GenerateStartAndEndLocations()
    {
        for (int mz = 0; mz < 17; mz++)
        {
            start.Add(new Vector2Int());
            end.Add(new Vector2Int());
            for (int i = 0; i < learningMazeGeneration.mazeSize[mz].x; i++)
            {
                for (int j = 0; j < learningMazeGeneration.mazeSize[mz].y; j++)
                {
                    if (learningMazeGeneration.mazeData[mz][i][j] == 2)
                    {
                        start[mz] = new Vector2Int(i, j);
                    }
                    else if (learningMazeGeneration.mazeData[mz][i][j] == 3)
                    {
                        end[mz] = new Vector2Int(i, j);
                    }
                }
            } 
        }

    }

    // Methods for UI
    public void Reset()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    void Update()
    {
        if (learningMlagentsAgentController.active)
        {
            comptimedata.text = learningMlagentsAgentController.averageCompute.ToString();
            compratedata.text = learningMlagentsAgentController.completionRate.ToString();
        }

    }
  
}
