using UnityEngine;
using System.Collections;

public class ball : MonoBehaviour {
    public int color_index;
    public int grid_x, grid_y,ball_index;
    SpriteRenderer spr_this;

    public void create_ball(int x,int y,int index) {
        //parameters
        grid_x = x;
        grid_y = y;
        ball_index = index;
        
        //sprite setup
        spr_this = gameObject.AddComponent<SpriteRenderer>();
        color_index = Random.Range(0,10);
        spr_this.sprite = ball_sprite_ref.object_reference.ball_sprite[color_index];

        //box collider
        gameObject.AddComponent<BoxCollider2D>().isTrigger = true;

    }

    void OnMouseDown()
    {
        Destroy(gameObject);
    }
}
