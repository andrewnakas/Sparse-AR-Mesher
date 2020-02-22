using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using g3;
using System.IO;
public class points : MonoBehaviour
{    public UnityEngine.XR.ARFoundation.ARPointCloudManager ARpoints;

    public GameObject spheres;
    public  DMesh3 currentMesh;
    public Text faceLog;
    // Start is called before the first frame update
    void Start()
    {

//fpMeshPointsfromTextFileWithaSecondPoints();//fpMeshPoints();
  //visPoints();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void visPoints(){
string toPull = ReadString();
toPull += ReadString1();
string[] linesInFile = toPull.Split('\n');

int counter = 0;
foreach (string line in linesInFile)
{ counter++;
//Debug.Log(line);

if (!string.IsNullOrEmpty(line)){
 Vector3d temp = (Vector3)(StringToVector3(line)); 
Vector3 temper = new Vector3((float)temp.x,(float)temp.y,(float)temp.z);

Instantiate(spheres,temper,Quaternion.identity); 
}
    
    }
    }
     public string ReadString()
    {
        string path = "Assets/Points.txt";

        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path); 
       // Debug.Log(reader.ReadToEnd());
        string str = reader.ReadToEnd();
        reader.Close();
    return str;
    }

         public string ReadString1()
    {
        string path = "Assets/Points1.txt";

        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path); 
       // Debug.Log(reader.ReadToEnd());
        string str = reader.ReadToEnd();
        reader.Close();
    return str;
    }

public void fpMeshPoints(){

      List<Vector3d> points = new List<Vector3d>();
      List<int> tris = new List<int>();
      List<Vector3d> normals = new List<Vector3d>();
              float normal = .01f ;

//string toPull = ReadString();
//string[] linesInFile = toPull.Split('\n');

foreach (var pointCloud in ARpoints.trackables){
//Debug.Log ("Lit" +pointCloud.transform.localPosition);
int counter = 0;
 var visualize  =GameObject.FindWithTag("allPoints").GetComponent<UnityEngine.XR.ARFoundation.ARAllPointCloudPointsParticleVisualizer>();
  foreach (var kvp in visualize.m_Points)
      {
          counter++;
          points.Add(      kvp.Value);
        tris.Add(counter);
tris.Add(counter);
tris.Add(counter);

normals.Add(new Vector3d(normal, normal, normal));
      }
} 


      
      DMesh3 pointSet = DMesh3Builder.Build(points, tris, normals);
     PointAABBTree3 bvtree = new PointAABBTree3(pointSet, true);
      bvtree.FastWindingNumber(Vector3d.Zero); 
// estimate point area based on nearest-neighbour distance
double[] areas = new double[pointSet.MaxVertexID];
foreach (int vid in pointSet.VertexIndices()) {
    bvtree.PointFilterF = (i) => { return i != vid; };   // otherwise vid is nearest to vid!
    int near_vid = bvtree.FindNearestPoint(pointSet.GetVertex(vid));
    double dist = pointSet.GetVertex(vid).Distance(pointSet.GetVertex(near_vid));
    areas[vid] = Circle2d.RadiusArea(dist);
}    
bvtree.FWNAreaEstimateF = (vid) => {
    return areas[vid];
};
 MarchingCubes mc = new MarchingCubes();
mc.Implicit = new PWNImplicit() { Spatial = bvtree };
mc.IsoValue = 0.0;
mc.CubeSize = bvtree.Bounds.MaxDim / 10;
mc.Bounds = bvtree.Bounds.Expanded(mc.CubeSize * 3);
mc.RootMode = MarchingCubes.RootfindingModes.Bisection;
mc.Generate();
DMesh3 resultMesh = mc.Mesh;
    g3UnityUtils.SetGOMesh(transform.gameObject, resultMesh);
 /*  MarchingCubes mc = new MarchingCubes();
      mc.Implicit = new PWNImplicit() { Spatial =  bvtree };
      mc.IsoValue = 0.0;
      mc.CubeSize = bvtree.Bounds.MaxDim / 10;
      mc.Bounds =  bvtree.Bounds.Expanded(mc.CubeSize * 3);
      mc.RootMode = MarchingCubes.RootfindingModes.Bisection;
      mc.Generate();
      DMesh3 resultMesh = mc.Mesh;
      g3UnityUtils.SetGOMesh(transform.gameObject, resultMesh);
*/
}
    

public void fpMeshPointsfromTextFileWithaSecondPoints(){
//this is used in the fast points winding scene to optimize algorithm and make sure its working well
//it reads points via text files and then meshes them
      List<Vector3d> points = new List<Vector3d>();
      List<int> tris = new List<int>();
      List<Vector3d> normals = new List<Vector3d>();
              float normal = .01f ;

string toPull = ReadString();
string[] linesInFile = toPull.Split('\n');

int counter = 0;
foreach (string line in linesInFile)
{ counter++;
//Debug.Log(line);

if (!string.IsNullOrEmpty(line)){
points.Add(StringToVector3(line));
tris.Add(counter);
tris.Add(counter);
tris.Add(counter);

normals.Add(new Vector3d(normal, normal, normal));

} 
}
      
      DMesh3 pointSet = DMesh3Builder.Build(points, tris, normals);
     PointAABBTree3 bvtree = new PointAABBTree3(pointSet, true);
      bvtree.FastWindingNumber(Vector3d.Zero); 
// estimate point area based on nearest-neighbour distance
double[] areas = new double[pointSet.MaxVertexID];
foreach (int vid in pointSet.VertexIndices()) {
    bvtree.PointFilterF = (i) => { return i != vid; };   // otherwise vid is nearest to vid!
    int near_vid = bvtree.FindNearestPoint(pointSet.GetVertex(vid));
    double dist = pointSet.GetVertex(vid).Distance(pointSet.GetVertex(near_vid));
    areas[vid] = Circle2d.RadiusArea(dist);
}    
bvtree.FWNAreaEstimateF = (vid) => {
    return areas[vid];
};
 MarchingCubes mc = new MarchingCubes();
mc.Implicit = new PWNImplicit() { Spatial = bvtree };
mc.IsoValue = 0.0;
mc.CubeSize = bvtree.Bounds.MaxDim / 50;
mc.Bounds = bvtree.Bounds.Expanded(mc.CubeSize * 3);
mc.RootMode = MarchingCubes.RootfindingModes.Bisection;
mc.Generate();
DMesh3 resultMesh = mc.Mesh;
    g3UnityUtils.SetGOMesh(transform.gameObject, resultMesh);
 /*  MarchingCubes mc = new MarchingCubes();
      mc.Implicit = new PWNImplicit() { Spatial =  bvtree };
      mc.IsoValue = 0.0;
      mc.CubeSize = bvtree.Bounds.MaxDim / 10;
      mc.Bounds =  bvtree.Bounds.Expanded(mc.CubeSize * 3);
      mc.RootMode = MarchingCubes.RootfindingModes.Bisection;
      mc.Generate();
      DMesh3 resultMesh = mc.Mesh;
      g3UnityUtils.SetGOMesh(transform.gameObject, resultMesh);
*/
//ok now that we meshed the first point set, lets try to take in a second frame of points and then add to orginal dmesh
       points = new List<Vector3d>();
       tris = new List<int>();
       normals = new List<Vector3d>();
              

toPull = ReadString1();
linesInFile = toPull.Split('\n');

counter = 0;
foreach (string line in linesInFile)
{ counter++;
Debug.Log(line);

if (!string.IsNullOrEmpty(line)){
points.Add(StringToVector3(line));
tris.Add(counter);
tris.Add(counter);
tris.Add(counter);

normals.Add(new Vector3d(normal, normal, normal));

} 
}
  pointSet = DMesh3Builder.Build(points, tris, normals);
   bvtree = new PointAABBTree3(pointSet, true);
      bvtree.FastWindingNumber(Vector3d.Zero); 
// estimate point area based on nearest-neighbour distance
areas = new double[pointSet.MaxVertexID];
foreach (int vid in pointSet.VertexIndices()) {
    bvtree.PointFilterF = (i) => { return i != vid; };   // otherwise vid is nearest to vid!
     int near_vid = bvtree.FindNearestPoint(pointSet.GetVertex(vid));
    double dist = pointSet.GetVertex(vid).Distance(pointSet.GetVertex(near_vid));
    areas[vid] = Circle2d.RadiusArea(dist);
}    
bvtree.FWNAreaEstimateF = (vid) => {
    return areas[vid];
};
  mc = new MarchingCubes();
mc.Implicit = new PWNImplicit() { Spatial = bvtree };
mc.IsoValue = 0.0;
mc.CubeSize = bvtree.Bounds.MaxDim / 50;
mc.Bounds = bvtree.Bounds.Expanded(mc.CubeSize * 3);
mc.RootMode = MarchingCubes.RootfindingModes.Bisection;
mc.Generate();
DMesh3 resultMesh1 = mc.Mesh;
      MeshEditor editor = new MeshEditor(resultMesh);

      editor.AppendMesh(resultMesh1, resultMesh.AllocateTriangleGroup());

      g3UnityUtils.SetGOMesh(transform.gameObject, resultMesh1);

  //  g3UnityUtils.SetGOMesh(transform.gameObject, resultMesh);

}
    

    class PWNImplicit : BoundedImplicitFunction3d {
    public PointAABBTree3 Spatial;
    public AxisAlignedBox3d Bounds() { return Spatial.Bounds; }
    public double Value(ref Vector3d pt) {
        return -(Spatial.FastWindingNumber(pt) - 0.5);
    }
}
   public static Vector3d StringToVector3(string sVector)
     {
         Debug.Log (sVector);
         // Remove the parentheses
         if (sVector.StartsWith ("(") && sVector.EndsWith (")")) {
             sVector = sVector.Substring(1, sVector.Length-2);
         }
 
double x;
double y;
double z;

         // split the items
         string[] sArray = sVector.Split(',');

            double.TryParse(sArray[0],out x);
             double.TryParse(sArray[1], out y);
             double.TryParse(sArray[2], out z);
         // store as a Vector3
         Vector3d result = new Vector3d(
             x,y,z);
 
         return result;
     }

     public void takePointsinandAddtoMesh(List<Vector3d> pointers){

        
        
        
          

      List<int> tris = new List<int>();
      List<Vector3d> normals = new List<Vector3d>();
                     float normal = .01f ;





         if (currentMesh == null){
//ok so first mesh is not been created yet so create it from the first frame
int counter = 0;
foreach (Vector3d line in pointers)
{ counter++;
//Debug.Log(line);
tris.Add(counter);
tris.Add(counter);
tris.Add(counter);

normals.Add(new Vector3d(normal, normal, normal));


}
      
      DMesh3 pointSet = DMesh3Builder.Build(pointers, tris, normals);
     PointAABBTree3 bvtree = new PointAABBTree3(pointSet, true);
      bvtree.FastWindingNumber(Vector3d.Zero); 
// estimate point area based on nearest-neighbour distance
double[] areas = new double[pointSet.MaxVertexID];
foreach (int vid in pointSet.VertexIndices()) {
    bvtree.PointFilterF = (i) => { return i != vid; };   // otherwise vid is nearest to vid!
    int near_vid = bvtree.FindNearestPoint(pointSet.GetVertex(vid));
    double dist = pointSet.GetVertex(vid).Distance(pointSet.GetVertex(near_vid));
    areas[vid] = Circle2d.RadiusArea(dist);
}    
bvtree.FWNAreaEstimateF = (vid) => {
    return areas[vid];
};
 MarchingCubes mc = new MarchingCubes();
mc.Implicit = new PWNImplicit() { Spatial = bvtree };
mc.IsoValue = 0.0;
mc.CubeSize = bvtree.Bounds.MaxDim / 10;
mc.Bounds = bvtree.Bounds.Expanded(mc.CubeSize * 3);
mc.RootMode = MarchingCubes.RootfindingModes.Bisection;
mc.Generate();
DMesh3 resultMesh = mc.Mesh;
  //  g3UnityUtils.SetGOMesh(transform.gameObject, resultMesh);
currentMesh = resultMesh;


         } else {

//ok so this is where we are proscessing second mesh
             int counter = 0;
foreach (Vector3d line in pointers)
{ counter++;
//Debug.Log(line);
tris.Add(counter);
tris.Add(counter);
tris.Add(counter);

normals.Add(new Vector3d(normal, normal, normal));


}
      
      DMesh3 pointSet = DMesh3Builder.Build(pointers, tris, normals);
     PointAABBTree3 bvtree = new PointAABBTree3(pointSet, true);
      bvtree.FastWindingNumber(Vector3d.Zero); 
// estimate point area based on nearest-neighbour distance
double[] areas = new double[pointSet.MaxVertexID];
foreach (int vid in pointSet.VertexIndices()) {
    bvtree.PointFilterF = (i) => { return i != vid; };   // otherwise vid is nearest to vid!
    int near_vid = bvtree.FindNearestPoint(pointSet.GetVertex(vid));
    double dist = pointSet.GetVertex(vid).Distance(pointSet.GetVertex(near_vid));
    areas[vid] = Circle2d.RadiusArea(dist);
}    
bvtree.FWNAreaEstimateF = (vid) => {
    return areas[vid];
};
 MarchingCubes mc = new MarchingCubes();
mc.Implicit = new PWNImplicit() { Spatial = bvtree };
mc.IsoValue = 0.0;
mc.CubeSize = bvtree.Bounds.MaxDim / 10;
mc.Bounds = bvtree.Bounds.Expanded(mc.CubeSize * 3);
mc.RootMode = MarchingCubes.RootfindingModes.Bisection;
mc.Generate();
DMesh3 resultMesh = mc.Mesh;
      MeshEditor editor = new MeshEditor(currentMesh);

      editor.AppendMesh(resultMesh, currentMesh.AllocateTriangleGroup());
//suspected its crashing after mesh is over 64000 faces,
faceLog.text = "Vertex Count =  " + transform.gameObject.GetComponent<MeshFilter>().mesh.triangles.Length;

    g3UnityUtils.SetGOMesh(transform.gameObject, currentMesh);

         }



     }
}
