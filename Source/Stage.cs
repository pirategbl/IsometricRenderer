using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IsometricGame.Source.ACharacter;
using System.Xml;
using IsometricGame.Source.Utilities;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using IsometricGame.Source.ATile;

namespace IsometricGame.Source.AStage
{
    public class Stage
    {
        #region Attributes
        private Texture2D[] _TextureList;
        private Vector2 _MapSize;
        private List<Tile[,]> _StageMatrix;
        private Character _MainCharacter;
        private Camera _Camera = new Camera();
        private float _PreviousMouseScrollWheelValue;
        private float _CurrentMouseScrollWheelValue;
        #endregion
        #region Enums
        private enum _Stages
        {
            FirstStage = 1
        }
        #endregion

        #region Properties
        /// <summary>
        /// Returns the main character.
        /// </summary>
        public Character MainCharacter { get { return _MainCharacter; } }
        /// <summary>
        /// Returns the camera.
        /// </summary>
        public Camera Camera { get { return _Camera; } }

        #endregion

        #region Constructors
        public Stage(ContentManager content)
        {
            _Camera.Position = new Vector2(0, 0);
            _TextureList = new Texture2D[4];
            LoadTextures(content);
            ChangeStage((int)_Stages.FirstStage);
            _MainCharacter = new Character(content);
            _MainCharacter.AbsolutePosition = (IsometricCoord.MapToIso(new Vector2(8, 8))); // X = J Y = I
            _MainCharacter.Neighbours = new Vector2[3, 3];

        }
        #endregion

        #region Methods
        /// <summary>
        /// Used to change the current stage. It loads the XML and fills the stage matrix.
        /// </summary>
        /// <param name="whichStage">Receives an enum(int) as input to indicate which stage to change to.</param>
        public void ChangeStage(int whichStage)
        {
            _StageMatrix = ReadXMLMap.GetStageMatrix(whichStage);
            _MapSize = ReadXMLMap.MapSize;
        }

        /// <summary>
        /// Loads every texture used by the stage.
        /// </summary>
        /// <param name="ct">Takes the content manager as parameter.</param>
        private void LoadTextures(ContentManager ct)
        {
            _TextureList[0] = (ct.Load<Texture2D>("tiles/brick"));
            _TextureList[1] = (ct.Load<Texture2D>("tiles/dirt"));
            _TextureList[2] = (ct.Load<Texture2D>("tiles/grass"));
            _TextureList[3] = (ct.Load<Texture2D>("tiles/mesa"));
        }

        private bool IsNeighbour(Vector2 TilePosition)
        {
            return true;
        }

        /// <summary>
        /// Method called when scene needs to be drawn.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="graphicsDevice">Takes the spritebatch component and graphics device as parameter.</param>
        public void Draw(SpriteBatch sb, GraphicsDevice graphicsDevice)
        {
            sb.Begin(SpriteSortMode.Immediate,
                        null,
                        null,
                        null,
                        null,
                        null,
                        _Camera.GetTransformationMatrix(graphicsDevice));
            #region Draws the first layer
            if (_StageMatrix.Count > 1)
            {
                // Floor Layer
                for (int i = 0; i < _MapSize.Y; i++)
                {
                    for (int j = 0; j < _MapSize.X; j++)
                    {
                        PlaceTile(_StageMatrix[0][i, j].Texture, _StageMatrix[0][i, j].Position, sb);
                    }
                }
            }
            #endregion

            #region Testing out the new way of drawing the scene
            for (int i = 0; i < _MapSize.Y; i++)
            {
                for (int j = 0; j < _MapSize.X; j++)
                {
                    // I believe this position is the isometric one, not the matrix, which is what I need.
                    if (!IsNeighbour(_StageMatrix[1][i, j].Position))
                    {

                    }
                }
            }
            #endregion

            #region All the map before the character
            /*
            for (int k = 1; k < _StageMatrix.Count; k++)
            {
                for (int i = 0; i < _MapSize.Y; i++)
                {
                    for (int j = 0; j < _MapSize.X; j++)
                    {
                        PlaceTile(_StageMatrix[k][i, j].Texture, _StageMatrix[0][i, j].Position, sb);
                    }
                }
            }
            
            _MainCharacter.Draw(sb);
            */
            #endregion

            #region All the map after the character
            /*
            _MainCharacter.Draw(sb);
            for (int k = 1; k < _StageMatrix.Count; k++)
            {
                for (int i = 0; i < _MapSize.Y; i++)
                {
                    for (int j = 0; j < _MapSize.X; j++)
                    {
                        PlaceTile(_StageMatrix[k][i, j].Texture, _StageMatrix[0][i, j].Position, sb);
                    }
                }
            } 
            */
            #endregion

            #region Draws the first step for Depth Sorting, which is the entire portion of the stage above the character (Black Layer)
            for (int i = 0; i < _MainCharacter.Neighbours[0, 0].Y; i++)
            {
                for (int j = 0; j < _MapSize.X; j++)
                {
                    PlaceTile(_StageMatrix[1][i, j].Texture, _StageMatrix[1][i, j].Position, sb);
                }
            }
            #endregion

            #region Draws the second step for Depth Sorting, which is the left portion of the map
            for (int i = (int)_MainCharacter.Neighbours[0, 0].Y; i <= _MainCharacter.Neighbours[2, 0].Y; i++)
            {
                for (int j = 0; j < (int)_MainCharacter.Neighbours[0, 0].X; j++)
                {
                    PlaceTile(_StageMatrix[1][i, j].Texture, _StageMatrix[1][i, j].Position, sb);
                }
            }
            #endregion

            #region Draws the third step for Depth Sorting, which is the 3x3 matrix around the character
            PlaceTile(_StageMatrix[1][(int)_MainCharacter.Neighbours[0, 0].Y, (int)_MainCharacter.Neighbours[0, 0].X].Texture,
                    _StageMatrix[1][(int)_MainCharacter.Neighbours[0, 0].Y, (int)_MainCharacter.Neighbours[0, 0].X].Position, sb);
            PlaceTile(_StageMatrix[1][(int)_MainCharacter.Neighbours[0, 1].Y, (int)_MainCharacter.Neighbours[0, 1].X].Texture,
                    _StageMatrix[1][(int)_MainCharacter.Neighbours[0, 1].Y, (int)_MainCharacter.Neighbours[0, 1].X].Position, sb);

            PlaceTile(_StageMatrix[1][(int)_MainCharacter.Neighbours[0, 2].Y, (int)_MainCharacter.Neighbours[0, 2].X].Texture,
                _StageMatrix[1][(int)_MainCharacter.Neighbours[0, 2].Y, (int)_MainCharacter.Neighbours[0, 2].X].Position, sb);

            PlaceTile(_StageMatrix[1][(int)_MainCharacter.Neighbours[1, 0].Y, (int)_MainCharacter.Neighbours[1, 0].X].Texture,
                _StageMatrix[1][(int)_MainCharacter.Neighbours[1, 0].Y, (int)_MainCharacter.Neighbours[1, 0].X].Position, sb);

            PlaceTile(_StageMatrix[1][(int)_MainCharacter.Neighbours[2, 0].Y, (int)_MainCharacter.Neighbours[2, 0].X].Texture,
                _StageMatrix[1][(int)_MainCharacter.Neighbours[2, 0].Y, (int)_MainCharacter.Neighbours[2, 0].X].Position, sb);

            _MainCharacter.Draw(sb);

            PlaceTile(_StageMatrix[1][(int)_MainCharacter.Neighbours[1, 2].Y, (int)_MainCharacter.Neighbours[1, 2].X].Texture,
                _StageMatrix[1][(int)_MainCharacter.Neighbours[1, 2].Y, (int)_MainCharacter.Neighbours[1, 2].X].Position, sb);

            PlaceTile(_StageMatrix[1][(int)_MainCharacter.Neighbours[2, 1].Y, (int)_MainCharacter.Neighbours[2, 1].X].Texture,
                _StageMatrix[1][(int)_MainCharacter.Neighbours[2, 1].Y, (int)_MainCharacter.Neighbours[2, 1].X].Position, sb);

            PlaceTile(_StageMatrix[1][(int)_MainCharacter.Neighbours[2, 2].Y, (int)_MainCharacter.Neighbours[2, 2].X].Texture,
                _StageMatrix[1][(int)_MainCharacter.Neighbours[2, 2].Y, (int)_MainCharacter.Neighbours[2, 2].X].Position, sb);
            #endregion

            #region Draws the fourth step for Depth Sorting, which is the right portion of the map
            for (int i = (int)_MainCharacter.Neighbours[0, 2].Y; i < (int)_MainCharacter.Neighbours[2, 2].Y; i++)
            {
                for (int j = (int)_MainCharacter.Neighbours[0, 2].X; j < _MapSize.X; j++)
                {
                    PlaceTile(_StageMatrix[1][i, j].Texture, _StageMatrix[1][i, j].Position, sb);
                }
            }
            #endregion

            #region Draws the fifth step for Depth Sorting, which is the bottom portion of the map
            for (int i = (int)_MainCharacter.Neighbours[2, 2].Y; i < _MapSize.Y; i++)
            {
                for (int j = 0; j < _MapSize.X; j++)
                {
                    PlaceTile(_StageMatrix[1][i, j].Texture, _StageMatrix[1][i, j].Position, sb);
                }
            }
            #endregion
        }

        /// <summary>
        /// Draw the tile at its position
        /// </summary>
        /// <param name="aTileType">Which tile is going to be drawn.</param>
        /// <param name="aPos">At what position it will be drawn.</param>
        /// <param name="aSb">Spritebatch component.</param>
        public void PlaceTile(int aTileType, Vector2 aPos, SpriteBatch aSb)
        {
            switch (aTileType)
            {
                case 0:
                    break;
                case 1:
                    aSb.Draw(_TextureList[0], aPos, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    break;
                case 2:
                    aSb.Draw(_TextureList[1], aPos, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    break;
                case 3:
                    aSb.Draw(_TextureList[2], aPos, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    break;
                case 4:
                    aSb.Draw(_TextureList[3], new Vector2(aPos.X, aPos.Y + _TextureList[3].Height / 4), null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    break;
            }
        }

        /// <summary>
        /// Updates the stage logic
        /// </summary>
        /// <param name="ks">Keyboard state.</param>
        /// <param name="ms">Mouse state.</param>
        public void Update(KeyboardState ks, MouseState ms)
        {
            _PreviousMouseScrollWheelValue = _CurrentMouseScrollWheelValue;
            _CurrentMouseScrollWheelValue = ms.ScrollWheelValue;

            #region Camera
            if (ks.IsKeyDown(Keys.W))
                _Camera.Move(new Vector2(0, -Camera._Speed));
            if (ks.IsKeyDown(Keys.S))
                _Camera.Move(new Vector2(0, Camera._Speed));
            if (ks.IsKeyDown(Keys.A))
                _Camera.Move(new Vector2(-Camera._Speed, 0));
            if (ks.IsKeyDown(Keys.D))
                _Camera.Move(new Vector2(Camera._Speed, 0));
            if (_CurrentMouseScrollWheelValue > _PreviousMouseScrollWheelValue)
                _Camera.Zoom = _Camera.Zoom + 0.05f;
            if (_CurrentMouseScrollWheelValue < _PreviousMouseScrollWheelValue)
                _Camera.Zoom = _Camera.Zoom - 0.05f;
            #endregion

            #region Character (To be moved)
            if (ks.IsKeyDown(Keys.Up))
            {
                Vector2 positionAux;
                Vector2 matrixAux;
                positionAux = _MainCharacter.AbsolutePosition - new Vector2(0, 1);
                matrixAux = IsometricCoord.IsoToMap(positionAux);
                if (matrixAux.Y > 0 && matrixAux.X > 0 && !doesItCollide(matrixAux))
                    _MainCharacter.AbsolutePosition = (new Vector2(_MainCharacter.AbsolutePosition.X, _MainCharacter.AbsolutePosition.Y - _MainCharacter.Speed));
            }

            if (ks.IsKeyDown(Keys.Right))
            {
                Vector2 positionAux;
                Vector2 matrixAux;
                positionAux = _MainCharacter.AbsolutePosition + new Vector2(1, 0);
                matrixAux = IsometricCoord.IsoToMap(positionAux);
                if (matrixAux.Y > 0 && matrixAux.X < _MapSize.X - 1 && !doesItCollide(matrixAux))
                    _MainCharacter.AbsolutePosition = (new Vector2(_MainCharacter.AbsolutePosition.X + _MainCharacter.Speed, _MainCharacter.AbsolutePosition.Y));
            }

            if (ks.IsKeyDown(Keys.Down))
            {
                Vector2 positionAux;
                Vector2 matrixAux;
                positionAux = _MainCharacter.AbsolutePosition + new Vector2(0, 1);
                matrixAux = IsometricCoord.IsoToMap(positionAux);
                if (matrixAux.Y < _MapSize.Y - 1 && matrixAux.X < _MapSize.X - 1 && !doesItCollide(matrixAux))
                    _MainCharacter.AbsolutePosition = (new Vector2(_MainCharacter.AbsolutePosition.X, _MainCharacter.AbsolutePosition.Y + _MainCharacter.Speed));
            }

            if (ks.IsKeyDown(Keys.Left))
            {
                Vector2 positionAux;
                Vector2 matrixAux;
                positionAux = _MainCharacter.AbsolutePosition - new Vector2(1, 0);
                matrixAux = IsometricCoord.IsoToMap(positionAux);
                if (matrixAux.X > 0 && matrixAux.Y < _MapSize.Y - 1 && !doesItCollide(matrixAux))
                    _MainCharacter.AbsolutePosition = (new Vector2(_MainCharacter.AbsolutePosition.X - _MainCharacter.Speed, _MainCharacter.AbsolutePosition.Y));
            }
            #endregion

            #region Updating the Main Characters Neighbours
            _MainCharacter.Neighbours[0, 0] = new Vector2(_MainCharacter.MatrixPosition.X - 1, _MainCharacter.MatrixPosition.Y - 1);
            _MainCharacter.Neighbours[0, 1] = new Vector2(_MainCharacter.MatrixPosition.X, _MainCharacter.MatrixPosition.Y - 1);
            _MainCharacter.Neighbours[0, 2] = new Vector2(_MainCharacter.MatrixPosition.X + 1, _MainCharacter.MatrixPosition.Y - 1);
            _MainCharacter.Neighbours[1, 0] = new Vector2(_MainCharacter.MatrixPosition.X - 1, _MainCharacter.MatrixPosition.Y);
            _MainCharacter.Neighbours[1, 1] = new Vector2(_MainCharacter.MatrixPosition.X, _MainCharacter.MatrixPosition.Y);
            _MainCharacter.Neighbours[1, 2] = new Vector2(_MainCharacter.MatrixPosition.X + 1, _MainCharacter.MatrixPosition.Y);
            _MainCharacter.Neighbours[2, 0] = new Vector2(_MainCharacter.MatrixPosition.X - 1, _MainCharacter.MatrixPosition.Y + 1);
            _MainCharacter.Neighbours[2, 1] = new Vector2(_MainCharacter.MatrixPosition.X, _MainCharacter.MatrixPosition.Y + 1);
            _MainCharacter.Neighbours[2, 2] = new Vector2(_MainCharacter.MatrixPosition.X + 1, _MainCharacter.MatrixPosition.Y + 1);
            #endregion
        }

        /// <summary>
        /// Check collisions.
        /// </summary>
        /// <param name="aux">Since this method is preemptive, the parameter is an auxiliary vector rather than the actual one.</param>
        /// <returns></returns>
        public bool doesItCollide(Vector2 aux)
        {
            if (_StageMatrix[1][(int)aux.Y, (int)aux.X].Texture != 0)
                return true;
            return false;
        }
        #endregion 
    }
}