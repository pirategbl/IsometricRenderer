using Microsoft.Xna.Framework;

namespace IsometricGame.Source.Utilities
{
    public static class IsometricCoord
    {
        #region Attributes
        /// <summary>
        /// Stores the tile width. It can't be changed as the logic is based around this value and the tile height's.
        /// </summary>
        private const int _TileWidth = 256;

        /// <summary>
        /// Stores the tile height. It can't be changed as the logic is based around this value and the tile width's.
        /// </summary>
        private const int _TileHeight = 128;
        #endregion

        #region Properties
        /// <summary>
        /// Returns the tile width value.
        /// </summary>
        public static int TileWidth { get { return _TileWidth; } }

        /// <summary>
        /// Returns the tile height value.
        /// </summary>
        public static int TileHeight { get { return _TileHeight; } }
        #endregion

        #region Methods

        /// <summary>
        /// Converts the current matrix position into the isometric position.
        /// </summary>
        /// <param name="aPosition">Current matrix position.</param>
        /// <returns></returns>
        public static Vector2 MapToIso(Vector2 aPosition)
        {
            float x = (aPosition.X - aPosition.Y) * (TileWidth / 2);
            float y = (aPosition.X + aPosition.Y) * (TileHeight / 2);
            return new Vector2((int)x, (int)y);
            //return aPosition;
        }

        /// <summary>
        /// Converts the current isometric position back to matrix position.
        /// </summary>
        /// <param name="aPosition">Current isometric position.</param>
        /// <returns></returns>
        public static Vector2 IsoToMap(Vector2 aPosition)
        {
            float x = ((aPosition.X / (TileWidth / 2)) + (aPosition.Y / (TileHeight / 2))) / 2;
            float y = ((aPosition.Y / (TileHeight / 2)) - (aPosition.X / (TileWidth / 2))) / 2;
            return new Vector2((int)x, (int)y);
        }
        #endregion
    }
}