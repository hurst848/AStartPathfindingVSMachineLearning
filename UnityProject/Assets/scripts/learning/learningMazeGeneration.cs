using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class learningMazeGeneration : MonoBehaviour
{
    public InputField inputMazePath;

    public Camera cam;
    public List<GameObject> mazeHolder;
    public static List<Vector2> mazeSize = new List<Vector2>();
    public List<GameObject> mazePeices;
    public static List<List<List<int>>> mazeData = new List<List<List<int>>>();
    public static List<List<List<GameObject>>> physicalMaze = new List<List<List<GameObject>>>();
    public static List<float> scale = new List<float>();
    public learningAgentHandler ppoAgentHandler;

    void Start()
    {
        for (int i = 0; i < 17; i++)
        {
            mazeSize.Add(new Vector2());
            mazeData.Add(new List<List<int>>());
            physicalMaze.Add(new List<List<GameObject>>());
            scale.Add(0.0f);
        }
        
    }

    public void generateSingleMaze()
    {
        // grab path from typed text in input field
        string path = inputMazePath.text;

        string tmpMazeData0 = System.IO.File.ReadAllText(@path);
        List<int> rawMazeData0 = parseData(tmpMazeData0);

        mazeSize[0] = new Vector2(rawMazeData0[0], rawMazeData0[1]);
        rawMazeData0.RemoveAt(0); rawMazeData0.RemoveAt(0);

        initData(0, rawMazeData0);

        float verticalExtents = cam.orthographicSize;
        float horizontalExtents = verticalExtents * Screen.width / Screen.height;

        scale[0] = verticalExtents / mazeSize[0].y;
        
        GameObject spawner0 = Instantiate(new GameObject(), mazeHolder[0].transform.position, mazeHolder[0].transform.rotation, mazeHolder[0].transform);
        spawner0.transform.position = new Vector3(mazeHolder[0].transform.position.x - ((scale[0] * mazeSize[0].x) / 2), mazeHolder[0].transform.position.y + ((scale[0] * mazeSize[0].y) / 2), 0);


        genPhysMaze(0, ref spawner0);
    }

    // NOT USED IN BUILD, USED AS TRAINING ENVIROMENT
    public void generateLearningMazes()
    {
        string tmpMazeData0 = System.IO.File.ReadAllText(@"Assets\mazeFiles\maze00.txt");
        string tmpMazeData1 = System.IO.File.ReadAllText(@"Assets\mazeFiles\maze01.txt");
        string tmpMazeData2 = System.IO.File.ReadAllText(@"Assets\mazeFiles\mazeH0.txt");
        string tmpMazeData3 = System.IO.File.ReadAllText(@"Assets\mazeFiles\mazeH1.txt");
        string tmpMazeData4 = System.IO.File.ReadAllText(@"Assets\mazeFiles\mazeH2.txt");
        string tmpMazeData5 = System.IO.File.ReadAllText(@"Assets\mazeFiles\mazeH3.txt");
        string tmpMazeData6 = System.IO.File.ReadAllText(@"Assets\mazeFiles\mazeH4.txt");
        string tmpMazeData7 = System.IO.File.ReadAllText(@"Assets\mazeFiles\mazeH5.txt");
        string tmpMazeData8 = System.IO.File.ReadAllText(@"Assets\mazeFiles\mazeH6.txt");
        string tmpMazeData9 = System.IO.File.ReadAllText(@"Assets\mazeFiles\mazeH7.txt");
        string tmpMazeData10 = System.IO.File.ReadAllText(@"Assets\mazeFiles\mazeH8.txt");
        string tmpMazeData11 = System.IO.File.ReadAllText(@"Assets\mazeFiles\mazeH9.txt");
        string tmpMazeData12 = System.IO.File.ReadAllText(@"Assets\mazeFiles\mazeH10.txt");
        string tmpMazeData13 = System.IO.File.ReadAllText(@"Assets\mazeFiles\mazeH11.txt");
        string tmpMazeData14 = System.IO.File.ReadAllText(@"Assets\mazeFiles\mazeH12.txt");
        string tmpMazeData15 = System.IO.File.ReadAllText(@"Assets\mazeFiles\mazeH13.txt");
        string tmpMazeData16 = System.IO.File.ReadAllText(@"Assets\mazeFiles\mazeH14.txt");

        Debug.Log(tmpMazeData0);
        // retreive file and create string containing all data
        List<int> rawMazeData0 = parseData(tmpMazeData0);
        List<int> rawMazeData1 = parseData(tmpMazeData1);
        List<int> rawMazeData2 = parseData(tmpMazeData2);
        List<int> rawMazeData3 = parseData(tmpMazeData3);
        List<int> rawMazeData4 = parseData(tmpMazeData4);
        List<int> rawMazeData5 = parseData(tmpMazeData5);
        List<int> rawMazeData6 = parseData(tmpMazeData6);
        List<int> rawMazeData7 = parseData(tmpMazeData7);
        List<int> rawMazeData8 = parseData(tmpMazeData8);
        List<int> rawMazeData9 = parseData(tmpMazeData9);
        List<int> rawMazeData10 = parseData(tmpMazeData10);
        List<int> rawMazeData11 = parseData(tmpMazeData11);
        List<int> rawMazeData12 = parseData(tmpMazeData12);
        List<int> rawMazeData13 = parseData(tmpMazeData13);
        List<int> rawMazeData14 = parseData(tmpMazeData14);
        List<int> rawMazeData15 = parseData(tmpMazeData15);
        List<int> rawMazeData16 = parseData(tmpMazeData16);
        

        Debug.Log(mazeSize.Count);

        // store the size of the maze in mazeSize and remove it from the data
        mazeSize[0] = new Vector2(rawMazeData0[0], rawMazeData0[1]);
        rawMazeData0.RemoveAt(0); rawMazeData0.RemoveAt(0);
        mazeSize[1] = new Vector2(rawMazeData1[0], rawMazeData1[1]);
        rawMazeData1.RemoveAt(0); rawMazeData1.RemoveAt(0);
        mazeSize[2] = new Vector2(rawMazeData2[0], rawMazeData2[1]);
        rawMazeData2.RemoveAt(0); rawMazeData2.RemoveAt(0);
        mazeSize[3] = new Vector2(rawMazeData3[0], rawMazeData3[1]);
        rawMazeData3.RemoveAt(0); rawMazeData3.RemoveAt(0);
        mazeSize[4] = new Vector2(rawMazeData4[0], rawMazeData4[1]);
        rawMazeData4.RemoveAt(0); rawMazeData4.RemoveAt(0);
        mazeSize[5] = new Vector2(rawMazeData5[0], rawMazeData5[1]);
        rawMazeData5.RemoveAt(0); rawMazeData5.RemoveAt(0);
        mazeSize[6] = new Vector2(rawMazeData6[0], rawMazeData6[1]);
        rawMazeData6.RemoveAt(0); rawMazeData6.RemoveAt(0);
        mazeSize[7] = new Vector2(rawMazeData7[0], rawMazeData7[1]);
        rawMazeData7.RemoveAt(0); rawMazeData7.RemoveAt(0);
        mazeSize[8] = new Vector2(rawMazeData8[0], rawMazeData8[1]);
        rawMazeData8.RemoveAt(0); rawMazeData8.RemoveAt(0);
        mazeSize[9] = new Vector2(rawMazeData9[0], rawMazeData9[1]);
        rawMazeData9.RemoveAt(0); rawMazeData9.RemoveAt(0);
        mazeSize[10] = new Vector2(rawMazeData10[0], rawMazeData10[1]);
        rawMazeData10.RemoveAt(0); rawMazeData10.RemoveAt(0);
        mazeSize[11] = new Vector2(rawMazeData11[0], rawMazeData11[1]);
        rawMazeData11.RemoveAt(0); rawMazeData11.RemoveAt(0);
        mazeSize[12] = new Vector2(rawMazeData12[0], rawMazeData12[1]);
        rawMazeData12.RemoveAt(0); rawMazeData12.RemoveAt(0);
        mazeSize[13] = new Vector2(rawMazeData13[0], rawMazeData13[1]);
        rawMazeData13.RemoveAt(0); rawMazeData13.RemoveAt(0);
        mazeSize[14] = new Vector2(rawMazeData14[0], rawMazeData14[1]);
        rawMazeData14.RemoveAt(0); rawMazeData14.RemoveAt(0);
        mazeSize[15] = new Vector2(rawMazeData15[0], rawMazeData15[1]);
        rawMazeData15.RemoveAt(0); rawMazeData15.RemoveAt(0);
        mazeSize[16] = new Vector2(rawMazeData16[0], rawMazeData16[1]);
        rawMazeData16.RemoveAt(0); rawMazeData16.RemoveAt(0);

        initData(0, rawMazeData0);
        initData(1, rawMazeData1);
        initData(2, rawMazeData2);
        initData(3, rawMazeData3);
        initData(4, rawMazeData4);
        initData(5, rawMazeData5);
        initData(6, rawMazeData6);
        initData(7, rawMazeData7);
        initData(8, rawMazeData8);
        initData(9, rawMazeData9);
        initData(10, rawMazeData10);
        initData(11, rawMazeData11);
        initData(12, rawMazeData12);
        initData(13, rawMazeData13);
        initData(14, rawMazeData14);
        initData(15, rawMazeData15);
        initData(16, rawMazeData16);

        // generate physical maze and scale it all correctly depending on the size of the maze
        float verticalExtents = cam.orthographicSize;
        float horizontalExtents = verticalExtents * Screen.width / Screen.height;

        // generate the scale so it looks nice
        for (int i = 0; i < 17; i++)
        {
            scale[i] = verticalExtents / mazeSize[i].y;

        }

        //generate the maze
        GameObject spawner0 = Instantiate(new GameObject(), mazeHolder[0].transform.position, mazeHolder[0].transform.rotation, mazeHolder[0].transform);
        GameObject spawner1 = Instantiate(new GameObject(), mazeHolder[1].transform.position, mazeHolder[1].transform.rotation, mazeHolder[1].transform);
        GameObject spawner2 = Instantiate(new GameObject(), mazeHolder[2].transform.position, mazeHolder[2].transform.rotation, mazeHolder[2].transform);
        GameObject spawner3 = Instantiate(new GameObject(), mazeHolder[3].transform.position, mazeHolder[3].transform.rotation, mazeHolder[3].transform);
        GameObject spawner4 = Instantiate(new GameObject(), mazeHolder[4].transform.position, mazeHolder[4].transform.rotation, mazeHolder[4].transform);
        GameObject spawner5 = Instantiate(new GameObject(), mazeHolder[5].transform.position, mazeHolder[5].transform.rotation, mazeHolder[5].transform);
        GameObject spawner6 = Instantiate(new GameObject(), mazeHolder[6].transform.position, mazeHolder[6].transform.rotation, mazeHolder[6].transform);
        GameObject spawner7 = Instantiate(new GameObject(), mazeHolder[7].transform.position, mazeHolder[7].transform.rotation, mazeHolder[7].transform);
        GameObject spawner8 = Instantiate(new GameObject(), mazeHolder[8].transform.position, mazeHolder[8].transform.rotation, mazeHolder[8].transform);
        GameObject spawner9 = Instantiate(new GameObject(), mazeHolder[9].transform.position, mazeHolder[9].transform.rotation, mazeHolder[9].transform);
        GameObject spawner10 = Instantiate(new GameObject(), mazeHolder[10].transform.position, mazeHolder[10].transform.rotation, mazeHolder[10].transform);
        GameObject spawner11 = Instantiate(new GameObject(), mazeHolder[11].transform.position, mazeHolder[11].transform.rotation, mazeHolder[11].transform);
        GameObject spawner12 = Instantiate(new GameObject(), mazeHolder[12].transform.position, mazeHolder[12].transform.rotation, mazeHolder[12].transform);
        GameObject spawner13 = Instantiate(new GameObject(), mazeHolder[13].transform.position, mazeHolder[13].transform.rotation, mazeHolder[13].transform);
        GameObject spawner14 = Instantiate(new GameObject(), mazeHolder[14].transform.position, mazeHolder[14].transform.rotation, mazeHolder[14].transform);
        GameObject spawner15 = Instantiate(new GameObject(), mazeHolder[15].transform.position, mazeHolder[15].transform.rotation, mazeHolder[15].transform);
        GameObject spawner16 = Instantiate(new GameObject(), mazeHolder[16].transform.position, mazeHolder[16].transform.rotation, mazeHolder[16].transform);


        spawner0.transform.position = new Vector3(mazeHolder[0].transform.position.x - ((scale[0] * mazeSize[0].x) / 2), mazeHolder[0].transform.position.y + ((scale[0] * mazeSize[0].y) / 2), 0);
        spawner1.transform.position = new Vector3(mazeHolder[1].transform.position.x - ((scale[1] * mazeSize[1].x) / 2), mazeHolder[1].transform.position.y + ((scale[1] * mazeSize[1].y) / 2), 0);
        spawner2.transform.position = new Vector3(mazeHolder[2].transform.position.x - ((scale[2] * mazeSize[2].x) / 2), mazeHolder[2].transform.position.y + ((scale[2] * mazeSize[2].y) / 2), 0);
        spawner3.transform.position = new Vector3(mazeHolder[3].transform.position.x - ((scale[3] * mazeSize[3].x) / 2), mazeHolder[3].transform.position.y + ((scale[3] * mazeSize[3].y) / 2), 0);
        spawner4.transform.position = new Vector3(mazeHolder[4].transform.position.x - ((scale[4] * mazeSize[4].x) / 2), mazeHolder[4].transform.position.y + ((scale[4] * mazeSize[4].y) / 2), 0);
        spawner5.transform.position = new Vector3(mazeHolder[5].transform.position.x - ((scale[5] * mazeSize[5].x) / 2), mazeHolder[5].transform.position.y + ((scale[5] * mazeSize[5].y) / 2), 0);
        spawner6.transform.position = new Vector3(mazeHolder[6].transform.position.x - ((scale[6] * mazeSize[6].x) / 2), mazeHolder[6].transform.position.y + ((scale[6] * mazeSize[6].y) / 2), 0);
        spawner7.transform.position = new Vector3(mazeHolder[7].transform.position.x - ((scale[7] * mazeSize[7].x) / 2), mazeHolder[7].transform.position.y + ((scale[7] * mazeSize[7].y) / 2), 0);
        spawner8.transform.position = new Vector3(mazeHolder[8].transform.position.x - ((scale[8] * mazeSize[8].x) / 2), mazeHolder[8].transform.position.y + ((scale[8] * mazeSize[8].y) / 2), 0);
        spawner9.transform.position = new Vector3(mazeHolder[9].transform.position.x - ((scale[9] * mazeSize[9].x) / 2), mazeHolder[9].transform.position.y + ((scale[9] * mazeSize[9].y) / 2), 0);
        spawner10.transform.position = new Vector3(mazeHolder[10].transform.position.x - ((scale[10] * mazeSize[10].x) / 2), mazeHolder[10].transform.position.y + ((scale[10] * mazeSize[10].y) / 2), 0);
        spawner11.transform.position = new Vector3(mazeHolder[11].transform.position.x - ((scale[11] * mazeSize[11].x) / 2), mazeHolder[11].transform.position.y + ((scale[11] * mazeSize[11].y) / 2), 0);
        spawner12.transform.position = new Vector3(mazeHolder[12].transform.position.x - ((scale[12] * mazeSize[12].x) / 2), mazeHolder[12].transform.position.y + ((scale[12] * mazeSize[12].y) / 2), 0);
        spawner13.transform.position = new Vector3(mazeHolder[13].transform.position.x - ((scale[13] * mazeSize[13].x) / 2), mazeHolder[13].transform.position.y + ((scale[13] * mazeSize[13].y) / 2), 0);
        spawner14.transform.position = new Vector3(mazeHolder[14].transform.position.x - ((scale[14] * mazeSize[14].x) / 2), mazeHolder[14].transform.position.y + ((scale[14] * mazeSize[14].y) / 2), 0);
        spawner15.transform.position = new Vector3(mazeHolder[15].transform.position.x - ((scale[15] * mazeSize[15].x) / 2), mazeHolder[15].transform.position.y + ((scale[15] * mazeSize[15].y) / 2), 0);
        spawner16.transform.position = new Vector3(mazeHolder[16].transform.position.x - ((scale[16] * mazeSize[16].x) / 2), mazeHolder[16].transform.position.y + ((scale[16] * mazeSize[16].y) / 2), 0);


        genPhysMaze(0, ref spawner0);
        genPhysMaze(1, ref spawner1);
        genPhysMaze(2, ref spawner2);
        genPhysMaze(3, ref spawner3);
        genPhysMaze(4, ref spawner4);
        genPhysMaze(5, ref spawner5);
        genPhysMaze(6, ref spawner6);
        genPhysMaze(7, ref spawner7);
        genPhysMaze(8, ref spawner8);
        genPhysMaze(9, ref spawner9);
        genPhysMaze(10, ref spawner10);
        genPhysMaze(11, ref spawner11);
        genPhysMaze(12, ref spawner12);
        genPhysMaze(13, ref spawner13);
        genPhysMaze(14, ref spawner14);
        genPhysMaze(15, ref spawner15);
        genPhysMaze(16, ref spawner16);


    }


    private List<int> parseData(string tmpMazeData)
    {
        List<int> rawMazeData = new List<int>(0);
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
        return rawMazeData;
    }

    private void initData(int _num, List<int> rawMazeData)
    {
        Debug.Log(mazeData.Count);
        for (int i = 0; i < mazeSize[_num].x; i++)
        {
            mazeData[_num].Add(new List<int>());
            physicalMaze[_num].Add(new List<GameObject>());
        }

        int itr = 0;
        for (int i = 0; i < rawMazeData.Count; i++)
        {
            mazeData[_num][itr].Add(rawMazeData[i]);
            physicalMaze[_num][itr].Add(new GameObject());
            if (itr == mazeSize[_num].x - 1)
            {
                itr = 0;
            }
            else
            {
                itr++;
            }
        }
    }

    private void genPhysMaze(int _num, ref GameObject spawner)
    {
        for (int i = 0; i < mazeSize[_num].x; i++)
        {
            for (int j = 0; j < mazeSize[_num].y; j++)
            {
                switch (mazeData[_num][i][j])
                {
                    case 0:
                        physicalMaze[_num][i][j] = Instantiate(mazePeices[0], spawner.transform.position, spawner.transform.rotation, mazeHolder[_num].transform);
                        physicalMaze[_num][i][j].transform.localScale = new Vector3(scale[_num], scale[_num], 1);
                        break;
                    case 1:
                        physicalMaze[_num][i][j] = Instantiate(mazePeices[1], spawner.transform.position, spawner.transform.rotation, mazeHolder[_num].transform);
                        physicalMaze[_num][i][j].transform.localScale = new Vector3(scale[_num], scale[_num], 1);
                        break;
                    case 2:
                        physicalMaze[_num][i][j] = Instantiate(mazePeices[2], spawner.transform.position, spawner.transform.rotation, mazeHolder[_num].transform);
                        physicalMaze[_num][i][j].transform.localScale = new Vector3(scale[_num], scale[_num], 1);
                        break;
                    case 3:
                        physicalMaze[_num][i][j] = Instantiate(mazePeices[3], spawner.transform.position, spawner.transform.rotation, mazeHolder[_num].transform);
                        physicalMaze[_num][i][j].transform.localScale = new Vector3(scale[_num], scale[_num], 1);
                        break;
                    default:
                        break;

                }
                spawner.transform.position = new Vector3(spawner.transform.position.x, spawner.transform.position.y - (scale[_num] * 1.25f), 0);

            }
            spawner.transform.position = new Vector3(spawner.transform.position.x + (scale[_num] * 1.25f), mazeHolder[_num].transform.position.y + ((scale[_num] * mazeSize[_num].y) / 2), 0);

            
            
        }
    }
}
