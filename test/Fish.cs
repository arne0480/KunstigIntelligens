using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIFramework;
using AIFramework.Actions;
using AIFramework.Entities;
using System.Diagnostics;
using System.Timers;

namespace Fish
{
    public class Fish : Agent
    {
        Random rnd;
        //Only for randomization of movement
        float moveX = 0;
        float moveY = 0;
        float xPos;
        float yPos;

        AIVector dirvector;
        AIVector plantPos;

        public Fish(IPropertyStorage propertyStorage)
            : base(propertyStorage)
        {
            rnd = new Random();
            MovementSpeed = 135;
            Strength = 0;
            Health = 25;
            Eyesight = 70;
            Endurance = 20;
            Dodge = 0;

            moveX = 0;
            moveY = 0;
            string ddd = this.GetType().FullName;
        }

        public override IAction GetNextAction(List<IEntity> otherEntities)
        {
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
            List<IEntity> nearEnemies = otherEntities.FindAll(x => x.GetType() != typeof(Fish) && x is Agent && AIVector.Distance(Position, x.Position) < AIModifiers.maxMeleeAttackRange);
            if (nearEnemies.Count > 0)
            {
                moveX = rnd.Next(-1, 2);
                moveY = rnd.Next(-1, 2);
                return new Move(new AIVector(moveX, moveY));
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
            if (moveX == 0 && moveY == 0)
            {
                moveX = rnd.Next(-1, 2);
                moveY = rnd.Next(-1, 2);
            }
            xPos = Position.X;
            yPos = Position.Y;
            return new Move(new AIVector(moveX, moveY));
        }
        public override void ActionResultCallback(bool success)
        {
            //Do nothing - AI dont take success of an action into account
        }
    }
}
