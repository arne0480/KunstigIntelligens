using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIFramework;
using AIFramework.Actions;
using AIFramework.Entities;
using System.Diagnostics;

namespace ExampleAI
{
    public class RandomAgent : Agent
    {
        Random rnd;

        //Only for randomization of movement
        float moveX = 0;
        float moveY = 0;
        float xPos;
        float yPos;


        //moveX = rnd.Next(-1, 2);
        //moveY = rnd.Next(-1, 2);
        AIVector dirvector;
        AIVector plantPos;
        public RandomAgent(IPropertyStorage propertyStorage)
            : base(propertyStorage)
        {
            rnd = new Random();
            MovementSpeed = 110;
            Strength = 0;
            Health = 50;
            Eyesight = 70;
            Endurance = 20;
            Dodge = 0;

            //moveX = rnd.Next(-1, 2);
            //moveY = rnd.Next(-1, 2);

            string ddd = this.GetType().FullName;
        }



        public override IAction GetNextAction(List<IEntity> otherEntities)
        {
            int action = 4;

            List<Agent> agents = otherEntities.FindAll(a => a is Agent).ConvertAll<Agent>(a => (Agent)a);
            List<IEntity> plants = otherEntities.FindAll(x => x is Plant);

            foreach (Plant plant in plants)
            {
                if (AIVector.Distance(Position, plant.Position) < AIModifiers.maxFeedingRange)
                {
                    //return new Feed(plant);
                    action = 3;
                }
            }









            Agent rndAgent = null;
            rndAgent = agents[rnd.Next(agents.Count)];

            switch (action)
            {
                case 1: //Procreate
                    if (rndAgent != null && rndAgent.GetType() == typeof(RandomAgent))
                    {
                        return new Procreate(rndAgent);
                    }
                    break;

                case 2: //Attack Melee
                    if (rndAgent != null && rndAgent.GetType() != typeof(RandomAgent))
                    {
                        return new Attack(rndAgent);
                    }
                    break;
                case 3: //Feed
                    if (plants.Count > 0)
                    {
                        return new Feed((Plant)plants[rnd.Next(plants.Count)]);

                    }
                    break;
                case 4: //Move
                    if (plants.Count > 0)
                    {
                        AIVector playerPos = new AIVector(agents[0].Position.X, agents[0].Position.Y);
                        plantPos = new AIVector(plants[0].Position.X, plants[0].Position.Y);
                        dirvector = plantPos - playerPos;
                        return new Move(dirvector.Normalize());
                    }
                    else if (plants.Count <= 0)
                    {

                        
                        
                        if (Position.X == xPos || Position.Y == yPos)
                        {
                            if (moveX == -1)
                            {
                                moveX = 1;
                                moveY = 1;
                            }
                            else
                            {
                                moveX = -1;
                                moveY = -1;
                            }
                        }
                        //else if (Position.X != xPos || Position.Y != yPos)
                        //{
                        //    moveX = 1;
                        //    moveY = 1;
                        //}

                        //    //moveX = rnd.Next(-1, 2);
                        //    //moveY = rnd.Next(-1, 2);
                        xPos = Position.X;
                        yPos = Position.Y;
                        return new Move(new AIVector(moveX, moveY));

                    }
                    
                    break;

                //return new Move(new AIVector(moveX, moveY));
                default:
                    return new Defend();
            }

            //return new Move(dirvector);

            return new Move(new AIVector(moveX, moveY));
            //return new Move(new AIVector(plants[0].Position.Y, plants[0].Position.X));
        }



        public override void ActionResultCallback(bool success)
        {
            //Do nothing - AI dont take success of an action into account
        }
    }
}
