using UnityEngine;
using System.Collections;

public class game_control : MonoBehaviour {
    public Transform grid_spawn_transform;
    public int grid_width = 5;
    public int grid_height = 7;
    public float grid_spacing = 1f;
    public GameObject test_ball;
	
	void Start () {
        generate_matrix();
	}

    void generate_matrix()
    {
        for (int i = 0; i < grid_width; i++)
        {
            for (int j = 0; j < grid_height; j++)
            {
                GameObject generic_ball = new GameObject();
                int ball_index = (i*grid_width + j);
                generic_ball.name = "ball_" + ball_index.ToString();
                generic_ball.AddComponent<ball>().create_ball(i,j,ball_index);
                generic_ball.transform.position = new Vector3(i * grid_spacing, j * grid_spacing, 0) + grid_spawn_transform.position;
            }
        }
    }

    public static GameObject[] find_all_balls()
    {
        return GameObject.FindGameObjectsWithTag("ball");
    }

}
