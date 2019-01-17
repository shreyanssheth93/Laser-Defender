using UnityEngine;
using System.Collections;

public class PlayerShip : MonoBehaviour {
		
	public float speed = 15.0f;
	public float padding = 0.5f;
	public GameObject projectile;
	public float projectileSpeed;
	public float firingRate = 0.2f;
	public float health = 250;

	public AudioClip FireSound;
	public AudioClip DeathSound;

	float xmin ;
	float xmax ;


	void Start() {
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 righttmost = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));

		xmin = leftmost.x + padding;
		xmax = righttmost.x - padding;
	}


	void Fire() {
		Vector3 offset = new Vector3 (0, 1, 0);

		GameObject beam = Instantiate(projectile,transform.position + offset,Quaternion.identity)as GameObject;
		beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0,projectileSpeed,0);
		AudioSource.PlayClipAtPoint (FireSound, transform.position);
	}


	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.Space)) {
			InvokeRepeating("Fire", 0.00001f,firingRate);
		}

		if (Input.GetKeyUp (KeyCode.Space)) {
			CancelInvoke("Fire");
		}


		if (Input.GetKey(KeyCode.LeftArrow))
		{
			//transform.position += new Vector3(- speed * Time.deltaTime, 0 , 0);

			transform.position += Vector3.left * speed * Time.deltaTime;
		} 

		else if (Input.GetKey(KeyCode.RightArrow)) 
		{
			//transform.position += new Vector3( speed * Time.deltaTime, 0 , 0);

			transform.position += Vector3.right * speed * Time.deltaTime;
		}
		//restrict the player to the game space
		float newX = Mathf.Clamp (transform.position.x, xmin, xmax);
		transform.position = new Vector3 (newX, transform.position.y, transform.position.z);



	}

	void OnTriggerEnter2D(Collider2D collider){
		//Debug.Log (collider);
		
		Projectile missile = collider.gameObject.GetComponent<Projectile> ();
		if (missile) {
			
			health -= missile.GetDamage();
			missile.Hit();
			if ( health <= 0){
				Die();
			}
			//Debug.Log("Hit by a Projectile");
			
		}
	}

	void Die(){
		//LevelManager man = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		var man = FindObjectOfType<LevelManager> ();
		man.LoadLevel ("Win Screen");
		AudioSource.PlayClipAtPoint(DeathSound,transform.position);
		Destroy(gameObject);
	}
}
