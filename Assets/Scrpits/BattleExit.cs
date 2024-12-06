using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AC;

public class BattleExit : MonoBehaviour
{
    public ActionList myActionList;

    public void RunAnActionList ()
    {
        myActionList.Interact ();
    }

    void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            Debug.Log("space key was pressed");
            RunAnActionList();
        }
    }
}
