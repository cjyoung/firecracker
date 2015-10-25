using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using firecracker;

//cboBaudRate.Items.Add(300);
//cboBaudRate.Items.Add(600);
//cboBaudRate.Items.Add(1200);
//cboBaudRate.Items.Add(2400);
//cboBaudRate.Items.Add(9600);
//cboBaudRate.Items.Add(14400);
//cboBaudRate.Items.Add(19200);
//cboBaudRate.Items.Add(38400);
//cboBaudRate.Items.Add(57600);
//cboBaudRate.Items.Add(115200);

//REFERENCE: http://www.linuxha.com/common/cm17a.html

namespace firecracker
{
    class Program
    {
        static void Main(string[] args)
        {
            FirecrackerSerialPort fc = new FirecrackerSerialPort("COM3");

            fc.Initialize();
            fc.SendCommand(FirecrackerSerialPort.House.A, 3, FirecrackerSerialPort.ModuleState.Off);
            System.Threading.Thread.Sleep(1000);
            fc.Close();
        }
    }


    class FirecrackerSerialPort : SerialPort
    {
        public FirecrackerSerialPort(string Port)
            : base(Port)
        {
        }

        public void Initialize()
        {
            Handshake = Handshake.None;
            //BaudRate = 19200;
            Open();
        }

        public void SendCommand(House house, int moduleNumber, ModuleState state)
        {
            byte[] a1on = GetBytesForModule(house, moduleNumber, state);
            SendHeader();
            SendByte(a1on[0]);
            SendByte(a1on[1]);
            SendFooter();
        }

        private void Reset()
        {
            DtrEnable = false;
            RtsEnable = false;

            DtrEnable = true;
            RtsEnable = true;
        }

        private void SendHeader()
        {
            SendByte(0xD5);
            SendByte(0xAA);
        }

        private void SendByte(byte b)
        {
            for (int i = 8; i > 0; i--)
            {
                var bit = (b & (1 << i - 1)) != 0;
                Console.Write(bit ? 1 : 0);

                if (bit)
                    Send1Bit();
                else
                    Send0Bit();
            }
            Console.Write(' ');
        }

        private void Send0Bit()
        {
            if (!this.DtrEnable)
                Console.WriteLine("oh no, dtr is low");

            RtsEnable = false;
            RtsEnable = true;
        }

        private void Send1Bit()
        {
            if (!this.RtsEnable)
                Console.WriteLine("oh no, rts is low");

            DtrEnable = false;
            DtrEnable = true;
        }

        private void SendFooter()
        {
            SendByte(0xAD);
        }

        private static byte[] GetBytesForModule(House house, int moduleNumber, ModuleState state)
        {
            byte[] ret = new byte[2];
            ret[0] = (byte)house;
            ret[1] = 0x0;

            //if module is 9-16, update household byte
            if (moduleNumber > 9)
                ret[0] = (byte)(ret[0] | 0x04);

            if (moduleNumber == 1 || moduleNumber == 9)
                //1/9   0000 0000 0x00
                ret[1] = 0x00;
            else if (moduleNumber == 2 || moduleNumber == 10)
                //2/10  0001 0000 0x10
                ret[1] = 0x10;
            else if (moduleNumber == 3 || moduleNumber == 11)
                //3/11  0000 1000 0x08
                ret[1] = 0x08;
            else if (moduleNumber == 4 || moduleNumber == 12)
                //4/12  0001 1000 0x18
                ret[1] = 0x18;
            else if (moduleNumber == 5 || moduleNumber == 13)
                //5/13  0100 0000 0x40
                ret[1] = 0x40;
            else if (moduleNumber == 6 || moduleNumber == 14)
                //6/14  0101 0000 0x50
                ret[1] = 0x50;
            else if (moduleNumber == 7 || moduleNumber == 15)
                //7/15  0100 1000 0x48
                ret[1] = 0x48;
            else
                //8/16  0101 1000 0x58
                ret[1] = 0x58;

            //apply on/off state
            ret[1] = (byte)(ret[1] | (byte)state);

            return ret; //"or" the bytes together to get the on or off
        }

        public enum ModuleState
        {
            On = 0x00,
            Off = 0x20
        }

        public enum House
        {
            A = 0x60,
            B = 0x70,
            C = 0x40,
            D = 0x50,
            E = 0x80,
            F = 0x90,
            G = 0xA0,
            H = 0xB0,
            I = 0xE0,
            J = 0xF0,
            K = 0xC0,
            L = 0xD0,
            M = 0x00,
            N = 0x10,
            O = 0x20,
            P = 0x30,
        }

    }
}
