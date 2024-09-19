using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Core.Retheme.NoFab
{
    public class Horse : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(ModContent.MountType<HorseMount>(), player, false);
            player.buffTime[buffIndex] = 10;
            player.Calamity().fab = false;
        }
    }
    public class HorseMount : ModMount
    {
        public override void SetStaticDefaults()
        {
            MountData.spawnDust = DustID.Torch;
            MountData.spawnDustNoGravity = true;
            MountData.buff = ModContent.BuffType<Horse>();
            MountData.heightBoost = 35;
            MountData.fallDamage = 0f;
            MountData.runSpeed = 5.6f;
            MountData.dashSpeed = 17.6f;
            MountData.flightTimeMax = 9999;
            MountData.fatigueMax = 0;
            MountData.jumpHeight = 12;
            MountData.acceleration = 0.4f;
            MountData.jumpSpeed = 9.21f;
            MountData.swimSpeed = 4f;
            MountData.blockExtraJumps = false;
            MountData.totalFrames = 15;
            MountData.constantJump = false;
            int num = 26;
            int[] array = new int[MountData.totalFrames];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = num;
            }
            array[1] = (array[3] = (array[5] = (array[7] = (array[12] = num - 2))));
            MountData.playerYOffsets = array;
            MountData.xOffset = -4;
            MountData.bodyFrame = 3;
            MountData.yOffset = 5;
            MountData.playerHeadOffset = 36;
            MountData.standingFrameCount = 1;
            MountData.standingFrameDelay = 12;
            MountData.standingFrameStart = 0;
            MountData.runningFrameCount = 8;
            MountData.runningFrameDelay = 42;
            MountData.runningFrameStart = 1;
            MountData.flyingFrameCount = 6;
            MountData.flyingFrameDelay = 4;
            MountData.flyingFrameStart = 9;
            MountData.inAirFrameCount = 1;
            MountData.inAirFrameDelay = 12;
            MountData.inAirFrameStart = 10;
            MountData.idleFrameCount = 1;
            MountData.idleFrameDelay = 12;
            MountData.idleFrameStart = 5;
            MountData.idleFrameLoop = true;
            MountData.swimFrameCount = MountData.inAirFrameCount;
            MountData.swimFrameDelay = MountData.inAirFrameDelay;
            MountData.swimFrameStart = MountData.inAirFrameStart;
            if (Main.netMode != NetmodeID.Server)
            {
                MountData.frontTextureExtra = ModContent.Request<Texture2D>("CalRemix/Core/Retheme/NoFab/HorseMount_Extra");
                MountData.textureWidth = Utils.Width(MountData.backTexture);
                MountData.textureHeight = Utils.Height(MountData.backTexture);
            }
        }
        public override void UpdateEffects(Player player)
        {
            if (player.HasBuff(BuffID.Inferno))
            {
                player.GetDamage<GenericDamageClass>() += 0.1f;
            }
            if (player.velocity.Length() > 9f)
            {
                int num = Main.rand.Next(2);
                bool flag = false;
                if (num == 1)
                {
                    flag = true;
                }
                Color color;
                if (flag)
                {
                    color = new Color(255, 68, 242);
                }
                else
                {
                    color = new Color(25, 105, 255);
                }
                Rectangle rect = player.getRect();
                int num2 = Dust.NewDust(new Vector2((float)rect.X, (float)rect.Y), rect.Width, rect.Height, DustID.Lava, 0f, 0f, 0, color, 1.25f);
                Main.dust[num2].noGravity = true;
            }
            if (player.velocity.Y != 0f)
            {
                if (player.mount.PlayerOffset == 28)
                {
                    if (!player.flapSound)
                    {
                        SoundEngine.PlaySound(SoundID.Item32, new Vector2?(player.Center), null);
                    }
                    player.flapSound = true;
                    return;
                }
                player.flapSound = false;
            }
        }
    }
}
