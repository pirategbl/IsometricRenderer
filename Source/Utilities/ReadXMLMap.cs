using IsometricGame.Source.AStage;
using IsometricGame.Source.ATile;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace IsometricGame.Source.Utilities
{
    public static class ReadXMLMap
    {
        #region Attributes
        /// <summary>
        /// Stores the map, which is a xml document.
        /// </summary>
        private static XmlDocument _XmlMap = new XmlDocument();

        /// <summary>
        /// Stores the xml map in a node list.
        /// </summary>
        private static XmlNodeList _XmlLayers;

        /// <summary>
        /// Stores the map height value.
        /// </summary>
        private static int _MapHeight;

        /// <summary>
        /// Stores the map width value;
        /// </summary>
        private static int _MapWidth;
        #endregion

        #region Properties
        /// <summary>
        /// Returns the size of the map.
        /// </summary>
        public static Vector2 MapSize
        {
            get
            {
                return new Vector2(_MapWidth, _MapHeight);
            }
        }
        #endregion

        #region Methods
        public static List<Tile[,]> GetStageMatrix(int whichMap)
        {
            /* Loads the XML Stage */
            #region Creates Stage Matrix
            switch (whichMap)
            {
                case 1:
                    _XmlMap.Load("Content/maps/MapaTD.tmx");
                    break;
            }

            _MapWidth = int.Parse(_XmlMap.DocumentElement.Attributes.GetNamedItem("width").Value);
            _MapHeight = int.Parse(_XmlMap.DocumentElement.Attributes.GetNamedItem("height").Value);

            /* Makes a list of every layer in the given stage */
            _XmlLayers = _XmlMap.GetElementsByTagName("layer");

            List<Tile[,]> layers = new List<Tile[,]>();
            for (int i = 0; i < _XmlLayers.Count; i++)
            {
                layers.Add(new Tile[_MapWidth, _MapHeight]);
            }

            #endregion

            #region Insert Values on the Stage Matrix
            for (int l = 0; l < layers.Count; l++)
            {
                /* Gets the string which is like " 1,1,1,1,1,..." */
                string FirstLayerArrayWithCommas = _XmlLayers[l].InnerText;

                /* Removes the commas so the string becomes only numbers and "\n" */
                string[] FirstLayerSplittedArray = FirstLayerArrayWithCommas.Split(',');

                for (int i = 0; i < _MapHeight; i++)
                {
                    for (int j = 0; j < _MapWidth; j++)
                    {
                        /* Since "SplittedArray" is a one dimensional array, the tiles are organized like this:
                            Row-Column-Column-Column(...)-Row(...)
                            So to fill the stage matrix correctly, I'm using the math below:
                            The first column is filled ENTIRELY (all of the first column's rows), then the second, then the third, and so on...
                            It's very important to note, however, that in order for this algorithm to work, the map must have the exact same
                            size of Height and Width */
                        layers[l][j, i] = new Tile(int.Parse(FirstLayerSplittedArray[i + (j * _MapHeight)]));
                    }
                }
            }
            #endregion

            #region Set up the tiles' position on the Stage Matrix
            Vector2 pos;

            for (int i = 0; i < _MapHeight; i++)
            {
                for (int j = 0; j < _MapWidth; j++)
                {
                    pos.X = j;
                    pos.Y = i;
                    pos = IsometricCoord.MapToIso(pos);
                    layers[0][i, j].Position = pos;
                }
            }

            for (int i = 0; i < _MapHeight; i++)
            {
                for (int j = 0; j < _MapWidth; j++)
                {
                    pos.X = j - 1;
                    pos.Y = i - 1;
                    pos = IsometricCoord.MapToIso(pos);
                    layers[1][i, j].Position = pos;
                }
            }
            #endregion

            return layers;
        }        
        #endregion
    }
}
