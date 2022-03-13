using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    /// <summary>
    /// 
    /// 
    /// NOT USED IN BUILD
    /// 
    /// 
    /// </summary>

    public List<GameObject> mazeHolders;
    public Camera mainCamera;
    private int index = 0;

    public void switchMazeUp()
    {
        if (index != mazeHolders.Count - 1)
        {
            mainCamera.transform.position = new Vector3(mazeHolders[index + 1].transform.position.x, mazeHolders[index + 1].transform.position.y, mainCamera.transform.position.z);
            index++; 
        }
        else
        {
            mainCamera.transform.position = new Vector3(mazeHolders[0].transform.position.x, mazeHolders[0].transform.position.y, mainCamera.transform.position.z);
            index = 0;
        }
    }
    public void switchMazeDown()
    {
        if (index != 0)
        {
            mainCamera.transform.position = new Vector3(mazeHolders[index - 1].transform.position.x, mazeHolders[index - 1].transform.position.y, mainCamera.transform.position.z);
            index--;
        }
        else
        {
            mainCamera.transform.position = new Vector3(mazeHolders[mazeHolders.Count-1].transform.position.x, mazeHolders[mazeHolders.Count-1].transform.position.y, mainCamera.transform.position.z);
            index = mazeHolders.Count-1;
        }
    }
}
