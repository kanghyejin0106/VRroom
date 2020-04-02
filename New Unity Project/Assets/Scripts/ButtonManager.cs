using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;
using System.Net.Json;
using System.IO;

public class ButtonManager : MonoBehaviour
{
    public GameObject Furnitrue;
    GameObject Furniture, Map, index ,Map2;

    String id = "001";
    public static float roomWidth = 3, roomLength = 5, roomHeight = 2;
    public static double wall = 0.0;
    public static int pm_state = 0;

    String jsonHouse = "", jsonWall = "";
    int furnitureNum = 4;
    ArrayList furniture;
    Furniture[] myFurniture;
    private int clickSignal = 0;
    // Start is called before the first frame update
    void Start()
    {
        Furniture = GameObject.Find("Furniture");
        Map = GameObject.Find("Map");
        Map.GetComponent<RectTransform>().sizeDelta = new Vector2(roomWidth*100, roomLength * 100);
        Map2 = GameObject.Find("Map2");
        getFurnitureInfo();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void btnClick()
    {
        if (clickSignal == 2)
        {
            index = EventSystem.current.currentSelectedGameObject;
            GameObject child = Instantiate(Furnitrue, Map2.transform.position, gameObject.transform.rotation);

            child.GetComponent<FurnitureCreate>().childname = index.name;
            child.transform.SetParent(Map2.transform);
            child.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            clickSignal = 1;
        }
        else {
            index = EventSystem.current.currentSelectedGameObject;
            GameObject child = Instantiate(Furnitrue, Map.transform.position, gameObject.transform.rotation);
            child.GetComponent<FurnitureCreate>().childname = index.name;
            child.transform.SetParent(Furniture.transform);
            child.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
    }
    public void map2ButtonClick()
    {
        clickSignal = 2;
    }
    public void plusClick()
    {
        pm_state = 1;
    }
    public void minusClick()
    {
        pm_state = 2;
    }
    public void rotateClick()
    {
        pm_state = 0;
    }
    public void deleteClick()
    {
        pm_state = 3;
    }
    public void makeJson()
    {

        initJsonHouse();
        makeJsonHouse();

        JsonTextParser parserHouse = new JsonTextParser();
        JsonObject objHouse = parserHouse.Parse(jsonHouse);
        JsonObjectCollection colHouse = (JsonObjectCollection)objHouse;
        File.WriteAllText("C:/Users/rkdwj/Desktop/house.json", colHouse.ToString());

        makeJsonWall();

        JsonTextParser parseWall = new JsonTextParser();
        JsonObject objWall = parserHouse.Parse(jsonWall);
        JsonObjectCollection colWall = (JsonObjectCollection)objWall;
        File.WriteAllText("C:/Users/rkdwj/Desktop/"+id+".arch.json", colWall.ToString());

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

    public void initJsonHouse()
    {
        furnitureNum = Furniture.transform.childCount;

        //방 matrix 만들기
        double minX = 38.3498855248876;
        double minZ = 0;
        double minY = 37.44011313910404;
        double maxX = 43.54520764093708;
        double maxZ = 2.739999938756229;
        double maxY = 42.61519713265855;

        double xscale = (roomWidth) / (maxX - minX);
        double zscale = (roomHeight) / (maxZ - minZ);
        double yscale = (roomLength) / (maxY - minY);

        double x = -(minX + maxX) / 2;
        double z = -minZ;
        double y = -(minY + maxY) / 2;
        double[,] t = {{1,0,0,0},{0,1,0,0},{0,0,1,0},{x,z,y,1}};
     
        x = roomWidth/2;
        z = 0.0;
        y = roomLength/2;
        double[,] t_scale = {{xscale, 0, 0, 0},{0,zscale,0,0},{0,0,yscale,0},{0,0,0,1}};
        double[,] t_shift = {{1,0,0,0},{0,1,0,0},{0,0,1,0},{x,z,y,1}};

        double[,] tempMatrix = new double[4, 4];
        for (int a = 0; a < 4; a++)
            for (int b = 0; b < 4; b++)
            {
                tempMatrix[a, b] = 0;
                for (int c = 0; c < 4; c++)
                    tempMatrix[a, b] += t[a, c] * t_scale[c, b];
            }

        double[,] roomMatrix = new double[4, 4];
        for (int a = 0; a < 4; a++)
            for (int b = 0; b < 4; b++)
            {
                roomMatrix[a, b] = 0;
                for (int c = 0; c < 4; c++)
                    roomMatrix[a, b] += tempMatrix[a, c] * t_shift[c, b];
            }

        jsonHouse += "{\r\n" +
                     "  \"version\": \"suncg@1.0.0\",\r\n" +
                     "  \"id\": \"" + id + "\",\r\n" +
                     "  \"up\": [\r\n" +
                     "    0,\r\n" +
                     "    1,\r\n" +
                     "    0\r\n" +
                     "  ],\r\n" +
                     "  \"front\": [\r\n" +
                     "    0,\r\n" +
                     "    0,\r\n" +
                     "    1\r\n" +
                     "  ],\r\n" +
                     "  \"scaleToMeters\": 1,\r\n" +
                     "  \"levels\": [\r\n" +
                     "    {\r\n" +
                     "      \"id\": \"0\",\r\n" +
                     "      \"bbox\": {\r\n" +
                     "        \"min\": [\r\n" +
                     "          0,\r\n" +
                     "          0,\r\n" +
                     "          0\r\n" +
                     "        ],\r\n" +
                     "        \"max\": [\r\n" +
                     "          " + roomWidth + ",\r\n" +
                     "          " + roomHeight + ",\r\n" +
                     "          " + roomLength + "\r\n" +
                     "        ]\r\n" +
                     "      },\r\n" +
                     "      \"nodes\": [\r\n" +
                     "        {\r\n" +
                     "          \"id\": \"0_0\",\r\n" +
                     "          \"type\": \"Room\",\r\n" +
                     "          \"valid\": 1,\r\n" +
                     "          \"modelId\": \"fr_0rm_0\",\r\n" +
                     "          \"nodeIndices\": [\r\n";

        for (int i = 1; i < furnitureNum; i++)
            jsonHouse += "            " + i + ",\r\n";
        jsonHouse += "            " + furnitureNum + "\r\n";

        jsonHouse += "          ],\r\n" +
                     "          \"transform\": [\r\n";

            for (int a = 0; a < 15; a++)
            jsonHouse += "            " + roomMatrix[a / 4, a % 4] + ",\r\n";
        jsonHouse += "            " + roomMatrix[3, 3] + "\r\n";

        jsonHouse += "          ],\r\n" +
                     "          \"roomTypes\": [\r\n" +
                     "            \"Bedroom\"\r\n" +
                     "          ],\r\n" +
                     "          \"bbox\": {\r\n" +
                     "            \"min\": [\r\n" +
                     "              0.0,\r\n" +
                     "              0.0,\r\n" +
                     "              0.0\r\n" +
                     "            ],\r\n" +
                     "            \"max\": [\r\n" +
                     "              " + (roomWidth) + ",\r\n" +
                     "              " + (roomHeight) + ",\r\n" +
                     "              " + (roomLength) + "\r\n" +
                     "            ]\r\n" +
                     "          }\r\n" +
                     "        },\r\n";

    }

    public void makeJsonHouse()
    {
        myFurniture = new Furniture[furnitureNum];
        for (int i = 0; i < furnitureNum; i++)
        {
            for (int j = 0; j < Furniture.transform.GetChild(i).transform.childCount; j++)
            {
                if (Furniture.transform.GetChild(i).transform.GetChild(j).gameObject.activeSelf)
                {
                    Debug.Log(Furniture.transform.GetChild(i).transform.GetChild(j).name);
                    int modelID = int.Parse(Furniture.transform.GetChild(i).transform.GetChild(j).name);

                    foreach (Furniture f in furniture)
                    {
                        if (modelID == f.modelID)
                        {
                            myFurniture[i] = new Furniture(f.modelID, f.width, f.height, f.length);
                            myFurniture[i].scale = Furniture.transform.GetChild(i).transform.localScale.x;
                            myFurniture[i].startPoint1 = (Furniture.transform.GetChild(i).transform.localPosition.x + roomWidth*50) / 100 - 
                                f.width * myFurniture[i].scale / 100 / 2;
                            myFurniture[i].startPoint2 = wall;
                            myFurniture[i].startPoint3 = (Furniture.transform.GetChild(i).transform.localPosition.y + roomLength * 50) / 100 - 
                                f.length * myFurniture[i].scale / 100 / 2;
                            myFurniture[i].rotation = -Furniture.transform.GetChild(i).transform.eulerAngles.z;
                        }
                    }

                }
            }

        }


        for (int i = 0; i < furnitureNum; i++)
        {

            jsonHouse += "        {\r\n" +
                            "          \"id\": \"0_" + (i + 1) + "\",\r\n" +
                            "          \"type\": \"Object\",\r\n" +
                            "          \"valid\": 1,\r\n" +
                            "          \"modelId\": \"" + myFurniture[i].modelID + "\",\r\n" +
                            "          \"transform\": [\r\n";

            double[,] scaleMatrix = {
                {myFurniture[i].scale,0,0,0},
                {0,myFurniture[i].scale,0,0},
                {0,0,myFurniture[i].scale,0},
                {0,0,0,1}
            };

            double[,] locationMatrix = {
                {1,0,0,0},
                {0,1,0,0},
                {0,0,1,0},
                {myFurniture[i].startPoint1 + myFurniture[i].width * myFurniture[i].scale/ 100 / 2,myFurniture[i].startPoint2, myFurniture[i].startPoint3 + myFurniture[i].length * myFurniture[i].scale / 100 / 2,1}
            };

            double[,] rotationMatrix = {
                {Math.Cos(myFurniture[i].rotation*Math.PI/180), 0, -Math.Sin(myFurniture[i].rotation*Math.PI/180), 0},
                {0, 1, 0, 0},
                {Math.Sin(myFurniture[i].rotation*Math.PI/180), 0, Math.Cos(myFurniture[i].rotation*Math.PI/180), 0},
                {0, 0, 0, 1}
            };

            double[,] tempMatrix = new double[4,4];
            for(int a=0;a<4;a++)
                for(int b=0;b<4;b++)
                {
                    tempMatrix[a,b] = 0;
                    for(int c=0;c<4;c++)
                        tempMatrix[a, b] += rotationMatrix[a, c] * scaleMatrix[c, b];
                }

            double[,] finalMatrix = new double[4,4];
            for (int a = 0; a < 4; a++)
                for (int b = 0; b < 4; b++)
                {
                    finalMatrix[a, b] = 0;
                    for (int c = 0; c < 4; c++)
                        finalMatrix[a, b] += tempMatrix[a, c] * locationMatrix[c, b];
                }

            for (int a = 0;a<15;a++)
                jsonHouse += "            "+finalMatrix[a/4,a%4]+",\r\n";
            jsonHouse += "            " + finalMatrix[3,3] + "\r\n";

            jsonHouse +=
                            "          ],\r\n" +
                            "          \"bbox\": {\r\n" +
                            "            \"min\": [\r\n" +
                            "              " + myFurniture[i].startPoint1 + ",\r\n" +
                            "              " + myFurniture[i].startPoint2 + ",\r\n" +
                            "              " + myFurniture[i].startPoint3 + "\r\n" +
                            "            ],\r\n" +
                            "            \"max\": [\r\n" +
                            "              " + (myFurniture[i].startPoint1 + myFurniture[i].width * myFurniture[i].scale / 100) + ",\r\n" +
                            "              " + (myFurniture[i].startPoint2 + myFurniture[i].height * myFurniture[i].scale / 100) + ",\r\n" +
                            "              " + (myFurniture[i].startPoint3 + myFurniture[i].length * myFurniture[i].scale / 100) + "\r\n" +
                            "            ]\r\n" +
                            "          },\r\n" +
                            "          \"materials\": [\r\n" +
                            "            {\r\n" +
                            "              \"name\": \"color\",\r\n" +
                            "              \"diffuse\": \"#a3a3a3\"\r\n" +
                            "            },\r\n" +
                            "            {\r\n" +
                            "              \"name\": \"color_1\",\r\n" +
                            "              \"diffuse\": \"#6990a3\"\r\n" +
                            "            }\r\n" +
                            "          ]\r\n";

            if (i == furnitureNum - 1)
                jsonHouse += "        }\r\n";
            else
                jsonHouse += "        },\r\n";

        }

        jsonHouse += "      ]\r\n" +
                        "    }\r\n" +
                        "  ],\r\n" +
                        "  \"bbox\": {\r\n" +
                        "    \"min\": [\r\n" +
                        "      0,\r\n" +
                        "      0,\r\n" +
                        "      0\r\n" +
                        "    ],\r\n" +
                        "    \"max\": [\r\n" +
                        "      " + roomWidth + ",\r\n" +
                        "      " + roomHeight + ",\r\n" +
                        "      " + roomLength + "\r\n" +
                        "    ]\r\n" +
                        "  }\r\n" +
                        "}";

    }

    public void makeJsonWall()
    {
        jsonWall += "{\r\n" +
                    "  \"version\": \"suncg-arch@1.0.0\",\r\n" +
                    "  \"id\": \"" + id + "\",\r\n" +
                    "  \"up\": [ 0, 1, 0 ],\r\n" +
                    "  \"front\": [ 0, 0, 1 ],\r\n" +
                    "  \"scaleToMeters\": 1,\r\n" +
                    "  \"defaults\": {\r\n" +
                    "    \"Wall\": {\r\n" +
                    "      \"depth\": 0.1,\r\n" +
                    "      \"extraHeight\": 0.035\r\n" +
                    "    },\r\n" +
                    "    \"Ceiling\": {\r\n" +
                    "      \"depth\": 0.05\r\n" +
                    "    },\r\n" +
                    "    \"Floor\": {\r\n" +
                    "      \"depth\": 0.05\r\n" +
                    "    },\r\n" +
                    "    \"Ground\": {\r\n" +
                    "      \"depth\": 0.08\r\n" +
                    "    }\r\n" +
                    "  },\r\n" +
                    "  \"elements\": [\r\n" +
                    "    {\r\n" +
                    "      \"id\": \"0_0_0\",\r\n" +
                    "      \"type\": \"Wall\",\r\n" +
                    "      \"roomId\": \"0_0\",\r\n" +
                    "      \"height\": " + roomHeight + ",\r\n" +
                    "      \"points\": [\r\n" +
                    "        [ 0, 0, 0 ],\r\n" +
                    "        [ 0, 0, " + (roomLength) + " ]\r\n" +
                    "      ],\r\n" +
                    "      \"holes\": [],\r\n" +
                    "      \"materials\": [\r\n" +
                    "        {\r\n" +
                    "          \"name\": \"inside\",\r\n" +
                    "          \"texture\": \"wallp_0\",\r\n" +
                    "          \"diffuse\": \"#7a0400\"\r\n" +
                    "        },\r\n" +
                    "        {\r\n" +
                    "          \"name\": \"outside\",\r\n" +
                    "          \"texture\": \"bricks_1\",\r\n" +
                    "          \"diffuse\": \"#ffffff\"\r\n" +
                    "        }\r\n" +
                    "      ],\r\n" +
                    "      \"depth\": 0.1\r\n" +
                    "    },\r\n" +
                    "    {\r\n" +
                    "\r\n" +
                    "      \"id\": \"0_0_1\",\r\n" +
                    "      \"type\": \"Wall\",\r\n" +
                    "      \"roomId\": \"0_0\",\r\n" +
                    "      \"height\": 3,\r\n" +
                    "      \"points\": [\r\n" +
                    "        [ " + (roomWidth) + ", 0, " + (roomLength) + " ],\r\n" +
                    "        [ " + (roomWidth) + ", 0, 0 ]\r\n" +
                    "      ],\r\n" +
                    "      \"holes\": [],\r\n" +
                    "      \"materials\": [\r\n" +
                    "        {\r\n" +
                    "          \"name\": \"inside\",\r\n" +
                    "          \"texture\": \"wallp_0\",\r\n" +
                    "          \"diffuse\": \"#7a0400\"\r\n" +
                    "        },\r\n" +
                    "        {\r\n" +
                    "          \"name\": \"outside\",\r\n" +
                    "          \"texture\": \"bricks_1\",\r\n" +
                    "          \"diffuse\": \"#ffffff\"\r\n" +
                    "        }\r\n" +
                    "      ],\r\n" +
                    "      \"depth\": 0.1\r\n" +
                    "    },\r\n" +
                    "    {\r\n" +
                    "      \"id\": \"0_0_2\",\r\n" +
                    "      \"type\": \"Wall\",\r\n" +
                    "      \"roomId\": \"0_0\",\r\n" +
                    "      \"height\": 3,\r\n" +
                    "      \"points\": [\r\n" +
                    "        [ " + (roomWidth) + ", 0, 0 ],\r\n" +
                    "        [ 0, 0, 0 ]\r\n" +
                    "      ],\r\n" +
                    "      \"holes\": [],\r\n" +
                    "      \"materials\": [\r\n" +
                    "        {\r\n" +
                    "          \"name\": \"inside\",\r\n" +
                    "          \"texture\": \"wallp_0\",\r\n" +
                    "          \"diffuse\": \"#7a0400\"\r\n" +
                    "        },\r\n" +
                    "        {\r\n" +
                    "          \"name\": \"outside\",\r\n" +
                    "          \"texture\": \"bricks_1\",\r\n" +
                    "          \"diffuse\": \"#ffffff\"\r\n" +
                    "        }\r\n" +
                    "      ],\r\n" +
                    "      \"depth\": 0.1\r\n" +
                    "    },\r\n" +
                    "    {\r\n" +
                    "      \"id\": \"0_0_3\",\r\n" +
                    "      \"type\": \"Wall\",\r\n" +
                    "      \"roomId\": \"0_0\",\r\n" +
                    "      \"height\": 3,\r\n" +
                    "      \"points\": [\r\n" +
                    "        [ 0, 0, " + (roomLength) + " ],\r\n" +
                    "        [ " + (roomWidth) + ", 0, " + (roomLength) + " ]\r\n" +
                    "      ],\r\n" +
                    "      \"holes\": [],\r\n" +
                    "      \"materials\": [\r\n" +
                    "        {\r\n" +
                    "          \"name\": \"inside\",\r\n" +
                    "          \"texture\": \"wallp_0\",\r\n" +
                    "          \"diffuse\": \"#7a0400\"\r\n" +
                    "        },\r\n" +
                    "        {\r\n" +
                    "          \"name\": \"outside\",\r\n" +
                    "          \"texture\": \"bricks_1\",\r\n" +
                    "          \"diffuse\": \"#ffffff\"\r\n" +
                    "        }\r\n" +
                    "      ],\r\n" +
                    "      \"depth\": 0.1\r\n" +
                    "    }\r\n" +
                    "  ]\r\n" +
                    "}";

    }
}

class Furniture
{
    public int modelID;
    public double width, height, length;
    public double startPoint1, startPoint2, startPoint3;
    public double rotation, scale;

    public Furniture(int modelID, double width, double height, double length)
    {
        this.modelID = modelID;
        this.width = width;
        this.height = height;
        this.length = length;
        startPoint1 = 0;
        startPoint2 = 0;
        startPoint3 = 0;
        rotation = 0;
        scale = 0;
    }

    public Furniture(int modelID)
    {
        this.modelID = modelID;
        this.width = 0;
        this.height = 0;
        this.length = 0;
        startPoint1 = 0;
        startPoint2 = 0;
        startPoint3 = 0;
        rotation = 0;
        scale = 0;
    }
}