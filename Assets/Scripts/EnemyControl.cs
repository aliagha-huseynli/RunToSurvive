using UnityEngine;
#if UNITY_EDITOR // This is for just editor mode. When game will be build that will not be build!
using UnityEditor;
#endif

public class EnemyControl : MonoBehaviour
{
    private int _image;
    private GameObject[] _pointToGoGameObject;
    private bool _getDistanceOnce = true; // Distance between game objects
    private Vector3 _distanceBetweenVector3;
    private int _distanceCounter;
    private bool _forwardOrBack = true;
    private GameObject _character;
    private RaycastHit2D _rayCastHit2D;
    public LayerMask LayerMask;
    private int _speed;
    public Sprite FrontSideSprite;
    public Sprite BackSideSprite;
    private SpriteRenderer _spriteRenderer;
    public GameObject BulletGameObject;
    private float _fireTime; //Default value is Zero

    private void Start()
    {
        _pointToGoGameObject = new GameObject[transform.childCount];
        _character = GameObject.FindGameObjectWithTag("Player");
        _spriteRenderer = GetComponent<SpriteRenderer>();

        for (var i = 0; i < _pointToGoGameObject.Length; i++)
        {
            _pointToGoGameObject[i] = transform.GetChild(0).gameObject;
            _pointToGoGameObject[i].transform.SetParent(transform.parent);
        }
    }

    private void FixedUpdate()
    {
        DidHeSeeMe();
        if (_rayCastHit2D.collider.tag=="Player")
        {
            _speed = 8;
            _spriteRenderer.sprite = FrontSideSprite;
            Fire();
        }
        else
        {
            _speed = 4;
            _spriteRenderer.sprite = BackSideSprite;
        }
        GoPoints();
    }

    private void Fire()
    {
        _fireTime += Time.deltaTime;
        if (_fireTime>Random.Range(0.5f,1))
        {
            Instantiate(BulletGameObject,transform.position, Quaternion.identity);
            _fireTime = 0;
        }
    }

    private void DidHeSeeMe()
    {
        var rayDirectionVector3 = _character.transform.position - transform.position;
        _rayCastHit2D = Physics2D.Raycast(transform.position, rayDirectionVector3, 1000, LayerMask);
        Debug.DrawLine(transform.position, _rayCastHit2D.point, Color.magenta);
    }

    private void GoPoints() //This block for Enemy movement between the game objects
    {
        if (_getDistanceOnce)
        {
            _distanceBetweenVector3 = (_pointToGoGameObject[_distanceCounter].transform.position - transform.position).normalized;
            _getDistanceOnce = false;
        }

        var distance = Vector3.Distance(transform.position, _pointToGoGameObject[_distanceCounter].transform.position);
        transform.position += _distanceBetweenVector3 * Time.deltaTime * _speed;

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

    public Vector2 GetDirection()
    {
        return (_character.transform.position - transform.position).normalized;
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

[CustomEditor(typeof(EnemyControl))]
[System.Serializable]

public class EnemyControlEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var scriptSaw = (EnemyControl)target;
        EditorGUILayout.Space();


        if (GUILayout.Button("Create", GUILayout.MinWidth(100), GUILayout.Width(100)))
        {
            var newGameObject = new GameObject();
            newGameObject.transform.parent = scriptSaw.transform;
            newGameObject.transform.position = scriptSaw.transform.position;
            newGameObject.name = scriptSaw.transform.childCount.ToString();
        }
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("LayerMask"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("FrontSideSprite"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("BackSideSprite"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("BulletGameObject"));
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }
}
#endif
