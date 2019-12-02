using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public bool Cheats;
    private void Start()
    {
        if (GameObject.Find("Player") != null)
        {
            Player = GameObject.Find("Player");
            ps = Player.GetComponent<PlayerScript>();
        }

    }
    void Update()
    {
        if (Player == null)
        {
            if (GameObject.Find("Player") != null)
            {
                Player = GameObject.Find("Player");
                ps = Player.GetComponent<PlayerScript>();
            }
        }
        if (Cheats)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                this.transform.position = Player.transform.position;
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                //Follower();
                //Follow();
            }
        }
    }
    private bool _allowFollow = false;
    public float speed = 10f;
    private List<Node> _walkBack = new List<Node>();
    void Follow()
    {
        if (FinalList.ToArray().Length > 0)
        {
            if (_allowFollow)
            {
                print("TRYING");
                this.transform.position = Vector2.MoveTowards(transform.position, FinalList[0].position, speed / 100f);
                if (Vector2.Distance(transform.position, FinalList[0].position) < 0.01f)
                {
                    this.transform.position = FinalList[0].position;
                    _walkBack.Add(FinalList[0]);
                    FinalList.RemoveAt(0);
                }
            }
        }
        else
        {
            if (_allowFollow)
            {
                _allowFollow = false;
                _walkBack.Reverse();
            }
            if (_walkBack.ToArray().Length > 0)
            {

                transform.position = Vector2.MoveTowards(transform.position, _walkBack[0].position, speed / 100f);
                if (Vector2.Distance(transform.position, _walkBack[0].position) < 0.01f)
                {
                    transform.position = _walkBack[0].position;
                    _walkBack.RemoveAt(0);
                }
            }
        }
    }

    public GameObject Player;
    private PlayerScript ps;

    List<Node> OpenList = new List<Node>();
    List<Node> ClosedList = new List<Node>();
    List<Node> FinalList = new List<Node>();

    void Follower()
    {
        OpenList.Clear();
        ClosedList.Clear();
        FinalList.Clear();

        Node n = new Node();
        n.position = this.transform.position;
        n.StartCost = 0;
        n.EndCost = Mathf.Pow((Player.transform.position.x - n.position.x), 2) + Mathf.Pow((Player.transform.position.y - n.position.y), 2);
        n.TotalCost = n.StartCost + n.EndCost;
        n.MotherNode = null;

        OpenList.Add(n);

        Node curNode = OpenList[0];
        //foreach(Node b in OpenList){
        //    if (b.TotalCost < curNode.TotalCost)
        //    {
        //        ClosedList.Add(curNode);
        //        curNode = b;
        //    }
        //}
        Node fin = new Node
        {
            position = this.transform.position,
            EndCost = 100,
            StartCost = 100,
            TotalCost = 200
        };
        while (fin.position != (Vector2)Player.transform.position)
        {
            for (var i = 0; i < 4; i++)
            {
                Node v = new Node();
                switch (i)
                {
                    case 0:
                        v.position = curNode.position + Vector2.up;
                        break;
                    case 1:
                        v.position = curNode.position + Vector2.down;
                        break;
                    case 2:
                        v.position = curNode.position + Vector2.left;
                        break;
                    case 3:
                        v.position = curNode.position + Vector2.right;
                        break;
                    default:
                        Debug.Log("Out of bounds");
                        break;
                }
                v.StartCost = (v.position.x - curNode.position.x) + (v.position.y - curNode.position.y);
                v.EndCost = Mathf.Pow((Player.transform.position.x - v.position.x), 2) + Mathf.Pow((Player.transform.position.y - v.position.y), 2);
                v.TotalCost = v.StartCost + v.EndCost;
                v.MotherNode = curNode;
                OpenList.Add(v);
                if (fin.EndCost > v.EndCost)
                {
                    fin = v;
                }
            }

            ClosedList.Add(fin);
            curNode = fin;
        }
        FinalList.Add(curNode);
        while (curNode.position != (Vector2)this.transform.position)
        {
            FinalList.Add(curNode.MotherNode);
            curNode = curNode.MotherNode;
        }
        FinalList.Reverse();
        _allowFollow = true;
        foreach (Node b in FinalList)
        {
            print(b.position);
        }
    }

}
