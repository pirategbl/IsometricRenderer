using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsometricGame.Source
{
    public class Camera
    {
        #region Attributes
        /// <summary>
        /// Stores the amount of zoom the camera has.
        /// </summary>
        private float _Zoom;

        /// <summary>
        /// Stores the matrix transformation.
        /// </summary>
        private Matrix _Transform;

        /// <summary>
        /// Stores the camera position.
        /// </summary>
        private Vector2 _Position;

        /// <summary>
        /// Stores the camera Rotation.
        /// </summary>
        private float _Rotation;

        /// <summary>
        /// Stores the character's speed
        /// </summary>
        public const int _Speed = 4;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the Zoom value.
        /// </summary>
        public float Zoom { get { return _Zoom; } set { _Zoom = value; if (_Zoom < 0.1f) _Zoom = 0.1f; if (_Zoom > 3.0f) _Zoom = 3.0f; } }

        /// <summary>
        /// Gets or sets the camera position.
        /// </summary>
        public Vector2 Position { get { return _Position; } set { _Position = value; } }

        /// <summary>
        /// Gets or sets the camera rotation.
        /// </summary>
        public float Rotation { get { return _Rotation; } set { _Rotation = value; } }
        
        #endregion

        #region Constructors
        /// <summary>
        /// Regular constructor. Initializes Zoom, Rotation and Position.
        /// </summary>
        public Camera()
        {
            _Zoom = 1.0f;
            _Rotation = 0.0f;
            _Position = Vector2.Zero;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Auxiliary method to Move the camera
        /// </summary>
        /// <param name="amount">Takes the amount of movements the camera does.</param>
        public void Move(Vector2 amount)
        {
            _Position += amount;
        }

        /// <summary>
        /// Returns the Transformation Matrix.
        /// </summary>
        /// <param name="graphicsDevice">Takes the graphics device as parameter.</param>
        /// <returns></returns>
        public Matrix GetTransformationMatrix(GraphicsDevice graphicsDevice)
        {
            Viewport viewPort = graphicsDevice.Viewport;
            _Transform =       // Thanks to o KB o for this solution
              Matrix.CreateTranslation(new Vector3(-_Position.X, -_Position.Y, 0)) *
                                         Matrix.CreateRotationZ(Rotation) *
                                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3(viewPort.Width * 0.5f, viewPort.Height * 0.5f, 0));
            return _Transform;
        }
        #endregion
    }
}
