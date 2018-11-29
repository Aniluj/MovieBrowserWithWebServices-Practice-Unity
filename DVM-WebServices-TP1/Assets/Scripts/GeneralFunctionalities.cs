using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralFunctionalities : MonoBehaviour {

    public GameObject[] panelsToActivate;
    public GameObject[] panelsToDeactivate;

	void Start () {
		
	}
	
	void Update () {
		
	}

    public void DeactivateAndActivatePanels()
    {
        if (panelsToDeactivate != null)
        {
            for(int i=0; i<panelsToDeactivate.Length;i++)
            {
                panelsToDeactivate[i].SetActive(false);
            }
        }
        if(panelsToActivate != null)
        {
            for (int i = 0; i < panelsToActivate.Length; i++)
            {
                panelsToActivate[i].SetActive(true);
            }
        }
    }
}
