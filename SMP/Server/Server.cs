﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace SMP
{
	public class Server
	{
		Server s;
		public bool shuttingDown = false;
		public static Socket listen;
		public static World mainlevel;
		public static int protocolversion = 14;
		public static string name = "sc";
		public static int port = 25565;
		public static Logger ServerLogger = new Logger();
		
		public Server()
		{
			Log("Starting Server");
			s = this;
			
			//TODO update thread for pos
			Setup();

			Log("Server Started");
		}

		public bool Setup()
		{
			//TODO: (in order)
			//load configuration
			Command.InitCore();
			//load plugins
			//load groups
			//load whitelist, banlist, reservelist //ask Keith (Silent) if you need/want to know what that is
			
			try
			{
				IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, port);
				listen = new Socket(endpoint.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				listen.Bind(endpoint);
				listen.Listen((int)SocketOptionName.MaxConnections);

				listen.BeginAccept(new AsyncCallback(Accept), null);
				return true;
			}
			catch (SocketException e) { Log(e.Message); return false; }
			catch (Exception e) { Log(e.Message); return false; }
		}

		void Accept(IAsyncResult result)
		{
			if (shuttingDown == false)
			{
				Player p = null;
				bool begin = false;
				try
				{
					using (p = new Player(listen.EndAccept(result)))
					{
						listen.BeginAccept(new AsyncCallback(Accept), null);
					}
					begin = true;
				}
				catch (SocketException e)
				{
					if (p != null)
						p.Disconnect();
					if (!begin)
						listen.BeginAccept(new AsyncCallback(Accept), null);
				}
				catch (Exception e)
				{
					Log(e.Message);
					if (p != null)
						p.Disconnect();
					if (!begin)
						listen.BeginAccept(new AsyncCallback(Accept), null);
				}
			}
		}

		/// <summary>
		/// To be depracted and replaced with ServerLogger 
		/// </summary>
		/// <param name="message">
		/// A <see cref="System.String"/>
		/// </param>
		public static void Log(string message)
		{
			ServerLogger.Log(message);
		}
	}
}
