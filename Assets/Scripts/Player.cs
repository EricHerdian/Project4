using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Projectile bulletPrefab;
    private int lives = 3;
    private Vector2 direction = new Vector2();
    private bool _shootAble;
    public Image[] live;
    public GameObject livesNumber;
    public GameObject panel;

    private void Update() {
        InputHandler();
        PositionUpdate(direction);
    }

    private void InputHandler()
    {
        direction.x = Input.GetAxisRaw("Horizontal");
        if(Input.GetButtonDown("Fire1")){
            Shoot();
        }
    }

    public void PositionUpdate(Vector2 direction)
    {
        rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
    }

    public void Shoot()
    {
        if(!_shootAble)
        {
            Projectile projectile = Instantiate(this.bulletPrefab, this.transform.position, Quaternion.identity);
            projectile.destroyed += BulletDestroyed;
            _shootAble = true;
        }
    }

    private void BulletDestroyed()
    {
        _shootAble = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyBullet")
        {
            if(lives > 0)
            {
                lives--;
                live[lives].enabled = false;
                livesNumber.GetComponent<TextMeshProUGUI>().text = lives.ToString();
            }
            if(lives == 0)
            {
                panel.SetActive(true);
            }
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
