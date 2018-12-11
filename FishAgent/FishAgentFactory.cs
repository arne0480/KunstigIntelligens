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
    class FishAgentFactory : AgentFactory
    {

        public override Agent CreateAgent(IPropertyStorage propertyStorage)
        {
            return new FishAgent(propertyStorage);
        }
        public override Agent CreateAgent(Agent parent1, Agent parent2, IPropertyStorage propertyStorage)
        {
            //mutation or splicing can be done here
            return new FishAgent(propertyStorage);
        }
        public override void RegisterWinners(List<Agent> sortedAfterDeathTime)
        {
            //Do data collection - an evolutionary algortime can be used

        }
        public override Type ProvidedAgentType
        {
            //The type of agent the factory produces
            get { return typeof(FishAgent); }
        }
        public override string Creators
        {
            get { return "TEAM MAD"; }
        }
    }
}



//test
