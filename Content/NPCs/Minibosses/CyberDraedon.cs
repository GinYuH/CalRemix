using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Terraria.DataStructures;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Materials;
using CalamityMod.Items.DraedonMisc;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using System.IO;
using CalRemix.Content.Projectiles.Hostile;
using CalamityMod.BiomeManagers;
using CalRemix.UI;
using System.Linq;
using CalRemix.Core.World;

namespace CalRemix.Content.NPCs.Minibosses
{
    public class CyberDraedon : ModNPC
    {
        private Player Target => Main.player[NPC.target];
        private bool phaseTwo = false;
        private bool killed = false;
        public ref float Timer => ref NPC.ai[0];
        public ref float State => ref NPC.ai[1];
        public override bool CheckActive() => false;
        public override void SetStaticDefaults()
        {
            NPCID.Sets.ImmuneToRegularBuffs[Type] = true;

            if (Main.dedServ)
                return;
            HelperMessage.New("CyberDraedonFight",
                "It appears you have alerted the high urgency security systems within that projector and summoned the nefarious Cyber Draedon. He's a real fickle foe who is able to deal percentage-based damage, meaning he'll always be a threat no matter how good your defenses and health are!",
                "FannyNuhuh",
                (ScreenHelperSceneMetrics scene) => scene.onscreenNPCs.Any(n => n.type == Type));
        }
        public override bool SpecialOnKill()
        {
            RemixDowned.downedCyberDraedon = true;
            return false;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 70;
            NPC.height = 112;
            NPC.lifeMax = 3000;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.alpha = 125;
            NPC.DR_NERD(0.1f);
            if (DownedBossSystem.downedYharon)
                NPC.lifeMax = 200000;
            else if (DownedBossSystem.downedProvidence)
                NPC.lifeMax = 100000;
            else if (NPC.downedGolemBoss)
                NPC.lifeMax = 40000;
            else if (Main.hardMode)
                NPC.lifeMax = 10000;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(gold: 1, silver: 20);
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            SpawnModBiomes = new int[1] { ModContent.GetInstance<ArsenalLabBiome>().Type };
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(phaseTwo);
            writer.Write(killed);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            phaseTwo = reader.ReadBoolean();
            killed = reader.ReadBoolean();
        }
        private static void Talk(string value)
        {
            CalRemixHelper.GetNPCDialog($"CyberDraedon.{value}", Color.Cyan);
        }
        public override void OnSpawn(IEntitySource source)
        {
            Talk("1");
        }
        public override void AI()
        {
            if (Target.dead && !killed)
            {
                Talk("2");
                killed = true;
            }
            NPC.velocity.X *= 0.922f;
            NPC.TargetClosest();
            if (!Target.dead && killed)
            {
                killed = false;
            }
            if (NPC.life < NPC.lifeMax / 2f && !phaseTwo)
            {
                if (Main.rand.NextBool(2))
                    Talk("3");
                else
                    Talk("4");
                phaseTwo = true;
            }
            foreach (Player player in Main.player)
            {
                if (player.getRect().Intersects(NPC.getRect()) && player.immuneTime <= 0)
                {
                    if (Main.expertMode)
                        player.Hurt(PlayerDeathReason.ByNPC(NPC.whoAmI), (int)(player.statLifeMax2 / 10f), (player.Center.X > NPC.Center.X) ? 1 : -1, dodgeable: false, armorPenetration: 10000);
                    else
                        player.Hurt(PlayerDeathReason.ByNPC(NPC.whoAmI), (int)(player.statLifeMax2 / 20f), (player.Center.X > NPC.Center.X) ? 1 : -1, dodgeable: false, armorPenetration: 10000);
                }
            }
            State++;
            if (Timer == 0 && State % 60 == 55)
            {
                NPC.velocity.Y -= 4;
            }
            if (Timer == 0 && State % 60 == 0)
            {
                NPC.velocity.X = (Target.Center.X > NPC.Center.X) ? 20 : -20;
            } 
            else if (Timer == 0 && State >= 235)
            {
                Timer = 1;
                State = 30;
            }
            if (Timer == 1 && State % 45 == 0 && Main.netMode != NetmodeID.Server)
            {
                Vector2 targetPos = NPC.DirectionTo(Target.Center);
                if (phaseTwo)
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, targetPos * 10f, ModContent.ProjectileType<LaserSlash>(), 0, 0);
                else
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + targetPos.SafeNormalize(Vector2.One) * 80f, targetPos * 0.01f, ModContent.ProjectileType<LaserSlash>(), 0, 0, ai0: 1);
            }
            if (Timer == 1 && State < 185)
                NPC.velocity.X = (Target.Center.X > NPC.Center.X) ? 0.01f : -0.01f;
            else if (Timer == 1 && State >= 185)
            {
                NPC.velocity = new Vector2((Target.Center.X > NPC.Center.X) ? 2 : -2, -2);
                Timer = 0;
                State = 30;
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<DubiousPlating>(), 1, 20, 50);
            npcLoot.Add(ModContent.ItemType<MysteriousCircuitry>(), 1, 20, 50);
            npcLoot.Add(ModContent.ItemType<SuspiciousScrap>(), 5);
            npcLoot.Add(ModContent.ItemType<Murasama>(), 100);
            npcLoot.AddIf(() => Target.Calamity().ZoneSunkenSea, ModContent.ItemType<EncryptedSchematicSunkenSea>());
            npcLoot.AddIf(() => Target.ZoneNormalSpace && Main.hardMode, ModContent.ItemType<EncryptedSchematicPlanetoid>());
            npcLoot.AddIf(() => Target.ZoneJungle && NPC.downedGolemBoss, ModContent.ItemType<EncryptedSchematicJungle>());
            npcLoot.AddIf(() => Target.ZoneUnderworldHeight && DownedBossSystem.downedProvidence, ModContent.ItemType<EncryptedSchematicHell>());
            npcLoot.AddIf(() => Target.ZoneSnow && DownedBossSystem.downedDoG, ModContent.ItemType<EncryptedSchematicIce>());
            npcLoot.AddIf(() => Target.ZoneNormalSpace && Main.hardMode, ModContent.ItemType<PlasmaDriveCore>(), 10);
        }
        public override void DrawEffects(ref Color drawColor)
        {
            if (Main.rand.NextBool(6))
            {
                int sped = 0;
                if (NPC.velocity.X == 0)
                    return;
                else if (NPC.velocity.X > 0)
                    sped = -3;
                else if (NPC.velocity.X < 0)
                    sped = 3;
                Dust dust = Dust.NewDustDirect(new Vector2(NPC.position.X + 10f, NPC.position.Y + NPC.height - 2f), 1, 0, DustID.Electric, sped);
                Dust dust2 = Dust.NewDustDirect(new Vector2(NPC.position.X + NPC.width - 10f, NPC.position.Y + NPC.height - 2f), 1, 0, DustID.Electric, sped);
                dust.noGravity = true;
                dust2.noGravity = true;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.alpha <= 125)
                NPC.localAI[0] = 0;
            else if (NPC.alpha >= 200)
                NPC.localAI[0] = 1;
            if (NPC.localAI[0] <= 0)
                NPC.alpha -= 5;
            else
                NPC.alpha += 5;
            SpriteEffects effect = SpriteEffects.None;
            if (NPC.velocity.X > 0)
                effect = SpriteEffects.FlipHorizontally;
            else if (NPC.velocity.X < 0)
                effect = SpriteEffects.None;
            else if (NPC.velocity.X == 0 && NPC.HasPlayerTarget)
                effect = (Target.Center.X > NPC.Center.X) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Texture2D texture = TextureAssets.Npc[Type].Value;
            Rectangle sourceRectangle = new(0, 0, texture.Width, texture.Height);
            Vector2 origin = sourceRectangle.Size() / 2f;
            Vector2 draw = NPC.Center - screenPos + new Vector2(0f, NPC.gfxOffY);
            spriteBatch.Draw(texture, draw, sourceRectangle, new Color(0, 255, 255, NPC.alpha), NPC.rotation, origin, NPC.scale, effect, 0f);
            return false;
        }
    }
}
