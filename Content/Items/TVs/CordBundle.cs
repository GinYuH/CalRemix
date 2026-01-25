using CalamityMod;
using CalRemix.Content.Tiles.TVs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRemix.Content.Items.TVs;

public class CordBundle : ModItem
{
    Point tvLocation = Point.Zero;
    Point playerLocation = Point.Zero;

    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 1;
    }

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 32;
        Item.useTurn = true;
        Item.autoReuse = true;
        Item.useAnimation = 15;
        Item.useTime = 15;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.value = Item.buyPrice(1, 0, 0, 0);
        Item.rare = ItemRarityID.Green;
    }

    public override void HoldItem(Player player)
    {
        if (player.whoAmI != Main.myPlayer)
            return;

        int wrappedTime = player.miscCounter % 60;

        if (tvLocation == Point.Zero && playerLocation == Point.Zero)
        {
            Point mouseTile = Main.MouseWorld.ToTileCoordinates();
            if (TileEntity.TryGet<MediaPlayerEntity>(mouseTile.X, mouseTile.Y, out var playerEntity))
            {
                foreach (Point16 p in playerEntity.ConnectedTVs)
                {
                    if (wrappedTime <= 30 && player.miscCounter % 5 == 0)
                    {
                        Vector2 spawnPos = Vector2.Lerp(playerEntity.Position.ToWorldCoordinates(), p.ToWorldCoordinates(), wrappedTime / 30f);
                        Dust.NewDustPerfect(spawnPos, DustID.AncientLight, Vector2.Zero).noGravity = true;
                    }
                }
            }
            else if (TileEntity.TryGet<TVTileEntity>(mouseTile.X, mouseTile.Y, out var tvEntity) && tvEntity.MediaPlayerPosition != Point16.Zero)
            {
                if (wrappedTime <= 30 && player.miscCounter % 5 == 0)
                {
                    Vector2 spawnPos = Vector2.Lerp(tvEntity.MediaPlayerPosition.ToWorldCoordinates(), tvEntity.Position.ToWorldCoordinates(), wrappedTime / 30f);
                    Dust.NewDustPerfect(spawnPos, DustID.AncientLight, Vector2.Zero).noGravity = true;
                }
            }
        }
        else if (wrappedTime <= 30 && player.miscCounter % 5 == 0)
        {
            bool tvSelected = tvLocation != Point.Zero;
            Vector2 basePosition = (tvSelected ? tvLocation : playerLocation).ToWorldCoordinates();
            Vector2 spawnPos = Vector2.Lerp(basePosition, Main.MouseWorld, tvSelected ? 1 - wrappedTime / 30f : wrappedTime / 30f);
            Dust.NewDustPerfect(spawnPos, DustID.AncientLight, Vector2.Zero).noGravity = true;
        }
    }

    public override bool? UseItem(Player player)
    {
        if (player.whoAmI != Main.myPlayer)
            return false;

        Point mouseTile = Main.MouseWorld.ToTileCoordinates();

        if (TileEntity.TryGet<MediaPlayerEntity>(mouseTile.X, mouseTile.Y, out var playerEntity))
            playerLocation = playerEntity.Position.ToPoint();
        else if (TileEntity.TryGet<TVTileEntity>(mouseTile.X, mouseTile.Y, out var tvEntity))
            tvLocation = tvEntity.Position.ToPoint();
        else
        {
            tvLocation = Point.Zero;
            playerLocation = Point.Zero;
            return false;
        }

        if (tvLocation != Point.Zero && !TileEntity.TryGet<TVTileEntity>(tvLocation.X, tvLocation.Y, out _))
            tvLocation = Point.Zero;
        if (playerLocation != Point.Zero && !TileEntity.TryGet<MediaPlayerEntity>(playerLocation.X, playerLocation.Y, out _))
            playerLocation = Point.Zero;

        if (tvLocation != Point.Zero && playerLocation != Point.Zero)
        {
            bool gotP = TileEntity.TryGet<MediaPlayerEntity>(playerLocation.X, playerLocation.Y, out var pEntity);
            bool gotT = TileEntity.TryGet<TVTileEntity>(tvLocation.X, tvLocation.Y, out var tEntity);
            if (!gotP || !gotT)
                return false;

            pEntity.ConnectedTVs.Add(tEntity.Position);
            tEntity.MediaPlayerPosition = pEntity.Position;

            tvLocation = Point.Zero;
            playerLocation = Point.Zero;
        }

        return true;
    }
}
