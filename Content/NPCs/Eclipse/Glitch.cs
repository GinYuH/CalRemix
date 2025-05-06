using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using CalamityMod.Items.Materials;
using Terraria.Audio;
using CalamityMod.Buffs.DamageOverTime;

namespace CalRemix.Content.NPCs.Eclipse
{
    public class Glitch : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Glitch");
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 30;
            NPC.height = 32;
            NPC.lifeMax = 3000;
            NPC.damage = 50;
            NPC.defense = 30;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(gold: 15);
            NPC.HitSound = null;
            NPC.DeathSound = null;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
        }

        public override void AI()
        {
            NPC.ai[0]++;
            if (NPC.ai[0] % 120 == 0)
            {
                if (NPC.CountNPCS(ModContent.NPCType<Corruption>()) < 7)
                {
                    if (Main.rand.NextBool())
                    {
                        SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Zombie_" + Main.rand.Next(1, 131)) with { PitchRange = (-1, 1) }, NPC.Center);
                    }
                    else
                    {
                        SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/Item_" + Main.rand.Next(1, 179)) with { PitchRange = (-1, 1) }, NPC.Center);
                    }
                    CalRemixHelper.SpawnNewNPC(NPC.GetSource_FromThis(), NPC.Center, ModContent.NPCType<Corruption>(), ai0: Main.rand.Next(0, 4));
                }
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Events.Eclipse,
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!DownedBossSystem.downedDoG || NPC.AnyNPCs(Type))
                return 0f;

            return SpawnCondition.SolarEclipse.Chance * 0.1f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<DarksunFragment>(), 1, 4, 6);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Vector2 npcOffset = NPC.Center - screenPos;
            spriteBatch.Draw(TextureAssets.Npc[Type].Value, npcOffset, null, NPC.GetAlpha(Color.White), 0f, TextureAssets.Npc[Type].Size() / 2, 1f, SpriteEffects.None, 1f);
            return false;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.rand.NextBool(4))
                target.AddBuff(BuffID.Cursed, CalamityUtils.SecondsToFrames(25));
            target.AddBuff(ModContent.BuffType<Vaporfied>(), CalamityUtils.SecondsToFrames(4));
        }
    }
}