using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using CalRemix.Core.Biomes;
using CalRemix.Content.Items.Materials;

namespace CalRemix.Content.NPCs.Subworlds.Sealed
{
    public class StonePriest : ModNPC
    {
        public Player Target => Main.player[NPC.target];
        public ref float Timer => ref NPC.ai[0];
        public ref float State => ref NPC.ai[1];
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 38;
            NPC.height = 80;
            NPC.lifeMax = 6000;
            NPC.damage = 50;
            NPC.defense = 8;
            NPC.knockBackResist = 0f;
            NPC.noGravity = false;
            NPC.HitSound = SoundID.DD2_OgreHurt with { Pitch = 0.4f };
            NPC.DeathSound = SoundID.DD2_OgreDeath with { Pitch = 0.4f };
            NPC.noTileCollide = false;
            NPC.value = Item.buyPrice(silver: 50);
            SpawnModBiomes = new int[1] { ModContent.GetInstance<VolcanicFieldBiome>().Type };
        }
        public override void AI()
        {
            NPC.TargetClosest();
            NPC.spriteDirection = NPC.direction;
            if (Main.player[NPC.target].Distance(NPC.Center) < 1000 || NPC.justHit)
            {
                State = 1;
            }

            if (State == 1)
            {
                Timer++;
                if (Timer == 20)
                {
                }
                if (Timer == 30)
                {
                    SoundEngine.PlaySound(BetterSoundID.ItemGolfClubSwing, NPC.Center);
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, NPC.DirectionTo(Main.player[NPC.target].Center) * 10, ProjectileID.RockGolemRock, CalRemixHelper.ProjectileDamage(80, 160), 1);
                    Timer = 0;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[Type].Value;
            SpriteEffects fx = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(texture, NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY), null, drawColor, NPC.rotation, texture.Size() / 2f, NPC.scale, fx, 0f);
            Texture2D arm = ModContent.Request<Texture2D>(Texture + "Arm").Value;




            float armRot = MathHelper.Lerp(MathHelper.ToRadians(190), MathHelper.ToRadians(0), CalamityUtils.SineInEasing(Utils.GetLerpValue(0, 20, Timer, true), 1));
            if (State == 0)
                armRot = 0;

            if (Timer > 20)
            {
                armRot = MathHelper.Lerp(MathHelper.ToRadians(0), MathHelper.ToRadians(190), CalamityUtils.SineOutEasing(Utils.GetLerpValue(20, 30, Timer, true), 1));
            }

            if (NPC.spriteDirection == 1)
                armRot += MathHelper.Pi;

            SpriteEffects fxArm = NPC.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 origin = NPC.spriteDirection == -1 ? new(arm.Width - 3, 3) : new Vector2(3, 3);
            spriteBatch.Draw(arm, NPC.Center - Main.screenPosition + new Vector2(0f, NPC.gfxOffY) - new Vector2(10 * -NPC.spriteDirection, 10), null, drawColor, armRot, origin, NPC.scale, fxArm, 0f);
            return false;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
        new FlavorTextBestiaryInfoElement(CalRemixHelper.LocalText($"Bestiary.{Name}").Value)
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ModContent.ItemType<Pebble>());
        }
    }
}
