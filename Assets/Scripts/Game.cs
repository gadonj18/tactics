using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {
	public enum States { ChooseAction, Move };
	public States state = States.ChooseAction;

	public GameObject tilePrefab;
	public GameObject trollPrefab;
	public GameObject golemPrefab;
	public Camera mainCamera;

	private List<GameObject> Players = new List<GameObject>();
	private int currentPlayerIdx;
	private List<Vector2> CameraPositions = new List<Vector2>() {
		new Vector2(-4.5f, -4.5f),
		new Vector2(4.5f, -4.5f),
		new Vector2(4.5f, 4.5f),
		new Vector2(-4.5f, 4.5f)
	};
	private int currentCameraRotation = 0;
	private Vector3 targetCameraPosition;
	private Quaternion targetCameraRotation;

	void Start () {
		//Create the level
		for (int x = 0; x < 9; x++) {
			for(int y = 0; y < 9; y++) {
				GameObject cube = (GameObject)GameObject.Instantiate(tilePrefab, new Vector3(x, 0f, y), Quaternion.Euler(0f, 0f, 0f));
				float colorCode = (x * 0.05f) + (y * 0.05f);
				Color color = new Color(colorCode, colorCode, colorCode);
				cube.renderer.material.color = color;
				cube.GetComponent<Tile>().BoardX = x;
				cube.GetComponent<Tile>().BoardY = y;
				cube.GetComponent<Tile>().color = color;
				cube.GetComponent<Tile>().gameLogic = this;
			}
		}

		//Create the characters
		GameObject troll = (GameObject)GameObject.Instantiate(trollPrefab, new Vector3 (4f, 0.5f, 0f), Quaternion.Euler(0f, 0f, 0f));
		GameObject golem = (GameObject)GameObject.Instantiate(golemPrefab, new Vector3 (4f, 0.5f, 8f), Quaternion.Euler(0f, 180f, 0f));
		Players.Add(troll);
		Players.Add(golem);
		currentPlayerIdx = 0;

		targetCameraPosition = new Vector3(
			Players[currentPlayerIdx].transform.position.x + CameraPositions[currentCameraRotation].x,
			4f,
			Players[currentPlayerIdx].transform.position.z + CameraPositions[currentCameraRotation].y
			);
	        targetCameraRotation = Quaternion.LookRotation(Players[currentPlayerIdx].transform.position - targetCameraPosition);
	}

	void Update () {
		bool cameraChanged = false;
		if(Input.GetKeyUp(KeyCode.RightArrow)) {
			currentCameraRotation++;
			if(currentCameraRotation > 3) currentCameraRotation = 0;
			cameraChanged = true;
		} else if(Input.GetKeyUp(KeyCode.LeftArrow)) {
			currentCameraRotation--;
			if(currentCameraRotation < 0) currentCameraRotation = 3;
			cameraChanged = true;
		}
		if(Input.GetMouseButtonUp(0)) {
			System.Reflection.MethodInfo method = this.GetType().GetMethod(state.ToString() + "_Click", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
			if(method != null) method.Invoke(this, null);
		}

		if(cameraChanged) {
			targetCameraPosition = new Vector3(
				Players[currentPlayerIdx].transform.position.x + CameraPositions[currentCameraRotation].x,
				4f,
	       		Players[currentPlayerIdx].transform.position.z + CameraPositions[currentCameraRotation].y
			);
	        targetCameraRotation = Quaternion.LookRotation(Players[currentPlayerIdx].transform.position - targetCameraPosition);
		}
		if(mainCamera.transform.position != targetCameraPosition) {
			mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetCameraPosition, Time.deltaTime * 3f);
		}
		if(mainCamera.transform.rotation != targetCameraRotation) {
			mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, targetCameraRotation, Time.deltaTime * 4.5f);
		}
	}

	private void Move_Click() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		if(Physics.Raycast(ray, out hit)) {
			if(hit.transform.gameObject.name == "Tile(Clone)") {
				Players[currentPlayerIdx].GetComponent<Player>().MoveTo(hit.transform.gameObject);
			}
		}
	}

	public void MenuMove_Click() {
		GameObject.Find("ActionMenu").SetActive(false);
		state = States.Move;
	}
}
