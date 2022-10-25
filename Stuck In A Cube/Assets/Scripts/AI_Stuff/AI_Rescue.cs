using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AI_Rescue : MonoBehaviour
{

    private Vector3 clear = new Vector3(100f, 0f, 100f);//Position so far out the bounds of the map that the AI will never choose this over a stuck ally within bounds

    public Vector3[] Positions = new Vector3[3];

    public bool StuckAI = false;

    public int Queue = 0;

    private int[] QueuePlacements = new int[3];

    private void Start()
    {
        Positions[0] = clear;//Sets all of the positions in the queue to clear at the beginning
        Positions[1] = clear;
        Positions[2] = clear;
    }

    private void Update()//checks if the queue is clear every new frame, this way the AI can check whether they need to search the queue or not
    {

        if (Positions[0] != clear)
        {
            StuckAI = true;
        }
        else if (Positions[1] != clear)
        {
            StuckAI = true;
        }
        else if (Positions[2] != clear)
        {
            StuckAI = true;
        }
        else
        {
            StuckAI = false;
        }
    }

    public void UpdatePositions(Vector3 AI_POS, bool addToPositions, int QueueCode)
    {
        if (addToPositions == true)//Adds the stuck AI's position to the first clear position in the queue
        {
            Queue += 1;//Only used to create unique queue codes for new instances of stuck AI, not to be used as queue size
            if (Positions[0] == clear)
            {
                Positions[0] = AI_POS;
                QueuePlacements[0] = QueueCode;
            }
            else if (Positions[1] == clear)
            {
                Positions[1] = AI_POS;
                QueuePlacements[1] = QueueCode;
            }
            else
            {
                Positions[2] = AI_POS;
                QueuePlacements[2] = QueueCode;
            }
        }
        else
        {
            for (int i = 0; i < QueuePlacements.Length; i++)
            {
                if (QueuePlacements[i] == QueueCode)//Searches for queue code of the newly unstuck AI
                {
                    Positions[i] = clear;//Clears the AI's position from the queue based on the position of the queue code in the separate array
                }
            }
        }
    }

}
