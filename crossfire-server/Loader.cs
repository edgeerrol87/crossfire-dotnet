﻿using System;
using System.Collections;
using System.IO;
using System.Threading;
using crossfire_server.server;
using Console = crossfire_server.util.Console;

namespace crossfire_server
{
    internal class Loader
    {
        private Thread _thread;
        private ArrayList _servers = new ArrayList();

        public Loader(string[] args) {
            _servers.Add(new LoginServer(args));
            _servers.Add(new GameServer(args));
        }
        
        public void Start() {
            foreach (Server server in _servers)
            {
                server.Start();
                Thread.Sleep(100);
            }
            _thread = new Thread(Run);
            _thread.Start();
        }

        public void StopAll()
        {
            foreach (Server server in _servers)
            {
                server.Stop();
            }
            _thread.Interrupt();
        }

        private void Run()
        {
            try
            {
                while (true)
                {
                    string args = System.Console.ReadLine();
                    if (args.ToLower().Equals("info"))
                    {
                        foreach (Server server in _servers)
                        {
                            server.GetServerInfo();
                        }
                    } else if (args.ToLower().Equals("stop-all"))
                    {
                        StopAll();
                    }
                }
            }
            catch (IOException e) {
                Console.Log(e.Message);
            }
        }
        
        public static void Main(string[] args)
        {
            Loader loader = new Loader(args);
            loader.Start();
        }
    }
}