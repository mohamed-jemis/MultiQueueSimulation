using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MultiQueueTesting;
using MultiQueueModels;
using System.IO;

namespace MultiQueueSimulation
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            SimulationSystem system = ReadingFiles.readfiles();
            TimeDistribution.cumlativeprob(system);

            //stopping criteria >> number of customers
            if (system.StoppingCriteria== Enums.StoppingCriteria.NumberOfCustomers)
            {
                SimulationSystem.create_table(system);
            }
            //stopping criteria >> Simulation end time 
            else
            {
              //we'll have to know how many customers entered the system (SimulationSystem.StoppingNumber)
            }

            PerformanceMeasures PM = new PerformanceMeasures();
            PM.AverageWaitingTime = PerformanceMeasures.waitingTime /system.StoppingNumber;
            //max queue lenght >> missing
            PM.WaitingProbability=PerformanceMeasures.WaitedCustomers / system.StoppingNumber;

            for(int i=0;i<system.NumberOfServers;i++)
            {
                system.Servers[i].IdleProbability = system.Servers[i].IdleTime / system.SimulationTable[system.StoppingNumber - 1].EndTime;
                if(system.Servers[i].SimTable.Count!=0)
                system.Servers[i].AverageServiceTime = system.Servers[i].serviceTime / system.Servers[i].SimTable.Count;
                system.Servers[i].Utilization= system.Servers[i].serviceTime / system.SimulationTable[system.StoppingNumber - 1].EndTime;
            }
            string result = TestingManager.Test(system, Constants.FileNames.TestCase1);
            MessageBox.Show(result);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

        }
    }
}
