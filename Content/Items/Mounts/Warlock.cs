using CalamityMod.Graphics.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.Mounts
{
    public class Warlock : ModMount
    {
        public override void SetStaticDefaults()
        {
            MountData.buff = ModContent.BuffType<Buffs.WarlockBuff>();
            MountData.heightBoost = 15;
            MountData.fallDamage = 0f;
            MountData.runSpeed = 14f;
            MountData.dashSpeed = 14f;
            MountData.flightTimeMax = int.MaxValue - 1;
            MountData.fatigueMax = int.MaxValue - 1;
            MountData.jumpHeight = 12;
            MountData.usesHover = true;
            MountData.acceleration = 0.35f;
            MountData.jumpSpeed = 5f;
            MountData.blockExtraJumps = false;
            MountData.totalFrames = 14;
            MountData.spawnDust = 127;
            MountData.spawnDustNoGravity = true;
            int[] array = new int[MountData.totalFrames];
            for (int l = 0; l < array.Length; l++)
            {
                array[l] = 0; // (increase this?)
            }
            MountData.playerYOffsets = array;
            MountData.bodyFrame = 6;
            MountData.yOffset = 10;
            MountData.xOffset = -238;
            MountData.playerHeadOffset = 0;
            MountData.standingFrameCount = 14;
            MountData.standingFrameDelay = 8;
            MountData.standingFrameStart = 0;
            MountData.runningFrameCount = 14;
            MountData.runningFrameDelay = 6;
            MountData.runningFrameStart = 0;
            MountData.flyingFrameCount = 14;
            MountData.flyingFrameDelay = 6;
            MountData.flyingFrameStart = 0;
            MountData.inAirFrameCount = 14;
            MountData.inAirFrameDelay = 7;
            MountData.inAirFrameStart = 0;
            MountData.idleFrameCount = 14;
            MountData.idleFrameDelay = 8;
            MountData.idleFrameStart = 0;
            MountData.idleFrameLoop = false;
            MountData.swimFrameCount = 14;
            MountData.swimFrameDelay = 40;
            MountData.swimFrameStart = 0;
            if (Main.netMode == NetmodeID.Server)
            {
                return;
            }

            MountData.textureWidth = MountData.frontTexture.Width();
            MountData.textureHeight = MountData.frontTexture.Height();
        }

        public List<Vector2> trailPoints = new List<Vector2>();
        public List<Vector2> trailPoints2 = new List<Vector2>();
        public List<Vector2> circlePoints = new List<Vector2>();

        public override void UpdateEffects(Player player)
        {
            MountData.totalFrames = 14;
            Vector2 playerPos = player.MountedCenter;

            int circleNum = 50;
            if (player.miscCounter % 6 == 0 || circlePoints.Count <= 0)
            {
                circlePoints.Clear();
                circlePoints.Add(playerPos + Vector2.UnitY * Main.rand.NextFloat(70f) - player.Center);
                for (int i = 0; i < circleNum; i++)
                {
                    circlePoints.Add(playerPos + Vector2.UnitY.RotatedBy(MathHelper.Lerp(0, MathHelper.TwoPi, (i) / (float)(circleNum - 1))) * Main.rand.NextFloat(50f, 100f) - player.Center);
                }
                circlePoints.Add(playerPos + Vector2.UnitY * Main.rand.NextFloat(70f) - player.Center);
                trailPoints.Clear();
                trailPoints2.Clear();
                Vector2 end = player.Center - Vector2.UnitX * player.direction * 240 + Vector2.UnitY * 40;
                for (int i = 0; i < circleNum; i++)
                {
                    trailPoints.Add((Vector2.Lerp(end, playerPos, ((i + 1) / (float)circleNum))) + Main.rand.NextVector2Circular(20, 20) - player.Center);
                    trailPoints2.Add((Vector2.Lerp(end, playerPos, ((i + 1) / (float)circleNum))) + Main.rand.NextVector2Circular(40, 40) - player.Center);
                }
            }
        }

        public override bool Draw(List<DrawData> playerDrawData, int drawType, Player drawPlayer, ref Texture2D texture, ref Texture2D glowTexture, ref Vector2 drawPosition, ref Rectangle frame, ref Color drawColor, ref Color glowColor, ref float rotation, ref SpriteEffects spriteEffects, ref Vector2 drawOrigin, ref float drawScale, float shadow)
        {
            GameShaders.Misc["CalamityMod:HeavenlyGaleLightningArc"].UseImage1("Images/Misc/Perlin");
            GameShaders.Misc["CalamityMod:HeavenlyGaleLightningArc"].Apply();


            PrimitiveRenderer.RenderTrail(trailPoints, new((float comp, Vector2 v) => 6, (float comp, Vector2 v) => Color.Lime, (_, _) => drawPlayer.Center, shader: GameShaders.Misc["CalamityMod:HeavenlyGaleLightningArc"]), 80);
            PrimitiveRenderer.RenderTrail(trailPoints, new((float comp, Vector2 v) => 2, (float comp, Vector2 v) => Color.White, (_, _) => drawPlayer.Center, shader: GameShaders.Misc["CalamityMod:HeavenlyGaleLightningArc"]), 80);
            PrimitiveRenderer.RenderTrail(trailPoints2, new((float comp, Vector2 v) => 6, (float comp, Vector2 v) => Color.Lime, (_, _) => drawPlayer.Center, shader: GameShaders.Misc["CalamityMod:HeavenlyGaleLightningArc"]), 80);
            PrimitiveRenderer.RenderTrail(trailPoints2, new((float comp, Vector2 v) => 2, (float comp, Vector2 v) => Color.White, (_, _) => drawPlayer.Center, shader: GameShaders.Misc["CalamityMod:HeavenlyGaleLightningArc"]), 80);


            PrimitiveRenderer.RenderTrail(circlePoints, new((float comp, Vector2 v) => 6, (float comp, Vector2 v) => Color.Lime, (_, _) => drawPlayer.Center, shader: GameShaders.Misc["CalamityMod:HeavenlyGaleLightningArc"]), 80);
            PrimitiveRenderer.RenderTrail(circlePoints, new((float comp, Vector2 v) => 2, (float comp, Vector2 v) => Color.White, (_, _) => drawPlayer.Center, shader: GameShaders.Misc["CalamityMod:HeavenlyGaleLightningArc"]), 80);

            return true;
        }
    }
}