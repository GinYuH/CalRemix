using CalRemix.Content.Tiles.TVs;
using CalRemix.Core.VideoPlayer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace CalRemix.UI.VideoPlayer;

public class TVRemoteUI : UIState
{
    private UIPanel _mainPanel;
    private TVTileEntity _currentTV;
    private UIText _channelText;
    private UIText _volumeText;

    public override void OnInitialize()
    {
        // Main panel
        _mainPanel = new UIPanel();
        _mainPanel.Width.Set(300f, 0f);
        _mainPanel.Height.Set(200f, 0f);
        _mainPanel.HAlign = 0.5f;
        _mainPanel.VAlign = 0.3f;
        _mainPanel.BackgroundColor = new Color(73, 94, 171) * 0.9f;
        Append(_mainPanel);

        // Title
        UIText title = new UIText("TV Remote", 1.2f);
        title.HAlign = 0.5f;
        title.Top.Set(10f, 0f);
        _mainPanel.Append(title);

        // Channel controls
        UIText channelLabel = new UIText("Channel:", 0.9f);
        channelLabel.Left.Set(20f, 0f);
        channelLabel.Top.Set(50f, 0f);
        _mainPanel.Append(channelLabel);

        UITextPanel<string> channelDown = new UITextPanel<string>("<");
        channelDown.Width.Set(30f, 0f);
        channelDown.Height.Set(30f, 0f);
        channelDown.Left.Set(120f, 0f);
        channelDown.Top.Set(45f, 0f);
        channelDown.OnLeftClick += (evt, element) => ChangeChannel(-1);
        _mainPanel.Append(channelDown);

        _channelText = new UIText("0", 1f);
        _channelText.Left.Set(165f, 0f);
        _channelText.Top.Set(50f, 0f);
        _mainPanel.Append(_channelText);

        UITextPanel<string> channelUp = new UITextPanel<string>(">");
        channelUp.Width.Set(30f, 0f);
        channelUp.Height.Set(30f, 0f);
        channelUp.Left.Set(200f, 0f);
        channelUp.Top.Set(45f, 0f);
        channelUp.OnLeftClick += (evt, element) => ChangeChannel(1);
        _mainPanel.Append(channelUp);

        // Volume controls
        UIText volumeLabel = new UIText("Volume:", 0.9f);
        volumeLabel.Left.Set(20f, 0f);
        volumeLabel.Top.Set(90f, 0f);
        _mainPanel.Append(volumeLabel);

        UITextPanel<string> volumeDown = new UITextPanel<string>("-");
        volumeDown.Width.Set(30f, 0f);
        volumeDown.Height.Set(30f, 0f);
        volumeDown.Left.Set(120f, 0f);
        volumeDown.Top.Set(85f, 0f);
        volumeDown.OnLeftClick += (evt, element) => ChangeVolume(-10);
        _mainPanel.Append(volumeDown);

        _volumeText = new UIText("100", 1f);
        _volumeText.Left.Set(160f, 0f);
        _volumeText.Top.Set(90f, 0f);
        _mainPanel.Append(_volumeText);

        UITextPanel<string> volumeUp = new UITextPanel<string>("+");
        volumeUp.Width.Set(30f, 0f);
        volumeUp.Height.Set(30f, 0f);
        volumeUp.Left.Set(200f, 0f);
        volumeUp.Top.Set(85f, 0f);
        volumeUp.OnLeftClick += (evt, element) => ChangeVolume(10);
        _mainPanel.Append(volumeUp);

        // Power button
        UITextPanel<string> powerButton = new UITextPanel<string>("Power OFF");
        powerButton.Width.Set(120f, 0f);
        powerButton.Height.Set(35f, 0f);
        powerButton.HAlign = 0.5f;
        powerButton.Top.Set(135f, 0f);
        powerButton.OnLeftClick += (evt, element) => TogglePower();
        _mainPanel.Append(powerButton);

        // Close button
        UITextPanel<string> closeButton = new UITextPanel<string>("Close");
        closeButton.Width.Set(60f, 0f);
        closeButton.Height.Set(25f, 0f);
        closeButton.HAlign = 1f;
        closeButton.Top.Set(5f, 0f);
        closeButton.OnLeftClick += (evt, element) => ModContent.GetInstance<TVRemoteUISystem>().CloseUI();
        _mainPanel.Append(closeButton);
    }

    public void SetTV(TVTileEntity tvEntity)
    {
        _currentTV = tvEntity;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (_currentTV == null)
            return;

        _channelText.SetText(_currentTV.CurrentChannel.ToString());
        _volumeText.SetText(_currentTV.Volume.ToString());
    }

    private void ChangeChannel(int delta)
    {
        if (_currentTV == null)
            return;

        int newChannel = _currentTV.CurrentChannel + delta;

        // Wrap around
        if (newChannel < VideoChannelManager.MIN_CHANNEL)
            newChannel = VideoChannelManager.MAX_CHANNEL;
        if (newChannel > VideoChannelManager.MAX_CHANNEL)
            newChannel = VideoChannelManager.MIN_CHANNEL;

        _currentTV.CurrentChannel = newChannel;
        UpdateDisplay();

        Main.NewText($"Switched to channel {newChannel}", Color.Cyan);
    }

    private void ChangeVolume(int delta)
    {
        if (_currentTV == null)
            return;

        int newVolume = _currentTV.Volume + delta;
        newVolume = System.Math.Clamp(newVolume, 0, 100);

        _currentTV.Volume = newVolume;
        UpdateDisplay();
    }

    private void TogglePower()
    {
        if (_currentTV == null)
            return;

        _currentTV.IsOn = !_currentTV.IsOn;
        Main.NewText($"TV turned {(_currentTV.IsOn ? "ON" : "OFF")}", Color.Cyan);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        // Close UI if player moves too far from TV
        if (_currentTV != null)
        {
            Vector2 tvCenter = new Vector2(
                (_currentTV.Position.X + 4) * 16f,
                (_currentTV.Position.Y + 2.5f) * 16f
            );

            float distance = Vector2.Distance(Main.LocalPlayer.Center, tvCenter);
            if (distance > 400f) // 25 tiles
            {
                ModContent.GetInstance<TVRemoteUISystem>().CloseUI();
            }
        }

        // Close on ESC
        if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
        {
            ModContent.GetInstance<TVRemoteUISystem>().CloseUI();
        }
    }
}

public class TVRemoteUISystem : ModSystem
{
    private UserInterface _remoteInterface;
    private TVRemoteUI _remoteUI;

    public override void Load()
    {
        if (!Main.dedServ)
        {
            _remoteInterface = new UserInterface();
            _remoteUI = new TVRemoteUI();
            _remoteUI.Activate();
        }
    }

    public override void Unload()
    {
        _remoteUI?.Deactivate();
    }

    public void OpenUI(TVTileEntity tvEntity)
    {
        if (_remoteUI == null)
            return;

        _remoteUI.SetTV(tvEntity);
        _remoteInterface?.SetState(_remoteUI);
    }

    public void CloseUI()
    {
        _remoteInterface?.SetState(null);
    }

    public bool IsUIOpen() => _remoteInterface?.CurrentState != null;

    public override void UpdateUI(GameTime gameTime)
    {
        _remoteInterface?.Update(gameTime);
    }

    public override void ModifyInterfaceLayers(System.Collections.Generic.List<GameInterfaceLayer> layers)
    {
        int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
        if (mouseTextIndex != -1)
        {
            layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                "CalRemix: TV Remote UI",
                delegate
                {
                    if (_remoteInterface?.CurrentState != null)
                    {
                        _remoteInterface.Draw(Main.spriteBatch, new GameTime());
                    }
                    return true;
                },
                InterfaceScaleType.UI)
            );
        }
    }
}
