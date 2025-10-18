using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Mounts
{
    public class DepthGlider : ModMount
    {
        public override void SetStaticDefaults()
        {
            MountData.buff = ModContent.BuffType<Buffs.DepthGliderBuff>();
            MountData.heightBoost = 15;
            MountData.fallDamage = 0f;
            MountData.runSpeed = 14f;
            MountData.dashSpeed = 14f;
            MountData.flightTimeMax = int.MaxValue - 1;
            MountData.fatigueMax = int.MaxValue - 1;
            MountData.jumpHeight = 12;
            MountData.usesHover = true;
            MountData.acceleration = 0.55f;
            MountData.jumpSpeed = 5f;
            MountData.blockExtraJumps = false;
            MountData.totalFrames = 1;
            MountData.spawnDust = DustID.Clentaminator_Cyan;
            MountData.spawnDustNoGravity = true;
            int[] array = new int[MountData.totalFrames];
            for (int l = 0; l < array.Length; l++)
            {
                array[l] = 0; // (increase this?)
            }
            MountData.playerYOffsets = array;
            MountData.bodyFrame = 6;
            MountData.yOffset = 10;
            MountData.xOffset = 10;
            MountData.playerHeadOffset = 0;
            MountData.standingFrameCount = 1;
            MountData.standingFrameDelay = 8;
            MountData.standingFrameStart = 0;
            MountData.runningFrameCount = 1;
            MountData.runningFrameDelay = 6;
            MountData.runningFrameStart = 0;
            MountData.flyingFrameCount = 1;
            MountData.flyingFrameDelay = 6;
            MountData.flyingFrameStart = 0;
            MountData.inAirFrameCount = 1;
            MountData.inAirFrameDelay = 7;
            MountData.inAirFrameStart = 0;
            MountData.idleFrameCount = 1;
            MountData.idleFrameDelay = 8;
            MountData.idleFrameStart = 0;
            MountData.idleFrameLoop = false;
            MountData.swimFrameCount = 1;
            MountData.swimFrameDelay = 40;
            MountData.swimFrameStart = 0;
            if (Main.netMode == NetmodeID.Server)
            {
                return;
            }

            MountData.textureWidth = MountData.frontTexture.Width();
            MountData.textureHeight = MountData.frontTexture.Height();
        }

        public override void UpdateEffects(Player player)
        {
            base.UpdateEffects(player);
        }

        public override void Dismount(Player player, ref bool skipDust)
        {
            NPC.NewNPC(player.GetSource_FromThis(), (int)player.Center.X, (int)player.Center.Y, ModContent.NPCType<NPCs.Subworlds.GreatSea.DepthGlider>());
        }
    }
}