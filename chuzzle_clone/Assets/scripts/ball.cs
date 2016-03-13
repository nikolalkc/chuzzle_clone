using UnityEngine;
using System.Collections;

public class ball : MonoBehaviour {
    public int color_index;
    public int grid_x, grid_y,ball_index;
    SpriteRenderer spr_this;
    public bool is_moving = false;

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
        gameObject.GetComponent<ball>().is_moving = true;
       // print(gameObject.name + "is moving");
        //GameObject[] all_balls = game_control.find_all_balls();
        //foreach (GameObject ball in all_balls)
        //{
        //    if (ball.GetComponent<ball>().grid_y == gameObject.GetComponent<ball>().grid_y)
        //    {
        //        ball.GetComponent<ball>().is_moving = true;
        //    }
        //}
    }

    void reset_is_moving_for_all_ball()
    {
        GameObject[] all_balls = game_control.find_all_balls();
        foreach (GameObject ball in all_balls)
        {
            if (ball.GetComponent<ball>().is_moving)
            {
                ball.GetComponent<ball>().is_moving = false;
                //print(ball.name + "moving:" + ball.GetComponent<ball>().is_moving);
            }

        }
    }

    void OnMouseUp()
    {
        //resetovanje misa
          //  print("RESET " + gameObject.name);
            reset_is_moving_for_all_ball();
    }

    void Update()
    {


        GameObject[] all_balls = game_control.find_all_balls();
        Vector3 mouse_position = new Vector3();

        foreach (GameObject ball in all_balls)
        {
            if (ball.GetComponent<ball>().is_moving)
            {
                 Vector3 mouse_pos = Input.mousePosition;
                 Vector3 converted_mouse_pos = Camera.main.ScreenToWorldPoint(mouse_pos);
                 converted_mouse_pos.z = -1;
                 ball.transform.position = converted_mouse_pos;
                                           
                                           


            }
        }
    }
}
