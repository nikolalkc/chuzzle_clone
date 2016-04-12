using UnityEngine;
using System.Collections;

public class ball : MonoBehaviour {
	public int color_index;
	public int grid_x, grid_y, ball_index;
	SpriteRenderer spr_this;
	public bool is_moving = false;
	Vector3 ball_position_when_clicked = Vector3.zero;

	#region hidden_stuff
	public void create_ball(int x, int y, int index) {
		//parameters
		grid_x = x;
		grid_y = y;
		ball_index = index;

		//sprite setup
		spr_this = gameObject.AddComponent<SpriteRenderer>();
		color_index = Random.Range(0, 10);
		spr_this.sprite = ball_sprite_ref.object_reference.ball_sprite[color_index];

		//components
		gameObject.AddComponent<BoxCollider2D>().isTrigger = true;
		gameObject.tag = "ball";

	}

	public static void store_current_positions_off_all_balls() {
		foreach (GameObject ball in game_control.all_balls()) {
			game_control.get_ball(ball).ball_position_when_clicked = ball.transform.position;
		}
	}

	void stop_dragging_balls() {
		//do the snapping
		snapping(game_control.drag_offset, game_control.moving_direction, false);

		//resetuj parametre
		game_control.moving_direction = game_control.direction.none;
		game_control.dragging_balls_active = false;
		game_control.mouse_position_when_clicked = Vector3.zero;

		//resetuj sacuvane pozicije
		foreach (GameObject ball in game_control.all_balls()) {
			game_control.get_ball(ball).ball_position_when_clicked = Vector3.zero;
		}

		//resetuj is moving
		foreach (GameObject ball in game_control.all_balls()) {
			if (ball.GetComponent<ball>().is_moving) {
				game_control.ball_is_movable(ball, false);
			}
		}

		//resetuj direction vektor
		game_control.direction_to_move_balls = Vector3.one;
	}

	void OnMouseUp() {
		game_control.clicked_ball = null;
		stop_dragging_balls();
	}
	#endregion

	void snapping(Vector3 offset, game_control.direction direction,bool call_from_update) {

		float half_spacing = game_control._grid_spacing / 2;

		//setovanje  distance
		float distance_from_start_position = 0;
		if (direction == game_control.direction.horizontal) {
			distance_from_start_position = offset.x;
		}
		else if (direction == game_control.direction.vertical) {
			distance_from_start_position = offset.y;
		}

		//setovanje pomeraja od grida
		float grid_offset = Mathf.Abs(distance_from_start_position % 1);

		//
		if (call_from_update) {
			//provera dal je otisao preko pola
			if (grid_offset > half_spacing) {
				game_control.debug_snapping_string = "NEXT";
			}
			else {
				game_control.debug_snapping_string = "PREVIOUS";
			}
		}
		else {
			//ACTUAL SNAPPING WHEN MOUSE IS UP
			if (game_control.debug_snapping_string == "NEXT") {
				//snappuj u pravcu pomeraja napred
				//+ (grid_spacing-grid_offset)
			}
			else if (game_control.debug_snapping_string == "PREVIOUS") {
				//snappuj u pravcu pomeraja nazad
				//-grid_offset
			}

		}

		

	}
	void OnMouseDown() {
		game_control.clicked_ball = gameObject;
		game_control.store_mouse_position_when_clicked();

		if (game_control.moving_direction == game_control.direction.none) {
			game_control.calculating_direction_active = true;
		}
	}


	void start_dragging_balls(int direction) {
		game_control.dragging_balls_active = true;
		ball.store_current_positions_off_all_balls();
		set_line_movable(game_control.clicked_ball, direction);
	}

	void set_line_movable(GameObject compared_ball, int line_type) {
		//ukljucivanje is movable parametra
		foreach (GameObject current_ball in game_control.all_balls()) {
			//0 = row
			if (line_type == 0) {
				if (current_ball.GetComponent<ball>().grid_y == compared_ball.GetComponent<ball>().grid_y) {
					current_ball.GetComponent<ball>().is_moving = true;
				}
			}
			//1 = col
			if (line_type == 1) {
				if (current_ball.GetComponent<ball>().grid_x == compared_ball.GetComponent<ball>().grid_x) {
					current_ball.GetComponent<ball>().is_moving = true;
				}
			}
		}

		//namestanje pravca kretanja
		//row
		if (line_type == 0) {
			game_control.direction_to_move_balls = new Vector3(1, 0, 0);	//da se pomera samo po x osi
		}
		//col
		if (line_type == 1) {
			game_control.direction_to_move_balls = new Vector3(0, 1, 0);	//da se pomera samo po y osi
		}
	}

	void Update() {

		//start moving
		if (game_control.start_dragging_balls) {
			if (game_control.moving_direction == game_control.direction.horizontal) {
				start_dragging_balls(0);
			}
			else {
				if (game_control.moving_direction == game_control.direction.vertical) {
						start_dragging_balls(1);
				}
			}
			game_control.start_dragging_balls = false;
		}

		//move movable balls
		if (game_control.dragging_balls_active) {
			foreach (GameObject ball in game_control.all_balls()) {
				if (game_control.ball_is_movable(ball)) {

					Vector3 start_ball_position = game_control.get_ball(ball).ball_position_when_clicked;
					Vector3 offset = Vector3.Scale(game_control.drag_offset,game_control.direction_to_move_balls);	//drag offset + pravac (x ili y)
					ball.transform.position = start_ball_position + offset;
					
				}
			}
		}


		//snapuj loptice na pravo mesto
		snapping(game_control.drag_offset, game_control.moving_direction,true);
	}



}



