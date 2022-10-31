using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiQueueModels
{
    public class TimeDistribution
    {
        public int Time { get; set; }
        public decimal Probability { get; set; }
        public decimal CummProbability { get; set; }
        public int MinRange { get; set; }
        public int MaxRange { get; set; }
        public static void cumlativeprob(SimulationSystem system)
        {
            //Server distribution
            decimal service_accumilative = 0;
            for (int i = 0; i < system.NumberOfServers; i++)
            {
                 service_accumilative = 0;
                for (int j = 0; j < system.Servers[i].TimeDistribution.Count; j++)
                {
                    system.Servers[i].TimeDistribution[j].CummProbability = service_accumilative + system.Servers[i].TimeDistribution[j].Probability;
                    system.Servers[i].TimeDistribution[j].MinRange = (int)(service_accumilative * 100) + 1;
                    service_accumilative += system.Servers[i].TimeDistribution[j].Probability;
                    system.Servers[i].TimeDistribution[j].MaxRange = (int)(service_accumilative * 100);
                }
            }
            //interarrival distribution
            service_accumilative = 0;
            for (int j = 0; j < system.InterarrivalDistribution.Count; j++)
            {

                system.InterarrivalDistribution[j].CummProbability = service_accumilative + system.InterarrivalDistribution[j].Probability;
                system.InterarrivalDistribution[j].MinRange = (int)(service_accumilative * 100) + 1;
                service_accumilative += system.InterarrivalDistribution[j].Probability;
                system.InterarrivalDistribution[j].MaxRange = (int)(service_accumilative * 100);
            }
        }

        public static void interarrivalMapping(SimulationSystem system, SimulationCase case1)
        {
            for (int j = 0; j < system.InterarrivalDistribution.Count; j++)
            {
                if (case1.RandomInterArrival >= system.InterarrivalDistribution[j].MinRange && case1.RandomInterArrival <= system.InterarrivalDistribution[j].MaxRange)
                {
                    case1.InterArrival = system.InterarrivalDistribution[j].Time;
                }
            }
        }

        public static void serviceTimeMapping(SimulationSystem system, SimulationCase case1)
        {
            for (int j = 0; j < case1.AssignedServer.TimeDistribution.Count; j++)
            {
                if (case1.RandomService >= case1.AssignedServer.TimeDistribution[j].MinRange && case1.RandomService <= case1.AssignedServer.TimeDistribution[j].MaxRange)
                {
                    case1.ServiceTime = case1.AssignedServer.TimeDistribution[j].Time;
                }
            }
        }
    }
}
