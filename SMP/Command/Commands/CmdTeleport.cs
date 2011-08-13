﻿using System;
using System.Collections.Generic;

namespace SMP
{
    public class CmdTeleport : Command
    {
        public override string Name { get { return "teleport"; } }
        public override List<String> Shortcuts { get { return new List<string> { "tp", "tppos" }; } }
        public override string Category { get { return "Mod"; } }
        public override bool ConsoleUseable { get { return false; } }
        public override string Description { get { return "Teleports to specified player or location"; } } //used for displaying what the commands does when using /help
        public override string PermissionNode { get { return "core.mod.teleport"; } }

        public override void Use(Player p, params string[] args)
        {
            if (args.Length == 0)
            {
                Help(p);
                return;
            }
            if (args.Length == 1)
            {
                Player who = Player.FindPlayer(args[0]); // cannot use a using here or players dissapear.
                if (who != null)
                {
                    p.Teleport_Player(who.pos[0], who.pos[1], who.pos[2]);
                    return;
                }
				else
				{
					p.SendMessage(HelpBot + "Can not find player.");	
				}
            }
            if (args.Length == 3)
            {
                p.Teleport_Player((double)(int.Parse(args[0])), (double)(int.Parse(args[1])), (double)(int.Parse(args[2])));
                return;
            }
            Help(p);
            /*byte[] bytes = new byte[41]; // some extra code.
            util.EndianBitConverter.Big.GetBytes(p.level.SpawnX).CopyTo(bytes, 0);
            util.EndianBitConverter.Big.GetBytes(p.Stance).CopyTo(bytes, 8);
            util.EndianBitConverter.Big.GetBytes(p.level.SpawnY).CopyTo(bytes, 16);
            util.EndianBitConverter.Big.GetBytes(p.level.SpawnZ).CopyTo(bytes, 24);
            util.EndianBitConverter.Big.GetBytes(p.rot[0]).CopyTo(bytes, 32);
            util.EndianBitConverter.Big.GetBytes(p.rot[1]).CopyTo(bytes, 36);
            bytes[40] = p.onground;
            p.SendRaw(0x0D, bytes);*/
        }

        public override void Help(Player p)
        {
            p.SendMessage(Description);
            p.SendMessage("/tp [player]");
        }
    }
}