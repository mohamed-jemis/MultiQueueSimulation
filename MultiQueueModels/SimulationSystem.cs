using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiQueueModels
{
    public class SimulationSystem
    {
        public SimulationSystem()
        {
            this.Servers = new List<Server>();
            for(int i = 1; i <= Servers.Count; i++)
            {
                free_Servers.Add(i);
                this.Servers[i].ID = i;
            }
            this.InterarrivalDistribution = new List<TimeDistribution>();
            this.PerformanceMeasures = new PerformanceMeasures();
            this.SimulationTable = new List<SimulationCase>();
        }

        ///////////// INPUTS ///////////// 
        public int NumberOfServers { get; set; }
        // number of customers
        public int StoppingNumber { get; set; }
        public List<Server> Servers { get; set; }
        public List<int> free_Servers { get; set; }
        public List<TimeDistribution> InterarrivalDistribution { get; set; }
        public Enums.StoppingCriteria StoppingCriteria { get; set; }
        public Enums.SelectionMethod SelectionMethod { get; set; }

        ///////////// OUTPUTS /////////////
        public List<SimulationCase> SimulationTable { get; set; }
        public PerformanceMeasures PerformanceMeasures { get; set; }
        public static List<int>get_free_list(SimulationSystem system,SimulationCase case1)
        {
            List<int> current_free_servers = new List<int>();
            for (int i = 0; i < system.NumberOfServers; i++)
            {
                int last = system.Servers[i].SimTable.Count - 1;

                if (last == -1 || system.Servers[i].SimTable[last].EndTime <= case1.ArrivalTime)
                {
                    current_free_servers.Add(i+1);
                }
            }
            return current_free_servers;
        }
        public static void create_table(SimulationSystem system)
        {
            Server.create_id(system);
            for (int i = 0; i < system.StoppingNumber; i++)
            {
                SimulationCase case1 = new SimulationCase();
                Random rnd = new Random();
                Server assigned= new Server() ;

                case1.RandomInterArrival = rnd.Next(1, 101);
                case1.RandomService = rnd.Next(1, 101);

                if (i == 0)
                {
                    case1.ArrivalTime = 0;
                    case1.StartTime = 0;
                }
                else
                {
                    TimeDistribution.interarrivalMapping(system, case1);
                    case1.ArrivalTime = system.SimulationTable[i - 1].ArrivalTime + case1.InterArrival;
                }

                //Selecting the server 
                
                if(system.SelectionMethod == Enums.SelectionMethod.HighestPriority)
                {
                    if (i==0)
                    {
                        assigned = system.Servers[0];
                    }
                    else
                    {
                        //??
                        assigned = SimulationSystem.HighPriorityServer(system, case1);
                    }

                }

                case1.AssignedServer = assigned;

                TimeDistribution.serviceTimeMapping(system, case1);

                case1.EndTime = case1.StartTime + case1.ServiceTime;
                assigned.SimTable.Add(case1);

                //idle time
                if (i!=0 && system.SimulationTable[i - 1].EndTime < case1.ArrivalTime && case1.AssignedServer.Equals(system.SimulationTable[i - 1].AssignedServer))
                {
                    case1.AssignedServer.IdleTime += (case1.ArrivalTime - system.SimulationTable[i - 1].EndTime);
                }
                else //waiting time
                {
                    if(i!=0)
                    {
                        //they have to be in the same assigned server .. 
                        //(elaboration) we have to back track till we reach to the same server that the current customer has been assigned to
                        int j = case1.AssignedServer.SimTable.Count - 1;//last case that has been assigned to the same server 
                        int serverEndTime = case1.AssignedServer.SimTable[j].EndTime;
                        case1.TimeInQueue = (serverEndTime - case1.ArrivalTime);
                        if(case1.TimeInQueue!=0)
                        {
                            PerformanceMeasures.waitingTime += case1.TimeInQueue;
                            PerformanceMeasures.WaitedCustomers += 1;
                        }
                    }
                }
                case1.StartTime = case1.ArrivalTime + case1.TimeInQueue;
                assigned.serviceTime += case1.ServiceTime;
                system.SimulationTable.Add(case1);

            }
        }
        
        public static Server HighPriorityServer(SimulationSystem system, SimulationCase case1)
        {
            //highest priority
            int min=1100000000;
            int index=0;
            for (int i= 0; i < system.NumberOfServers; i++)
            {
                int last = system.Servers[i].SimTable.Count - 1;
                
                if(last==-1 || system.Servers[i].SimTable[last].EndTime <= case1.ArrivalTime)
                {
                    return system.Servers[i];
                }
                else // find the minimum time
                {
                    if(min> system.Servers[i].SimTable[last].EndTime)
                    {
                        min = system.Servers[i].SimTable[last].EndTime;
                        index = i;
                    }
                }
            }
            return system.Servers[index];
        }
        //public static Server RandomServer(SimulationSystem system, int currentCase)
        //{
        //    for (int i = 0; i < system.NumberOfServers; i++)
        //    {

        //    }
        //    return null;
        //}
    }
}
