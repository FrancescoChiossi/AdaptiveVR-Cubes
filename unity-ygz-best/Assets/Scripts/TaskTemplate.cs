//============== Created by Christian Krauter for Bachelor Thesis =============
//
// Template for tasks
//
//=============================================================================
using UnityEngine;

abstract public class TaskTemplate : MonoBehaviour {
    abstract public void StartTask();
    abstract public string StopTask();
    abstract public void MoveObject();
    abstract public void saveOnAction(string action);
}
