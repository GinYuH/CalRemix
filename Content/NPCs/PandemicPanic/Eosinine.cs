using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using System;
using CalRemix.Content.Projectiles.Hostile;
using CalRemix.Core.Biomes;

namespace CalRemix.Content.NPCs.PandemicPanic
{
    public class Eosinine : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eosinine");
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 60;
            NPC.width = 42;
            NPC.height = 42;
            NPC.defense = 4;
            NPC.lifeMax = 600;
            NPC.knockBackResist = 0f;
            NPC.value = 0;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.scale = 2f;
            NPC.HitSound = CalamityMod.NPCs.Perforator.PerforatorHeadMedium.HitSound;
            NPC.DeathSound = CalamityMod.NPCs.Perforator.PerforatorHeadMedium.DeathSound;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<PandemicPanicBiome>().Type };
        }

        public override void AI()
        {
            int xSpeed = 5;
            float yRange = 5;
            float ySpeedMult = 0.05f;
            if (NPC.ai[0] == 0)
            {
                NPC.velocity.X = Main.rand.NextBool() ? -xSpeed : xSpeed;
                NPC.ai[0] = 1;
            }
            NPC.ai[1]++;
            NPC.velocity.Y = (float)Math.Sin(NPC.ai[1] * ySpeedMult) * yRange;
            if (Main.netMode != NetmodeID.MultiplayerClient)
                if (NPC.ai[1] % 30 == 0)
            {
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(-NPC.velocity.X, 0), ModContent.ProjectileType<EosinineProj>(), (int)(NPC.damage * 0.33f), 0f);
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(NPC.velocity.X * 2, 0), ModContent.ProjectileType<EosinineProj>(), (int)(NPC.damage * 0.33f), 0f);
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement("Allergens are a common and consistent problem throughout the world. Cells like these are always ready to combat them.")
            });
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.IsABestiaryIconDummy)
                return true;
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Vector2 position = NPC.Center - Main.screenPosition;
            Vector2 origin = texture.Size() * 0.5f;
            Color color = Color.MediumPurple * 0.1f;
            Vector2 scale = new Vector2(NPC.scale * MathHelper.Max(0.2f, 1 - Math.Abs((NPC.velocity.Y / 10))), NPC.scale * MathHelper.Max(0.2f, Math.Abs(NPC.velocity.Y / 10)));
            for (int i = 0; i < 10; i++)
            {
                Vector2 vector2 = (MathF.PI * 2f * (float)i / 10f).ToRotationVector2() + (MathF.PI * 2f * (float)i / 10f).ToRotationVector2() * 2 * Math.Abs((float)Math.Sin(Main.GlobalTimeWrappedHourly));
                Main.spriteBatch.Draw(texture, position + vector2, null, color, NPC.rotation, origin, scale, SpriteEffects.None, 0f);
            }
            Main.spriteBatch.Draw(texture, position, null, NPC.GetAlpha(drawColor), NPC.rotation, origin, scale, SpriteEffects.None, 0f);
            if (Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().phd)
            {
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture + "_Gray").Value, position, null, Color.Lime, NPC.rotation, origin, scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.ShadowbeamStaff, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.ShadowbeamStaff, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }
    }
}
