using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideBar_Anim : MonoBehaviour
{
    public bool isOnScreen = false;
    private bool inAnim = false;
    private bool horizontal;
    private RectTransform SideBar_Trf;
    private Vector3[] corners = new Vector3[4];

    //private Transform SideBar_SidePoint;
    //public Transform left_point;
    public const float sidebar_anim_time = 0.32f;
    const int sidebaranim_numofmoves = 20;
    float sidebar_anim_distance_move; // = Width
    float sidebar_anim_move_step;
    Vector3 direction_vector;
    
    //
    // Summary:
    //     Side: 1.Up, 2.Right, 3.Down, 4.Left.
    public void SetProp(RectTransform sidebar_trf, int side, bool isOnScreen)
    {
        this.SideBar_Trf = sidebar_trf;
        this.isOnScreen = isOnScreen;
        direction_vector=Vector3.zero;
        if(side==1)
        {
            direction_vector=Vector3.up;
            horizontal = false;
        }
        else if(side==2)
        {
            direction_vector=Vector3.right;
            horizontal=true;
        }
        else if(side==3)
        {
            direction_vector=Vector3.down;
            horizontal=false;
        }
        else if(side==4)
        {
            direction_vector=Vector3.left;
            horizontal=true;
        }
    }    

    public void ToggleSideBar() 
    {
        if(!inAnim)
        {
            inAnim=true;
            SideBar_Trf.GetWorldCorners(corners);
            if(horizontal)
                sidebar_anim_distance_move = Mathf.Abs(corners[0].x - corners[2].x);
            else sidebar_anim_distance_move = Mathf.Abs(corners[0].y - corners[2].y);
            Debug.Log(sidebar_anim_distance_move);
            sidebar_anim_move_step = sidebar_anim_distance_move/sidebaranim_numofmoves;
            StartCoroutine(MovingSideBar());
        }
    }
    
    public IEnumerator MovingSideBar()
    {
        if(isOnScreen)
        {
            for(int i=0; i<sidebaranim_numofmoves;i++)
            {
                SideBar_Trf.Translate(direction_vector*sidebar_anim_move_step, Space.Self);
                yield return new WaitForSeconds(sidebar_anim_time/sidebaranim_numofmoves);
            }
            isOnScreen=false;
            inAnim=false;
        }
        else 
        {
            for(int i=0; i<sidebaranim_numofmoves;i++)
            {
                SideBar_Trf.Translate(-direction_vector*sidebar_anim_move_step, Space.Self);
                yield return new WaitForSeconds(sidebar_anim_time/sidebaranim_numofmoves);
            }
            isOnScreen=true;
            inAnim=false;
        }

    }
}
