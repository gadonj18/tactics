using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	public int BoardX;
	public int BoardY;
	public Color color;
	public Game gameLogic;

	void OnMouseOver() {
		if(gameLogic.state == Game.States.Move) renderer.material.color = Color.magenta;
	}

	void OnMouseExit() {
		if(gameLogic.state == Game.States.Move) renderer.material.color = color;
	}
}