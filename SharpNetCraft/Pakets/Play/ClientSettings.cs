﻿using SharpNetCraft.Utils;

namespace SharpNetCraft.Pakets.Play
{
	public class ClientSettingsPacket : Packet<ClientSettingsPacket>
	{
		public string Locale;
		public byte ViewDistance;
		public int ChatMode;
		public bool ChatColors;
		public byte SkinParts;
		public int MainHand;
		public bool DisableTextFiltering;

		public ClientSettingsPacket()
		{
			PacketId = 0x05;
		}

		public override void Decode(MinecraftStream stream)
		{
			Locale = stream.ReadString();
			ViewDistance = (byte) stream.ReadByte();
			ChatMode = stream.ReadVarInt();
			ChatColors = stream.ReadBool();
			SkinParts = (byte) stream.ReadByte();
			MainHand = stream.ReadVarInt();
			DisableTextFiltering = stream.ReadBool();
		}

		public override void Encode(MinecraftStream stream)
		{
			stream.WriteString(Locale);
			stream.WriteByte(ViewDistance);
			stream.WriteVarInt(ChatMode);
			stream.WriteBool(ChatColors);
			stream.WriteByte(SkinParts);
			stream.WriteVarInt(MainHand);
			stream.WriteBool(DisableTextFiltering);
		}
	}
}
