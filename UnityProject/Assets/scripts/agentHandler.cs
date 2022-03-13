using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class agentHandler : MonoBehaviour
{
    /// <summary>
    /// 
    /// 
    /// DEPRECATED
    /// 
    /// 
    /// </summary>

    

    public generateMaze mazeData;
    public GameObject _agentPrefab;


    private List<GameObject> agents;
    private Vector2Int start;
    private Vector2Int end;

    [System.Obsolete("DEPRECATED IN BUILD")]
    public void createAgent()
    {
        GameObject tmp = Instantiate(_agentPrefab, generateMaze.physicalMaze[start.x][start.y].transform.position, generateMaze.physicalMaze[start.x][start.y].transform.rotation);
        tmp.transform.localScale = new Vector3(mazeData.scale *0.75f, mazeData.scale *0.75f, 0);
        tmp.GetComponent<mlagentsAgentController>().start = start;
        tmp.GetComponent<mlagentsAgentController>().end = end;

    }
    public void createAgent(int _num)
    {
        GenerateStartAndEndLocations();
        for (int i = 0; i < _num; i++)
        {
            GameObject tmp = Instantiate(_agentPrefab, generateMaze.physicalMaze[start.x][start.y].transform.position, generateMaze.physicalMaze[start.x][start.y].transform.rotation);
            tmp.transform.localScale = new Vector3(mazeData.scale * 0.75f, mazeData.scale * 0.75f, 0);
            tmp.GetComponent<mlagentsAgentController>().start = start;
            tmp.GetComponent<mlagentsAgentController>().end = end;
        }
    }
  
    public void GenerateStartAndEndLocations()
    {
        for (int i = 0; i < generateMaze.mazeSize.x; i++)
        {
            for (int j = 0; j < generateMaze.mazeSize.y; j++)
            {
                if (generateMaze.mazeData[i][j] == 2)
                {
                    start = new Vector2Int(i, j);
                }
                else if (generateMaze.mazeData[i][j] == 3)
                {
                    end = new Vector2Int(i, j);
                }
            }
        }
        
    }
}
