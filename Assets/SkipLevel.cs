using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipLevel : MonoBehaviour
{
    public Transform playerTransform;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("i"))
        {
            WalkSoundScript.Castle = true;
            playerTransform.position = new Vector3(391+56, 54+10, 45+49);
        }
    }
}
