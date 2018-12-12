using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIFramework;
using AIFramework.Entities;
using System.Diagnostics;
using System.Reflection;

namespace FishAgent


{
    public class FishAgentFactory : AgentFactory
    {
        public override Agent CreateAgent(IPropertyStorage propertyStorage)
        {
            return new FishAgent(propertyStorage);
        }

        public override Agent CreateAgent(Agent parent1, Agent parent2, IPropertyStorage propertyStorage)
        {
            return new FishAgent(propertyStorage);
        }

        public override Type ProvidedAgentType
        {
            get { return typeof(FishAgent); }
        }

        public override string Creators
        {
            get { return "Fish"; }
        }

        public override void RegisterWinners(List<Agent> sortedAfterDeathTime)
        {
            //Do data collection - Perhaps used to evolutionary algoritmen
        }
    }
}
