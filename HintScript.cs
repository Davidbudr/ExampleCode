using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintScript : MonoBehaviour
{
    public GameObject SparklePrefab;
    private List<GameObject> Sparkles = new List<GameObject>();

    public List<GameObject> Bubbles = new List<GameObject>();
    public List<Vector2> hint = new List<Vector2>();
    public bool Active;
    private bool first = true;

    private LayerMask _activeMask;
    private LayerMask _bubbleMask;

    public float HintTime;
    private float timer;

    void Start()
    {
        _activeMask = LayerMask.GetMask("ActiveZone");
        _bubbleMask = LayerMask.GetMask("Bubble");
    }
    void Update()
    {

        timer += Time.deltaTime;
        if (timer >= HintTime)
        {
            Active = true;
        }
        if (Input.GetMouseButtonDown(0))
        {
            timer = 0;
            Active = false;
        }

        if (Active)
        {
            if (first)
            {
                first = false;
                foreach (GameObject g in GameObject.FindGameObjectsWithTag("Bubbles"))
                {
                    Bubbles.Clear();
                    Bubbles.Add(g);
                    if (checksurround(g))
                    {
                        break;
                    }
                }
            }
        }
        else
        {
            if (Sparkles.Count > 0)
            {
                foreach (GameObject sprkl in Sparkles)
                {
                    Destroy(sprkl);
                }

                Sparkles.Clear();
                hint.Clear();
                Bubbles.Clear();

                first = true;
            }
        }
    }
    bool checksurround(GameObject core)
    {
        int val = 0;
        while (true)
        {
            BubbleScript bs = core.GetComponent<BubbleScript>();
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
                var bbm = Physics2D.OverlapCircle((Vector2)core.transform.position + dir, 0.3f, _bubbleMask);
                if (bbm != null)
                {
                    if (!Bubbles.Contains(bbm.gameObject))
                    {
                        if (bbm.GetComponent<BubbleScript>().TypeID == bs.TypeID)
                        {
                            Bubbles.Add(bbm.gameObject);
                        }
                    }
                }
            }
            if (Bubbles.Count -1 > val)
            {
                val++;
                core = Bubbles[val];
            }
            else
            {
                if (Bubbles.Count > 3)
                {
                    for (var i = 0; i < Bubbles.Count; i++)
                    {
                        GameObject sprk = Instantiate(SparklePrefab);
                        sprk.transform.position = Bubbles[i].transform.position;
                        Sparkles.Add(sprk);
                    }
                    return true;
                }
                if (Bubbles.Count < 3)
                {
                    //prevent a lockout
                    Active = false;
                    for (var del = 0; del < 3; del++)
                    {
                        Destroy(GameObject.FindGameObjectsWithTag("Bubbles")[0]);
                    }
                    return true;
                }
            }
        }
    }
}
