using SharpNetCraft.Utils;
using SharpNetCraft.Utils.Math;

namespace SharpNetCraft.Pakets.Play
{
	public class NamedSoundEffectPacket : Packet<NamedSoundEffectPacket>
	{
		public string           SoundName  { get; set; }
		public SoundEffectPacket.SoundCategory Category { get; set; }
		public Vector3       Position { get; set; }
		public float         Volume   { get; set; }
		public float         Pitch    { get; set; }
		/// <inheritdoc />
		public override void Decode(MinecraftStream stream)
		{
			SoundName = stream.ReadString();
			Category = (SoundEffectPacket.SoundCategory) stream.ReadVarInt();
			var x = stream.ReadInt();
			var y = stream.ReadInt();
			var z = stream.ReadInt();

			Position = new Vector3(x / 8f, y / 8f, z / 8f);
			
			Volume = stream.ReadFloat();
			Pitch = stream.ReadFloat();
		}

		/// <inheritdoc />
		public override void Encode(MinecraftStream stream)
		{
			throw new System.NotImplementedException();
		}
	}
}