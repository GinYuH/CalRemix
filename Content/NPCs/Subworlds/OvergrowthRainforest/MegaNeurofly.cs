using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalamityMod.BiomeManagers;
using CalRemix.Content.Items.Placeables.Banners;
using Microsoft.Xna.Framework;
using System;
using Terraria.Enums;
using CalamityMod.NPCs.NormalNPCs;
using Microsoft.Xna.Framework.Graphics;
using CalRemix.Content.Items.Placeables.Subworlds.Wolf;
using Terraria.GameContent;
using CalamityMod.Items.Materials;
using CalRemix.Core.Biomes.Subworlds;

namespace CalRemix.Content.NPCs.Subworlds.OvergrowthRainforest
{
    public class MegaNeurofly : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 1;
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.GreenDragonfly);
            NPC.aiStyle = NPCAIStyleID.Dragonfly;
            AIType = NPCID.GreenDragonfly;
            NPC.damage = 0;
            NPC.width = 60;
            NPC.height = 60;
            NPC.defense = 20;
            NPC.lifeMax = 2000;
            NPC.value = Item.buyPrice(silver: 1);
            NPC.HitSound = SoundID.NPCHit32 with { Pitch = 0.4f };
            NPC.DeathSound = SoundID.NPCDeath35 with { Pitch = 0.4f };
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().VulnerableToSickness = true;
            SpawnModBiomes = [ModContent.GetInstance<OvergrowthRainforestBiome>().Type, ModContent.GetInstance<TitanicTrunksBiome>().Type];
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
        public override void AI()
        {
            if (CalRemixAddon.CalVal != null)
            {
                foreach (Projectile p in Main.ActiveProjectiles)
                {
                    if (p.type == ProjectileID.CorruptSpray || p.type == ProjectileID.VilePowder)
                    {
                        if (p.getRect().Intersects(NPC.getRect()))
                        {
                            NPC.Transform(CalRemixAddon.CalVal.Find<ModNPC>("NeuroFly").Type);
                            break;
                        }
                    }
                }
            }
            NPC.spriteDirection = NPC.direction;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = 0;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[Type].Value;
            Texture2D wing = ModContent.Request<Texture2D>(Texture + "_Wing").Value;
            float wingOrigXProximal = (NPC.spriteDirection == 1) ? 0 : wing.Width;
            float wingOrigXDistal = (NPC.spriteDirection == -1) ? 0 : wing.Width;
            spriteBatch.Draw(wing, NPC.Center - screenPos, null, drawColor * 0.3f, MathF.Sin(Main.GlobalTimeWrappedHourly * -44 - NPC.whoAmI), new Vector2(wingOrigXProximal, wing.Height), NPC.scale, NPC.FlippedEffects(true), 0);
            spriteBatch.Draw(tex, NPC.Center - screenPos, null, NPC.GetAlpha(drawColor), NPC.rotation, tex.Size() / 2, NPC.scale, NPC.FlippedEffects(), 0);
            spriteBatch.Draw(wing, NPC.Center - screenPos, null, drawColor * 0.3f, MathF.Sin(Main.GlobalTimeWrappedHourly * 44 + NPC.whoAmI), new Vector2(wingOrigXDistal, wing.Height), NPC.scale, NPC.FlippedEffects(false), 0);
            return false;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GreenBlood, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GreenBlood, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<SulphuricScale>(), 3);
        }
    }
}
