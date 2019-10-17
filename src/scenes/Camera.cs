using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using EVCMonoGame.src.input;

using EVCMonoGame.src;
using EVCMonoGame.src.gui;
using EVCMonoGame.src.input;
using EVCMonoGame.src.scenes;

namespace EvcMonogame.src.scenes
{
    enum Screenpoint
    {
        UP_LEFT_EDGE,
        UP_RIGHT_EDGE,
        DOWN_RIGHT_EDGE,
        DOWN_LEFT_EDGE,
        CENTER
    }
    class Camera
    {
        protected float cameraPositionX;
        protected float cameraPositionY;
        protected float zoom = 1;
        public Camera(AnimatedSprite sprite) { }
        public Camera(AnimatedSprite sprite, Screenpoint screenpoint) { }
        public Camera(Player player) { }
        public Camera(Player player, Screenpoint screenpoint) { }
        public Camera(Vector2 position) { }
        public Camera(Vector2 position, Screenpoint screenpoint) { }
        public void setCameraPointtoPosition(Vector2 position, Screenpoint point)
        {
        }
        public void setZoom(float zoom)
        {
            return;
        }
        public void moveCamera(Vector2 direction, float distance)
        {
            return;
        }
        public Vector2 getCameraPoint(Screenpoint pointt)
        {
            return Vector2.Zero;
        }
        public Matrix getTransformationMatrix()
        {
            return new Matrix(
                     new Vector4(zoom, 0, 0, 0),
                     new Vector4(0, zoom, 0, 0),
                     new Vector4(0, 0, 1, 0),
                     new Vector4(cameraPositionX, cameraPositionY, 0, 1));
        }
    }
}
