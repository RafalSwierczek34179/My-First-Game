using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerObstacleRadius : MonoBehaviour
{

    public Player player;

    private float[] Distances = new float[4];
    public bool _predator_active = false;

    void Update()
    {
        ShortestDistance();       
    }


    private void SizeCalculator(float distance)//Always called from ShortestDistance(), called every new frame
    {
        if (_predator_active == true)
        {
            NavMeshObstacle _playerRadius = player.transform.GetComponent<NavMeshObstacle>();//essentially makes the player invisible to AI if they have predator active
            _playerRadius.radius = 0.1f;
        }
        else
        {
            if (distance < 3.75f)//If the shaortest distance between any AI and the player is below 3.5m, then this function starts adjusting the players obstacle size
            {
                if (distance > 3f)
                {
                    NavMeshObstacle _playerRadius = player.transform.GetComponent<NavMeshObstacle>();
                    _playerRadius.radius = 2.5f;
                }
                else if (distance > 2f)
                {
                    NavMeshObstacle _playerRadius = player.transform.GetComponent<NavMeshObstacle>();
                    _playerRadius.radius = 1.5f;
                }
                else
                {
                    NavMeshObstacle _playerRadius = player.transform.GetComponent<NavMeshObstacle>();
                    _playerRadius.radius = 0.4f;
                }
            }
            else
            {
                NavMeshObstacle _playerRadius = player.transform.GetComponent<NavMeshObstacle>();//doesn't run above 1 second because there's no need to constantly set the radius to 3m
                _playerRadius.radius = 3f;
            }
        }
    }

    public void DistanceStore(float _distance, string _id)//Each AI calculates the distance between them and the player, then call this method to store it
    {
        if (_id == "Enemy")
        {
            Distances[0] = _distance;
        }
        else if (_id == "Enemy1")
        {
            Distances[1] = _distance;
        }
        else if (_id == "Enemy2")
        {
            Distances[2] = _distance;
        }
        else
        {
            Distances[3] = _distance;
        }
    }

    private void ShortestDistance()//Goes through a list of distances between AI-Player and finds the smallest one, then feeds it to the method which actually calculates the size the players obstacle should be
    {
        float ShortestDist = Distances[0];
        for (int i = 1; i < Distances.Length; i++)
        {
            if (Distances[i] < ShortestDist)
            {
                ShortestDist = Distances[i];
            }
        }
        SizeCalculator(ShortestDist);
    }


}
