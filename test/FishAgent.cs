using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIFramework;
using AIFramework.Actions;
using AIFramework.Entities;
using System.Diagnostics;

namespace FishAgent
{
    public class FishAgent : Agent
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
        AIVector enemyPos;
        public FishAgent(IPropertyStorage propertyStorage)
            : base(propertyStorage)
        {
            rnd = new Random();
            MovementSpeed = 115;
            Strength = 0;
            Health = 25;
            Eyesight = 90;
            Endurance = 20;
            Dodge = 0;

            //moveX = rnd.Next(-1, 2);
            //moveY = rnd.Next(-1, 2);

            moveX = 1;
            moveY = 0;

            string ddd = this.GetType().FullName;
        }



        public override IAction GetNextAction(List<IEntity> otherEntities)
        {
            //int action = 4;

            List<Agent> agents = otherEntities.FindAll(a => a is Agent).ConvertAll<Agent>(a => (Agent)a);
            agents.Remove(this);

            List<IEntity> plants = otherEntities.FindAll(x => x is Plant);

            //food
            foreach (Plant plant in plants)
            {
                if (AIVector.Distance(Position, plant.Position) < AIModifiers.maxFeedingRange)
                {
                    return new Feed(plant);
                }
            }
            if (plants.Count > 0)
            {
                plantPos = new AIVector(plants[0].Position.X, plants[0].Position.Y);
                dirvector = plantPos - Position;
                return new Move(dirvector.Normalize());
            }

            //sex
            if (ProcreationCountDown == 0)
            {
                foreach (Agent agent in agents)
                {

                    if (AIVector.Distance(Position, agent.Position) < AIModifiers.maxProcreateRange)
                    {
                        if (agent.ProcreationCountDown == 0)
                        {
                            return new Procreate(agent);
                        }
                        
                    }
                }
            }

            //move
            //Find all enemies witin Eyesight range
            List<IEntity> nearEnemies = otherEntities.FindAll(x => x.GetType() != typeof(FishAgent) && x is Agent && AIVector.Distance(Position, x.Position) < AIModifiers.maxMeleeAttackRange);
            //if (nearEnemies.Count > 0)
            //{
            //    return new Attack((Agent)nearEnemies[0]);
            //}

            if (nearEnemies.Count > 0)
            {
                enemyPos = new AIVector(nearEnemies[0].Position.X, nearEnemies[0].Position.Y);
                dirvector = Position - enemyPos;
                return new Move(dirvector.Normalize());
            }

            if (Position.X == xPos && Position.Y != yPos)
            {
                if (moveX == 1)
                    moveX = -1;
                else
                    moveX = 1;
            }
            if (Position.X != xPos && Position.Y == yPos)
            {
                if (moveY == 1)
                    moveY = -1;
                else
                    moveY = 1;
            }
            xPos = Position.X;
            yPos = Position.Y;
            return new Move(new AIVector(moveX, moveY));

            
            //(otherEntities);

            //if (AIVector.Distance(Position, agents.Position) < AIModifiers.maxProcreateRange)
            //{
            //    action = 1;
            //}

            //for (int i = 0; i < agents.Count; i++)
            //{
            //    agents.Remove(i);
            //}





            //Agent rndAgent = null;
            //rndAgent = agents[rnd.Next(agents.Count)];

            //switch (action)
            //{
            //    case 1: //Procreate
            //        if (rndAgent != null && rndAgent.GetType() == typeof(FishAgent))
            //        {
            //            return new Procreate(rndAgent);
            //        }
            //        break;

            //    case 2: //Attack Melee
            //        if (rndAgent != null && rndAgent.GetType() != typeof(FishAgent))
            //        {
            //            return new Attack(rndAgent);
            //        }
            //        break;
            //    case 3: //Feed
            //        if (plants.Count > 0)
            //        {
            //            return new Feed((Plant)plants[rnd.Next(plants.Count)]);

            //        }
            //        break;
            //    case 4: //Move
            //        if (plants.Count > 0)
            //        {
            //            AIVector playerPos = new AIVector(this.Position.X, this.Position.Y);
            //            plantPos = new AIVector(plants[0].Position.X, plants[0].Position.Y);
            //            dirvector = plantPos - playerPos;
            //            return new Move(dirvector.Normalize());
            //        }
            //        else if (plants.Count <= 0)
            //        {
            //            if (Position.X == xPos && Position.Y != yPos)
            //            {
            //                if (moveX == 1)
            //                    moveX = -1;
            //                else
            //                    moveX = 1;
            //            }
            //            if (Position.X != xPos && Position.Y == yPos)
            //            {
            //                if (moveY == 1)
            //                    moveY = -1;
            //                else
            //                    moveY = 1;
            //            }


                        //if (Position.X == xPos || Position.Y == yPos)
                        //{
                        //    if (moveX == -1)
                        //    {
                        //        moveX = 1;
                        //        moveY = 1;
                        //    }
                        //    else
                        //    {
                        //        moveX = -1;
                        //        moveY = -1;
                        //    }
                        //}
                        //else if (Position.X != xPos || Position.Y != yPos)
                        //{
                        //    moveX = 1;
                        //    moveY = 1;
                        //}

                        //    //moveX = rnd.Next(-1, 2);
                        //    //moveY = rnd.Next(-1, 2);
                        //xPos = Position.X;
                        //yPos = Position.Y;
                        //return new Move(new AIVector(moveX, moveY));

            //        }

            //        break;

            //    //return new Move(new AIVector(moveX, moveY));
            //    default:
            //        return new Defend();
            //}

            ////return new Move(dirvector);

            //return new Move(new AIVector(moveX, moveY));
            //return new Move(new AIVector(plants[0].Position.Y, plants[0].Position.X));
        }



        public override void ActionResultCallback(bool success)
        {
            //Do nothing - AI dont take success of an action into account
        }
    }
}
