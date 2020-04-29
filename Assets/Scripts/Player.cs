using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    private bool _isTripleShotActive;
    private bool _isSpeedBoostActive;
    private bool _isShieldsActive;
    [SerializeField]
    private int _score;
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _leftEngine, _rightEngine;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioSource _emptyGunAudio;
    [SerializeField]
    private AudioClip _laserSoundClip;
    private int _shieldLives = 3;
    private int _ammoCount = 15;
    private bool _stopLaser = true;
    [SerializeField]
    private GameObject _multiShotPrefab;
    private bool _isMultiShotActive;
    [SerializeField]
    private float _thrusterSpeedRate = 0.5f;
    private float _canThrusterSpeed = -1f;
    [SerializeField]
    private Camera _mainCamera;
    private CameraShake _shake;
   
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0,0,0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas_UI").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _shake = GameObject.Find("Main Camera").GetComponent<CameraShake>();

        _rightEngine.SetActive(false);
        _leftEngine.SetActive(false);

        if (_shake == null)
        {
            Debug.LogError("Shake is NULL");
        }

        if (_spawnManager == null)
        {
            Debug.LogError("The spawn manager is NULL");
        }

        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is NULL");
        }

        if (_audioSource == null)
        {
            Debug.LogError("Audio Source on the player is NULL");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > _canThrusterSpeed)
        {
            _canThrusterSpeed = Time.time + _thrusterSpeedRate;
            SpeedIncrease(2.0f);
            
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            SpeedDecrease(2.0f);            
        }

       
    }

    void PlayerMovement()
    {
        float _horizontalInput = Input.GetAxis("Horizontal");
        float _verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(_horizontalInput, _verticalInput, 0);

       
            transform.Translate(direction * _speed * Time.deltaTime);
        

        if (transform.position.y >= 6f)
        {

            transform.position = new Vector3(transform.position.x, 6f, 0);
        }
        else if (transform.position.y <= -4f)
        {
            transform.position = new Vector3(transform.position.x, -4f, 0);
        }

        if (transform.position.x >= 9f)
        {
            transform.position = new Vector3(-9f, transform.position.y, 0);
        }
        else if (transform.position.x <= -9f)
        {
            transform.position = new Vector3(9f, transform.position.y, 0);
        }

    }
    
    void FireLaser()
    {
            _canFire = Time.time + _fireRate;

            if (_isTripleShotActive == true && _stopLaser == true)
            {

                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
                 _ammoCount -= 3;
                 _audioSource.Play();
            }
            else if (_stopLaser == true && _isMultiShotActive == false)
            {
                var _offset = new Vector3(0, 1.05f, 0);
                var _laserPosition = transform.position;
                Instantiate(_laserPrefab, _laserPosition + _offset, Quaternion.identity);
               _ammoCount--;
               _audioSource.Play();
            }

            else if (_isMultiShotActive == true && _stopLaser == true) //if not available ammo you can't have this powerup but unlimited if you have ammo
            {
                Instantiate(_multiShotPrefab, transform.position, Quaternion.identity);
                 _audioSource.Play();
            }

             if (_ammoCount < 1)
             {
                _ammoCount = 0;
                _stopLaser = false;
               _emptyGunAudio.Play();
             }
    }

    public void Damage()
    {
        if (_isShieldsActive == true)
        {
            _shieldLives--;
            
            switch (_shieldLives)
            {
                case 2:
                     _shieldVisualizer.GetComponent<SpriteRenderer>().color = Color.green;
                    break;
                case 1:
                    _shieldVisualizer.GetComponent<SpriteRenderer>().color = Color.red;
                    break;
                case 0:
                    _isShieldsActive = false;
                    _shieldVisualizer.SetActive(false);      
                    break;
            }     
        }

        else

        {
            _lives--;

            _uiManager.UpdateLives(_lives);

            if (_lives == 2)
            {
                _rightEngine.SetActive(true);
                _shake.Shake();
                 
            }

            else if (_lives == 1)
            {
                _leftEngine.SetActive(true);
                _shake.Shake();
            } 

            if (_lives < 1)
            {
                _spawnManager.OnPlayerDeath();
                _shake.Shake();
                Destroy(this.gameObject);
            }
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
    }

    public void MultiShotActive()
    {
        _isMultiShotActive = true;
        _stopLaser = true;
        StartCoroutine(MultiShotDownRoutine());
    }

    IEnumerator MultiShotDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isMultiShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= +_speedMultiplier;
        StartCoroutine(SpeedPowerDownRoutine());
        
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }
  
    public void ShieldPowerActive()
    {
        _isShieldsActive = true;
        _shieldVisualizer.SetActive(true);
        _shieldLives = 3;
        _shieldVisualizer.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void AmmoRefillActive()
    {
        _ammoCount = 15;
        _stopLaser = true;
    }

    public void ExtraLifeActive()
    {
        _lives++;
        _uiManager.UpdateLives(_lives);

        if (_lives == 3)
        {
            _leftEngine.SetActive(false);
            _rightEngine.SetActive(false);
        }

        else if (_lives == 2)
        {
            _rightEngine.SetActive(true);
            _leftEngine.SetActive(false);
        }
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
    
   public void SpeedIncrease(float speedAmount)
    {
          _speed += speedAmount;
        _uiManager.ThrusterBarUp();
        
    }

  public void SpeedDecrease(float speedAmount)
    {
        _speed -= speedAmount;
        _uiManager.ThrusterBarDown();
    }

    

}
