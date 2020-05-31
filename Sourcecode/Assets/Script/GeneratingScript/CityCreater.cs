using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random;

public class CityCreater : MonoBehaviour
{
    [SerializeField]
    private float SpaceBetweenBuildings;
    [SerializeField]
    private float Radius;
    [SerializeField]
    private float AddRadiusPerCircel;
    [SerializeField]
    private int BuildingsCount;
    [SerializeField]
    private GameObject NPC_prefab;
    [SerializeField]
    private GameObject Sword;

    [SerializeField]
    float f_h = 30f;
    [SerializeField]
    float f_w = 30f;

    private readonly float gate_size = 6.2f;
    private readonly float wall_size = 2f;
    Vector3 m_size;

    GameObject m_area;

    Vector3 m_center;

    Vector3[] Corner = new Vector3[4];
    Vector3[] CornerCC = new Vector3[4];
    Vector3[] CityCorners = new Vector3[4];

    Vector3 CC_corner;
    List<Vector3> VerticeList = new List<Vector3>();

    Vector3[] EdgeVector = new Vector3[2];
    Vector3[] EdgeVectorCC = new Vector3[2];

    float DistanceToWallRight;
    float DistanceToWallUp;

    public BuildingsCreater buildingsCreater;
    public int NPC_count;

    [HideInInspector]
    public Bounds cityBounds { get; private set; }

    void Start()
    {
        if (buildingsCreater == null)
        {
            throw new Exception("Kein BuildingsCreater gefunden");
        }
        m_area = GameObject.FindGameObjectWithTag("CityArea");
        if (m_area == null)
        {
            Debug.LogException(new Exception("Es wurden keine CityAreas gefunden"));
        }
        foreach (Vector3 v in m_area.GetComponent<MeshFilter>().sharedMesh.vertices)
            VerticeList.Add(v);


        FindArea(); //Get the Pos of the Corners and Calc the Edges
        FindCC();
        GetCitySize(); //Calc the size of the City                  //     1____________0
        GetCityCorners(); //Order the Cornders                      //     |            |
        SetWalls(); //Set the walls                                 //     |            |
        SetBuildings();  //Set the buildings                        //     |            |
        SetCityBounds();  //Create a bounds of the city             //     |____________|
        for (int i = 0; i < NPC_count; i++)                         //     3            2   
        {
            SetNPCs(); //Spawn NPCs
        }
        SetSword(); //Spawn Sword
    }

    private void SetSword()
    {
        RaycastHit hit;
        float u = Range(0.0f, 1.0f);
        float v = Range(0.0f, 1.0f);
        if (Physics.Raycast(new Vector3(                        // cast a ray up above the Random choosen point
                Range(cityBounds.min.x, cityBounds.max.x),      // cast a ray up above the Random choosen point
                0f,                                             // cast a ray up above the Random choosen point
                Range(cityBounds.min.z, cityBounds.max.z))      // cast a ray up above the Random choosen point
                + Vector3.up * 1000, -Vector3.up, out hit))     // cast a ray up above the Random choosen point
        {
            if (Helper.CheckForValidPos(hit.point, new BoxCollider())) //if there is no BoxColl == no Building then...
            {
                Instantiate(Sword, hit.point, Sword.transform.rotation); //... Spawn
            }
            else
            {
                SetSword(); //if no... try again
            }
        }
    }

    private void SetNPCs()
    {
        RaycastHit hit;
        float u = Range(-1.0f, 1.0f);
        float v = Range(-1.0f, 1.0f);
        if (Physics.Raycast(new Vector3(                         // cast a ray up above the Random choosen point
                Range(cityBounds.min.x, cityBounds.max.x),       // cast a ray up above the Random choosen point
                0f,                                              // cast a ray up above the Random choosen point
                Range(cityBounds.min.z, cityBounds.max.z))       // cast a ray up above the Random choosen point
                + Vector3.up * 1000, -Vector3.up, out hit))      // cast a ray up above the Random choosen point
        {
            if (Helper.CheckForValidPos(hit.point, new BoxCollider()))            //if there is no BoxColl == no Building then...
            {
                Instantiate(NPC_prefab, hit.point, NPC_prefab.transform.rotation); //... Spawn
            }
            else
            {
                SetNPCs(); //if no... try again
            }
        }
    }

    private void SetCityBounds()
    {
        cityBounds = new Bounds(m_center, new Vector3(DistanceToWallRight * 0.9f, 5f, DistanceToWallUp * 0.9f) * 2);
    }

    private void SetBuildings()
    {
        bool unDone = true;
        List<Vector3> UsedPositions = new List<Vector3>(); //used Pos will be remembered to dont spawn inside something
        int counter = 0;
        int Circels = GetCircelsNummber(); //Get Circel Count

        buildingsCreater.Create(BuildingsType.MARKET, m_center); //Create Market in the center

        UsedPositions.Add(m_center);
        Radius *= 1.5f;
        for (int o = 0; o < Circels; o++) //Foreach Circel
        {
            for (int i = 0; i < BuildingsCount; i++) //For the Amount of Buildings on one Circel
            {
                unDone = true;
                while (unDone) //while there was no point found, look for another
                {
                    counter++;
                    if (counter > 50 * o + 1) //if ties too much, give up
                    {
                        unDone = false;
                        break;
                    }
                    unDone = true;
                    float Degree = Range(0, 360);
                    SetCityBounds();
                    Vector3 tmpPos = Helper.GetUnitOnCircle(Degree, Radius) + m_center; //Get Random Pos on Circel
                    if (!Helper.InsideArea(tmpPos, cityBounds)) //Check if Point is inside the City
                    {
                        continue;
                    }
                    if (!Helper.InsideRange(tmpPos, SpaceBetweenBuildings, UsedPositions)) //Check if Point is not too cose to the used Points
                    {
                        continue;
                    }
                    if (o + 3 > 10) //if the city is too big, then dont spawn more buildings
                    {
                        return;
                    }
                    buildingsCreater.Create((BuildingsType)o + 3, tmpPos, Vector3.one, Quaternion.Euler(new Vector3(0, Range(0, 360), 0))); //create Building
                    UsedPositions.Add(tmpPos);
                    unDone = false;
                }
            }
            Radius += AddRadiusPerCircel; //Create bigger Circel
            counter = 0;
        }
    }

    private int GetCircelsNummber()
    {
        #region -Calc the distance to the Walls-
        RaycastHit hit;
        if (Physics.Raycast(m_center + Vector3.up, Vector3.right, out hit))             
        {                                                                               
            DistanceToWallRight = (hit.point - m_center).magnitude;                     
        }                                                                               
        else                                                                            
        {                                                                               
            DistanceToWallRight = 0f;                                                   
            throw new Exception("Es wurde kein Ray vom m_center nach rechts getroffen");
        }                                                                               
        if (Physics.Raycast(m_center + Vector3.up, Vector3.forward, out hit))           
        {                                                                               
            DistanceToWallUp = (hit.point - m_center).magnitude;                        
        }                                                                               
        else                                                                            
        {                                                                               
            DistanceToWallUp = 0f;                                                      
            throw new Exception("Es wurde kein Ray vom m_center nach oben getroffen");  
        }
        #endregion
        float[] Distances = { DistanceToWallRight, DistanceToWallUp };
        return (int)(Helper.GetLowestNummber(Distances) / Radius);
    }

    private void SetWalls()
    {
        //Create Tower First
        buildingsCreater.Create(BuildingsType.TOWER, CityCorners[0], Vector3.one, Quaternion.Euler(0, -90, 0));     //Set Tower
        buildingsCreater.Create(BuildingsType.TOWER, CityCorners[1], Vector3.one, Quaternion.Euler(0, 180, 0));     //Set Tower
        buildingsCreater.Create(BuildingsType.TOWER, CityCorners[2], Vector3.one, Quaternion.Euler(0, 0, 0));       //Set Tower
        buildingsCreater.Create(BuildingsType.TOWER, CityCorners[3], Vector3.one, Quaternion.Euler(0, 90, 0));      //Set Tower
        //Set Walls
        Vector3 ToSetWalls = Helper.GetEdge(CityCorners[0], CityCorners[2]);
        int offsett = (int)(ToSetWalls.magnitude / wall_size);
        int counter = 0;

        //I start from a corner, set the gate halfway and then come from the opposite side to the gate
        #region -Vertical-
        offsett = (Mathf.FloorToInt(offsett * 0.5f) - 2);
        for (int x = 1; x < offsett; x++) //0-2
        {
            counter++;
            buildingsCreater.Create(BuildingsType.WALL, CityCorners[0] + new Vector3(0, 0, -wall_size) * x, Vector3.one,
                Quaternion.Euler(new Vector3(0, 180, 0)));
        }
        //Gate
        buildingsCreater.Create(BuildingsType.GATE, CityCorners[0] + new Vector3(0, 0, -wall_size) * counter + new Vector3(0, 0, -gate_size),
            Vector3.one, Quaternion.Euler(new Vector3(0, 180, 0)));

        for (int x = 0; x < offsett + 2; x++) //2-0
        {
            buildingsCreater.Create(BuildingsType.WALL, CityCorners[2] + new Vector3(0, 0, wall_size) * x, Vector3.one,
                Quaternion.Euler(new Vector3(0, 180, 0)));
        }

        counter = 0;
        for (int x = 1; x < offsett; x++) //1-3
        {
            counter++;
            buildingsCreater.Create(BuildingsType.WALL, CityCorners[1] + new Vector3(0, 0, -wall_size) * x, Vector3.one,
                Quaternion.Euler(new Vector3(0, 0, 0)));
        }
        //Gate
        buildingsCreater.Create(BuildingsType.GATE, CityCorners[1] + new Vector3(0, 0, -wall_size) * counter + new Vector3(0, 0, -gate_size),
            Vector3.one, Quaternion.Euler(new Vector3(0, 0, 0)));

        for (int x = 0; x < offsett + 2; x++) //3-1
        {
            buildingsCreater.Create(BuildingsType.WALL, CityCorners[3] + new Vector3(0, 0, wall_size) * x, Vector3.one,
                Quaternion.Euler(new Vector3(0, 0, 0)));
        }
        #endregion

        #region -Horizontal-
        ToSetWalls = Helper.GetEdge(CityCorners[0], CityCorners[1]);
        offsett = (int)(ToSetWalls.magnitude / wall_size);
        offsett = (Mathf.FloorToInt(offsett * 0.5f) - 2);
        counter = 0;
        for (int x = 1; x < offsett; x++)//0-1
        {
            counter++;
            buildingsCreater.Create(BuildingsType.WALL, CityCorners[0] + new Vector3(-wall_size, 0, 0) * x, Vector3.one,
                Quaternion.Euler(new Vector3(0, 90, 0)));
        }
        //Gate
        buildingsCreater.Create(BuildingsType.GATE, CityCorners[0] + new Vector3(-wall_size, 0, 0) * counter + new Vector3(-gate_size, 0, 0),
            Vector3.one, Quaternion.Euler(new Vector3(0, 90, 0)));

        for (int x = 0; x < offsett + 2; x++)//1-0
        {
            buildingsCreater.Create(BuildingsType.WALL, CityCorners[1] + new Vector3(wall_size, 0, 0) * x, Vector3.one,
                Quaternion.Euler(new Vector3(0, 90, 0)));
        }

        counter = 0;
        for (int x = 1; x < offsett; x++)//2-3
        {
            counter++;
            buildingsCreater.Create(BuildingsType.WALL, CityCorners[2] + new Vector3(-wall_size, 0, 0) * x, Vector3.one,
                Quaternion.Euler(new Vector3(0, -90, 0)));
        }
        //Gate
        buildingsCreater.Create(BuildingsType.GATE, CityCorners[2] + new Vector3(-wall_size, 0, 0) * counter + new Vector3(-gate_size, 0, 0),
            Vector3.one, Quaternion.Euler(new Vector3(0, -90, 0)));

        for (int x = 0; x < offsett + 2; x++)//3-2
        {
            buildingsCreater.Create(BuildingsType.WALL, CityCorners[3] + new Vector3(wall_size, 0, 0) * x, Vector3.one,
                Quaternion.Euler(new Vector3(0, -90, 0)));
        }
        #endregion
    }


    private void GetCityCorners() //Convert to my Order                            //     1____________0
    {                                                                              //     |            |
        Vector3[] tmp_Corner = new Vector3[CityCorners.Length];                    //     |            |
        //Get Corners                                                              //     |            |
        tmp_Corner[0] = m_center + m_size;                                         //     |____________|
        tmp_Corner[1] = m_center + new Vector3(-m_size.x, 0, m_size.z);            //     3            2
        tmp_Corner[2] = m_center + new Vector3(m_size.x, 0, -m_size.z);
        tmp_Corner[3] = m_center + new Vector3(-m_size.x, 0, -m_size.z);
        //Set in right order
        CityCorners[0] = Helper.GetTopRight(tmp_Corner);
        CityCorners[1] = Helper.GetTopLeft(tmp_Corner);
        CityCorners[2] = Helper.GetBotRight(tmp_Corner);
        CityCorners[3] = Helper.GetBotLeft(tmp_Corner);
    }

    private void GetCitySize()
    {
        Vector3 ClosestCorner = Helper.FindClosestPoint(m_center, Corner);
        m_size.x = ClosestCorner.x - m_center.x;
        m_size.z = ClosestCorner.z - m_center.z;
    }

    /// <summary>
    /// Find City Center
    /// </summary>
    private void FindCC()
    {
        FindCityCenterCorners(); //scale the CitySize
        CalcCityCenterEdge(); //Calc the new Edges
        FindCenter(); //Find and Set the m_center;
    }

    /// <summary>
    /// Find City Area
    /// </summary>
    private void FindArea()
    {
        FindCorners();
        CalcEdge();
    }

    private void CalcCityCenterEdge()
    {
        EdgeVectorCC[0] = CornerCC[1] - CornerCC[3];
        EdgeVectorCC[1] = CornerCC[2] - CornerCC[3];
    }

    private void FindCityCenterCorners()
    {
        CornerCC[0] = m_area.transform.TransformPoint(VerticeList[0]) + new Vector3(-f_w, 0, -f_h);
        CornerCC[1] = m_area.transform.TransformPoint(VerticeList[10]) + new Vector3(f_w, 0, -f_h);
        CornerCC[2] = m_area.transform.TransformPoint(VerticeList[110]) + new Vector3(-f_w, 0, f_h);
        CornerCC[3] = m_area.transform.TransformPoint(VerticeList[120]) + new Vector3(f_w, 0, f_h);
    }

    private void CalcEdge()
    {
        EdgeVector[0] = Corner[1] - Corner[3];
        EdgeVector[1] = Corner[2] - Corner[3];
    }

    private void FindCorners()
    {
        Corner[0] = m_area.transform.TransformPoint(VerticeList[0]);
        Corner[1] = m_area.transform.TransformPoint(VerticeList[10]);
        Corner[2] = m_area.transform.TransformPoint(VerticeList[110]);
        Corner[3] = m_area.transform.TransformPoint(VerticeList[120]);
    }

    private void FindCenter()
    {
        int RandomCorner = Range(0, 2) == 0 ? 0 : 2;
        float u = Range(0.0f, 1.0f);
        float v = Range(0.0f, 1.0f);
        CC_corner = CornerCC[3];
        RaycastHit hit;
        if (Physics.Raycast(CC_corner + u * EdgeVectorCC[0] + v * EdgeVectorCC[1] + Vector3.up * 1000, -Vector3.up, out hit))
        {
            m_center = hit.point;
        }
    }
}
