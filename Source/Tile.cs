using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace IsometricGame.Source.ATile
{
    public enum DepthSorting : uint
    {
        Behind, Front
    }
    /// <summary>
    /// This class contains the information about the tiles, which includes the texture used by it and its position
    /// </summary>
    public class Tile
    {
        #region Attributes

        /// <summary>
        /// Stores a number that indicates what is this tile's texture.
        /// </summary>
        private int _Texture;

        /// <summary>
        /// Stores a Vector2 that indicates what is the position of this tile.
        /// </summary>
        private Vector2 _Position;

        /// <summary>
        /// Stores a Integer without signal that indicates which depth this Tile has.
        /// </summary>
        private uint _Depth;
        #endregion

        #region Properties

        /// <summary>
        /// Gets of Sets the tile's texture.
        /// </summary>
        public int Texture { get { return _Texture; } set { _Texture = value; } }

        /// <summary>
        /// Gets or Sets the tile's position.
        /// </summary>
        public Vector2 Position { get { return _Position; } set { _Position = value; } }

        /// <summary>
        /// Gets or sets the tile's depth value - 0 means behind the character and 1 in front of the character.
        /// </summary>
        public uint Depth { get { return _Depth; } set { _Depth = value; } }
        #endregion

        #region Constructors

        /// <summary>
        /// Standard constructor
        /// </summary>
        /// <param name="texture">Passes the texture id to the current Tile object, indicating what texture this tile has.</param>
        public Tile(int texture)
        {
            _Texture = texture;
        }
        #endregion
    }
}
