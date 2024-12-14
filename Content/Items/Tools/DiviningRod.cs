#region Clops
/*
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Items;
using CalamityMod;
using CalamityMod.Rarities;

namespace CalRemix.Content.Items.Tools
{
    public class DiviningRod : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Divining Rod");
            Tooltip.SetDefault("Homes in on the presence of powerful elemental magic\nIn the event there are multiple sources, it will home in on the one most approachable in your current state");
        }
        public override void SetDefaults()
        {
            Item.useAnimation = 17;
            Item.holdStyle = 16;

            Item.width = 40;
            Item.height = 50;
            Item.useTurn = true;
            Item.value = Item.buyPrice(gold: 2);
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = null;
            Item.noMelee = true;
            Item.autoReuse = true;
        }
        //treasure pinger stuff, borrowed with permission to save time
        public void SetItemInHand(Player player, Rectangle heldItemFrame)
        {
            //Make the player face where they're aiming.
            if (player.Calamity().mouseWorld.X > player.Center.X)
            {
                player.ChangeDir(1);
            }
            else
            {
                player.ChangeDir(-1);
            }

            float itemRotation = player.compositeBackArm.rotation + MathHelper.PiOver2 * player.gravDir;
            Vector2 itemPosition = player.GetBackHandPositionImproved(player.compositeBackArm).Floor();
            Vector2 itemSize = new Vector2(40, 50);
            Vector2 itemOrigin = new Vector2(-10, 45);

            CalamityUtils.CleanHoldStyle(player, itemRotation, itemPosition, itemSize, itemOrigin);
        }

        public void SetPlayerArms(Player player)
        {
            //Calculate the dirction in which the players arms should be pointing at.
            Vector2 playerToCursor = (player.Calamity().mouseWorld - player.Center).SafeNormalize(Vector2.UnitX);
            float armPointingDirection = (playerToCursor.ToRotation() + MathHelper.PiOver2).Modulo(MathHelper.TwoPi);

            //"crop" the rotation so the player only tilts the fishing rod slightly up and slightly down.
            if (armPointingDirection < MathHelper.Pi)
            {
                armPointingDirection = armPointingDirection / MathHelper.Pi * MathHelper.PiOver4 * 0.5f - MathHelper.PiOver4 * 0.3f;
            }

            //It gets a bit harder if its pointing left; ouch
            else
            {
                armPointingDirection -= MathHelper.Pi;

                armPointingDirection = armPointingDirection / MathHelper.Pi * MathHelper.PiOver4 * 0.5f - MathHelper.PiOver4 * 0.3f + MathHelper.Pi;
            }

            player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, armPointingDirection * player.gravDir - MathHelper.PiOver2);
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, armPointingDirection * player.gravDir - MathHelper.PiOver2);
        }

        public override void HoldStyle(Player player, Rectangle heldItemFrame) => SetItemInHand(player, heldItemFrame);
        public override void UseStyle(Player player, Rectangle heldItemFrame) => SetItemInHand(player, heldItemFrame);
        public override void HoldItemFrame(Player player) => SetPlayerArms(player);
        public override void UseItemFrame(Player player) => SetPlayerArms(player);

    }
}
*/
#endregion