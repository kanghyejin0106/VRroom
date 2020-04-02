using UnityEngine;
using System;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Text;

public class CreateRoom : MonoBehaviour
{
    float floorWidth = 3, floorLength = 5, floorHeight = 0.2f;
    float wallThick = 0.1f;
    float roomWidth, roomLength, roomHeight;
    Furniture[] myFurniture;
    ArrayList furniture;
    JObject json;
    public Camera camera;
    private int resWidth;
    private int resHeight;
    string path;
    string id = "001";
    GameObject floor, wall0, wall1, wall2, wall3;
    // Start is called before the first frame update
    void Start()
    {

        //가구 정보들 받기
        getFurnitureInfo();

        //json 파일 가져옴
        getJson();

        //캡처
        Screenshot_();

        //벽
        Createwall();

        //바닥, 벽 obj 저장
        SaveArchObj();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void getJson()
    {

        String temp = System.IO.File.ReadAllText("C:/Users/rkdwj/Desktop/house.json");
        json = new JObject(JObject.Parse(temp));
        id = json["id"].ToString();
        JArray jsonLevels = json["levels"] as JArray;

        //방 크기 저장
        JObject jsonBbox = jsonLevels[0]["bbox"] as JObject;
        floorWidth = int.Parse(jsonBbox["max"][0].ToString());
        roomHeight = int.Parse(jsonBbox["max"][1].ToString());
        floorLength = int.Parse(jsonBbox["max"][2].ToString());

        roomWidth = floorWidth + 2 * wallThick;
        roomLength = floorLength + 2 * wallThick;
        roomHeight = 2.0f + floorHeight;


        //바닥 크기 설정
        floor = GameObject.Find("Floor");
        floor.transform.localScale = new Vector3(floorWidth, floorHeight, floorLength);
        floor.transform.localPosition = new Vector3(roomWidth / 2, floorHeight / 2, roomLength / 2);

        //벽 변수
        wall0 = GameObject.Find("Wall0");
        wall1 = GameObject.Find("Wall1");
        wall2 = GameObject.Find("Wall2");
        wall3 = GameObject.Find("Wall3");


        //json의 가구들 저장
        JArray jsonNodes = jsonLevels[0]["nodes"] as JArray;
        myFurniture = new Furniture[jsonNodes.Count - 1];

        for (int i = 1; i < jsonNodes.Count; i++)
        {
            int modelId = int.Parse(jsonNodes[i]["modelId"].ToString());

            foreach (Furniture f in furniture)
                if (modelId == f.modelID)
                    myFurniture[i - 1] = new Furniture(f.modelID, f.width, f.height, f.length);

            JArray jsonTransform = jsonNodes[i]["transform"] as JArray;
            double[,] tempMatrix = new double[4, 4];
            for (int j = 0; j < 15; j++)
                tempMatrix[j / 4, j % 4] = double.Parse(jsonTransform[j].ToString());

            myFurniture[i - 1].scale = tempMatrix[1, 1];

            ////myFurniture[i - 1].startPoint1 = tempMatrix[3, 0] - myFurniture[i - 1].width * myFurniture[i - 1].scale / 100 / 2;
            //myFurniture[i - 1].startPoint1 = tempMatrix[3, 0];
            //myFurniture[i - 1].startPoint2 = tempMatrix[3, 1];
            ////myFurniture[i - 1].startPoint3 = tempMatrix[3, 2] - myFurniture[i - 1].length * myFurniture[i - 1].scale / 100 / 2;
            //myFurniture[i - 1].startPoint3 = tempMatrix[3, 2];

            //double cos = tempMatrix[0, 0] / myFurniture[i - 1].scale;
            //double sin = -tempMatrix[0, 2] / myFurniture[i - 1].scale;
            //myFurniture[i - 1].rotation = 180 / Math.PI * Math.Atan(sin / cos);
            myFurniture[i - 1].startPoint1 = tempMatrix[3, 0];
            myFurniture[i - 1].startPoint2 = tempMatrix[3, 1];
            myFurniture[i - 1].startPoint3 = tempMatrix[3, 2];

            double cos = tempMatrix[0, 0] / myFurniture[i - 1].scale;
            double sin = -tempMatrix[0, 2] / myFurniture[i - 1].scale;

            myFurniture[i - 1].rotation = 180 / Math.PI * Math.Atan2((double)sin, (double)cos);

            if (sin < 0.001 && sin > -0.001 && cos > -1.001 && cos < -0.999)
                myFurniture[i - 1].rotation = 180;

            //가구 저장하면서 화면에 띄움
            Instantiate(Resources.Load(myFurniture[i - 1].modelID.ToString()),
                new Vector3((float)myFurniture[i - 1].startPoint1, (float)myFurniture[i - 1].startPoint2, (float)myFurniture[i - 1].startPoint3),
                Quaternion.Euler(new Vector3(0, (float)myFurniture[i - 1].rotation, 0)));

            //Debug.Log(i+": "+myFurniture[i - 1].startPoint1 + " " + myFurniture[i - 1].startPoint3 + " " + myFurniture[i - 1].rotation + " " + myFurniture[i - 1].scale);

        }

    }

    public void Createwall()
    {
        wall0.SetActive(true);
        wall1.SetActive(true);
        wall2.SetActive(true);
        wall3.SetActive(true);

        Debug.Log("ssibar: "+roomWidth + " " + roomHeight);
        //wall0.transform.localScale = new Vector3(roomWidth, roomHeight + 0.2f, 0.1f);
        //wall0.transform.localPosition = new Vector3(roomWidth / 2, roomHeight / 2, roomLength / 2 + roomLength / 2);

        //wall1.transform.localScale = new Vector3(0.1f, roomHeight + 0.2f, roomLength + 0.2f);
        //wall1.transform.localPosition = new Vector3(roomWidth / 2 - roomWidth / 2, roomHeight / 2, roomLength / 2);

        //wall2.transform.localScale = new Vector3(roomWidth, roomHeight + 0.2f, 0.1f);
        //wall2.transform.localPosition = new Vector3(roomWidth / 2, roomHeight / 2, roomLength / 2 - roomLength / 2);

        //wall3.transform.localScale = new Vector3(0.1f, roomHeight + 0.2f, roomLength + 0.2f);
        //wall3.transform.localPosition = new Vector3(roomWidth / 2 + roomWidth / 2, roomHeight / 2, roomLength / 2);

        wall0.transform.localScale = new Vector3(roomWidth, roomHeight, wallThick);
        wall0.transform.localPosition = new Vector3(roomWidth / 2, roomHeight / 2, roomLength - wallThick / 2);

        wall1.transform.localScale = new Vector3(wallThick, roomHeight, floorLength);
        wall1.transform.localPosition = new Vector3(wallThick / 2, roomHeight / 2, roomLength / 2);

        wall2.transform.localScale = new Vector3(roomWidth, roomHeight, wallThick);
        wall2.transform.localPosition = new Vector3(roomWidth / 2, roomHeight / 2, wallThick / 2);

        wall3.transform.localScale = new Vector3(wallThick, roomHeight, floorLength);
        wall3.transform.localPosition = new Vector3(roomWidth - wallThick / 2, roomHeight / 2, roomLength / 2);
    }

    void SaveArchObj()
    {
        //바닥
        DoExport(false, floor);

        //벽
        ArrayList meshFilters = new ArrayList();
        meshFilters.Add((MeshFilter)wall0.GetComponent("MeshFilter"));
        meshFilters.Add((MeshFilter)wall1.GetComponent("MeshFilter"));
        meshFilters.Add((MeshFilter)wall2.GetComponent("MeshFilter"));
        meshFilters.Add((MeshFilter)wall3.GetComponent("MeshFilter"));
        CombineInstance[] combine = new CombineInstance[meshFilters.Count];
        int i = 0;
        while (i < meshFilters.Count)
        {
            combine[i].mesh = ((MeshFilter)meshFilters[i]).sharedMesh;
            combine[i].transform = ((MeshFilter)meshFilters[i]).transform.localToWorldMatrix;
            //((MeshFilter)meshFilters[i]).gameObject.active = false;
            i++;
        }

        GameObject mergedWall = new GameObject("MergedWall");
        mergedWall.AddComponent<MeshFilter>().mesh = new Mesh();
        mergedWall.AddComponent<MeshRenderer>();
        ((MeshFilter)mergedWall.GetComponent("MeshFilter")).mesh.CombineMeshes(combine);
        mergedWall.gameObject.SetActive(false);

        DoExport(false, mergedWall);
    }

    static void DoExport(bool makeSubmeshes, GameObject gameObject)
    {
        string meshName = gameObject.name;
        //string fileName = EditorUtility.SaveFilePanel("Export .obj file", "C:/Users/ruddm/Desktop/", meshName, "obj");
        string fileName = "C:/Users/rkdwj/Desktop/" + meshName + ".obj";
        ObjExporterScript.Start();

        StringBuilder meshString = new StringBuilder();

        meshString.Append("#" + meshName + ".obj"
                          + "\n#" + System.DateTime.Now.ToLongDateString()
                          + "\n#" + System.DateTime.Now.ToLongTimeString()
                          + "\n#-------"
                          + "\n\n");

        Transform t = gameObject.transform;

        Vector3 originalPosition = t.position;
        t.position = Vector3.zero;

        if (!makeSubmeshes)
        {
            meshString.Append("g ").Append(t.name).Append("\n");
        }

        meshString.Append(ProcessTransform(t, makeSubmeshes));

        WriteToFile(meshString.ToString(), fileName);

        t.position = originalPosition;

        ObjExporterScript.End();
    }

    static string ProcessTransform(Transform t, bool makeSubmeshes)
    {
        StringBuilder meshString = new StringBuilder();

        meshString.Append("#" + t.name
                          + "\n#-------"
                          + "\n");

        if (makeSubmeshes)
        {
            meshString.Append("g ").Append(t.name).Append("\n");
        }

        MeshFilter mf = t.GetComponent<MeshFilter>();
        if (mf != null)
        {
            meshString.Append(ObjExporterScript.MeshToString(mf, t));
        }

        for (int i = 0; i < t.childCount; i++)
        {
            meshString.Append(ProcessTransform(t.GetChild(i), makeSubmeshes));
        }

        return meshString.ToString();
    }

    static void WriteToFile(string s, string filename)
    {
        using (StreamWriter sw = new StreamWriter(filename))
        {
            sw.Write(s);
        }
    }

    public void Screenshot_()
    {
        camera.transform.localPosition = new Vector3(roomWidth / 2, roomHeight * 5, roomLength / 2);
        resWidth = Screen.width; resHeight = Screen.height;
        path = "C:/Users/rkdwj/Desktop/";

        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists)
        {
            Directory.CreateDirectory(path);
        }
        string name;
        name = id + ".png";
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        camera.targetTexture = rt;
        Texture2D screenshot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        Rect rec = new Rect(0, 0, screenshot.width, screenshot.height);
        camera.Render();
        RenderTexture.active = rt;
        screenshot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenshot.Apply();
        byte[] bytes = screenshot.EncodeToPNG();
        File.WriteAllBytes(name, bytes);
        //System.IO.File.WriteAllBytes("capture.png", bytes);
    }
    public void getFurnitureInfo()
    {
        furniture = new ArrayList();

        TextAsset data = Resources.Load("furniture", typeof(TextAsset)) as TextAsset;
        StringReader sr = new StringReader(data.text);

        string line = sr.ReadLine();
        while (line != null)
        {
            string[] words = line.Split('\t');
            furniture.Add(new Furniture(int.Parse(words[0]), double.Parse(words[2]), double.Parse(words[3]), double.Parse(words[4])));

            line = sr.ReadLine();
        }

    }
}



public class ObjExporterScript
{
    private static int StartIndex = 0;

    public static void Start()
    {
        StartIndex = 0;
    }
    public static void End()
    {
        StartIndex = 0;
    }


    public static string MeshToString(MeshFilter mf, Transform t)
    {
        Vector3 s = t.localScale;
        Vector3 p = t.localPosition;
        Quaternion r = t.localRotation;


        int numVertices = 0;
        Mesh m = mf.sharedMesh;
        if (!m)
        {
            return "####Error####";
        }
        Material[] mats = mf.GetComponent<Renderer>().sharedMaterials;
        StringBuilder sb = new StringBuilder();

        foreach (Vector3 vv in m.vertices)
        {
            Vector3 v = t.TransformPoint(vv);
            numVertices++;
            sb.Append(string.Format("v {0} {1} {2}\n", v.x, v.y, -v.z));
        }
        sb.Append("\n");
        foreach (Vector3 nn in m.normals)
        {
            Vector3 v = r * nn;
            sb.Append(string.Format("vn {0} {1} {2}\n", -v.x, -v.y, v.z));
        }
        sb.Append("\n");
        foreach (Vector3 v in m.uv)
        {
            sb.Append(string.Format("vt {0} {1}\n", v.x, v.y));
        }
        for (int material = 0; material < m.subMeshCount; material++)
        {
            sb.Append("\n");
            //sb.Append("usemtl ").Append(mats[material].name).Append("\n");
            //sb.Append("usemap ").Append(mats[material].name).Append("\n");

            int[] triangles = m.GetTriangles(material);
            for (int i = 0; i < triangles.Length; i += 3)
            {
                sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
                                        triangles[i] + 1 + StartIndex, triangles[i + 1] + 1 + StartIndex, triangles[i + 2] + 1 + StartIndex));
            }
        }

        StartIndex += numVertices;
        return sb.ToString();
    }
}