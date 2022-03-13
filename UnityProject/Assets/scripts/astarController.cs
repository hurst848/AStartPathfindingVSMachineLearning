using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Diagnostics;
using System.Threading;

public class astarController : MonoBehaviour
{
    // dependencies
    public GameObject _agentPrefab;
    public learningMazeGeneration gmz;
    public Text compTimeText;

    public List<move> path = new List<move>();
    public List<GameObject> physicalPath = new List<GameObject>();

    private GameObject _agent;

    private Vector2Int start;
    private Vector2Int end;

  
    private void spawnAgent()
    {
          _agent = Instantiate(_agentPrefab , learningMazeGeneration.physicalMaze[0][start.x][start.y].transform.position, learningMazeGeneration.physicalMaze[0][start.x][start.y].transform.rotation);
          _agent.transform.localScale = new Vector3(learningMazeGeneration.scale[0]/2, learningMazeGeneration.scale[0]/2, 0);               
    }

    private void moveAgent(int _x, int _y)
    {
        _agent.transform.position = learningMazeGeneration.physicalMaze[0][_x][_y].transform.position;
    }

    public void executeAStar()
    {
        // stopwatch for compute time started
        Stopwatch compTime = new Stopwatch();
        compTime.Start();
        retriveEnds();
        
        // create agent
        spawnAgent();

        // create the open and closed lists
        List<move> openList = new List<move>();
        List<move> closedList = new List<move>();

        // create the starting move and add it to the open list
        move tmp = new move(start.x, start.y, start.x, start.y);
        tmp.cost = calculateCost(tmp);
        openList.Add(tmp);

        move endMove = new move();

        while (openList.Count > 0)
        {
            // get lowest cost node from the open list
            int indxOfLowestCost = 0;
            float lowestCost = int.MaxValue;
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].cost < lowestCost)
                {
                    lowestCost = openList[i].cost;
                    indxOfLowestCost = i;
                }
            }
            move currentMove = openList[indxOfLowestCost];
            moveAgent(currentMove.current.x, currentMove.current.y);
            // if current move == end, break and path complete
            if (currentMove.current == end)
            {
                endMove = currentMove;
                break;
            }
            
            // move current move from the open list to the closed list
            openList.RemoveAt(indxOfLowestCost);
            closedList.Add(currentMove);

            // generate all adjacent moves
            List<move> possibleMoves = new List<move>();
                possibleMoves.Add(new move(currentMove.current.x - 1, currentMove.current.y, currentMove.current.x, currentMove.current.y));
                possibleMoves.Add(new move(currentMove.current.x - 1, currentMove.current.y + 1, currentMove.current.x, currentMove.current.y));
                possibleMoves.Add(new move(currentMove.current.x - 1, currentMove.current.y - 1, currentMove.current.x, currentMove.current.y));
                possibleMoves.Add(new move(currentMove.current.x, currentMove.current.y + 1, currentMove.current.x, currentMove.current.y));
                possibleMoves.Add(new move(currentMove.current.x, currentMove.current.y - 1, currentMove.current.x, currentMove.current.y));
                possibleMoves.Add(new move(currentMove.current.x + 1, currentMove.current.y, currentMove.current.x, currentMove.current.y));
                possibleMoves.Add(new move(currentMove.current.x + 1, currentMove.current.y + 1, currentMove.current.x, currentMove.current.y));
                possibleMoves.Add(new move(currentMove.current.x + 1, currentMove.current.y - 1, currentMove.current.x, currentMove.current.y));
            
            // check all generated possible moves to see if valid, then add them to a list
            List<move> verifiedMoves = new List<move>();
            for (int i = 0; i < possibleMoves.Count; i++)
            {
                if (checkIfValidMove(ref openList,ref closedList,possibleMoves[i]))
                {
                    verifiedMoves.Add(possibleMoves[i]);
                }
            }

            // calculate cost of all of the verified moves and add them to the open list
            for (int i = 0; i < verifiedMoves.Count; i++)
            {
                verifiedMoves[i].cost = calculateCost(verifiedMoves[i]);
                openList.Add(verifiedMoves[i]);
            }
        }

        // trace back the path to the start and save it to the path list
        path.Add(endMove);
        while (true)
        {
            if (path[path.Count-1].current == path[path.Count-1].pointer)
            {
                break;
            }
            else
            {
                Vector2Int nextPath = path[path.Count-1].pointer;
                for (int i =0; i < closedList.Count; i++)
                {
                    if (closedList[i].current == nextPath)
                    {
                        path.Add(closedList[i]);
                        closedList.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        path.Reverse();
        // spawn path sprites to illustrate path
        for (int i = 1; i< path.Count - 1; i++)
        {
            physicalPath.Add(Instantiate(gmz.mazePeices[4], learningMazeGeneration.physicalMaze[0][path[i].current.x][path[i].current.y].transform.position, learningMazeGeneration.physicalMaze[0][path[i].current.x][path[i].current.y].transform.rotation));
            physicalPath[i - 1].transform.localScale = new Vector3(learningMazeGeneration.scale[0],learningMazeGeneration.scale[0],0);
        }

        // stop stopwatch and display compute time
        compTime.Stop();
        TimeSpan ts = compTime.Elapsed;
        string timeElapsed = ts.TotalMilliseconds.ToString() + " ms";
        compTimeText.text = timeElapsed;
    }

    private float calculateCost(move _loc)
    {
        // returns the physical distance between the start and the end combined
        return Mathf.Abs(Vector2.Distance(_loc.current, start)) + Mathf.Abs(Vector2.Distance(_loc.current, start));
    }

    private bool checkIfValidMove(ref List<move> _openList,ref List<move> _closedList,move _move)
    {
        // returns false if not a valid move and true if it is a valid move


        // check to see if it a valid move on the grid
        if (_move.current.x < 0 || _move.current.y < 0 || _move.current.x >= learningMazeGeneration.mazeSize[0].x || _move.current.y >= learningMazeGeneration.mazeSize[0].y)
        {
            return false;
        }
        // check to see if the move is on an obstacle
        if (learningMazeGeneration.mazeData[0][_move.current.x][_move.current.y] == 1)
        {
            return false;
        }
        // check to see if the move is on the open list
        for (int i = 0; i < _openList.Count; i++)
        {
            if (_move.current == _openList[i].current)
            {
                return false;
            }
        }
        // check to see if the move is on the closed list
        for (int i = 0; i < _closedList.Count; i++)
        {
            if (_move.current == _closedList[i].current)
            {
                return false;
            }
        }
        // if point reached, the move is valid and return tru
        return true;
    }

    private void retriveEnds()
    {
        for (int i = 0; i < learningMazeGeneration.mazeSize[0].x; i ++)
        {
            for (int j = 0; j < learningMazeGeneration.mazeSize[0].y; j++)
            {
                if (learningMazeGeneration.mazeData[0][i][j] == 2)
                {
                    start = new Vector2Int(i, j);
                }
                else if (learningMazeGeneration.mazeData[0][i][j] == 3)
                {
                    end = new Vector2Int(i, j);
                }
            }
        }
    }
}

public class move
{
    public move()
    {
        current = new Vector2Int(0,0);
        pointer = new Vector2Int(0, 0);
    }
    public move(int cx, int cy, int px, int py)
    {
        current = new Vector2Int(cx, cy);
        pointer = new Vector2Int(px, py);
    }

    public Vector2Int current;
    public Vector2Int pointer;
    public float cost = 0.00f;
}