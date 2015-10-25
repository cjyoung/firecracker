/*Console.WriteLine("Don't forget to open and reset before sending commands");

            ConsoleKeyInfo key = Console.ReadKey();

            while (key.Key != ConsoleKey.Escape)
            {
                switch (key.Key)
                {
                    case ConsoleKey.H:
                        Console.WriteLine("Sending Header");
                        fc.SendHeader();
                        break;
                    case ConsoleKey.F:
                        Console.WriteLine("Sending Footer");
                        fc.SendFooter();
                        break;
                    case ConsoleKey.D0:
                        fc.Send0Bit();
                        break;
                    case ConsoleKey.D1:
                        fc.Send1Bit();
                        break;
                    case ConsoleKey.R:
                        Console.WriteLine("Resetting");
                        fc.Reset();
                        break;
                    case ConsoleKey.O:
                        Console.WriteLine("Opening COM port");
                        fc.Open();
                        break;
                    case ConsoleKey.C:
                        Console.WriteLine("Closing COM port");
                        fc.Close();
                        break;
                    case ConsoleKey.A:
                        byte[] a1on = GetBytesForModule(House.A, 1, ModuleState.On);
                        fc.SendByte(a1on[0]);
                        fc.SendByte(a1on[1]);
                        break;
                    case ConsoleKey.B:
                        byte[] a1off = GetBytesForModule(House.A, 1, ModuleState.Off);
                        fc.SendByte(a1off[0]);
                        fc.SendByte(a1off[1]);
                        break;
                    default:
                        break;
                }

                key = Console.ReadKey();
            }*/