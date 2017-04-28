using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTargetSpawner : MonoBehaviour {

    // Update is called once per frame
    float surfaceOffset = .01f;
    void Update ()
    {
        if(!Input.GetMouseButtonDown(1))
            return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(!Physics.Raycast(ray, out hit))
        {
            return;
        }
        var pos = hit.point + hit.normal * surfaceOffset;
        var go = Instantiate(targetPrefab, pos, Quaternion.identity) as GameObject;
        go.GetComponent<MeshRenderer>().materials[0].color = Color.green;
        go.AddComponent<DestroyAfterTime>();
	}

    public GameObject targetPrefab;

}
