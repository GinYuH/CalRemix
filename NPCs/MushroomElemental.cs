using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using CalamityMod;
using CalRemix.Items.Placeables;
using CalRemix.Biomes;
using System;
using Microsoft.Xna.Framework;
using CalamityMod.Projectiles.Boss;
using Terraria.Audio;

namespace CalRemix.NPCs
{
    public class MushroomElemental : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mushroom Elemental");
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.damage = 20;
            NPC.width = 20;
            NPC.height = 46;
            NPC.defense = 10;
            NPC.lifeMax = 25;
            NPC.knockBackResist = 0f;
            NPC.value = 300;
            NPC.lavaImmune = false;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.Calamity().VulnerableToCold = true;
            NPC.Calamity().VulnerableToHeat = true;
            NPC.Calamity().VulnerableToSickness = false;
        }

        public override void AI()
        {
            NPC.noGravity = true;
            NPC.TargetClosest();
            float speed = 6f;
            float acceleration = 0.25f;
            Vector2 playerCenter = new Vector2(NPC.Center.X, NPC.Center.Y);
            float maxX = Main.player[NPC.target].Center.X - playerCenter.X;
            float maxY = Main.player[NPC.target].Center.Y - playerCenter.Y - 200f;
            float direction = (float)Math.Sqrt(maxX * maxX + maxY * maxY);
            if (direction < 20f)
            {
                maxX = NPC.velocity.X;
                maxY = NPC.velocity.Y;
            }
            else
            {
                direction = speed / direction;
                maxX *= direction;
                maxY *= direction;
            }
            if (NPC.velocity.X < maxX)
            {
                NPC.velocity.X += acceleration;
                if (NPC.velocity.X < 0f && maxX > 0f)
                {
                    NPC.velocity.X += acceleration * 2f;
                }
            }
            else if (NPC.velocity.X > maxX)
            {
                NPC.velocity.X -= acceleration;
                if (NPC.velocity.X > 0f && maxX < 0f)
                {
                    NPC.velocity.X -= acceleration * 2f;
                }
            }
            if (NPC.velocity.Y < maxY)
            {
                NPC.velocity.Y += acceleration;
                if (NPC.velocity.Y < 0f && maxY > 0f)
                {
                    NPC.velocity.Y += acceleration * 2f;
                }
            }
            else if (NPC.velocity.Y > maxY)
            {
                NPC.velocity.Y -= acceleration;
                if (NPC.velocity.Y > 0f && maxY < 0f)
                {
                    NPC.velocity.Y -= acceleration * 2f;
                }
            }
            if (NPC.position.X + (float)NPC.width > Main.player[NPC.target].position.X && NPC.position.X < Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width && NPC.position.Y + (float)NPC.height < Main.player[NPC.target].position.Y && Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height) && Main.netMode != 1)
            {
                NPC.ai[0] += 1f;
                if (NPC.ai[0] > 30f)
                {
                    NPC.ai[0] = 0f;
                    int spawnPos = (int)(NPC.position.X + 10f + (float)Main.rand.Next(NPC.width - 20));
                    int projVelocity = (int)(NPC.position.Y + (float)NPC.height + 4f);
                    SoundEngine.PlaySound(BetterSoundID.ItemMissileFireSqueak, NPC.Center);
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), spawnPos, projVelocity, 0f, 5f, ModContent.ProjectileType<MushBomb>(), (int)(NPC.damage * 0.25f), 0f, Main.myPlayer);
                }
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundMushroom,
        new FlavorTextBestiaryInfoElement("The elementals of this world often consist of large humanoid figures. This fungus is not that.")
            });
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.PlayerSafe || spawnInfo.Player.Calamity().ZoneAbyss ||
                spawnInfo.Player.Calamity().ZoneSunkenSea || !spawnInfo.Player.ZoneGlowshroom || NPC.AnyNPCs(Type))
            {
                return 0f;
            }
            return 0.2f;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GlowingMushroom, hit.HitDirection, -1f, 0, default, 1f);
            }
            if (NPC.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GlowingMushroom, hit.HitDirection, -1f, 0, default, 1f);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemID.GlowingMushroom, new Fraction(9, 10), 6, 12);
            npcLoot.AddIf(()=>!WorldGen.crimson, ItemID.VileMushroom, new Fraction(8, 10), 2, 5);
            npcLoot.AddIf(() => WorldGen.crimson, ItemID.ViciousMushroom, new Fraction(8, 10), 2, 5);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Confused, 180);
        }
    }
}
