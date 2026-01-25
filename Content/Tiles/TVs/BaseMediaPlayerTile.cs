using CalamityMod;
using CalRemix.Core.VideoPlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalRemix.Content.Tiles.TVs;

/// <summary>
/// Base class for all tiles which can be hooked up to TVs to play unique media.
/// </summary>
public abstract class BaseMediaPlayerTile : ModTile
{
    public override bool RightClick(int i, int j)
    {
        if (TileEntity.TryGet<MediaPlayerEntity>(i, j, out var playerEntity))
        {
            if (playerEntity.StoredItem == -1)
                return false;

            playerEntity.CurrentContentPath = "";
            Item.NewItem(Item.GetSource_NaturalSpawn(), playerEntity.Position.ToWorldCoordinates(), playerEntity.StoredItem);
            playerEntity.StoredItem = -1;

            return true;
        }
        return false;
    }
}
