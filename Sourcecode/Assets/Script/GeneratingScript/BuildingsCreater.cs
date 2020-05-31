using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingsType
{
    NONE = -1,
    GATE = 0,
    TOWER = 1,
    WALL = 2,
    MARKET = 3,
    HOUSE = 4,
    IRONWORKS = 5,
    BAKERY = 6,
    DAIRY = 7,
    SLAUGHTERHOUSE = 8,
    WAREHOUSE = 9,
    WINDMILL = 10
}

[CreateAssetMenu(fileName = "BuildingCreater", menuName = "ScripabelObject/BuildingCreater", order = 1)]
public class BuildingsCreater : ScriptableObject
{
    public GameObject m_CityParentPrefab;
    [SerializeField]
    private GameObject m_gate = null;
    [SerializeField]
    private GameObject m_wall = null;
    [SerializeField]
    private GameObject m_tower = null;
    [SerializeField]
    private GameObject m_market = null;
    [SerializeField]
    private GameObject m_house = null;
    [SerializeField]
    private GameObject m_ironworks = null;
    [SerializeField]
    private GameObject m_bakery = null;
    [SerializeField]
    private GameObject m_dairy = null;
    [SerializeField]
    private GameObject m_slaughterhouse = null;
    [SerializeField]
    private GameObject m_warehouse = null;
    [SerializeField]
    private GameObject m_windmill = null;

    private GameObject m_CityParent;

    /// <summary>
    /// Create a Building
    /// </summary>
    /// <param name="_type">The type of the Building</param>
    /// <param name="_trans">The Position, Rotation and Scale of the Building</param>
    public void Create(BuildingsType _type, Transform _trans)
    {
        if (m_CityParent == null)
        {
            m_CityParent = Instantiate(m_CityParentPrefab);
        }
        switch (_type)
        {
            case BuildingsType.GATE:
                {
                    GameObject Obj = Instantiate(m_gate, _trans);
                    Obj.AddComponent<NavMeshSourceTag>();
                    Obj.AddComponent<BoxCollider>();
                    Obj.AddComponent<GateScript>();
                    Obj.transform.parent = m_CityParent.transform;
                    break;
                }
            case BuildingsType.TOWER:
                {
                    GameObject Obj = Instantiate(m_tower, _trans);
                    Obj.AddComponent<NavMeshSourceTag>();
                    Obj.AddComponent<BoxCollider>();
                    Obj.transform.parent = m_CityParent.transform;
                    break;
                }
            case BuildingsType.WALL:
                {
                    GameObject Obj = Instantiate(m_wall, _trans);
                    Obj.AddComponent<NavMeshSourceTag>();
                    Obj.AddComponent<BoxCollider>();
                    Obj.transform.parent = m_CityParent.transform;
                    break;
                }
            case BuildingsType.MARKET:
                {
                    GameObject Obj = Instantiate(m_market, _trans);
                    Obj.AddComponent<NavMeshSourceTag>();
                    Obj.AddComponent<BoxCollider>();
                    Obj.transform.parent = m_CityParent.transform;
                    break;
                }
            case BuildingsType.HOUSE:
                {
                    GameObject Obj = Instantiate(m_house, _trans);
                    Obj.AddComponent<NavMeshSourceTag>();
                    Obj.AddComponent<BoxCollider>();
                    Obj.transform.parent = m_CityParent.transform;
                    break;
                }
            case BuildingsType.IRONWORKS:
                {
                    GameObject Obj = Instantiate(m_ironworks, _trans);
                    Obj.AddComponent<NavMeshSourceTag>();
                    Obj.AddComponent<BoxCollider>();
                    Obj.transform.parent = m_CityParent.transform;
                    break;
                }
            case BuildingsType.BAKERY:
                {
                    GameObject Obj = Instantiate(m_bakery, _trans);
                    Obj.AddComponent<NavMeshSourceTag>();
                    Obj.AddComponent<BoxCollider>();
                    Obj.transform.parent = m_CityParent.transform;
                    break;
                }
            case BuildingsType.DAIRY:
                {
                    GameObject Obj = Instantiate(m_dairy, _trans);
                    Obj.AddComponent<NavMeshSourceTag>();
                    Obj.AddComponent<BoxCollider>();
                    Obj.transform.parent = m_CityParent.transform;
                    break;
                }
            case BuildingsType.SLAUGHTERHOUSE:
                {
                    GameObject Obj = Instantiate(m_slaughterhouse, _trans);
                    Obj.AddComponent<NavMeshSourceTag>();
                    Obj.AddComponent<BoxCollider>();
                    Obj.transform.parent = m_CityParent.transform;
                    break;
                }
            case BuildingsType.WAREHOUSE:
                {
                    GameObject Obj = Instantiate(m_warehouse, _trans);
                    Obj.AddComponent<NavMeshSourceTag>();
                    Obj.AddComponent<BoxCollider>();
                    Obj.transform.parent = m_CityParent.transform;
                    break;
                }
            case BuildingsType.WINDMILL:
                {
                    GameObject Obj = Instantiate(m_windmill, _trans);
                    List<GameObject> childrenList = new List<GameObject>();
                    Transform[] children = Obj.GetComponentsInChildren<Transform>(true);
                    for (int i = 0; i < children.Length; i++)
                    {
                        Transform child = children[i];
                        if (child != Obj.transform)
                        {
                            childrenList.Add(child.gameObject);
                        }
                    }
                    for (int i = 1; i < childrenList.Count; i++)
                    {
                        childrenList[i].AddComponent<NavMeshSourceTag>();
                        childrenList[i].AddComponent<BoxCollider>();
                    }

                    Obj.transform.parent = m_CityParent.transform;
                    break;
                }
            case BuildingsType.NONE:
                {
                    break;
                }
            default:
                {
                    throw new System.Exception("Es wurde kein gültiger BuildingsType übergeben");
                }
        }
    }

    /// <summary>
    /// Create a Building
    /// </summary>
    /// <param name="_type">Type of the Building</param>
    /// <param name="_pos">Position of the Building</param>
    public void Create(BuildingsType _type, Vector3 _pos)
    {
        GameObject tmp_gameObject = new GameObject();
        tmp_gameObject.transform.position = _pos;
        tmp_gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        tmp_gameObject.transform.localScale = new Vector3(1, 1, 1);

        Create(_type, tmp_gameObject.transform);
        Destroy(tmp_gameObject);
    }

    /// <summary>
    /// Create a Building
    /// </summary>
    /// <param name="_type">Type of the Building</param>
    /// <param name="_pos">Position of the Building</param>
    /// <param name="_scale">Scale of the Building</param>
    public void Create(BuildingsType _type, Vector3 _pos, Vector3 _scale)
    {
        GameObject tmp_gameObject = new GameObject();
        tmp_gameObject.transform.position = _pos;
        tmp_gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        tmp_gameObject.transform.localScale = _scale;

        Create(_type, tmp_gameObject.transform);
        Destroy(tmp_gameObject);
    }

    /// <summary>
    /// Create a Building
    /// </summary>
    /// <param name="_type">Type of the Building</param>
    /// <param name="_pos">Position of the Building</param>
    /// <param name="_scale">Scale of the Building</param>
    /// <param name="_rotation">Rotation of the Building</param>
    public void Create(BuildingsType _type, Vector3 _pos, Vector3 _scale, Quaternion _rotation)
    {
        GameObject tmp_gameObject = new GameObject();
        tmp_gameObject.transform.position = _pos;
        tmp_gameObject.transform.rotation = _rotation;
        tmp_gameObject.transform.localScale = _scale;

        Create(_type, tmp_gameObject.transform);
        Destroy(tmp_gameObject);
    }
}
