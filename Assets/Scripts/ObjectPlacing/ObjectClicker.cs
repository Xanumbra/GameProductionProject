using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectClicker : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log("Object Clicker Enabled");
    }

    private void OnDisable()
    {
        Debug.Log("Object Clicker Disabled");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                PrintName(hit.transform.gameObject);
                Enums.BuildingType type = Enums.BuildingType.None;
                if (hit.transform.gameObject.name.Contains("Edge"))
                {
                    type = Enums.BuildingType.Road;
                }
                else if (hit.transform.gameObject.name.Contains("Vertex"))
                {
                    type = Enums.BuildingType.Settlement;
                }

                Player.localPlayer.PlaceBuilding(hit.transform.gameObject, type);
            }
            else
            {
                Debug.Log("Nothing");
            }
        }
    }
    private void PrintName(GameObject go)
    {
        Debug.Log(go.name);
    }
}
