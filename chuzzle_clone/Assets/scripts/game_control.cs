﻿using UnityEngine;
using System.Collections;

public class game_control : MonoBehaviour {
	public Transform grid_spawn_transform;
	public int grid_width = 5;
	public int grid_height = 7;
	public float grid_spacing = 1f;
	public GameObject test_ball;
	public static Vector3 mouse_position_when_clicked;
	public static bool dragging_balls_active = false;
	public static Vector3 drag_offset;
	void Start() {
		generate_matrix();
	}

	void generate_matrix() {
		for (int i = 0; i < grid_width; i++) {
			for (int j = 0; j < grid_height; j++) {
				GameObject generic_ball = new GameObject();
				int ball_index = (i * grid_width + j);
				generic_ball.name = "ball_" + ball_index.ToString();
				generic_ball.AddComponent<ball>().create_ball(i, j, ball_index);
				generic_ball.transform.position = new Vector3(i * grid_spacing, j * grid_spacing, 0) + grid_spawn_transform.position;
			}
		}
	}

	public static GameObject[] all_balls() {
		return GameObject.FindGameObjectsWithTag("ball");
	}

	public static Vector3 mouse_position() {
		Vector3 converted_mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		converted_mouse_pos.z = -1;

		return converted_mouse_pos;
	}

	public static ball get_ball(GameObject g) {
		return g.GetComponent<ball>();
	}

	public static void ball_is_movable(GameObject ball, bool is_movable) {
		game_control.get_ball(ball).is_moving = is_movable;
	}

	public static bool ball_is_movable(GameObject ball) {
		return game_control.get_ball(ball).is_moving;
	}

	public static void store_mouse_position_when_clicked() {
		mouse_position_when_clicked = game_control.mouse_position();
	}


	void Update() {

		//calculate drag offset
		if (Input.GetMouseButton(0)) {
			float distance_x = game_control.mouse_position().x - mouse_position_when_clicked.x;
			float distance_y = game_control.mouse_position().y - mouse_position_when_clicked.y;
			//print("X:" + distance_x + " Y:" + distance_y);
			drag_offset = new Vector3(distance_x, distance_y, 0);
		}


	}


	//DEBUG SCREEN
	void OnGUI() {
		int w = Screen.width, h = Screen.height;
		GUIStyle style = new GUIStyle();
		Rect rect = new Rect(0, 0, w, h * 2 / 100);
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 2 / 100;
		style.normal.textColor = Color.cyan;

		//stuff
		string text = "Drag Offset: \n\tX:" + drag_offset.x + "\n\tY:" + drag_offset.y;

		//

		GUI.Label(rect, text, style);
	}


}
