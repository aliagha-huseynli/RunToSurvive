using System;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;


public class CharacterControl : MonoBehaviour
{
    [SerializeField] private Sprite[] IdleAnimation;
    [SerializeField] private Sprite[] JumpingAnimation;
    [SerializeField] private Sprite[] MovementAnimation;
    [SerializeField] private Text HealthPercentageText;
    [SerializeField] private Text goldText;
    [SerializeField] private Text totalGoldText;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject finishScreen;

    private int _healthPercentage = 100;
    private int HealthPercentage
    {
        get => _healthPercentage;
        set
        {
            if (value <= 0)
            {
                value = 0;
                isDead = true;
                ShowDeathScene();
            }
            _healthPercentage = value;
        }
    }

    //Audio of Game
    public AudioClip coinAudioClip;
    public AudioClip walkingAudioClip;
    public AudioClip bulletAudioClip;
    public AudioClip chestAudioClip;
    public AudioClip finishAudioClip;

    [SerializeField] private AudioSource audioSource;


    private bool isDead = false;
    private int goldQty = 0;
    public bool onPlatform = false;
    private Platform currentPlatform;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _physics;
    private Vector3 _vector3;
    private Vector3 _camLastPositionVector3;
    private Vector3 _camFirstPositionVector3;

    private float _horizontal; // default value is Zero
    private float _idleAnimationTime; // default value is Zero
    private float _movementAnimationTime; // default value is Zero

    public bool _oneTimeJump = true;

    private int _idleAnimationCount; // default value is Zero
    private int _movementAnimationCount; // default value is Zero

    private GameObject _camGameObject;

    private bool playSFX = true;

    private void Start()
    {
        Time.timeScale = 1;
        isDead = false;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        _physics = GetComponent<Rigidbody2D>();
        _camGameObject = GameObject.FindGameObjectWithTag("MainCamera");
        _camFirstPositionVector3 = _camGameObject.transform.position - transform.position;
        playSFX = Boolean.Parse(PlayerPrefs.GetString("sfx"));
        audioSource.enabled = playSFX;
        UpdateHpLabel();
        UpdateGoldLabel();
    }

    private void ShowDeathScene()
    {
        Time.timeScale = 0;
        audioSource.Stop();
        deathScreen.SetActive(true);
        totalGoldText.text = "Total gold: " + goldQty;
    }

    private void ShowFinishScene()
    {
        Time.timeScale = 0;
        audioSource.Stop();
        finishScreen.SetActive(true);
        totalGoldText.text = "Total gold: " + goldQty;
    }

    private void LateUpdate()
    {
        CamControl();
    }

    private void FixedUpdate()
    {

        if (!isDead)
        {
            CharacterMovement();
            Animation();
            Jump();
            if (_oneTimeJump)
            {
                _physics.gravityScale = 4;
            }
            else
            {
                _physics.gravityScale = 1;
            }

        }


    }

    private void Jump()
    {
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            if (_oneTimeJump)
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
                _physics.velocity = new Vector2(_physics.velocity.x, 0);
                _physics.AddForce(new Vector2(0, 500));
                _oneTimeJump = false;
            }
        }
    }

    private void CharacterMovement()
    {
        _horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        if (_horizontal == 0)
        {
            //audioSource.clip = walkingAudioClip;
            //audioSource.Stop();
        }
        else
        {
            if (!audioSource.isPlaying && _oneTimeJump)
            {
                audioSource.clip = walkingAudioClip;
                audioSource.Play();
            }

        }
        _vector3 = new Vector3(_horizontal * 10, _physics.velocity.y, 0);
        _physics.velocity = _vector3;

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            isDead = true;
            ShowDeathScene();
        }

        _oneTimeJump = true;

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.CompareTag("Platform"))
        {
            onPlatform = true;
            if (transform.parent == null)
            {
                transform.parent = other.transform;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.CompareTag("Platform"))
        {
            //print("qqq");
            onPlatform = false;
            if (transform.parent != null)
            {
                transform.SetParent((null));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            audioSource.clip = bulletAudioClip;
            audioSource.Play();
            ReduceHealth((5));
            Destroy(other.gameObject);
            UpdateHpLabel();

        }

        if (other.gameObject.tag == "Enemy")
        {
            ReduceHealth((0));
            UpdateHpLabel();
        }

        if (other.gameObject.tag == "Saw")
        {
            audioSource.clip = bulletAudioClip;
            audioSource.Play();
            ReduceHealth((10));
            UpdateHpLabel();
        }

        if (other.gameObject.tag == "Gold")
        {
            audioSource.clip = coinAudioClip;
            audioSource.Play();
            Destroy(other.gameObject);
            goldQty++;
            UpdateGoldLabel();

        }

        if (other.gameObject.tag == "Gold Chest")
        {
            audioSource.clip = chestAudioClip;
            audioSource.Play();
            other.GetComponent<BoxCollider2D>().enabled = false;
            other.GetComponent<GoldChestAnimaton>().enabled = true;
            IncreaseGold(10);
            Destroy(other.gameObject, 2);
            UpdateGoldLabel();

        }

        if (other.gameObject.tag == "Health Chest")
        {
            audioSource.clip = chestAudioClip;
            audioSource.Play();
            other.GetComponent<BoxCollider2D>().enabled = false;
            other.GetComponent<HealthChestAnimation>().enabled = true;
            if (HealthPercentage != 100)
            {
                IncreaseHealth(10);
                Destroy(other.gameObject, 2);
                UpdateHpLabel();
            }
            else
            {
                Destroy(other.gameObject, 2);
                UpdateHpLabel();
            }
           
            

        }

        if (other.gameObject.tag == "Finish")
        {
            audioSource.clip = finishAudioClip;
            audioSource.Play();
            ShowFinishScene();
        }

        if (other.gameObject.tag == "LimitMap")
        {
            ShowDeathScene();
        }

    }

    private void IncreaseGold(int quantity)
    {
        goldQty += quantity;
    }

    private void IncreaseHealth(int quantity)
    {
        HealthPercentage += quantity;
    }

    private void ReduceHealth(int quantity)
    {
        HealthPercentage -= quantity;
    }
    private void UpdateHpLabel()
    {
        HealthPercentageText.text = "" + HealthPercentage;
    }

    private void UpdateGoldLabel()
    {
        goldText.text = "" + goldQty;
    }
    private void CamControl()
    {
        _camLastPositionVector3 = _camFirstPositionVector3 + transform.position;
        _camGameObject.transform.position = Vector3.Lerp(_camGameObject.transform.position, _camLastPositionVector3, 0.07f);
    }

    private void Animation()
    {
        if (_oneTimeJump)
        {
            if (_horizontal == 0)
            {
                _idleAnimationTime += Time.deltaTime;
                if (_idleAnimationTime > 0.05f)
                {
                    _spriteRenderer.sprite = IdleAnimation[_idleAnimationCount++];
                    if (_idleAnimationCount == IdleAnimation.Length)
                    {
                        _idleAnimationCount = 0;

                    }
                    _idleAnimationTime = 0;

                }

            }
            else if (_horizontal > 0)
            {
                _movementAnimationTime += Time.deltaTime;
                if (_movementAnimationTime > 0.01f)
                {
                    _spriteRenderer.sprite = MovementAnimation[_movementAnimationCount++];
                    if (_movementAnimationCount == MovementAnimation.Length)
                    {
                        _movementAnimationCount = 0;

                    }
                    _movementAnimationTime = 0;

                }
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (_horizontal < 0)
            {
                _movementAnimationTime += Time.deltaTime;
                if (_movementAnimationTime > 0.01f)
                {
                    _spriteRenderer.sprite = MovementAnimation[_movementAnimationCount++];
                    if (_movementAnimationCount == MovementAnimation.Length)
                    {
                        _movementAnimationCount = 0;

                    }
                    _movementAnimationTime = 0;

                }
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {
            _spriteRenderer.sprite = _physics.velocity.y > 0 ? JumpingAnimation[0] : JumpingAnimation[1];

            if (_horizontal > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);

            }
            else if (_horizontal < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);

            }
        }
    }
}
