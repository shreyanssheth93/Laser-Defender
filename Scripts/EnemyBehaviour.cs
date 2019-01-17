using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

	public GameObject projectile;
	public float projectileSpeed = 10f;
	public float health = 150;
	public float shotsPerSeconds = 0.5f;
	public int scoreValue = 150;

	public AudioClip FireSound;
	public AudioClip DeathSound;

	private ScoreKeeper scoreKeeper;

	void Start(){
		scoreKeeper = GameObject.Find ("Score").GetComponent<ScoreKeeper> ();
	}

	void Update(){
		float probability = shotsPerSeconds * Time.deltaTime;
		if (Random.value < probability) {
			Fire ();
		}
	}


	void Fire(){
		GameObject missile = Instantiate (projectile, transform.position, Quaternion.identity)as GameObject;
		missile.GetComponent<Rigidbody2D>().velocity = new Vector2 (0, -projectileSpeed);
		AudioSource.PlayClipAtPoint (FireSound,transform.position);
	}

	void OnTriggerEnter2D(Collider2D collider){


		Projectile missile = collider.gameObject.GetComponent<Projectile> ();
		if (missile) {

			health -= missile.GetDamage();
			missile.Hit();
			if ( health <= 0){
				Die();
			}
		}
	}
	void Die(){
		AudioSource.PlayClipAtPoint(DeathSound,transform.position);
		Destroy(gameObject);
		scoreKeeper.Score(scoreValue);
	}
}
