using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace Pathfinder
{
    class Dijkstra
    {
        public bool[,] closed; //whether or not location is closed
        public double[,] cost; //cost value for each location
        public Coord2[,] link; //link for each location = coords of a neighbouring location
        public bool[,] inPath; //whether or not a location is in the final path

        public Dijkstra() //Initialises Arrays
        {
            closed = new bool[40, 40];
            cost = new double[40, 40];
            link = new Coord2[40, 40];
            inPath = new bool[40, 40];
        }

        public void Build(Level level, AiBotBase bot, Player plr)
        {
            int countX = 0;
            int countY = 0;

            foreach(bool isClosed in closed) //Sets all nodes to open
            {
                closed[countX, countY] = false;

                if (countX != 40)
                {
                    countX++;
                }

                if (countX == 40)
                {
                    countY++;
                    countX = 0;
                }
            }

            countX = 0;
            countY = 0;

            foreach (float isCost in cost) //Sets all costs to 1000000
            {
                cost[countX, countY] = 1000000;

                if (countX != 40)
                {
                    countX++;
                }

                if (countX == 40)
                {
                    countY++;
                    countX = 0;
                }
            }

            countX = 0;
            countY = 0;

            foreach (Coord2 isLink in link) //Sets all links to -1,-1
            {
                link[countX, countY] = new Coord2(-1, -1);

                if (countX != 40)
                {
                    countX++;
                }

                if (countX == 40)
                {
                    countY++;
                    countX = 0;
                }
            }

            countX = 0;
            countY = 0;

            foreach (bool path in inPath) //Sets every value of inPath to false
            {
                inPath[countX, countY] = false;

                if (countX != 40)
                {
                    countX++;
                }

                if (countX == 40)
                {
                    countY++;
                    countX = 0;
                }
            }

            countX = 0;
            countY = 0;

            closed[bot.GridPosition.X, bot.GridPosition.Y] = false; //Sets the bot's position to open
            cost[bot.GridPosition.X, bot.GridPosition.Y] = 0; //Sets the bot's position to zero
            bool done = false;
            while (!done)
            {
                double lowestCost = 1000000;
                int lowestCostPosX = 0;
                int lowestCostPosY = 0;
                foreach (double currentCost in cost)
                {

                    if (cost[countX, countY] < lowestCost && !closed[countX, countY]) //Finds the location with the lowest cost that is open
                    {
                        lowestCost = cost[countX, countY];
                        lowestCostPosX = countX;
                        lowestCostPosY = countY;
                    }

                    if (countX != 40)
                    {
                        countX++;
                    }

                    if (countX == 40)
                    {
                        countY++;
                        countX = 0;
                    }
                }
                countX = 0;
                countY = 0;

                closed[lowestCostPosX, lowestCostPosY] = true; // Sets that location to closed
                if (lowestCostPosX != 0)
                {
                    if (cost[lowestCostPosX - 1, lowestCostPosY] > lowestCost + 1) // If the location one to the left of the lowest cost would be lower than the lowest location + 1
                    {
                        if (level.ValidPosition(new Coord2(lowestCostPosX - 1, lowestCostPosY)))
                        {
                            cost[lowestCostPosX - 1, lowestCostPosY] = cost[lowestCostPosX, lowestCostPosY] + 1; // Set the new cost
                            link[lowestCostPosX - 1, lowestCostPosY] = new Coord2(lowestCostPosX, lowestCostPosY); // Sets the link value to parent
                        }
                    }
                }

                if (lowestCostPosX < 39)
                {
                    if (cost[lowestCostPosX + 1, lowestCostPosY] > lowestCost + 1)// If the location one to the right of the lowest cost would be lower than the lowest location + 1
                    {
                        if (level.ValidPosition(new Coord2(lowestCostPosX + 1, lowestCostPosY)))
                        {
                            cost[lowestCostPosX + 1, lowestCostPosY] = cost[lowestCostPosX, lowestCostPosY] + 1; // Set the new cost
                            link[lowestCostPosX + 1, lowestCostPosY] = new Coord2(lowestCostPosX, lowestCostPosY); // Sets the link value to parent
                        }
                    }
                }
                if (lowestCostPosY != 0)
                {
                    if (cost[lowestCostPosX, lowestCostPosY - 1] > lowestCost + 1)// If the location one above the lowest cost would be lower than the lowest location + 1
                    {
                        if (level.ValidPosition(new Coord2(lowestCostPosX, lowestCostPosY - 1)))
                        {
                            cost[lowestCostPosX, lowestCostPosY - 1] = cost[lowestCostPosX, lowestCostPosY] + 1; // Set the new cost
                            link[lowestCostPosX, lowestCostPosY - 1] = new Coord2(lowestCostPosX, lowestCostPosY); // Sets the link value to parent
                        }
                    }
                }
                if (lowestCostPosY < 39)
                {
                    if (cost[lowestCostPosX, lowestCostPosY + 1] > lowestCost + 1)// If the location one below the lowest cost would be lower than the lowest location + 1
                    {
                        if (level.ValidPosition(new Coord2(lowestCostPosX, lowestCostPosY + 1)))
                        {
                            cost[lowestCostPosX, lowestCostPosY + 1] = cost[lowestCostPosX, lowestCostPosY] + 1; // Set the new cost
                            link[lowestCostPosX, lowestCostPosY + 1] = new Coord2(lowestCostPosX, lowestCostPosY); // Sets the link value to parent
                        }
                    }
                }
                if (lowestCostPosX != 0 && lowestCostPosY != 0)
                { 
                  if (cost[lowestCostPosX - 1, lowestCostPosY - 1] > lowestCost + 1.4)// If the location one up and left to the lowest cost would be lower than the lowest location + 1.4
                  {
                      if (level.ValidPosition(new Coord2(lowestCostPosX - 1, lowestCostPosY - 1)))
                      {
                           cost[lowestCostPosX - 1, lowestCostPosY - 1] = cost[lowestCostPosX, lowestCostPosY] + 1.4; // Set the new cost
                           link[lowestCostPosX - 1, lowestCostPosY - 1] = new Coord2(lowestCostPosX, lowestCostPosY); // Sets the link value to parent
                        }
                  }
                }

                if (lowestCostPosY != 0 && lowestCostPosX < 39)
                {
                    if (cost[lowestCostPosX + 1, lowestCostPosY - 1] > lowestCost + 1.4)// If the location one up and right to the lowest cost would be lower than the lowest location + 1.4
                    {
                        if (level.ValidPosition(new Coord2(lowestCostPosX + 1, lowestCostPosY - 1)))
                        {
                            cost[lowestCostPosX + 1, lowestCostPosY - 1] = cost[lowestCostPosX, lowestCostPosY] + 1.4; // Set the new cost
                            link[lowestCostPosX + 1, lowestCostPosY - 1] = new Coord2(lowestCostPosX, lowestCostPosY); // Sets the link value to parent
                        }
                    }
                }

                if (lowestCostPosX != 0 && lowestCostPosY < 39)
                {
                    if (cost[lowestCostPosX - 1, lowestCostPosY + 1] > lowestCost + 1.4)// If the location one down and left to the lowest cost would be lower than the lowest location + 1.4
                    {
                        if (level.ValidPosition(new Coord2(lowestCostPosX - 1, lowestCostPosY + 1)))
                        {
                            cost[lowestCostPosX - 1, lowestCostPosY + 1] = cost[lowestCostPosX, lowestCostPosY] + 1.4; // Set the new cost
                            link[lowestCostPosX - 1, lowestCostPosY + 1] = new Coord2(lowestCostPosX, lowestCostPosY); // Sets the link value to parent
                        }
                    }
                }
                if (lowestCostPosX < 39 && lowestCostPosY < 39)
                {
                    if (cost[lowestCostPosX + 1, lowestCostPosY + 1] > lowestCost + 1.4)// If the location one down and right to the lowest cost would be lower than the lowest location + 1.4
                    {
                        if (level.ValidPosition(new Coord2(lowestCostPosX + 1, lowestCostPosY + 1)))
                        {
                            cost[lowestCostPosX + 1, lowestCostPosY + 1] = cost[lowestCostPosX, lowestCostPosY] + 1.4; // Set the new cost
                            link[lowestCostPosX + 1, lowestCostPosY + 1] = new Coord2(lowestCostPosX, lowestCostPosY); // Sets the link value to parent
                        }
                    }
                }
                if (closed[plr.GridPosition.X, plr.GridPosition.Y]) // If the player's position becomes closed, end loop
                {
                    done = true;
                }
            }

            done = false; //set to true when we are back at the bot position
            Coord2 nextClosed = plr.GridPosition; //start of path
            while (!done)
            {
                
                inPath[nextClosed.X, nextClosed.Y] = true;
                nextClosed = link[nextClosed.X, nextClosed.Y];
                if (nextClosed == bot.GridPosition) done = true;
            }

        }
    }
}
