using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Windows.Forms;

namespace NetworkMonitor
{
    class NetMon
    {
        private readonly Network network;

        private const int gigaByte = 1000000000;

        private const int megaByte = 1000000;

        private const int pauseForTenSeconds = 10000;

        private const string folderPath = @"c:\temp\NetworkMonitor";

        private const string fileName = "Log";

        private readonly string localHost = SystemInformation.ComputerName;

        public NetMon()
        {
            network = new Network();

            Console.Title = $"{Assembly.GetExecutingAssembly().GetName().Name} {Assembly.GetExecutingAssembly().GetName().Version}";
        }

        private IEnumerable<NetworkInterface> GetNetworkInterfaces()
        { 
            return NetworkInterface.GetAllNetworkInterfaces();
        }

        public void DisplayHostNetworkInterfaceInfo()
        {
            try
            {
                while (true)
                {
                    Console.Clear();

                    Console.WriteLine("\n\t=====| Network Adapter Info |=====\n\n");

                    if (GetNetworkInterfaces() != null)
                    {
                        switch (GetNetworkInterfaces().Count())
                        {
                            case 0:
                                Console.Write("\n\tNo network devices present.\n\nPress any key to exit...");

                                Console.ReadKey();

                                break;

                            default:
                                foreach (var netInt in GetNetworkInterfaces())
                                {
                                    var ipProps = netInt.GetIPProperties();

                                    var ipv4Props = netInt.GetIPv4Statistics();

                                    if (netInt.Speed / gigaByte >= 1)
                                    {
                                        Console.WriteLine($"\tName: {netInt.Name}" +
                                                    $"\n\tId: {netInt.Id}" +
                                                    $"\n\tDescription: {netInt.Description}" +
                                                    $"\n\tStatus: {netInt.OperationalStatus}" +
                                                    $"\n\tSpeed: {netInt.Speed / gigaByte} G/bits per second" +
                                                    $"\n\tInterface type: {netInt.NetworkInterfaceType}" +
                                                    $"\n\tNetwork available: {network.IsAvailable}");

                                        Console.WriteLine("\n\tIPv4 and IPv6 addresses");

                                        foreach (System.Net.IPAddress addresses in System.Net.Dns.GetHostEntry(SystemInformation.ComputerName).AddressList)
                                        {
                                            Console.WriteLine($"\t{addresses}");
                                        }

                                        Console.WriteLine();
                                    }
                                    else
                                    {
                                        Console.WriteLine($"\tName: {netInt.Name}" +
                                                    $"\n\tId: {netInt.Id}" +
                                                    $"\n\tDescription: {netInt.Description}" +
                                                    $"\n\tStatus: {netInt.OperationalStatus}" +
                                                    $"\n\tSpeed: {netInt.Speed / megaByte} M/bits per second" +
                                                    $"\n\tInterface type: {netInt.NetworkInterfaceType}" +
                                                    $"\n\tNetwork available: {network.IsAvailable}");

                                        Console.WriteLine("\n\tIPv4 and IPv6 addresses");

                                        foreach (System.Net.IPAddress addresses in System.Net.Dns.GetHostEntry(localHost).AddressList)
                                        {
                                            Console.WriteLine($"\t{addresses}");
                                        }
                                    }

                                    foreach (var ipProp in ipProps.GatewayAddresses)
                                        Console.WriteLine($"\tRouter gateway: {ipProp.Address}");

                                    if (ipv4Props.BytesSent >= gigaByte)
                                    {
                                        //Console.WriteLine($"\tBytes received: {ipv4Props.BytesSent / gigaByte}GB");
                                        Console.WriteLine($"\tBytes received: {ipv4Props.BytesSent:d1}");
                                    }
                                    else
                                    {
                                        //Console.WriteLine($"\tBytes received: {ipv4Props.BytesSent / megaByte}MB");
                                        Console.WriteLine($"\tBytes received: {ipv4Props.BytesSent:d1}");
                                    }

                                    if (ipv4Props.BytesReceived >= gigaByte)
                                    {
                                        //Console.WriteLine($"\tBytes sent: {ipv4Props.BytesReceived / gigaByte}GB");
                                        Console.WriteLine($"\tBytes sent: {ipv4Props.BytesReceived:d1}");
                                    }
                                    else
                                    {
                                        //Console.WriteLine($"\tBytes sent: {ipv4Props.BytesReceived / megaByte}MB");
                                        Console.WriteLine($"\tBytes sent: {ipv4Props.BytesReceived:d1}");
                                    }

                                    Console.WriteLine($"\tUnicast packets received: {ipv4Props.UnicastPacketsReceived:d1}");

                                    Console.WriteLine($"\tUnicast packets sent: {ipv4Props.UnicastPacketsSent:d1}");

                                    Console.WriteLine($"\tIncoming unknown protocol packets: {ipv4Props.IncomingUnknownProtocolPackets}");

                                    Console.WriteLine($"\tIncoming packets discarded: {ipv4Props.IncomingPacketsDiscarded}\n\n");
                                }
                                break;
                        }
                    }
                    else
                        throw new ArgumentNullException($"No network hardware has been detected on {localHost}.", new Exception("Network hardware"));

                    System.Threading.Thread.Sleep(pauseForTenSeconds);
                }
            }
            catch (Exception errors)
            {
                NetMonErrorLogger.ErrorLogger(errors, folderPath, fileName);
            }
        }
    }
}
