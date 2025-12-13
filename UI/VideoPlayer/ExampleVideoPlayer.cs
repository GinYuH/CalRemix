using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace CalRemix.UI.VideoPlayer;

public class ExampleVideoPlayerUI : UIState
{
    private UIPanel _mainPanel;
    private VideoPlayerUIElement _videoPlayer;
    private UIPanel _controlPanel;

    // Control buttons
    private UITextPanel<string> _playButton;
    private UITextPanel<string> _pauseButton;
    private UITextPanel<string> _stopButton;
    private UITextPanel<string> _closeButton;

    // Test video path (change this to your video)
    private string _testVideoPath = @"C:\Users\evanm\OneDrive\Documents\My Games\Terraria\tModLoader\ModSources\CalRemix\UI\VideoPlayer\PortedHim.mp4";

    public override void OnInitialize()
    {
        // Create main container panel
        _mainPanel = new UIPanel();
        _mainPanel.Width.Set(800, 0f);
        _mainPanel.Height.Set(520, 0f);
        _mainPanel.HAlign = 0.5f; // Center horizontally
        _mainPanel.VAlign = 0.5f; // Center vertically
        _mainPanel.BackgroundColor = new Color(33, 43, 79) * 0.9f;
        Append(_mainPanel);

        // Create video player (700x400 display, 1280x720 internal resolution)
        _videoPlayer = new VideoPlayerUIElement(700, 400, 1280, 720);
        _videoPlayer.HAlign = 0.5f;
        _videoPlayer.Top.Set(20, 0f);
        _mainPanel.Append(_videoPlayer);

        // Create control panel
        _controlPanel = new UIPanel();
        _controlPanel.Width.Set(700, 0f);
        _controlPanel.Height.Set(60, 0f);
        _controlPanel.HAlign = 0.5f;
        _controlPanel.Top.Set(440, 0f);
        _controlPanel.BackgroundColor = new Color(25, 33, 63);
        _mainPanel.Append(_controlPanel);

        // Create control buttons
        float buttonSpacing = 10f;
        float buttonWidth = 100f;
        float startX = (700 - (buttonWidth * 4 + buttonSpacing * 3)) / 2;

        // Play button
        _playButton = new UITextPanel<string>("Play");
        _playButton.Width.Set(buttonWidth, 0f);
        _playButton.Height.Set(40, 0f);
        _playButton.Left.Set(startX, 0f);
        _playButton.VAlign = 0.5f;
        _playButton.OnLeftClick += OnPlayClicked;
        _controlPanel.Append(_playButton);

        // Pause button
        _pauseButton = new UITextPanel<string>("Pause");
        _pauseButton.Width.Set(buttonWidth, 0f);
        _pauseButton.Height.Set(40, 0f);
        _pauseButton.Left.Set(startX + buttonWidth + buttonSpacing, 0f);
        _pauseButton.VAlign = 0.5f;
        _pauseButton.OnLeftClick += OnPauseClicked;
        _controlPanel.Append(_pauseButton);

        // Stop button
        _stopButton = new UITextPanel<string>("Stop");
        _stopButton.Width.Set(buttonWidth, 0f);
        _stopButton.Height.Set(40, 0f);
        _stopButton.Left.Set(startX + (buttonWidth + buttonSpacing) * 2, 0f);
        _stopButton.VAlign = 0.5f;
        _stopButton.OnLeftClick += OnStopClicked;
        _controlPanel.Append(_stopButton);

        // Close button
        _closeButton = new UITextPanel<string>("Close");
        _closeButton.Width.Set(buttonWidth, 0f);
        _closeButton.Height.Set(40, 0f);
        _closeButton.Left.Set(startX + (buttonWidth + buttonSpacing) * 3, 0f);
        _closeButton.VAlign = 0.5f;
        _closeButton.OnLeftClick += OnCloseClicked;
        _closeButton.BackgroundColor = new Color(100, 50, 50);
        _controlPanel.Append(_closeButton);
    }

    private void OnPlayClicked(UIMouseEvent evt, UIElement listeningElement)
    {
        if (_videoPlayer.IsPlaying)
        {
            _videoPlayer.Resume();
        }
        else
        {
            _videoPlayer.PlayUrl("https://www.youtube.com/watch?v=EbqIrVctisA");
        }
    }

    private void OnPauseClicked(UIMouseEvent evt, UIElement listeningElement)
    {
        _videoPlayer.Pause();
    }

    private void OnStopClicked(UIMouseEvent evt, UIElement listeningElement)
    {
        _videoPlayer.Stop();
    }

    private void OnCloseClicked(UIMouseEvent evt, UIElement listeningElement)
    {
        // Close this UI
        ModContent.GetInstance<ExampleVideoUISystem>().HideUI();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        // Allow ESC to close
        if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape) &&
            !Main.oldKeyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
        {
            ModContent.GetInstance<ExampleVideoUISystem>().HideUI();
        }
    }

    // Clean up when UI is closed
    public void OnClose()
    {
        _videoPlayer?.Stop();
        _videoPlayer?.RemoveCleanup();
    }
}

/// <summary>
/// System to manage the video player UI state.
/// </summary>
public class ExampleVideoUISystem : ModSystem
{
    private UserInterface _videoUserInterface;
    internal ExampleVideoPlayerUI _videoUI;

    public override void Load()
    {
        if (Main.dedServ) return;

        _videoUI = new ExampleVideoPlayerUI();
        _videoUI.Activate();
        _videoUserInterface = new UserInterface();
    }

    public override void Unload()
    {
        _videoUI?.OnClose();
    }

    public void ShowUI()
    {
        _videoUserInterface?.SetState(_videoUI);
    }

    public void HideUI()
    {
        _videoUI?.OnClose();
        _videoUserInterface?.SetState(null);
    }

    public void ToggleUI()
    {
        if (_videoUserInterface?.CurrentState != null)
        {
            HideUI();
        }
        else
        {
            ShowUI();
        }
    }

    public override void UpdateUI(GameTime gameTime)
    {
        _videoUserInterface?.Update(gameTime);
    }

    public override void ModifyInterfaceLayers(System.Collections.Generic.List<GameInterfaceLayer> layers)
    {
        int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
        if (mouseTextIndex != -1)
        {
            layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                "CalRemix: Video Player UI",
                delegate
                {
                    if (_videoUserInterface?.CurrentState != null)
                    {
                        _videoUserInterface.Draw(Main.spriteBatch, new GameTime());
                    }
                    return true;
                },
                InterfaceScaleType.UI)
            );
        }
    }
}

/// <summary>
/// Example item that opens the video player UI.
/// </summary>
public class VideoPlayerOpenerItem : ModItem
{
    public override string Texture => "CalamityMod/Projectiles/Boss/SignusScythe";

    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 32;
        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.useTime = 30;
        Item.useAnimation = 30;
        Item.rare = ItemRarityID.Blue;
    }

    public override bool? UseItem(Player player)
    {
        if (Main.myPlayer == player.whoAmI)
        {
            ModContent.GetInstance<ExampleVideoUISystem>().ShowUI();
        }
        return true;
    }
}
