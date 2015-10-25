using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using firecracker;

namespace firecracker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("House ID (A-P): ");
            string houseInput = Console.ReadLine();
            FirecrackerSerialPort.House houseId;

            while (!Enum.TryParse<FirecrackerSerialPort.House>(houseInput, true, out houseId))
            {
                Console.WriteLine("Invalid House ID, try again");
                houseInput = Console.ReadLine();
            }

            Console.Write("Module Number (1-16): ");
            string moduleInput = Console.ReadLine();
            int moduleNumber = 0;

            while (!int.TryParse(moduleInput, out moduleNumber) || moduleNumber > 16 || moduleNumber < 1)
            {
                Console.WriteLine("Invalid Module Number, try again");
                moduleInput = Console.ReadLine();
            }

            Console.Write("On/Off: ");
            string action = Console.ReadLine();
            FirecrackerSerialPort.ModuleState moduleState;
            while (!Enum.TryParse<FirecrackerSerialPort.ModuleState>(action, true, out moduleState))
            {
                Console.WriteLine("\"on\" or \"off\" only, try again");
                action = Console.ReadLine();
            }
                        
            FirecrackerSerialPort fc = new FirecrackerSerialPort("COM3");
            fc.Initialize();
            fc.SendCommand(houseId, moduleNumber, moduleState);
            System.Threading.Thread.Sleep(1000);
            //Console.ReadKey();
            fc.Close();
        }
    }
}
