using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelInfo : MonoBehaviour {

    public Vector3 position_Offset;
    public Quaternion rotation_Offset;
    public Vector3 scale_Offset;
    public GameObject model;

    public ModelInfo(GameObject _model, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        model = _model;
        position_Offset = position;
        rotation_Offset = rotation;
        scale_Offset = scale;
    }

    public ModelInfo(GameObject _model, Vector3 position, Vector3 rotation, Vector3 scale)
    {
        model = _model;
        position_Offset = position;
        rotation_Offset = Quaternion.Euler(rotation);
        scale_Offset = scale;
    }

    public ModelInfo(GameObject _model)
    {
        model = _model;
        rotation_Offset = Quaternion.identity;
        position_Offset = Vector3.zero;
        scale_Offset = Vector3.zero;
    }
}
