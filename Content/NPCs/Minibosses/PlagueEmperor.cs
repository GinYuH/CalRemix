using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.GameContent.ItemDropRules;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Materials;
using Terraria.Audio;
using CalamityMod.Items.Pets;
using System;
using CalamityMod.NPCs.PlaguebringerGoliath;
using CalRemix.Content.Projectiles.Hostile;
using System.IO;
using CalRemix.Core.World;
using CalRemix.Core.Biomes;

namespace CalRemix.Content.NPCs.Minibosses
{
    public class PlagueEmperor : ModNPC
    {
        public Player Target => Main.player[NPC.target];
        public ref float Move => ref NPC.ai[0];
        public ref float Attack => ref NPC.ai[1];
        public bool activated = false;
        public override void SetStaticDefaults()
        {
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<Plague>()] = true;
        }
        public override bool SpecialOnKill()
        {
            RemixDowned.downedPlagueEmperor = true;
            return false;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 250;
            NPC.height = 260;
            NPC.lifeMax = 420000;
            NPC.damage = 0;
            NPC.defense = 40;
            NPC.DR_NERD(0.2f);
            NPC.dontTakeDamage = true;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(gold: 40);
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<PlagueBiome>().Type };
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(activated);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            activated = reader.ReadBoolean();
        }
        public override void AI()
        {
            Attack++;
            Move += 0.05f;
            NPC.position.Y += (float)Math.Cos(Move / 6.50) * 0.25f;
            NPC.TargetClosest();
            if (Target.Distance(NPC.Center) < 480 && !activated)
            {
                SoundEngine.PlaySound(PlaguebringerGoliath.AttackSwitchSound, NPC.Center);
                Attack = 0;
                activated = true;
                NPC.dontTakeDamage = false;
            }
            if (Attack > 300 && !activated)
            {
                SoundEngine.PlaySound(SoundID.Item10, NPC.Center);
                Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.One.RotatedByRandom(MathHelper.ToRadians(360)) * 4f, ModContent.ProjectileType<EvilPlagueSeeker>(), 140 / 2, 6f, ai0: NPC.whoAmI);
                Attack = 0;
            }
            if (!activated)
                return;
            if (Attack % 180 == 0)
            {
                SoundEngine.PlaySound(SoundID.Item10, NPC.Center);
                CalRemixHelper.SpawnNewNPC(NPC.GetSource_FromAI(), NPC.Center, ModContent.NPCType<EvilPlagueBee>(), npcTasks: (NPC npc) =>
                {
                    npc.velocity = NPC.DirectionTo(Target.Center) * 6f;
                });
            }
            if (Attack % 90 == 0)
            {
                SoundEngine.PlaySound(SoundID.Item10, NPC.Center);
                Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(Target.Center) * 12f, ModContent.ProjectileType<EvilPlagueSeeker>(), 140 / 2, 6f, ai0: NPC.whoAmI);
            }
            if (Attack > 600)
            {
                SoundEngine.PlaySound(PlaguebringerGoliath.BarrageLaunchSound, NPC.Center);
                CalRemixHelper.SpawnNewNPC(NPC.GetSource_FromAI(), NPC.Center, ModContent.NPCType<PlagueHomingMissile>(), npcTasks: (NPC npc) =>
                {
                    npc.velocity = NPC.DirectionTo(Target.Center) * 10f;
                    npc.damage = Main.expertMode ? 280 : 140;
                    npc.dontTakeDamage = true;
                });
                CalRemixHelper.SpawnNewNPC(NPC.GetSource_FromAI(), NPC.Center, ModContent.NPCType<PlagueHomingMissile>(), npcTasks: (NPC npc) =>
                {
                    npc.velocity = NPC.DirectionTo(Target.Center).RotatedBy(MathHelper.ToRadians(-45)).RotatedByRandom(MathHelper.ToRadians(22.5f)) * 10f;
                    npc.damage = Main.expertMode ? 280 : 140;
                    npc.dontTakeDamage = true;
                });
                CalRemixHelper.SpawnNewNPC(NPC.GetSource_FromAI(), NPC.Center, ModContent.NPCType<PlagueHomingMissile>(), npcTasks: (NPC npc) =>
                {
                    npc.velocity = NPC.DirectionTo(Target.Center).RotatedBy(MathHelper.ToRadians(45)).RotatedByRandom(MathHelper.ToRadians(22.5f)) * 10f;
                    npc.damage = Main.expertMode ? 280 : 140;
                    npc.dontTakeDamage = true;
                });
                Attack = 0;
            }
        }
        public override void DrawEffects(ref Color drawColor)
        {
            for (int i = 0; i < 10; i++)
            {
                double angle = Main.rand.NextDouble() * 2d * Math.PI;
                Vector2 offset = new();
                offset.X += (float)(Math.Sin(angle) * 480);
                offset.Y += (float)(Math.Cos(angle) * 480);
                Dust dust = Dust.NewDustDirect(NPC.Center + offset, 0, 0, DustID.GemEmerald);
                dust.noGravity = true;
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] 
            {
		        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.GetModPlayer<CalRemixPlayer>().ZonePlague && !NPC.AnyNPCs(Type))
                return 0.015f;
            return 0f;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            LeadingConditionRule expert = npcLoot.DefineConditionalDropSet(() => Main.expertMode);
            expert.Add(ModContent.ItemType<PlagueCellCanister>(), 1, 89, 270);
            expert.AddFail(ModContent.ItemType<PlagueCellCanister>(), 1, 56, 230);
            expert.Add(ItemID.LunarBar, 1, 89, 270);
            expert.AddFail(ItemID.LunarBar, 1, 56, 230);
            expert.Add(ModContent.ItemType<InfectedArmorPlating>(), 1, 36, 45);
            expert.AddFail(ModContent.ItemType<InfectedArmorPlating>(), 1, 20, 36);
            npcLoot.Add(ModContent.ItemType<PlagueCaller>(), 5, 1, 1);
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Minibosses/PlagueEmperorGlow").Value;
            spriteBatch.Draw(texture, NPC.position + new Vector2(NPC.width, NPC.height) / 2f - screenPos + new Vector2(0f, NPC.gfxOffY), null, new Color(255, 255, 255, 255), NPC.rotation, texture.Size() / 2, NPC.scale, SpriteEffects.None, 0f);
            if (activated)
            {
                Texture2D eyes = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/Minibosses/PlagueEmperorEyes").Value;
                spriteBatch.Draw(eyes, NPC.position + new Vector2(NPC.width, NPC.height) / 2f - screenPos + new Vector2(0f, NPC.gfxOffY), null, new Color(255, 255, 255, 255), NPC.rotation, eyes.Size() / 2, NPC.scale, SpriteEffects.None, 0f);
            }
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, CalRemix.CalMod.Find<ModGore>("Plagueshell2").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, CalRemix.CalMod.Find<ModGore>("Plagueshell3").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, CalRemix.CalMod.Find<ModGore>("PlaguebringerGoliathGore6").Type, NPC.scale);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, CalRemix.CalMod.Find<ModGore>("PlaguebringerGoliathGore6").Type, NPC.scale);
                }
            }
        }
    }
}
