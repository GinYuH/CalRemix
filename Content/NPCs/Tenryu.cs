using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using Terraria.ModLoader.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Projectiles.Melee;
using Terraria.Audio;
using CalamityMod.Projectiles.Typeless;

namespace CalRemix.Content.NPCs
{
    public class Tenryu : ModNPC
    {
        public Player Target => Main.player[NPC.target];
        public ref float Timer => ref NPC.ai[1];
        public ref float State => ref NPC.ai[2];
        private bool wait = false;
        public override void SetStaticDefaults()
        {
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<Nightwither>()] = true;
            NPCID.Sets.TrailingMode[Type] = 1;
        }
        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.ChaosElemental);
            AIType = NPCID.ChaosElemental;
            NPC.lifeMax = 1200;
            NPC.damage = 200;
            NPC.defense = 23;
            NPC.knockBackResist = 0.25f;
            NPC.value = Item.buyPrice(silver: 10);
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath50;
        }
        public override void AI()
        {
            if (!NPC.HasValidTarget)
            {
                Timer = 0;
                return;
            }
            Vector2 org = new(NPC.Center.X, NPC.Top.Y - 48f);
            Timer++;
            if (Timer > 300)
            {
                NPC.ai[3] = 180;
                wait = true;
                Timer = 0;
            }
            if (Timer % 12 == 0 && Timer > 180)
                SoundEngine.PlaySound(SoundID.Item43 with { Pitch = (Timer > 300) ? 300 : Timer / 300 }, NPC.Center);
            if (NPC.ai[3] >= 180 && wait && Main.netMode != NetmodeID.MultiplayerClient)
            {
                SoundEngine.PlaySound(SoundID.Item84, NPC.Center);
                Projectile proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), org, org.DirectionTo(Target.Center) * 12f, ModContent.ProjectileType<StratusBlackHole>(), NPC.damage, 6f);
                proj.friendly = false;
                proj.hostile = true;
                proj.tileCollide = false;
                proj.penetrate = 1;
                wait = false;
            }
        }
        public override void DrawEffects(ref Color drawColor)
        {
            Vector2 org = new(NPC.Center.X, NPC.Top.Y - 48f);
            if (Timer % 12 == 0 && Timer > 180)
            {
                for (int i = 0; i < 5; i++)
                {
                    Dust dust = Dust.NewDustDirect(org, 1, 1, DustID.DungeonSpirit);
                    dust.velocity = NPC.velocity + Vector2.One.RotatedByRandom(MathHelper.ToRadians(360)) * (2.5f + Main.rand.NextFloat());
                    dust.scale = 1.0f + Main.rand.NextFloat() / 2f;
                    dust.noGravity = true;
                }
            }
            if (Timer % 6 == 0 && Main.rand.NextBool(3))
                Gore.NewGore(NPC.GetSource_FromAI(), NPC.position, Vector2.One.RotatedByRandom(MathHelper.ToRadians(360)) * (0.5f + Main.rand.NextFloat() / 2), Main.rand.Next(16, 18));
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] 
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon,
		        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.PlayerSafe && spawnInfo.Player.ZoneDungeon && DownedBossSystem.downedPolterghast)
            {
                return SpawnCondition.Dungeon.Chance * 0.1f;
            }
            return 0f;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<RuinousSoul>(), 10, 1, 1);
            npcLoot.Add(ModContent.ItemType<ExodiumCluster>(), 1, 5, 17);
            npcLoot.Add(ModContent.ItemType<Lumenyl>(), 3, 1, 1);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            NPC.spriteDirection = NPC.direction;
            SpriteEffects effect = (NPC.spriteDirection > 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            for (int i = 0; i < 10; i += 2)
                spriteBatch.Draw(texture, NPC.oldPos[i] + new Vector2(NPC.width, NPC.height) / 2f - screenPos + new Vector2(0f, NPC.gfxOffY), null, new Color(0, 200, 255, 100), NPC.rotation, texture.Size() / 2, NPC.scale, effect, 0f);
            spriteBatch.Draw(texture, NPC.position + new Vector2(NPC.width, NPC.height) / 2f - screenPos + new Vector2(0f, NPC.gfxOffY), null, new Color(255, 255, 255, 255), NPC.rotation, texture.Size() / 2, NPC.scale, effect, 0f);
            return false;
        }
    }
}
