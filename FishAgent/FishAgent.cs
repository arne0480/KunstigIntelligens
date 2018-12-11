using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIFramework;
using AIFramework.Actions;
using AIFramework.Entities;

namespace FishAgent
{
    public class FishAgent : Agent
    {
        //state attributes - these may only be modified by framework
        //public AIVector Position { get; set; }
        //public float Hunger { get; set; }
        //public float Hitpoints { get; set; }
        //public bool Defending { get; set; }
        //public float ProcreationCountdown { get; set; }


        //public int MovementSpeed { get; set; }
        //public int Strength { get; set; }
        //public int Health { get; set; }
        //public int Eyesight { get; set; }
        //public int Endurance { get; set; }
        //public int Dodge { get; set; }


        public FishAgent(IPropertyStorage propertyStorage)
            : base(propertyStorage)
        {
            MovementSpeed = 110;
            Strength = 0;
            Health = 50;
            Eyesight = 70;
            Endurance = 20;
            Dodge = 0;
        }



        public override IAction GetNextAction(List<IEntity> otherEntities)
        {
            //Find all enemies witin Eyesight range
            List<IEntity> nearEnemies = otherEntities.FindAll(x => x.GetType() != typeof(FishAgent) && x is Agent && AIVector.Distance(Position, x.Position) < AIModifiers.maxMeleeAttackRange);
            if (nearEnemies.Count > 0)
            {
                return new Attack((Agent)nearEnemies[0]);
            }

            //Find all food withing eyesight range
            List<IEntity> plants = otherEntities.FindAll(x => x is Plant && AIVector.Distance(Position, x.Position) < AIModifiers.maxFeedingRange);
            if (plants.Count > 0)
            {
                return new Feed((Plant)plants[0]);
            }

            return new Move(new AIVector(-1, 0));

        }
        public override void ActionResultCallback(bool success)
        {
            
        }


    }
}
