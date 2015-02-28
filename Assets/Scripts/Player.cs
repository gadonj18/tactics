using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	private string playerName;
	private Sprite avatar;
	private int level;
	private int exp;
	private int maxHP;
	private int currentHP;
	private int maxMP;
	private int currentMP;
	private int maxCT;
	private int currentCT;
	private GameObject currentTile;
	private bool moving = false;
	private float walkSpeed = 0.4f;
	private float runSpeed = 0.9f;
	
	public void MoveTo(GameObject tile) {
		if(!moving) StartCoroutine("MoveToTile", tile);
	}
	
	private IEnumerator MoveToTile(GameObject tile) {
		moving = true;
		Vector3 targetPos = new Vector3(tile.transform.position.x, transform.position.y, tile.transform.position.z);
		float distance = Vector3.Distance(targetPos, transform.position);
		animation.Play(distance > 2f ? "Run" : "Walk");
		
		transform.rotation = Quaternion.LookRotation(targetPos - transform.position) * Quaternion.Euler(-15f, 0f, 0f);
		while(transform.position != targetPos) {
			transform.position = Vector3.MoveTowards (transform.position, targetPos, Time.deltaTime * (distance > 2 ? runSpeed : walkSpeed));
			yield return null;
		}
		moving = false;
		animation.Stop();
		animation.Play("Idle_01");
	}
}