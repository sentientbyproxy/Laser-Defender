
using UnityEngine;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public GameObject laser;
    public GameObject SmallExplosion;
    public GameObject DeathExplosion;
    public float projectileSpeed = 10;
	public float projectileRepeatRate = 0.2f;
	
	public float speed = 15.0f;
	public float padding = 1;
    public float health = 250;

    public AudioClip PlayerLaser;
    public AudioClip PlayerExplosion;

    private float xmax = -5;
	private float xmin = 5;

    void OnTriggerEnter2D(Collider2D collider)
    {
        Projectile missile = collider.gameObject.GetComponent<Projectile>();
        if (missile) {
            health -= missile.GetDamage();
            missile.Hit();
            GameObject smallExplodey = Instantiate(SmallExplosion, transform.position, Quaternion.identity) as GameObject;
            if (health <= 0) {
                Die();
            }
        }
    }

    void Die() {
        LevelManager man = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        man.LoadLevel("Win Screen");
        Destroy(gameObject);
        GameObject deathExplodey = Instantiate(DeathExplosion, transform.position, Quaternion.identity) as GameObject;
        AudioSource.PlayClipAtPoint(PlayerExplosion, transform.position);
    }

    void Start(){
		Camera camera = Camera.main;
		float distance = transform.position.z - camera.transform.position.z;
		xmin = camera.ViewportToWorldPoint(new Vector3(0,0,distance)).x + padding;
		xmax = camera.ViewportToWorldPoint(new Vector3(1,1,distance)).x - padding;
	}

	void Fire(){
        Vector3 offset = new Vector3(0, 1, 0);
		GameObject beam = Instantiate(laser, transform.position, Quaternion.identity) as GameObject;
		beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed, 0);
        AudioSource.PlayClipAtPoint(PlayerLaser, transform.position);
	}
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			InvokeRepeating("Fire", 0.0001f, projectileRepeatRate);
		}
		if(Input.GetKeyUp(KeyCode.Space)){
			CancelInvoke("Fire");
		}
		if(Input.GetKey(KeyCode.LeftArrow)){
			transform.position = new Vector3(
				Mathf.Clamp(transform.position.x - speed * Time.deltaTime, xmin, xmax),
				transform.position.y, 
				transform.position.z 
			);
		}else if (Input.GetKey(KeyCode.RightArrow)){
			transform.position = new Vector3(
				Mathf.Clamp(transform.position.x + speed * Time.deltaTime, xmin, xmax),
				transform.position.y, 
				transform.position.z 
			);
		}
	}
}