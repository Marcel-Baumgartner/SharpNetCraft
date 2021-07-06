﻿using SharpNetCraft.Utils;
using fNbt;

namespace SharpNetCraft.Pakets.Play
{
	public class JoinGamePacket : Packet<JoinGamePacket>
	{
		public JoinGamePacket()
		{
			PacketId = 0x24;
		}

		public int         EntityId;
		public byte        Gamemode, PreviousGamemode;
		public NbtCompound Dimension;
		//public byte Difficulty;
		//public byte MaxPlayers;
		public bool ReducedDebugInfo;
		public long HashedSeed;
		public bool EnableRespawnScreen;

		public string[] WorldNames;
		public NbtCompound DimensionCodec;
		public string WorldName;

		public int ViewDistance;

		public bool IsDebug, IsFlat;
		public bool IsHardcore;
		public override void Decode(MinecraftStream stream)
		{
			EntityId = stream.ReadInt();
			IsHardcore = stream.ReadBool();
			Gamemode = (byte) stream.ReadByte();
			PreviousGamemode = (byte) stream.ReadByte();

			int worldCount = stream.ReadVarInt();
			WorldNames = new string[worldCount];
			for (int i = 0; i < worldCount; i++)
			{
				WorldNames[i] = stream.ReadString();
			}

			DimensionCodec = stream.ReadNbtCompound();

			Dimension = stream.ReadNbtCompound(); //stream.ReadString();
			WorldName = stream.ReadString();
			
			HashedSeed = stream.ReadLong();
			//Difficulty = (byte) stream.ReadByte();
			stream.ReadByte();
			ViewDistance = stream.ReadVarInt();
			ReducedDebugInfo = stream.ReadBool();
			EnableRespawnScreen = stream.ReadBool();
			IsDebug = stream.ReadBool();
			IsFlat = stream.ReadBool();
		}

		public override void Encode(MinecraftStream stream)
		{
			/*stream.WriteInt(EntityId);
			stream.WriteByte(Gamemode);
			stream.WriteString(Dimension);
			stream.WriteLong(HashedSeed);
		//	stream.WriteByte(Difficulty);
			stream.WriteByte(255);
			stream.WriteBool(ReducedDebugInfo);
			stream.WriteBool(EnableRespawnScreen);*/
		}
	}
}
