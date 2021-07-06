﻿using System;

using SharpNetCraft.Utils;

namespace SharpNetCraft.Pakets.Play
{
    public class ChangeGameStatePacket : Packet<ChangeGameStatePacket>
    {
	    public GameStateReason Reason;
	    public float Value;

		public override void Decode(MinecraftStream stream)
		{
			Reason = (GameStateReason) stream.ReadByte();
			Value = stream.ReadFloat();
		}

	    public override void Encode(MinecraftStream stream)
	    {
		    throw new NotImplementedException();
	    }
    }

	public enum GameStateReason : byte
	{
		InvalidBed = 0,
		EndRain = 1,
		StartRain = 2,
		ChangeGamemode = 3,
		ExitEnd = 4,
		DemoMessage = 5,
		ArrowHitPlayer = 6,
		FadeValue = 7,
		FadeTime = 8,
		PlayerElderGuardianMob = 10
	}
}
