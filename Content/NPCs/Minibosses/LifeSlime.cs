using CalamityMod;
using CalRemix.Core.Biomes;
using CalRemix.Content.Items.Placeables;
using CalRemix.Content.Projectiles.Hostile;
using CalRemix.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using System.Linq;
using CalamityMod.Items.Materials;
using System.IO;
using CalRemix.Core.World;

namespace CalRemix.Content.NPCs.Minibosses
{
    public class LifeSlime : ModNPC
	{
        public bool angry = false;
        private float timer = 0;
        private int state = 0;
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Life Slime");
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.BlueSlime];
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;

            if (Main.dedServ)
                return;
            HelperMessage.New("LifeBiome",
                "The Life Heart is a joint meeting area of the elements of fire, ice, and nature. With their powers combined, they make a powerful new bar which can be used to upgrade your items! Be careful of the dreaded Life Slimes that visciously guard the place though.",
                "FannyNuhuh",
                (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == Type)).AddItemDisplay(ModContent.ItemType<LifeAlloy>());
        }
        public override bool SpecialOnKill()
        {
            RemixDowned.downedLifeSlime = true;
            return false;
        }
        public override void SetDefaults()
		{ 
			NPC.aiStyle = NPCAIStyleID.Slime;
			NPC.width = 48;
			NPC.height = 30;
            NPC.lavaImmune = true;
            NPC.noTileCollide = false;
			NPC.noGravity = false;
			NPC.damage = 260;
			NPC.defense = 12;
			NPC.lifeMax = 2400;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.rarity = 2;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = Item.buyPrice(gold: 1, silver: 5);
			AnimationType = NPCID.BlueSlime;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<LifeBiome>().Type };
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new System.Collections.Generic.List<IBestiaryInfoElement>
			{
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (DownedBossSystem.downedRavager && spawnInfo.Player.InModBiome(ModContent.GetInstance<LifeBiome>()) && !NPC.AnyNPCs(Type))
				return SpawnCondition.Cavern.Chance * 0.1f;
            return 0;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(angry);
            writer.Write(timer);
            writer.Write(state);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            angry = reader.ReadBoolean();
            timer = reader.ReadSingle();
            state = reader.ReadInt32();
        }
        public override void AI()
		{
            var source = NPC.GetSource_FromAI();
            int projdam = 50;
            Player target = Main.player[NPC.target];
            if ((NPC.life < NPC.lifeMax || target.InModBiome<LifeBiome>()) && !angry)
                angry = true;
            if (angry)
            {
                timer++;
                switch (state)
                {
                    case 0:
                        if (timer % 90 == 0)
                        {
                            SoundEngine.PlaySound(SoundID.Item33, NPC.Center);
                            Projectile.NewProjectile(source, new Vector2(NPC.Center.X, NPC.Center.Y - NPC.height), Vector2.Normalize(target.Center - NPC.Center) * 12f, ModContent.ProjectileType<LifeIcicle>(), projdam, 0);
                            Projectile.NewProjectile(source, new Vector2(NPC.Center.X, NPC.Center.Y - NPC.height), Vector2.Normalize(target.Center - NPC.Center).RotatedBy(MathHelper.ToRadians(25f)) * 12f, ModContent.ProjectileType<LifeIcicle>(), projdam, 0);
                            Projectile.NewProjectile(source, new Vector2(NPC.Center.X, NPC.Center.Y - NPC.height), Vector2.Normalize(target.Center - NPC.Center).RotatedBy(MathHelper.ToRadians(25f)) * 12f, ModContent.ProjectileType<LifeIcicle>(), projdam, 0);
                            for (int i = 0; i < 3; i++)
                            {
                                Projectile.NewProjectile(source, new Vector2(NPC.Center.X, NPC.Center.Y - NPC.height), Vector2.Normalize(target.Center - NPC.Center).RotatedByRandom(MathHelper.ToRadians(45f)) * 12f, ModContent.ProjectileType<LifeIcicle>(), projdam, 0);
                            }
                        }
                        break;
                    case 1:
                        if (timer % 60 == 0)
                        {
                            SoundEngine.PlaySound(SoundID.Item33, NPC.Center);

                            for (int i = 0; i < 4; i++)
                            {
                                Dust dust = Dust.NewDustDirect(new Vector2(target.Center.X, target.Center.Y + 192), 0, 0, DustID.TerraBlade);
                                dust.noGravity = true;
                                dust.velocity = Vector2.One.RotatedByRandom(MathHelper.ToRadians(360)) * 4f;
                            }
                            Vector2 randThorn = new Vector2(target.Center.X + (Main.rand.NextBool(2) ? Main.rand.Next(-128, -33) : Main.rand.Next(32, 129)), target.Center.Y + 192);
                            for (int i = 0; i < 4; i++)
                            {
                                Dust dust = Dust.NewDustDirect(randThorn, 0, 0, DustID.TerraBlade);
                                dust.noGravity = true;
                                dust.velocity = Vector2.One.RotatedByRandom(MathHelper.ToRadians(360)) * 4f;
                            }
                            Projectile.NewProjectile(source, new Vector2(target.Center.X, target.Center.Y + 192), new Vector2(0, -32f), ModContent.ProjectileType<LifeThorn>(), projdam, 0);
                            Projectile.NewProjectile(source, randThorn, new Vector2(0, -32f), ModContent.ProjectileType<LifeThorn>(), projdam, 0);
                        }
                        break;
                    case 2:
                        if (timer % 180 == 0)
                        {
                            SoundEngine.PlaySound(SoundID.Item33, NPC.Center);
                            Projectile.NewProjectile(source, new Vector2(NPC.Center.X, NPC.Center.Y - NPC.height), Vector2.Normalize(target.Center - NPC.Center) * 6f, ModContent.ProjectileType<LifeMeteor>(), projdam, 0);
                        }
                        break;
                }
                if (timer > 180)
                {
                    timer = 0;
                    state = Main.rand.Next(3);
                }
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<LifeOre>(), 1, 20, 46);
        }
    }
}