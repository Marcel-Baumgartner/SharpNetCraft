﻿using System;

using SharpNetCraft.Utils;
using SharpNetCraft.Utils.Data;
using SharpNetCraft.Utils.Math;

namespace SharpNetCraft.Pakets.Play
{
    public class PlayerBlockPlacementPacket : Packet<PlayerBlockPlacementPacket>
    {
        public BlockCoordinates Location;
        public BlockFace Face;
        public int Hand;
        public Vector3 CursorPosition;
        public bool InsideBlock;
        public PlayerBlockPlacementPacket()
        {
            PacketId = 0x2E;
        }

        public override void Decode(MinecraftStream stream)
        {
            throw new NotImplementedException();
        }

        public override void Encode(MinecraftStream stream)
        {
            stream.WriteVarInt(Hand);
            stream.WritePosition(Location);
            switch (Face)
            {
                case BlockFace.Down:
                    stream.WriteVarInt(0);
                    break;
                case BlockFace.Up:
                    stream.WriteVarInt(1);
                    break;
                case BlockFace.North:
                    stream.WriteVarInt(2);
                    break;
                case BlockFace.South:
                    stream.WriteVarInt(3);
                    break;
                case BlockFace.West:
                    stream.WriteVarInt(4);
                    break;
                case BlockFace.East:
                    stream.WriteVarInt(5);
                    break;
                case BlockFace.None:
                    stream.WriteVarInt(1);
                    break;
            }
            
            stream.WriteFloat(CursorPosition.X);
            stream.WriteFloat(CursorPosition.Y);
            stream.WriteFloat(CursorPosition.Z);
            stream.WriteBool(InsideBlock);
        }
    }
}
