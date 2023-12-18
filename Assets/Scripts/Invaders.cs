using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Invaders : MonoBehaviour
{
    [SerializeField] private Enemy[] prefabs;
    [SerializeField] private Projectile bulletPrefab;
    [SerializeField] private int rows = 5;
    [SerializeField] private int columns = 11;
    [SerializeField] private AnimationCurve moveSpeed;
    [SerializeField] private float shootRate;
    private Vector3 _direction = Vector2.right;
    public int totalInvaders => this.rows * this.columns;
    public int countDestroyed { get; private set; }
    public int countAlived => this.totalInvaders - this.countDestroyed;
    public float percentDestroyed => (float)this.countDestroyed / (float)this.totalInvaders;
    private bool _shootAble;
    
    private void Start() {

        InvokeRepeating(nameof(Shoot), this.shootRate, this.shootRate);
    }

    private void Awake()
    {
        for(int row = 0; row < this.rows; row++)
        {
            float width = 1.0f * (this.columns - 1);
            float height = 1.0f * (this.rows - 1);
            Vector2 centering = new Vector2(-width / 2, -height / 2);
            Vector3 rowPosition = new Vector3(centering.x, centering.y + (row * 1.0f), 0.0f);

            for(int col = 0; col < this.columns; col++)
            {
                Enemy invader = Instantiate(this.prefabs[row], this.transform);
                invader.destroyed += InvaderDestroyed;
                Vector3 position = rowPosition;
                position.x += col * 1f;
                invader.transform.position = position;
                invader.transform.localPosition = position;
            }
        }
    }
    
    private void Update()
    {
        this.transform.position += _direction * this.moveSpeed.Evaluate(percentDestroyed) * Time.deltaTime;

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        
        foreach(Transform invader in this.transform)
        {
            Collider2D other = new Collider2D();

            if(!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            if(_direction == Vector3.right && invader.position.x >= (rightEdge.x - 12.0f))
            {
                AdvanceRow();
            }
            else if(_direction == Vector3.left && invader.position.x <= (leftEdge.x + 12.0f))
            {
                AdvanceRow();
            }
        }
    }

    private void AdvanceRow()
    {
        _direction.x *= -1.0f;

        Vector3 position = this.transform.position;
        position.y -= 1.0f;
        this.transform.position = position;
    }

    private void Shoot()
    {
        foreach(Transform invader in this.transform)
        {
            if(!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            if(!_shootAble)
            {
                if(Random.value < (1.0f / (float)this.countAlived))
                {
                    Projectile projectile = Instantiate(this.bulletPrefab, invader.position, Quaternion.identity);
                    projectile.destroyed += BulletDestroyed;
                    _shootAble = true;
                    
                }        
            }
        }
    }

    private void BulletDestroyed()
    {
        _shootAble = false;
    }

    private void InvaderDestroyed()
    {
        this.countDestroyed++;

        if(this.countDestroyed >= this.totalInvaders)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
