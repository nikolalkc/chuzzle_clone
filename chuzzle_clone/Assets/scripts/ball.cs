using UnityEngine;
using System.Collections;

public class ball : MonoBehaviour {
    public int color_index;
    public int grid_x, grid_y,ball_index;
    SpriteRenderer spr_this;
    public bool is_moving = false;
	Vector3 ball_position_when_clicked = Vector3.zero;
    public void create_ball(int x,int y,int index) {
        //parameters
        grid_x = x;
        grid_y = y;
        ball_index = index;
        
        //sprite setup
        spr_this = gameObject.AddComponent<SpriteRenderer>();
        color_index = Random.Range(0,10);
        spr_this.sprite = ball_sprite_ref.object_reference.ball_sprite[color_index];

        //components
        gameObject.AddComponent<BoxCollider2D>().isTrigger = true;
        gameObject.tag = "ball";

    }

    void OnMouseDown()
    {
		start_dragging_balls();
    }

	void start_dragging_balls() {

		game_control.dragging_balls_active = true;
		game_control.store_mouse_position_when_clicked();
		ball.store_current_positions_off_all_balls();
		set_row_movable(gameObject);
	}

	void set_row_movable(GameObject g) {
		foreach (GameObject ball in game_control.all_balls()) {
			if (ball.GetComponent<ball>().grid_y == g.GetComponent<ball>().grid_y) {
				ball.GetComponent<ball>().is_moving = true;
			}
		}
	}

	public static void store_current_positions_off_all_balls() {
		foreach (GameObject ball in game_control.all_balls()) {
			game_control.get_ball(ball).ball_position_when_clicked = ball.transform.position;
		}
	}

    void stop_dragging_balls()
    {
		game_control.dragging_balls_active = false;
		foreach (GameObject ball in game_control.all_balls()) {
			game_control.get_ball(ball).ball_position_when_clicked = Vector3.zero;
		}
         foreach (GameObject ball in game_control.all_balls())
        {
            if (ball.GetComponent<ball>().is_moving)
            {
				game_control.ball_is_movable(ball, false);
                //print(ball.name + "moving:" + ball.GetComponent<ball>().is_moving);
            }
        }
    }

    void OnMouseUp()
    {
	    stop_dragging_balls();
    }

    void Update()
    {

		//moving movable balls
        foreach (GameObject ball in game_control.all_balls())
        {
            if (game_control.ball_is_movable(ball) && game_control.get_ball(ball).ball_position_when_clicked != Vector3.zero)
            {
				ball.transform.position = game_control.get_ball(ball).ball_position_when_clicked + game_control.drag_offset;
            }
        }
    }



}



