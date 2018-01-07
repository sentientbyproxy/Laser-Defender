using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {
    public GameObject projectile;
    public float projectileSpeed = 10f;
    public float health = 150f;
    public float shotsPerSecond = 1f;
    public int scoreValue = 150;
    public GameObject Explosion;

    public AudioClip EnemyLaser;
    public AudioClip EnemyExplosion;

    private ScoreKeeper scoreKeeper;

    void Start(){
        scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
    } 

    void Update() {
        float probability = Time.deltaTime * shotsPerSecond;
        if (Random.value < probability) {
            Fire();
        }
    }
    void Fire() {
        Vector3 offset = new Vector3(0, -1.0f, 0);
        Vector3 firePos = transform.position + offset;
        GameObject missile = Instantiate(projectile, firePos, Quaternion.identity) as GameObject;
        missile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(EnemyLaser, transform.position);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        Projectile missile = collider.gameObject.GetComponent<Projectile>();
        if (missile) {
            health -= missile.GetDamage();
            missile.Hit();
            if (health <= 0) {
                Die();
            }
        }
    }
    void Die() {
        AudioSource.PlayClipAtPoint(EnemyExplosion, transform.position);
        GameObject Explodey = Instantiate(Explosion, transform.position, Quaternion.identity) as GameObject;
        scoreKeeper.Score(scoreValue);
        Destroy(gameObject);
    }
}
