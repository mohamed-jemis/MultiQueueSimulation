using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace MultiQueueModels
{
     public class ReadingFiles
    {
        public static SimulationSystem readfiles()
        {
            SimulationSystem system = new SimulationSystem();

            //CAUTION: Path is not relative
            using (StreamReader file = new StreamReader("C:\\Users\\avata\\Desktop\\MultiQueueSimulation\\MultiQueueSimulation\\TestCases\\TestCase1.txt"))
            {

                string ln;
                while ((ln = file.ReadLine()) != null)
                {
                    ln = file.ReadLine();
                    system.NumberOfServers = int.Parse(ln);
                    file.ReadLine();
                    file.ReadLine();
                    ln = file.ReadLine();
                    system.StoppingNumber = int.Parse(ln);
                    file.ReadLine();
                    file.ReadLine();
                    ln = file.ReadLine();
                    system.StoppingCriteria = (Enums.StoppingCriteria)int.Parse(ln);
                    file.ReadLine();
                    file.ReadLine();
                    ln = file.ReadLine();
                    system.SelectionMethod = (Enums.SelectionMethod)int.Parse(ln);
                    file.ReadLine();
                    file.ReadLine();
                    while (true)
                    {
                        TimeDistribution timeDistribution = new TimeDistribution();
                        List<String> listStrLineElements;
                        ln = file.ReadLine();
                        if (ln == "")
                        {
                            break;
                        }
                        listStrLineElements = ln.Split(',').ToList();
                        timeDistribution.Time = int.Parse(listStrLineElements[0]);
                        timeDistribution.Probability = decimal.Parse(listStrLineElements[1].Trim());


                        system.InterarrivalDistribution.Add(timeDistribution);

                    }

                    for (int i = 0; i < system.NumberOfServers; i++)
                    {
                        system.Servers.Add(new Server());

                        file.ReadLine();
                        while (true)
                        {
                            TimeDistribution timeDistribution = new TimeDistribution();
                            List<String> listStrLineElements;
                            ln = file.ReadLine();
                            if (ln == "" || ln == null)
                            {
                                break;
                            }
                            listStrLineElements = ln.Split(',').ToList();
                            timeDistribution.Time = int.Parse(listStrLineElements[0]);
                            timeDistribution.Probability = decimal.Parse(listStrLineElements[1].Trim());


                            system.Servers[i].TimeDistribution.Add(timeDistribution);

                        }
                    }
                }
                file.Close();
            }

            return system;
        }
    }
}
