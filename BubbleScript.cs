using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleScript : MonoBehaviour
{
    //layer information
    private LayerMask activeMask;
    private LayerMask bubbleMask;

    //object information
    public int TypeID;
    private bool moveState;

    //external play area information
    public PlayArea CurrentPa;
    private PlayArea futurePa;

    private Animator anim;

    void Start()
    {
        activeMask = LayerMask.GetMask("ActiveZone");
        bubbleMask = LayerMask.GetMask("Bubble");
        anim = this.GetComponent<Animator>();
        var _pa = Physics2D.OverlapCircle(this.transform.position, 0.3f, activeMask);
        if (_pa)
        {
            CurrentPa = _pa.GetComponent<PlayArea>();
            CurrentPa.Filled = true;
            futurePa = CurrentPa;
        }
    }
    void Update()
    {
        if (!moveState)
        {
            Vector2 _checkDir = Vector2.down;
            for (var i = 0; i < 3; i++)
            {
                //check all 3 lower directions if there is a PlayArea
                switch (i)
                {
                    case 0:
                        //Check down
                        _checkDir = new Vector2(0, -1);
                        break;
                    case 1:
                        //Check down left
                        _checkDir = new Vector2(-1, -1);
                        break;
                    case 2:
                        //Check down right
                        _checkDir = new Vector2(1, -1);
                        break;
                    default:
                        break;
                }
                var _pa = Physics2D.OverlapCircle(this.transform.position + (Vector3)_checkDir, 0.3f, activeMask);
                //if there is a play area check if it is filled
                if (_pa)
                {
                    PlayArea _checkpa = _pa.GetComponent<PlayArea>();
                    if (!_checkpa.Filled)
                    {
                        //check if there is another bubble over the area you are trying to move to
                        _checkDir -= Vector2.down;
                        var _bub = Physics2D.OverlapCircle(this.transform.position + (Vector3)_checkDir, 0.3f, bubbleMask);
                        switch (i)
                        {
                            case 0:
                                futurePa = _checkpa;
                                futurePa.Filled = true;
                                moveState = true;
                                break;
                            case 1:
                            case 2:
                                if (!_bub)
                                {
                                    futurePa = _checkpa;
                                    futurePa.Filled = true;
                                    moveState = true;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                if (moveState)
                {
                    break;
                }

            }

        }
        else if (CurrentPa != futurePa)
        {
            this.transform.position = Vector2.Lerp(this.transform.position, futurePa.transform.position, 0.15f);
            if (Vector2.Distance(this.transform.position, futurePa.transform.position) < 0.15f)
            {
                this.transform.position = futurePa.transform.position;
                if (CurrentPa != null) { CurrentPa.Filled = false; }
                moveState = false;
                CurrentPa = futurePa;
            }
        }
    }
    public void DeathMark()
    {
        anim.SetBool("Destroy", true);
        Invoke("removefill", 0.5f);
    }
    void removefill()
    {
        CurrentPa.Filled = false;
        Destroy(this.gameObject);
    }
    public bool AdjacencyCheck(GameObject obj)
    {
        for (var i = 0; i < 8; i++)
        {
            Vector2 dir = Vector2.zero;
            switch (i)
            {
                case 0:
                    dir = Vector2.up;
                    break;
                case 1:
                    dir = Vector2.down;
                    break;
                case 2:
                    dir = Vector2.left;
                    break;
                case 3:
                    dir = Vector2.right;
                    break;
                case 4:
                    dir = Vector2.up + Vector2.right;
                    break;
                case 5:
                    dir = Vector2.up + Vector2.left;
                    break;
                case 6:
                    dir = Vector2.down + Vector2.right;
                    break;
                case 7:
                    dir = Vector2.down + Vector2.left;
                    break;
                default:
                    Debug.Log("Something went wrong here");
                    break;
            }
            if (Physics2D.OverlapCircle((Vector2)this.transform.position + dir, 0.3f, bubbleMask))
            {
                if (Physics2D.OverlapCircle((Vector2)this.transform.position + dir, 0.3f, bubbleMask).gameObject == obj)
                {
                    return true;
                }
            }
            else
            {
                continue;
            }

        }

        return false;
    }
}
