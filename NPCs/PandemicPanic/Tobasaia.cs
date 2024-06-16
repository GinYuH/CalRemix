using CalamityMod.Dusts;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalamityMod.BiomeManagers;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using System;
using CalRemix.Projectiles.Hostile;
using CalRemix.Biomes;

namespace CalRemix.NPCs.PandemicPanic
{
    public class Tobasaia : ModNPC
    {
        Entity target = null;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tobasaia");
            Main.npcFrameCount[NPC.type] = 6;
        }

        public override void SetDefaults()
        {
            NPC.npcSlots = 0.5f;
            NPC.aiStyle = -1;
            NPC.damage = 60;
            NPC.width = 26; //324
            NPC.height = 60; //216
            NPC.defense = 5;
            NPC.lifeMax = 1500;
            NPC.knockBackResist = 0f;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            AIType = -1;
            NPC.value = Item.buyPrice(0, 0, 0, 0);
            NPC.HitSound = CalamityMod.NPCs.Perforator.PerforatorHeadMedium.HitSound;
            NPC.DeathSound = CalamityMod.NPCs.Perforator.PerforatorHeadMedium.DeathSound;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<PandemicPanicBiome>().Type };
        }

        public override void AI()
        {
            if (target == null || !target.active)
            {
                target = PandemicPanic.BioGetTarget(false, NPC);
            }
            if (target != null && target.active && !(target is NPC n && n.life <= 0))
            {
                NPC.ai[1]++;
                if (NPC.ai[1] >=0 && NPC.ai[1] % 5 == 0)
                {
                    if (NPC.ai[1] % 15 == 0)
                    SoundEngine.PlaySound(SoundID.DD2_BallistaTowerShot, NPC.Center);
                    Vector2 acidSpeed = (Vector2.UnitY *-16).RotatedBy(MathHelper.ToRadians((float)Math.Sin(NPC.ai[1] / 10) * 45));
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, acidSpeed, ModContent.ProjectileType<TobaccoSeed>(), NPC.damage, 0);
                    if (NPC.ai[1] > 120)
                    {
                        NPC.ai[1] = -180;
                    }
                }
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement("Among all of the viscous invading microbes, this one is unique in that it seems to go after plant-life instead of animal-life. How lost it is.")
            });
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
                return true;
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Vector2 position = NPC.Center - Main.screenPosition;
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 8);
            Color color = NPC.GetAlpha(Color.Red * 0.6f);
            Vector2 scale = Vector2.One;
            SpriteEffects fx = NPC.spriteDirection == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None;
            for (int i = 0; i < 10; i++)
            {
                Vector2 vector2 = (MathF.PI * 2f * (float)i / 10f).ToRotationVector2() + (MathF.PI * 2f * (float)i / 10f).ToRotationVector2() * 2 * Math.Abs((float)Math.Sin(Main.GlobalTimeWrappedHourly));
                Main.spriteBatch.Draw(texture, position + vector2, NPC.frame, color, NPC.rotation + MathHelper.Pi, origin, scale, fx, 0f);
            }
            Main.spriteBatch.Draw(texture, position, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation + MathHelper.Pi, origin, scale, fx, 0f);
            return false;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Plantera_Green, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Plantera_Green, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.075f;
            NPC.frameCounter %= Main.npcFrameCount[NPC.type];
            int frame = (int)NPC.frameCounter;
            NPC.frame.Y = frame * frameHeight;
        }
    }
}
