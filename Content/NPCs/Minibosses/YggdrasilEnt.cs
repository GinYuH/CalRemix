using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.GameContent.ItemDropRules;
using Microsoft.Xna.Framework;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Items.Materials;
using Terraria.Audio;
using System;
using CalRemix.Content.Projectiles.Hostile;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.Items.SummonItems;
using CalamityMod.World;
using CalamityMod.Items.Pets;
using System.IO;
using Terraria.ModLoader.Utilities;
using CalRemix.Core.World;
using CalRemix.Core.Biomes;

namespace CalRemix.Content.NPCs.Minibosses
{
    public class YggdrasilEnt : ModNPC
    {
        public Player Target => Main.player[NPC.target];
        public ref float Move => ref NPC.ai[0];
        public ref float State => ref NPC.ai[1];
        public ref float Attack => ref NPC.ai[2];

        public bool angry = false;
        private Vector2 pos;
        public override void SetStaticDefaults()
        {
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][ModContent.BuffType<HolyFlames>()] = true;
        }
        public override bool SpecialOnKill()
        {
            RemixDowned.downedYggdrasilEnt = true;
            return false;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 414;
            NPC.height = 444;
            NPC.lifeMax = 38000;
            NPC.damage = 180;
            if (CalamityWorld.death)
                NPC.damage = 206;
            else if (CalamityWorld.revenge)
                NPC.damage = 223;
            NPC.defense = 20;
            NPC.DR_NERD(0.1f);
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(gold: 60);
            NPC.HitSound = ScornEater.HitSound;
            NPC.DeathSound = ScornEater.DeathSound;
            NPC.lavaImmune = true;
            NPC.chaseable = false;
            NPC.noGravity = true; 
            SpawnModBiomes = new int[1] { ModContent.GetInstance<ProfanedDesertBiome>().Type };            
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(angry);
            writer.WriteVector2(pos);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            angry = reader.ReadBoolean();
            pos = reader.ReadVector2();
        }
        public override void AI()
        {
            Move += 0.05f;
            NPC.position.Y += (float)Math.Cos(Move / 6.50) * 0.25f;
            NPC.TargetClosest();
            if (NPC.life < NPC.lifeMax || Target.HasItem(ModContent.ItemType<BloodyVein>()))
            {
                NPC.chaseable = true;
                angry = true;

            }
             if (!angry)
                return;
            Attack++;
            if (State == 0)
            {
                pos = Target.Center + Vector2.One.RotatedByRandom(MathHelper.ToRadians(360)) * 1080f;
                State = 1;
            }
            if (Attack % 6 == 0)
            {
                Dust dust = Dust.NewDustDirect(pos, 0, 0, DustID.CopperCoin);
                dust.velocity = Vector2.One.RotatedByRandom(MathHelper.ToRadians(360)) * 6f;
            }
            if (Attack > 90)
            {
                if (Target.Distance(NPC.Center) < 4000)
                {
                    SoundEngine.PlaySound(SoundID.NPCDeath43, NPC.Center);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), pos, pos.DirectionTo(Target.Center) * 32f, ModContent.ProjectileType<YggThorn>(), 120 / 2, 5);
                    Attack = 0;
                    State = 0;
                }
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] 
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
		        new FlavorTextBestiaryInfoElement("A magical, living tree that contains a vast amount of divine energy. Its essence may contain the key to unlock something more sinister.")
            });
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 120);
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe || !NPC.downedMoonlord || spawnInfo.Player.Calamity().ZoneCalamity || NPC.AnyNPCs(Type))
                return 0f;
            if (SpawnCondition.Underworld.Chance > 0f)
                return SpawnCondition.Underworld.Chance / 22f;
            return SpawnCondition.OverworldHallow.Chance / 22f;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<ProfanedShard>(), 1, 1, 1);
            npcLoot.Add(ModContent.ItemType<UnholyEssence>(), 1, 6, 8);
            LeadingConditionRule expert = npcLoot.DefineConditionalDropSet(() => Main.expertMode);
            expert.Add(ItemID.Pearlwood, 1, 89, 270);
            expert.AddFail(ItemID.Pearlwood, 1, 56, 230);
        }
    }
}
