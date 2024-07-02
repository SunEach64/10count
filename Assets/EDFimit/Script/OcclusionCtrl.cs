using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OcclusionCtrl : MonoBehaviour
{
    [Header("半透明オブジェクト")] public GameObject bt;

    private string cTag = "OcclTrigger";
    private MeshRenderer mr;
    private bool cEnter;
    private bool cStay;
    private bool cExit;

    void Start()
    {
        mr = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if(cEnter || cStay)
        {
            mr.enabled = false;
            bt.SetActive(true);
        }
        else if(cExit)
        {
            mr.enabled = true;
            bt.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == cTag)
        {
            cEnter = true;

            cExit = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == cTag)
        {
            cStay = true;

            cExit = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == cTag)
        {
            cExit = true;

            cEnter = false;
            cStay = false;
        }
    }
}
