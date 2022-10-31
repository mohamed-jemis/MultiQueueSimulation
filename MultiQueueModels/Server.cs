using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiQueueModels
{
    public class Server
    {
        public Server()
        {
            this.TimeDistribution = new List<TimeDistribution>();
            this.SimTable = new List<SimulationCase>();
        }

        public int ID { get; set; }
        public decimal IdleProbability { get; set; }
        public int IdleTime { get; set; }
        public decimal AverageServiceTime { get; set; } 
        public decimal Utilization { get; set; }
        public decimal serviceTime { get; set; }
        //all the cases that enters the server(cases per server)--for high priority 
        public List<SimulationCase> SimTable { get; set; }


        public List<TimeDistribution> TimeDistribution;
     

        //optional if needed use them
        public int FinishTime { get; set; }
        public int TotalWorkingTime { get; set; }
        public static void create_id(SimulationSystem system )
        {
            for (int i = 1; i <= system.NumberOfServers; i++)
            {
                system.Servers[i - 1].ID = i;
            }
        }
        
 
    }
}
