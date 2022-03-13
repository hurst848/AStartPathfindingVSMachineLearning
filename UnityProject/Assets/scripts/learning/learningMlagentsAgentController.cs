using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
public class learningMlagentsAgentController : Agent
{
    //public Var
    public List<Vector2Int> _start = new List<Vector2Int>();
    public List<Vector2Int> _end = new List<Vector2Int>();

    public float movementThreshold;
    public float timePerMove;

    //private Var
    private Vector2Int current = new Vector2Int();
    private float movementStep;
    private bool episodeFinished = false;
    private float prevDistance = 0.0f;
    [SerializeField]
    private bool moveComplete = false;
    private int mazeNum = 0;

    // compute time varibles
    private Stopwatch compTime = new Stopwatch();
    [SerializeField]
    public static double averageCompute = 0.00f;

    // completion rate Varibles
    [SerializeField]
    public static float completionRate = 0.00f;
    private static int numCompletedL = 0;
    private static int numAttemptsL = 0;

    public static bool active = false;
    //public methods
    void start()
    {
        
        mazeNum = 0;
        /*
        mazeNum = Random.Range(0, 7);
        
        switch (mazeNum)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                mazeNum = 4;
                break;
            case 3:
                mazeNum = 5;
                break;
            case 4:
                mazeNum = 6;
                break;
            case 5:
                mazeNum = 7;
                break;
            case 6:
                mazeNum = 8;
                break;
        }
        */
        //MaxStep = (int)(learningMazeGeneration.mazeSize[mazeNum].x * learningMazeGeneration.mazeSize[mazeNum].y) * 10;
        UnityEngine.Debug.Log(MaxStep);
        transform.localScale = new Vector2(learningMazeGeneration.scale[mazeNum], learningMazeGeneration.scale[mazeNum]);
        current = _start[mazeNum];
        transform.position = learningMazeGeneration.physicalMaze[mazeNum][_start[mazeNum].x][_start[mazeNum].y].transform.position;
        movementStep = Mathf.Abs(Vector2.Distance(learningMazeGeneration.physicalMaze[mazeNum][0][0].transform.position, learningMazeGeneration.physicalMaze[mazeNum][0][1].transform.position) / 100);
        
    }

    public override void OnEpisodeBegin()
    {
        // update compute time
        active = true;
        compTime.Stop();
        System.TimeSpan ts = compTime.Elapsed;
        averageCompute = (averageCompute + ts.TotalMilliseconds) / 2;

        // update completion rate
        completionRate = (float)numCompletedL / (float)numAttemptsL;
        numAttemptsL++;


        episodeFinished = true;
        moveComplete = true;
        //mazeNum = Random.Range(0, 7);
        /*
        switch (mazeNum)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                mazeNum = 4;
                break;
            case 3:
                mazeNum = 5;
                break;
            case 4:
                mazeNum = 6;
                break;
            case 5:
                mazeNum = 7;
                break;
            case 6:
                mazeNum = 8;
                break;
        }
        */
        //MaxStep = (int)(learningMazeGeneration.mazeSize[mazeNum].x * learningMazeGeneration.mazeSize[mazeNum].y) * 10;
        movementStep = Mathf.Abs(Vector2.Distance(learningMazeGeneration.physicalMaze[mazeNum][0][0].transform.position, learningMazeGeneration.physicalMaze[mazeNum][0][1].transform.position) / 100);
        UnityEngine.Debug.Log("new episode");
        current = _start[mazeNum];
        transform.position = learningMazeGeneration.physicalMaze[mazeNum][_start[mazeNum].x][_start[mazeNum].y].transform.position;
        transform.localScale = new Vector3(learningMazeGeneration.scale[mazeNum], learningMazeGeneration.scale[mazeNum],1);
        StartCoroutine(waitTimeAndReset(timePerMove));
        compTime.Reset();
        compTime.Start();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Mathf.Abs(Vector2.Distance(transform.position, learningMazeGeneration.physicalMaze[mazeNum][_start[mazeNum].x][_start[mazeNum].y].transform.position)));
        sensor.AddObservation(Mathf.Abs(Vector2.Distance(transform.position, learningMazeGeneration.physicalMaze[mazeNum][_end[mazeNum].x][_end[mazeNum].y].transform.position)));
        sensor.AddObservation(_end[mazeNum]);
        sensor.AddObservation(current);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (!episodeFinished)
        {
            Vector2 direction = new Vector2(actions.ContinuousActions[0], actions.ContinuousActions[1]);
            int parsedMove = validateMove(direction);
            if (parsedMove != -1)
            {
                StartCoroutine(moveAgent(parsedMove));
                float currentDistance = Mathf.Abs(Vector2.Distance(current, _end[mazeNum]));
                if (currentDistance <= prevDistance)
                {
                    SetReward(1.0f);
                }
                else
                {
                    SetReward(-0.01f);
                }
                prevDistance = currentDistance;
            }
            else
            {
                moveComplete = true;
                SetReward(-1.0f);
            }
            if (current == _end[mazeNum])
            {
                active = true;
                numCompletedL++;
                episodeFinished = true;
                moveComplete = false;
                SetReward(1000.0f);
                EndEpisode();
            }
        }

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var contActOut = actionsOut.ContinuousActions;
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            contActOut[0] = -1f;
            contActOut[1] = 1f;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            contActOut[0] = -1f;
            contActOut[1] = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            contActOut[0] = -1f;
            contActOut[1] = -1f;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            contActOut[0] = 0;
            contActOut[1] = 1f;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            contActOut[0] = 0;
            contActOut[1] = -1f;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            contActOut[0] = 1f;
            contActOut[1] = 1f;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            contActOut[0] = 1f;
            contActOut[1] = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            contActOut[0] = 1f;
            contActOut[1] = -1f;
        }
        else
        {
            contActOut[0] = 0f;
            contActOut[1] = 0f;
        }
    }

   
    IEnumerator moveAgent(int _dir)
    {
        compTime.Stop();
        float multX = 0;
        float multY = 0;

        switch (_dir)
        {
            case 0:
                multX = -1f;
                multY = 1f;
                current = new Vector2Int(current.x - 1, current.y - 1);
                break;
            case 1:
                multX = -1f;
                multY = 0f;
                current = new Vector2Int(current.x - 1, current.y);
                break;
            case 2:
                multX = -1f;
                multY = -1f;
                current = new Vector2Int(current.x - 1, current.y + 1);
                break;
            case 3:
                multX = 0f;
                multY = 1f;
                current = new Vector2Int(current.x, current.y - 1);
                break;
            case 4:
                multX = 0f;
                multY = -1f;
                current = new Vector2Int(current.x, current.y + 1);
                break;
            case 5:
                multX = 1f;
                multY = 1f;
                current = new Vector2Int(current.x + 1, current.y - 1);
                break;
            case 6:
                multX = 1f;
                multY = 0f;
                current = new Vector2Int(current.x + 1, current.y);
                break;
            case 7:
                multX = 1f;
                multY = -1f;
                current = new Vector2Int(current.x + 1, current.y + 1);
                break;
            default:
                UnityEngine.Debug.LogError("UNREACHABLE CODE DETECTED");//unreachable
                break;
        }
        Vector2 step = new Vector2(movementStep * multX, movementStep * multY);

        for (int i = 0; i < 100; i++)
        {
            if (episodeFinished)
            {
                active = true;
                break;
            }
            transform.position = new Vector3(transform.position.x + step.x, transform.position.y + step.y, transform.position.z);
            yield return new WaitForSeconds(timePerMove / 100);

        }
        if (!episodeFinished)
        {
            moveComplete = true;
        }
        compTime.Start();
        yield return null;
    }

    private int validateMove(Vector2 _move)
    {
        // Return meanings:
        //      -1 = fail, skip movement and punnish
        //     0-7 = move corresponding to diagram above class

        // Convert vector into number between 0 and 7
        int mv = 0;
        if (_move.x < -movementThreshold && _move.y > movementThreshold)
        {
            //top-left
            mv = 0;
        }
        else if (_move.x < -movementThreshold && _move.y < -movementThreshold)
        {
            //bottom-left
            mv = 2;
        }
        else if (_move.x > movementThreshold && _move.y > movementThreshold)
        {
            //top-right
            mv = 5;
        }
        else if (_move.x > movementThreshold && _move.y < -movementThreshold)
        {
            //bottom-right
            mv = 7;
        }
        else if (_move.x < -movementThreshold)
        {
            //left
            mv = 1;
        }
        else if (_move.x > movementThreshold)
        {
            //right
            mv = 6;
        }
        else if (_move.y > movementThreshold)
        {
            //top
            mv = 3;
        }
        else if (_move.y < -movementThreshold)
        {
            //bottom
            mv = 4;
        }

        //check move to see if valid
        switch (mv)
        {
            case 0:
                if (current.x != 0 && current.y != 0)
                {
                    if (learningMazeGeneration.mazeData[mazeNum][current.x - 1][current.y - 1] != 1)
                    {
                        return 0;
                    }
                }
                break;
            case 1:
                if (current.x != 0)
                {
                    if (learningMazeGeneration.mazeData[mazeNum][current.x - 1][current.y] != 1)
                    {
                        return 1;
                    }
                }
                break;
            case 2:
                if (current.x != 0 && current.y != learningMazeGeneration.mazeSize[mazeNum].y - 1)
                {
                    if (learningMazeGeneration.mazeData[mazeNum][current.x - 1][current.y + 1] != 1)
                    {
                        return 2;
                    }
                }
                break;
            case 3:
                if (current.y != 0)
                {
                    if (learningMazeGeneration.mazeData[mazeNum][current.x][current.y - 1] != 1)
                    {
                        return 3;
                    }
                }
                break;
            case 4:
                if (current.y != learningMazeGeneration.mazeSize[mazeNum].y - 1)
                {
                    if (learningMazeGeneration.mazeData[mazeNum][current.x][current.y + 1] != 1)
                    {
                        return 4;
                    }
                }
                break;
            case 5:
                if (current.x != learningMazeGeneration.mazeSize[mazeNum].x - 1 && current.y != 0)
                {
                    if (learningMazeGeneration.mazeData[mazeNum][current.x + 1][current.y - 1] != 1)
                    {
                        return 5;
                    }
                }
                break;
            case 6:
                if (current.x != learningMazeGeneration.mazeSize[mazeNum].x - 1)
                {
                    if (learningMazeGeneration.mazeData[mazeNum][current.x + 1][current.y] != 1)
                    {
                        return 6;
                    }
                }
                break;
            case 7:
                if (current.x != learningMazeGeneration.mazeSize[mazeNum].x - 1 && current.y != learningMazeGeneration.mazeSize[mazeNum].y - 1)
                {
                    if (learningMazeGeneration.mazeData[mazeNum][current.x + 1][current.y + 1] != 1)
                    {
                        return 7;
                    }
                }
                break;
        }


        return -1;
    }

    void FixedUpdate()
    {
        if (moveComplete && !episodeFinished)
        {
            moveComplete = false;
            RequestDecision();
        }
    }

    IEnumerator waitTimeAndReset(float x)
    {
        yield return new WaitForSeconds(x / 100);
        moveComplete = true;
        episodeFinished = false;
        yield return null;
    }
}
