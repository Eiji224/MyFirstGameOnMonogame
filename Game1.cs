using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyGame;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    private Sprite _playerSprite;
    
    private List<Sprite> _coins;
    
    private Texture2D _background;
    private Texture2D _coinTexture;
    
    private Dictionary<Keys, (int x, int y)> _movementKeys;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _movementKeys = new Dictionary<Keys, (int x, int y)>
        {
            { Keys.W, (0, -1) },
            { Keys.A, (-1, 0) },
            { Keys.S, (0, 1) },
            { Keys.D, (1, 0) }
        };
        
        _coins = new List<Sprite>();
        
        //CoinGenerator.GenerateCoin(_coins, _coinTexture, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        _background = Content.Load<Texture2D>("background");
        _coinTexture = Content.Load<Texture2D>("coin");
        
        var playerTexture = Content.Load<Texture2D>("player");
        _playerSprite = new Sprite(playerTexture, new Rectangle(0, 0, 50, 50));
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        foreach (var key in _movementKeys.Where(key => Keyboard.GetState().IsKeyDown(key.Key)))
        {
            _playerSprite.MoveTo(key.Value.x, key.Value.y);
        }

        CheckForCollectCoin();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _spriteBatch.Draw(_background,
            new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
        _spriteBatch.Draw(_playerSprite.Texture, _playerSprite.Position, Color.White);
        foreach (var coin in _coins)
        {
            _spriteBatch.Draw(coin.Texture, coin.Position, Color.White);
        }
        
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }

    private void CheckForCollectCoin()
    {
        for (var i = 0; i < _coins.Count; i++)
        {
            if (_playerSprite.Position.Intersects(_coins[i].Position))
            {
                _coins.Remove(_coins[i]);
            }
        }


        if (_coins.Count < 10)
        {
            CoinGenerator.GenerateCoin(_coins, _coinTexture,
                GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
        }
    }
}