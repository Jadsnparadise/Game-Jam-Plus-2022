using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Trigger.System2D;
using Unity.VisualScripting;

public class ChunckController : MonoBehaviour
{
    [SerializeField] List<BoxTrigger2D> chuncks;

    private void Update()
    {
        foreach(BoxTrigger2D chunck in chuncks)
        {
            chunck.InTrigger(gameObject);
        }
    }
    private void OnDrawGizmos()
    {
        foreach (BoxTrigger2D chunck in chuncks)
        {
            chunck.DrawTrigger(gameObject);
        }
    }
}

