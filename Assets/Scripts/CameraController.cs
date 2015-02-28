using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {
	public Game gameLogic;
	
	private List<Vector2> CameraPositions = new List<Vector2>() {
		new Vector2(-4.5f, -4.5f),
		new Vector2(4.5f, -4.5f),
		new Vector2(4.5f, 4.5f),
		new Vector2(-4.5f, 4.5f)
	};
	private int currentCameraRotation = 0;
	private Vector3 targetCameraPosition;
	private Quaternion targetCameraRotation;
	
	public void RotateRight() {
		currentCameraRotation++;
		if(currentCameraRotation > 3) currentCameraRotation = 0;
	}
	
	public void RotateLeft() {
		currentCameraRotation--;
		if(currentCameraRotation < 0) currentCameraRotation = 3;
	}
	
	// Update is called once per frame
	void Update () {
		targetCameraPosition = new Vector3(
			gameLogic.CurrentPlayer.transform.position.x + CameraPositions[currentCameraRotation].x,
			4f,
			gameLogic.CurrentPlayer.transform.position.z + CameraPositions[currentCameraRotation].y
			);
		targetCameraRotation = Quaternion.LookRotation(gameLogic.CurrentPlayer.transform.position - targetCameraPosition);
		if(transform.position != targetCameraPosition) {
			transform.position = Vector3.Lerp(transform.position, targetCameraPosition, Time.deltaTime * 3f);
		}
		if(transform.rotation != targetCameraRotation) {
			transform.rotation = Quaternion.Lerp(transform.rotation, targetCameraRotation, Time.deltaTime * 4.5f);
		}
	}
}