using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Sounds;
using CalRemix.Content.Items.Placeables.Banners;
using Terraria.ModLoader.Utilities;
using CalRemix.Content.Projectiles.Hostile;
using Microsoft.Xna.Framework;
using CalRemix.Content.Projectiles;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;

namespace CalRemix.Content.NPCs
{
    public class WulfrumCactus : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = false;
            NPC.Calamity().VulnerableToElectricity = true;
            NPC.width = 30;
            NPC.height = 60;
            NPC.damage = 10;
            NPC.defense = 12;
            NPC.lifeMax = 78;
            NPC.knockBackResist = 0f;
            NPC.value = 120;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = CommonCalamitySounds.WulfrumNPCDeathSound;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<WulfrumCactusBanner>();
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.WulfrumCactus").Value)
            });
        }

        public int shotCooldown = 90;
        public bool isShooting = false;
        public override void FindFrame(int frameheight)
        {
            float frameSpeed = 6f;
            if (isShooting == true) NPC.frameCounter++;

            if (NPC.frameCounter >= frameSpeed)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameheight;
                if (NPC.frame.Y == frameheight * 2)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        Dust.NewDust(NPC.Center + new Vector2(-7, -60), NPC.width/2, NPC.height/2, DustID.t_Martian, Main.rand.Next(-3, 4), -8, 0, default, 0.8f);
                    }
                    SoundEngine.PlaySound(SoundID.Item61, NPC.Center);
                    Projectile.NewProjectileDirect(NPC.GetSource_FromThis(), NPC.Center, new Vector2(0, -7), ModContent.ProjectileType<WulfrumNanite>(), 3, 1, -1, 0, NPC.target);
                }
                if (NPC.frame.Y >= frameheight * Main.npcFrameCount[Type])
                {
                    NPC.frame.Y = 0; // reset to 0 once frames are done
                    isShooting = false; // Stop the animation from repeating..
                    NPC.ai[0] = 0;
                } 
            }
        }
        public override void AI()
        {
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];
            NPC.ai[0]++;

            //Shooting
            if (Main.player[NPC.target].Distance(NPC.Center) < 850 && NPC.ai[0] % shotCooldown == 0)
            {
                isShooting = true;
                
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
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.Center + new Vector2(0,-5), NPC.width, NPC.height, DustID.t_Martian, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.Center + new Vector2(0, -5), NPC.width, NPC.height, DustID.t_Martian, hit.HitDirection, -1f, 0, default, 1f);
                }
                if (!Main.dedServ)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(-1, Main.rand.Next(-3, -2)), Mod.Find<ModGore>("WulfrumCactus1").Type);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(1, Main.rand.Next(-3, -2)), Mod.Find<ModGore>("WulfrumCactus2").Type);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2(0, -5), Mod.Find<ModGore>("WulfrumCactus3").Type);
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.PlayerSafe && spawnInfo.Player.ZoneDesert)
            {
                return 0.35f;
            }
            return 0f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<WulfrumMetalScrap>(), 1, 1, 2);
        }


    }
}
