using CalRemix.Content.NPCs.PandemicPanic;
using CalRemix.Content.Tiles;
using CalRemix.Core.World;
using System.IO;
using Terraria.DataStructures;
using CalRemix.UI.Anomaly109;
using Terraria;
using Terraria.ID;
namespace CalRemix;
enum RemixMessageType
{
    Anomaly109Toggle,
    Anomaly109Help,
    Anomaly109Unlock,
    Anomaly109UnlockAll,
    ToggleHelpers,
    SyncIonmaster,
    IonQuestLevel,
    OxydayTime,
    TrueStory,
    StartPandemicPanic,
    EndPandemicPanic,
    KillDefender,
    KillInvader,
    ShadeQuestIncrement
}
public static class CalRemixPacketManager
{
    public static void HandlePacket(BinaryReader reader, int whoAmI)
    {
        RemixMessageType msgType = (RemixMessageType)reader.ReadByte();
        switch (msgType)
        {
            case RemixMessageType.Anomaly109Toggle:
                {
                    Anomaly109Manager.ToggleOption(reader.ReadInt32(), Main.netMode == NetmodeID.MultiplayerClient);
                    break;
                }
            case RemixMessageType.Anomaly109Help:
                {
                    Anomaly109Manager.UnlockHelp(Main.netMode == NetmodeID.MultiplayerClient);
                    break;
                }
            case RemixMessageType.Anomaly109Unlock:
                {
                    Anomaly109Manager.UnlockOption(reader.ReadInt32(), Main.netMode == NetmodeID.MultiplayerClient);
                    break;
                }
            case RemixMessageType.Anomaly109UnlockAll:
                {
                    Anomaly109Manager.UnlockAllOptions(Main.netMode == NetmodeID.MultiplayerClient);
                    break;
                }
            case RemixMessageType.ToggleHelpers:
                {
                    Anomaly109Manager.ToggleHelpers(Main.netMode == NetmodeID.MultiplayerClient);
                    break;
                }
            case RemixMessageType.SyncIonmaster:
                {
                    int kennyID = reader.ReadByte();
                    float posX = reader.ReadSingle();
                    float posY = reader.ReadSingle();
                    float desiredX = reader.ReadSingle();
                    float desiredY = reader.ReadSingle();
                    string text = reader.ReadString();
                    float textLife = reader.ReadSingle();
                    int lookedItem = reader.ReadInt32();
                    int itemTimer = reader.ReadInt32();
                    float rotation = reader.ReadSingle();
                    float desRotation = reader.ReadSingle();

                    if (TileEntity.ByID.TryGetValue(kennyID, out TileEntity t))
                    {
                        if (t is IonCubeTE kendrick)
                        {
                            kendrick.positionX = posX;
                            kendrick.positionY = posY;
                            kendrick.desiredX = desiredX;
                            kendrick.desiredY = desiredY;
                            kendrick.rotation = rotation;
                            kendrick.desiredRotation = desRotation;
                            kendrick.lookedAtItem = lookedItem;
                            kendrick.lookingAtItem = itemTimer;
                            kendrick.displayText = text;
                            kendrick.textLifeTime = textLife;
                        }
                    }

                    break;
                }
            case RemixMessageType.IonQuestLevel:
                {
                    int level = reader.ReadByte();
                    CalRemixWorld.ionQuestLevel = level;
                    break;
                }
            case RemixMessageType.OxydayTime:
                {
                    int oxygenTime = reader.ReadByte();
                    CalRemixWorld.oxydayTime = oxygenTime;
                    break;
                }
            case RemixMessageType.TrueStory:
                {
                    string uuid = reader.ReadString();
                    CalRemixWorld.playerSawTrueStory.Add(uuid);
                    break;
                }
            case RemixMessageType.StartPandemicPanic:
                {
                    PandemicPanic.IsActive = true;
                    PandemicPanic.DefendersKilled = 0;
                    PandemicPanic.InvadersKilled = 0;
                    CalRemixWorld.UpdateWorldBool();
                    break;
                }
            case RemixMessageType.EndPandemicPanic:
                {
                    PandemicPanic.IsActive = false;
                    PandemicPanic.DefendersKilled = 0;
                    PandemicPanic.InvadersKilled = 0;
                    PandemicPanic.LockedFinalSide = 0;
                    PandemicPanic.SummonedPathogen = false;
                    CalRemixWorld.UpdateWorldBool();
                    break;
                }
            case RemixMessageType.KillDefender:
                {
                    PandemicPanic.DefendersKilled = reader.ReadInt32();
                    break;
                }
            case RemixMessageType.KillInvader:
                {
                    PandemicPanic.InvadersKilled = reader.ReadInt32();
                    break;
                }
            case RemixMessageType.ShadeQuestIncrement:
                {
                    int count = reader.ReadInt32();
                    CalRemixWorld.shadeQuestLevel = count;
                    CalRemixWorld.UpdateWorldBool();
                    break;
                }
        }
    }
}