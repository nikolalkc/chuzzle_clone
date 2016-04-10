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
		game_control.dragging_balls_active = false;

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
	}

	void OnMouseUp() {
		stop_dragging_balls();
	}
	#endregion



	void OnMouseDown() {
		start_dragging_balls();
	}

	void start_dragging_balls() {
		game_control.dragging_balls_active = true;
		game_control.store_mouse_position_when_clicked();
		ball.store_current_positions_off_all_balls();
		set_line_movable(gameObject, 0);
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
	}



}



