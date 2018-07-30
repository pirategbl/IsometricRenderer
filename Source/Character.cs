using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using IsometricGame.Source.Utilities;

namespace IsometricGame.Source.ACharacter
{
    public class Character
    {
        #region Attributes
        /// <summary>
        /// Stores the character's texture.
        /// </summary>
        private Texture2D _Texture;

        /// <summary>
        /// Stores the position the character is in the matrix, starting at 0,0. X represents the j value and Y the i value.
        /// </summary>
        private Vector2 _MatrixPosition;

        /// <summary>
        /// Stores the X and Y positions in isometric coordinates.
        /// </summary>
        private Vector2 _AbsolutePosition;

        /// <summary>
        /// Stores the squared matrix that indicates which tiles are near the character.
        /// </summary>
        private Vector2[,] _DepthSorting;

        /// <summary>
        /// Stores the character's speed
        /// </summary>
        public const int _Speed = 4;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the character's texture.
        /// </summary>
        public Texture2D Texture { get { return _Texture; } set { _Texture = value; } }

        /// <summary>
        /// Gets or sets the current Matrix Position. X should be the j value and Y should be the i value.
        /// </summary>
        public Vector2 MatrixPosition { get { return _MatrixPosition; } set { _MatrixPosition = value; } }

        /// <summary>
        /// Gets or sets the current Isometric Position.
        /// </summary>
        public Vector2 AbsolutePosition { get { return _AbsolutePosition; } set { _AbsolutePosition = value; } }

        /// <summary>
        /// Returns the character's speed.
        /// </summary>
        public int Speed { get { return _Speed; } }

        /// <summary>
        /// Gets or sets the characters neighbours in a squared matrix, the middle position being the character.
        /// </summary>
        public Vector2[,] Neighbours { get { return _DepthSorting; } set { _DepthSorting = value; } }
        #endregion

        #region Constructors
        /// <summary>
        /// Regular Constructor
        /// </summary>
        /// <param name="content">Takes the Content Manager as a parameter to enable the loading of the texture.</param>
        public Character(ContentManager content)
        {
            Texture = content.Load<Texture2D>("character/character");
            MatrixPosition = new Vector2(0, 0);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Updates the Character object
        /// </summary>
        /// <param name="keyboardState">Receives the keyboard state as parameter to determine what action to do.</param>
        public void Update(KeyboardState keyboardState)
        {
            _MatrixPosition = IsometricCoord.IsoToMap(_AbsolutePosition);
        }

        /// <summary>
        /// Draws the character on the screen.
        /// </summary>
        /// <param name="spriteBatch">Receives the spritebatch as parameter, enabling drawing.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_Texture, new Vector2(_AbsolutePosition.X + _Texture.Width + 20, _AbsolutePosition.Y - _Texture.Height + 25),
                null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
        }
        #endregion
    }
}
