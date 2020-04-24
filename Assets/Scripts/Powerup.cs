using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _tripleShotSpeed = 3.0f;
    [SerializeField]
    private int _powerupID;
    [SerializeField]
    private AudioClip _clip;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _tripleShotSpeed * Time.deltaTime);
        if (transform.position.y < -4f)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player playerPowerUp = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_clip, transform.position);

            if (playerPowerUp != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        playerPowerUp.TripleShotActive();
                        break;
                    case 1:
                        playerPowerUp.SpeedBoostActive();
                        break;
                    case 2:
                        playerPowerUp.ShieldPowerActive();
                        break;
                    default:
                        Debug.Log("Default values");
                            break;
                }
            }
            
            Destroy(this.gameObject);
        }
    }
}
