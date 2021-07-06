﻿using System;
using System.Threading;

using MiNET.Utils.IO;

using SharpNetCraft.Pakets.Play;
using SharpNetCraft.Utils.Data;
using SharpNetCraft.Utils.Math;
using SharpNetCraft.Utils.Provider;

namespace SharpNetCraft
{
    public class JavaNetworkProvider
    {
        private NetConnection Client { get; }
        private HighPrecisionTimer NetworkReportTimer { get; }
        public JavaNetworkProvider(NetConnection client)
        {
            Client = client;

            NetworkReportTimer = new HighPrecisionTimer(1000,
                        state =>
                        {
                            long packetSizeOut = Interlocked.Exchange(ref Client.PacketSizeOut, 0L);
                            long packetSizeIn = Interlocked.Exchange(ref Client.PacketSizeIn, 0L);

                            long packetCountOut = Interlocked.Exchange(ref Client.PacketsOut, 0L);
                            long packetCountIn = Interlocked.Exchange(ref Client.PacketsIn, 0L);

                            _connectionInfo = new ConnectionInfo(
                                Client.StartTime, Client.Latency, -1, -1, -1, -1, -1,
                                packetSizeIn, packetSizeOut, packetCountIn, packetCountOut);
                        });
        }

        /// <inheritdoc />
        public bool IsConnected => Client.IsConnected;

        private ConnectionInfo _connectionInfo = new ConnectionInfo(DateTime.UtcNow, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        public ConnectionInfo GetConnectionInfo()
        {
            return _connectionInfo;
        }

        /// <inheritdoc />
        public void PlayerOnGroundChanged(Player player, bool onGround)
        {
            if (Client.ConnectionState != ConnectionState.Play)
                return;

            PlayerMovementPacket packet = PlayerMovementPacket.CreateObject();
            packet.OnGround = onGround;

            Client.SendPacket(packet);
        }

        /// <inheritdoc />
        public void EntityFell(long entityId, float distance, bool inVoid)
        {

        }

        public void EntityAction(int entityId, EntityAction action)
        {
            if (action == SharpNetCraft.Utils.Data.EntityAction.Jump)
                return;

            EntityActionPacket packet = EntityActionPacket.CreateObject();

            packet.EntityId = entityId;
            packet.Action = action;
            packet.JumpBoost = 0;
            Client.SendPacket(packet);
        }

        /// <inheritdoc />
        public void PlayerAnimate(PlayerAnimations animation)
        {
            var packet = AnimationPacket.CreateObject();
            switch (animation)
            {
                case PlayerAnimations.SwingLeftArm:
                    packet.Hand = 1;
                    break;

                case PlayerAnimations.SwingRightArm:
                    packet.Hand = 0;
                    break;
            }

            Client.SendPacket(packet);
        }

        public void SendChatMessage(string message)
        {
            var packet = ChatMessagePacket.CreateObject();
            packet.Position = ChatMessagePacket.Chat;
            packet.Message = message;
            packet.ServerBound = true;

            Client.SendPacket(packet);
        }

        public void BlockPlaced(BlockCoordinates position, BlockFace face, int hand, int slot, Vector3 cursorPosition, Entity p)
        {
            if (hand < 0) hand = 0;
            if (hand > 1) hand = 1;

            var packet = PlayerBlockPlacementPacket.CreateObject();
            packet.CursorPosition = cursorPosition;
            packet.Location = position;
            packet.Face = face;
            packet.Hand = hand;
            packet.InsideBlock = p.HeadInBlock;

            Client.SendPacket(packet);
        }

        public void PlayerDigging(DiggingStatus status, BlockCoordinates position, BlockFace face, Vector3 cursorPosition)
        {
            var packet = PlayerDiggingPacket.CreateObject();
            packet.Face = face;
            packet.Location = position;
            packet.Status = status;
            Client.SendPacket(packet);
        }

        public void EntityInteraction(Entity player, Entity target, ItemUseOnEntityAction action, int hand, int slot, Vector3 cursorPosition)
        {
            if (hand < 0) hand = 0;
            if (hand > 1) hand = 1;

            switch (action)
            {
                case ItemUseOnEntityAction.Interact:
                    {
                        var packet = InteractEntityPacket.CreateObject();
                        packet.EntityId = (int)target.EntityId;
                        packet.Type = 0;
                        packet.Hand = hand;
                        packet.Sneaking = player.IsSneaking;

                        Client.SendPacket(packet);
                    }
                    break;
                case ItemUseOnEntityAction.Attack:
                    {
                        var packet = InteractEntityPacket.CreateObject();
                        packet.EntityId = (int)target.EntityId;
                        packet.Type = 1;
                        packet.Hand = hand;
                        packet.Sneaking = player.IsSneaking;

                        Client.SendPacket(packet);
                    }
                    break;
                case ItemUseOnEntityAction.ItemInteract:
                    break;
            }
        }

        public void WorldInteraction(Entity entity, BlockCoordinates position, BlockFace face, int hand, int slot, Vector3 cursorPosition)
        {
            if (hand < 0) hand = 0;
            if (hand > 1) hand = 1;

            var packet = PlayerBlockPlacementPacket.CreateObject();
            packet.Location = position;
            packet.Face = face;
            packet.Hand = hand;
            packet.CursorPosition = cursorPosition;
            packet.InsideBlock = entity.HeadInBlock;

            Client.SendPacket(packet);
        }

        public void UseItem(Item item, int hand, ItemUseAction action, BlockCoordinates position, BlockFace face, Vector3 cursorPosition)
        {
            if (hand > 1)
                hand = 1;

            if (hand < 0)
                hand = 0;

            var packet = UseItemPacket.CreateObject();
            packet.Hand = hand;
            Client.SendPacket(packet);
        }

        public void HeldItemChanged(Item item, short slot)
        {
            var packet = HeldItemChangePacket.CreateObject();
            packet.Slot = slot;

            Client.SendPacket(packet);
        }

        /// <inheritdoc />
        public void DropItem(BlockCoordinates position, BlockFace face, Item item, bool dropFullStack)
        {
            var packet = PlayerDiggingPacket.CreateObject();
            packet.Face = face;
            packet.Location = position;
            packet.Status = DiggingStatus.DropItem;
            Client.SendPacket(packet);
        }

        public void Close()
        {
            //NetworkReportTimer?.Change(Timeout.Infinite, Timeout.Infinite);
            try
            {
                NetworkReportTimer?.Dispose();
            }
            catch { }
        }

        /// <inheritdoc />
        public void SendChatMessage(string message, UUID uuid)
        {
            Client.SendPacket(new ChatMessagePacket()
            { ServerBound = true, Message = message, UUID = uuid });
        }
    }
}
