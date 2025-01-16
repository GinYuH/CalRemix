using CalamityMod;
using CalamityMod.Events;
using CalamityMod.NPCs;
using CalamityMod.NPCs.Cryogen;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Boss;
using CalamityMod.World;
using CalRemix.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.NPCs
{
    public class Dendritus : ModNPC
    {
        public ref float AITimer => ref NPC.ai[0];
        public ref float IdealPositionX => ref NPC.ai[1];
        public ref float IdealPositionY => ref NPC.ai[2];
        public override void SetStaticDefaults()
        {
            this.HideFromBestiary();
            Main.npcFrameCount[NPC.type] = 1;
        }
        public override void SetDefaults()
        {
            NPC.Calamity().canBreakPlayerDefense = true;
            NPC.width = 54;
            NPC.height = 70;
            NPC.damage = 0;
            NPC.defense = 999999999;
            NPC.takenDamageMultiplier = 0.01f;
            NPC.lifeMax = 8;
            NPC.knockBackResist = 0f;
            NPC.value = 0f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.Zombie5;
            NPC.DeathSound = SoundID.Item27;
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.Calamity().VulnerableToHeat = true;
        }
        public override void OnSpawn(IEntitySource source)
        {
            AITimer = Main.rand.Next(0, 250);
        }
        public override void AI()
        {
            // failsafe 
            if (NPC.FindFirstNPC(ModContent.NPCType<Cryogen>()) == -1)
            {
                NPC.active = false;
            }
            
            NPC.TargetClosest();

            IdealPositionX = Main.npc[NPC.FindFirstNPC(ModContent.NPCType<Cryogen>())].Center.X - (int)(Math.Cos(AITimer * 0.025f) * 220);
            IdealPositionY = Main.npc[NPC.FindFirstNPC(ModContent.NPCType<Cryogen>())].Center.Y - (int)(Math.Sin(AITimer * 0.025f) * 220);

            NPC.velocity += NPC.Center.DirectionTo(new Vector2(IdealPositionX, IdealPositionY));

            if (NPC.velocity.X > 30)
            {
                NPC.velocity.X = 30;
            }
            else if (NPC.velocity.X < -30)
            {
                NPC.velocity.X = -30;
            }

            if (NPC.velocity.Y > 30)
            {
                NPC.velocity.Y = 30;
            }
            else if (NPC.velocity.Y < -30)
            {
                NPC.velocity.Y = -30;
            }

            //Dust.NewDustPerfect(new Vector2(IdealPositionX, IdealPositionY), DustID.CrimsonSpray);
            AITimer++;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            // create a beautiful harmony
            for (int i = 0; i < 3; i++)
            {
                SoundEngine.PlaySound(SoundID.Zombie5 with { MaxInstances = 0, Volume = 2f, Pitch = Main.rand.NextFloat(-3.0f, 3.0f) }, NPC.Center);
            }

            for (int i = 0; i < 15; i++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Ice, hit.HitDirection, -1f, 0, default, 1f);
            }
        }
        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            projectile.active = false;
        }
        public override void OnKill()
        {
            SoundEngine.PlaySound(SoundID.Zombie92 with { MaxInstances = 0, Volume = 3, Pitch = Main.rand.NextFloat(-1.0f, 1.0f) }, NPC.Center);

            for (int i = 0; i < 50; i++)
            {
                Vector2 target = NPC.Center.DirectionTo(Main.player[NPC.target].Center) * Main.rand.NextFloat(1, 10);
                target.X += Main.rand.NextFloat(-1, 1);
                target.Y += Main.rand.NextFloat(-1, 1);
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, target, ModContent.ProjectileType<IceRain>(), 75, 5);
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            ReLogic.Content.Asset<Texture2D> texture = ModContent.Request<Texture2D>(Texture);
            ReLogic.Content.Asset<Texture2D> eye = ModContent.Request<Texture2D>("CalRemix/Content/NPCs/DendritusEye");
            //NPC.DrawBackglow(Color.AliceBlue, 4f, SpriteEffects.None, NPC.frame, screenPos);
            Main.EntitySpriteDraw(texture.Value, NPC.Center - Main.screenPosition, null, Color.AliceBlue, NPC.rotation + AITimer * 0.25f, texture.Size() / 2, NPC.scale, 0, 0);
            Main.EntitySpriteDraw(eye.Value, NPC.Center - Main.screenPosition + NPC.Center.DirectionTo(new Vector2(IdealPositionX, IdealPositionY)) * 5, null, Color.AliceBlue, NPC.rotation, eye.Size() / 2, NPC.scale, 0, 0);
            return false;
        }
    }
}
