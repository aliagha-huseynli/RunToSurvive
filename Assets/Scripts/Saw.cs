using UnityEngine;
#if UNITY_EDITOR // This is for just editor mode. When game will be build that will not be build!
using UnityEditor;
#endif

public class Saw : MonoBehaviour
{
    private int _image;
    private GameObject[] _pointToGoGameObject;
    private bool _getDistanceOnce = true; // Distance between game objects
    private Vector3 _distanceBetweenVector3;
    private int _distanceCounter;
    private bool _forwardOrBack = true;

    private void Start()
    {
        _pointToGoGameObject = new GameObject[transform.childCount];

        for (var i = 0; i < _pointToGoGameObject.Length; i++)
        {
            _pointToGoGameObject[i] = transform.GetChild(0).gameObject;
            _pointToGoGameObject[i].transform.SetParent(transform.parent);
        }
    }

    private void FixedUpdate()
    {
        transform.Rotate(0, 0, 5); // Loop Rotate
        GoPoints();
    }

    private void GoPoints() //This block for saw movement between the game objects
    {
        if (_getDistanceOnce)
        {
            _distanceBetweenVector3 = (_pointToGoGameObject[_distanceCounter].transform.position - transform.position).normalized;
            _getDistanceOnce = false;
        }

        var distance = Vector3.Distance(transform.position, _pointToGoGameObject[_distanceCounter].transform.position);
        transform.position += _distanceBetweenVector3 * Time.deltaTime * 10;

        if (!(distance < 0.5f)) return;
        _getDistanceOnce = true;
        if (_distanceCounter == _pointToGoGameObject.Length - 1)
        {
            _forwardOrBack = false;
        }
        else if (_distanceCounter == 0)
        {
            _forwardOrBack = true;
        }
        if (_forwardOrBack)
        {
            _distanceCounter++;
        }
        else
        {
            _distanceCounter--;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() //It Draw Game Object for Saw
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.GetChild(i).transform.position, 1);
        }

        for (var i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.GetChild(i).transform.position, transform.GetChild(i + 1).transform.position);
        }
    }
#endif
}

#if UNITY_EDITOR

[CustomEditor(typeof(Saw))]
[System.Serializable]

public class SawEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var scriptSaw = (Saw)target;
        if (!GUILayout.Button("Create", GUILayout.MinWidth(100), GUILayout.Width(100))) return;
        var newGameObject = new GameObject();
        newGameObject.transform.parent = scriptSaw.transform;
        newGameObject.transform.position = scriptSaw.transform.position;
        newGameObject.name = scriptSaw.transform.childCount.ToString();
    }
}
#endif
