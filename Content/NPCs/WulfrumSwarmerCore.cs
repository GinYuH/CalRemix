using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.NPCs;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.Sounds;
using CalRemix.Content.Items.Placeables.Banners;
using CalRemix.Content.NPCs.Subworlds;
using CalRemix.Content.Projectiles;
using CalRemix.Content.Projectiles.Hostile;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace CalRemix.Content.NPCs
{
    public class WulfrumSwarmerCore : ModNPC
    {
        public int frame = 0;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
           
        }

        public override void SetDefaults()
        {
            NPC.width = NPC.height = 30;
            NPC.damage = 0;
            NPC.defense = 12;
            NPC.damage = 30;
            NPC.lifeMax = 2;
            NPC.knockBackResist = 0f;
            NPC.HitSound = SoundID.NPCHit3;
            NPC.DeathSound = SoundID.NPCHit53;
            NPC.dontTakeDamage = true;
            NPC.aiStyle = NPCAIStyleID.HoveringFighter;
            NPC.noGravity = true;

        }

        public bool TimeToDie = false;
        public int SwarmerCount = Main.rand.Next(2, 6);
        public List<int> Swarmers = null;
        public NPC SwarmerToSpawn = null;
        public int GuysDead = 0;

        public override void FindFrame(int frameheight)
        {
            float frameSpeed = 7f;
            NPC.frameCounter++;
            if (NPC.frameCounter >= frameSpeed)
            {
                if (NPC.frame.Y >= frameheight * (Main.npcFrameCount[Type] - 1))
                {
                    NPC.frame.Y = 0;
                }
                else
                {
                    NPC.frame.Y += frameheight;
                }
                NPC.frameCounter = 0;
            }
        }

        public override void AI()
        {

            if (Swarmers == null)
            {
                Swarmers = new(SwarmerCount);
            }

            // spawn swarmers
            if (NPC.Calamity().newAI[0] == 3)
            {
                for (int i = 0; i < SwarmerCount; i++)
                {
                    SwarmerToSpawn = NPC.NewNPCDirect(new EntitySource_Parent(NPC), NPC.Center, ModContent.NPCType<WulfrumSwarmer>());
                    Swarmers.Add(SwarmerToSpawn.whoAmI);
                }
            }

            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            NPC.Calamity().newAI[0]++;
            Lighting.AddLight(NPC.Center, new Vector3(0.2f, 1.5f, 0.65f));

            // check if a swarmer dies
            if (NPC.Calamity().newAI[0] % 20 == 0 && Swarmers != null && SwarmerToSpawn != null)
            {
                for (int i = 0; i < Swarmers.Count; i++)
                {
                    var SpecificSwarmer = Main.npc[Swarmers[i]];
                    if (SpecificSwarmer is null || SpecificSwarmer.active == false || SwarmerToSpawn.type != SpecificSwarmer.type)
                    {
                        GuysDead++;
                        // remove the swarmer from the array so it doesn't get counted a million times
                        Swarmers.RemoveAt(i);
                    }
                }
            }
            // begin to die if everyone is dead
            if (GuysDead == SwarmerCount && !TimeToDie)
            {
                TimeToDie = true;
            }

            if (!TimeToDie)
            {
                // move away from the player
                if (NPC.Center.Distance(player.Center) < 300 && NPC.Calamity().newAI[0] % 25 == 0)
                {
                    NPC.velocity += NPC.Center.DirectionFrom(player.Center) * Main.rand.Next(2, 4);
                }

                if (Main.rand.Next(20) == 1)
                {
                    Dust.NewDust(NPC.Center + new Vector2(0, -5), NPC.width, NPC.height, DustID.t_Martian, Main.rand.Next(10) * 0.1f, Main.rand.Next(10) * -0.1f, 0, default, 0.5f);
                    NPC.velocity.Y -= Main.rand.Next(3);
                }
            }
            //dying animation
            else
            {
                NPC.Calamity().newAI[1]++;
                NPC.velocity *= 0.93f;
                NPC.Center += new Vector2(0.03f * NPC.Calamity().newAI[1], 0.03f * NPC.Calamity().newAI[1]).RotatedByRandom(MathHelper.TwoPi);
                if (NPC.Calamity().newAI[1] % 49 == 0)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Dust.NewDust(NPC.Center, NPC.width / 3, NPC.height / 3, DustID.t_Martian, Main.rand.Next(-3, 4), Main.rand.Next(-4, -2), 0, default, 1f);
                    }
                    SoundEngine.PlaySound(SoundID.NPCHit53 with { Pitch = 1.5f }, NPC.Center);
                }
                if (NPC.Calamity().newAI[1] >= 200)
                {
                    NPC.StrikeInstantKill();
                }
            }
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            modifiers.Knockback.Base += 5;
            SoundEngine.PlaySound(SoundID.Item6 with { Pitch = 1.1f}, NPC.Center);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Electrified, 60);
            for (int j = 0; j < 10; j++)
            {
                Dust.NewDust(NPC.Center, NPC.width/3, NPC.height/3, DustID.t_Martian, Main.rand.Next(-3, 4), Main.rand.Next(-4, -2), 0, default, 1.5f);
            }
            NPC.velocity.X = 0;
            NPC.velocity.Y = -4;
        }
        public override bool PreKill()
        {
            for (int j = 0; j < 10; j++)
            {
                Dust.NewDust(NPC.Center, NPC.width / 3, NPC.height / 3, DustID.t_Martian, Main.rand.Next(-3, 4), Main.rand.Next(-4, -2), 0, default, 1.5f);
            }
            return true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.Calamity().ZoneSulphur || (!spawnInfo.Player.ZoneOverworldHeight && !Main.remixWorld) || (!spawnInfo.Player.ZoneNormalCaverns && spawnInfo.Player.ZoneGlowshroom && Main.remixWorld))
                return 0f;

            return (Main.remixWorld ? SpawnCondition.Cavern.Chance : SpawnCondition.OverworldDaySlime.Chance) * (Main.hardMode ? 0.055f : 0.135f) * (NPC.AnyNPCs(ModContent.NPCType<WulfrumAmplifier>()) ? 5.5f : 1f);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<EnergyCore>(), 1, 1);
        }


    }
}
