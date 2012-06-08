using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace MapGame
{
    class map
    {
        Texture2D terrain;
        Vector2 position;
        Vector2 motion;
        float speed = 4;
        KeyboardState direction;
        GraphicsDevice graphicsdevice;
        Rectangle border; // the edge of the screen

        public map(Texture2D texture, Rectangle border)
        {
            terrain = texture;
            this.border = border;
            SetInStartPos();
        }

        public map(GraphicsDevice graphicsdevice, heightMapGauss terrwain, Rectangle border)
        {
            this.graphicsdevice = graphicsdevice;
            GenerateMap(terrwain);
            this.border = border;
        }

        public map (GraphicsDevice graphicsdevice, Rectangle border)
        {
            //new map(graphicsdevice,new heightMapGauss(), border);
            this.graphicsdevice = graphicsdevice;
            GenerateMap(new heightMapGauss());
            this.border = border;
        }

        public void Update()
        {
            motion = Vector2.Zero;

            direction = Keyboard.GetState();

            if (direction.IsKeyDown(Keys.Left))  
                motion.X = 1;
            if (direction.IsKeyDown(Keys.Right))
                motion.X = -1;
            if (direction.IsKeyDown(Keys.Up))
                motion.Y = 1;
            if (direction.IsKeyDown(Keys.Down))
                motion.Y = -1;
            if (direction.IsKeyDown(Keys.Enter))
                GenerateMap();
            motion *= speed;
            position += motion;
            LockMap();
        }

        private void GenerateMap(heightMapGauss terrwain)
        {
            int[,] heightMap = terrwain.getHeightMap();
            Color[] colors = new Color[heightMap.Length];
            int heightMapMax = terrwain.findMaxHeight();
            int heightMapWidth = heightMap.GetUpperBound(0) + 1;
            int heightMapHeight = heightMap.GetUpperBound(1) + 1;
            terrain = new Texture2D(this.graphicsdevice, heightMapWidth, heightMapHeight, true, SurfaceFormat.Color);
            for (int i = 0; i < heightMapHeight; i++) // TODO currently, we have to manually change all of these 1025's when we increase the size of the map. UGH!
            {
                for (int j = 0; j < heightMapWidth; j++)
                {
                    int cell = heightMap[i, j];
                    if (cell > 0)
                    {
                        colors[i * heightMapWidth + j] = new Color((255 * cell) / heightMapMax, 192 + (63 * (cell) / heightMapMax), (255 * cell) / heightMapMax);
                    }
                    else
                    {
                        colors[i * heightMapWidth + j] = new Color(0, 0, 150);
                    }
                }
            }
            terrain.SetData<Color>(colors);
            SetInStartPos();
        }

        private void GenerateMap()
        {
            GenerateMap(new heightMapGauss());
        }


        private void LockMap()
        {
            if (position.X > 0)
                position.X = 0;
            if (position.Y > 0)
                position.Y = 0;
            if (position.X + terrain.Width < border.Width)
                position.X = border.Width - terrain.Width;
            if (position.Y + terrain.Height < border.Height)
                position.Y = border.Height - terrain.Height;
        }

        public void SetInStartPos()
        {
            position.X = 0;
            position.Y = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(terrain, position, Color.White);
        }
    }
}
