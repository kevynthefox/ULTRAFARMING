using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class area_designator : MonoBehaviour
{
    public GameObject corner_1;
    public GameObject corner_2;
    public GameObject point_prefab;
    public GameObject interact_point;//this is for things like depositing into an area? though you could potentially just check collision boxes instead.

    public TextMeshProUGUI corner_1_text;
    public TextMeshProUGUI corner_2_text;
    
    public List<GameObject> points;
    public GameObject points_holder;
    private float x_modifier = 0.5f;
    private float z_modifier = 0.5f;
    private int point_count_x;
    private int point_count_z;
    private List<int> x_point_positions;
    private List<int> z_point_positions;
    //public GameObject points_holder_move;
    
    public int x_between;
    public int z_between;
    public float divider; //this is for if you feel there is too many points.

    public task_assigner taskAssigner;

    [ContextMenu("do_all")]
    public void do_all()
    {
        calculate_area_between_cubes();
        clear_points();
        place_points_between_corners();
    }


    public void display_distance()
    {
        calculate_area_between_cubes();
        corner_1_text.text = "x: " +  x_between.ToString() + "<br>z: " +  z_between.ToString();
        corner_2_text.text = "x: " +  x_between.ToString() + "<br>z: " +  z_between.ToString();
        
    }
    
    public void calculate_area_between_cubes()
    {
        x_between = Mathf.FloorToInt((corner_2.transform.position.x - corner_1.transform.position.x)/divider);
        z_between = Mathf.FloorToInt((corner_2.transform.position.z - corner_1.transform.position.z)/divider);
    }

    void place_points_between_corners()
    {
        if (x_between > 0)
        {
            for (int x = 0; x < x_between; x++)
            {
                if (z_between > 0)
                {
                    for (int z = 0; z < z_between; z++)
                    {
                        create_point(x, z);
                    }
                }
                if (z_between < 0)
                {
                    for (int z = 0; z > z_between; z--)
                    {
                        create_point(x, z);
                    }
                }
                
            }
        }
        if (x_between < 0)
        {
            for (int x = 0; x > x_between; x--)
            {
                if (z_between > 0)
                {
                    for (int z = 0; z < z_between; z++)
                    {
                        create_point(x, z);
                    }
                }
                if (z_between < 0)
                {
                    for (int z = 0; z > z_between; z--)
                    {
                        create_point(x, z);
                    }
                }
                
            }
        }

        fix_points_positions();
        points_holder.transform.localPosition = new Vector3(corner_1.transform.localPosition.x + ((corner_2.transform.localPosition.x - corner_1.transform.localPosition.x) / 2), 0, corner_1.transform.localPosition.z + ((corner_2.transform.localPosition.z - corner_1.transform.localPosition.z) / 2));
        //points_holder_move.transform.localPosition = new Vector3((-x_between / 2) + (1/x_between), 0, (-z_between / 2) + (1/z_between));
    }

    void create_point(int x, int z)
    {
        var new_point = Instantiate(point_prefab, Vector3.zero, Quaternion.identity);
        new_point.transform.SetParent(points_holder.transform);
        points.Add(new_point);
        taskAssigner.dig_points.Add(new_point.transform);
        
        if (x < 0)
        {
            x_modifier = -x_modifier;
        }
        if (z < 0)
        {
            z_modifier = -z_modifier;
        }
        
        new_point.transform.localPosition = new Vector3((x*divider) - (x_between/2) + x_modifier, 0, (z*divider)  - (z_between/2) + z_modifier);
        new_point.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        
    }

    void clear_points()
    {
        foreach (var point in points)
        {
            Destroy(point);
        }
        points.Clear();
        taskAssigner.dig_points.Clear();
    }

    void fix_points_positions()
    {
        point_count_x = 0;
        point_count_z = 0;
        
        foreach (var point in points)
        {
            point.transform.parent = null;
            point.transform.localPosition = Vector3.zero;

            point.transform.localPosition = new Vector3(point_count_x,0, point_count_z);
            point_count_x++;
            
            if (point_count_x >= Mathf.Abs(x_between))
            {
                point_count_z++;
                point_count_x = 0;
            }
        }

        points_holder.transform.parent = null;
        points_holder.transform.localPosition = new Vector3(points.Last().transform.position.x / 2, 0, points.Last().transform.position.z / 2);
        

        foreach (var point in points)
        {
            point.transform.parent = points_holder.transform;
            point.transform.localPosition = new Vector3(point.transform.localPosition.x * divider, 0, point.transform.localPosition.z * divider);
        }
        
        
        
        //points_holder.transform.localScale = new Vector3(divider, divider, divider);
        
        points_holder.transform.parent = this.transform;

    }
}
