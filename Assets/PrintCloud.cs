using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PrintCloud : MonoBehaviour
{

    public UnityEngine.XR.ARFoundation.ARPointCloudManager ARpoints;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DownloadPoints(){

// do things to call points from ARPoints
string strToSave="";

TextWriter tw = new StreamWriter(Application.persistentDataPath+@"/Points.txt");

foreach (var pointCloud in ARpoints.trackables){
Debug.Log ("Lit" +pointCloud.transform.localPosition);

 var visualize  =GameObject.FindWithTag("allPoints").GetComponent<UnityEngine.XR.ARFoundation.ARAllPointCloudPointsParticleVisualizer>();
  foreach (var kvp in visualize.m_Points)
      {
          
          strToSave =      kvp.Value.ToString("F4");
tw.Write(strToSave);
tw.Write("\n");
                     
     }
// for loop that iterates though native slice

//for ()


}

tw.Close();

    }
}
