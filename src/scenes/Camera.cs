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
using EVCMonoGame.src.scenes;
using EVCMonoGame.src.utility;

namespace EVCMonoGame.src.scenes
{
    // TODO: Kamerafahrt.

    public enum Screenpoint
    {
        UP_LEFT_EDGE,
        UP_RIGHT_EDGE,
        DOWN_RIGHT_EDGE,
        DOWN_LEFT_EDGE,
        UP,
        RIGHT,
        DOWN,
        LEFT,
        CENTER
    }

    public struct PathPoint
    {
        public Vector2 p1;
        public Vector2 p2;
        public int duration;

        public PathPoint(Vector2 p1, Vector2 p2, int duration)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.duration = duration;
        }
    }

    public class Camera : IUpdateable
    {
        #region Fields

        private Vector2 cameraPosition;
        private float zoom = 1;
        private SceneManager sceneManager;
        private ITranslatable focusObject;
        private bool followsFocusObject = true;
        private Screenpoint focusPoint = Screenpoint.CENTER;
        private Vector2 offset = Vector2.Zero;
        private Easer moveEaser = new Easer(new Vector2(900, -200), new Vector2(900, 700), 5000, Easing.SineEaseInOut);

        private float currentYRightThumbStick = 0;
        private float rightThumbStickMaxZoomRate = 0.025f;

        #endregion

        #region Properties
        public bool DoUpdate
        {
            get; set;
        } = true;

        public ITranslatable FocusObject
        {
            get { return focusObject; }
        }

        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; }
        }

        #endregion


        public Camera(SceneManager manager, ITranslatable focusObject, Screenpoint focusPoint = Screenpoint.CENTER)
        {
            sceneManager = manager;
            this.focusObject = focusObject;
            this.focusPoint = focusPoint;
            Viewport viewport = sceneManager.GraphicsDevice.Viewport;
            cameraPosition = focusObject.Position + new Vector2(viewport.Width * 0.5f, viewport.Height * 0.5f);
            SetCameraToFocusObject(focusObject);
        }

        //public Camera(SceneManager manager, ITranslatable focusObject, Screenpoint focusPoint)
        //{
        //    sceneManager = manager;
        //    this.focusObject = focusObject;
        //    this.focusPoint = focusPoint;
        //    Viewport viewport = sceneManager.GraphicsDevice.Viewport;
        //    cameraPosition = focusObject.Position + new Vector2(viewport.Width * 0.5f, viewport.Height * 0.5f);
        //    setCameraToFocusObject(focusObject);
        //}

        public Camera(SceneManager manager, ITranslatable focusObject, Screenpoint screenpoint, Vector2 offset)
        {
            this.offset = offset;
            sceneManager = manager;
            this.focusObject = focusObject;
            this.focusPoint = screenpoint;
            Viewport viewport = sceneManager.GraphicsDevice.Viewport;
            cameraPosition = focusObject.Position + new Vector2(viewport.Width * 0.5f, viewport.Height * 0.5f);
            SetCameraToFocusObject(focusObject);
        }

        public Camera(SceneManager manager, Vector2 position)
            : this(manager, new ITranslatablePosition(position)) { }

        public Camera(SceneManager manager, Vector2 position, Screenpoint screenpoint)
            : this(manager, new ITranslatablePosition(position), screenpoint) { }

        public void SetOffset(Vector2 offset)
        {
            this.offset = offset;
        }

        public void SetCameraToPosition(Vector2 position)
        {
            SetCameraToFocusObject(new ITranslatablePosition(position));
        }

        public void SetCameraToFocusObject(ITranslatable focusObject, Screenpoint screenpoint)
        {
            followsFocusObject = true;

            this.focusPoint = screenpoint;
            SetCameraToFocusObject(focusObject);
        }

        public void SetCameraToFocusObject(ITranslatable focusObject)
        {
            followsFocusObject = true;

            this.focusObject = focusObject;
            Viewport viewport = sceneManager.GraphicsDevice.Viewport;
            switch (focusPoint)
            {
                case Screenpoint.UP_LEFT_EDGE:
                    cameraPosition = (-zoom) * focusObject.Position;
                    break;
                case Screenpoint.UP_RIGHT_EDGE:
                    cameraPosition = (-zoom) * focusObject.Position + new Vector2(viewport.Width, 0);
                    break;
                case Screenpoint.DOWN_RIGHT_EDGE:
                    cameraPosition = (-zoom) * focusObject.Position + new Vector2(viewport.Width, viewport.Height);
                    break;
                case Screenpoint.DOWN_LEFT_EDGE:
                    cameraPosition = (-zoom) * focusObject.Position + new Vector2(0, viewport.Height);
                    break;
                case Screenpoint.UP:
                    cameraPosition = (-zoom) * focusObject.Position + new Vector2(viewport.Width * 0.5f, 0);
                    break;
                case Screenpoint.RIGHT:
                    cameraPosition = (-zoom) * focusObject.Position + new Vector2(viewport.Width, viewport.Height * 0.5f);
                    break;
                case Screenpoint.DOWN:
                    cameraPosition = (-zoom) * focusObject.Position + new Vector2(viewport.Width * 0.5f, viewport.Height);
                    break;
                case Screenpoint.LEFT:
                    cameraPosition = (-zoom) * focusObject.Position + new Vector2(0, viewport.Height * 0.5f);
                    break;
                case Screenpoint.CENTER:
                    cameraPosition = ((-zoom) * focusObject.Position + new Vector2(viewport.Width * 0.5f, viewport.Height * 0.5f));
                    break;
            }
            cameraPosition += offset;
        }

        public void SetCameraToPosition(Vector2 position, Screenpoint point)
        {
            this.focusPoint = point;
            SetCameraToPosition(position);
        }

        //public void SetZoom(float zoom)
        //{
        //    this.zoom = zoom;
        //}

        public void MoveCamera(Vector2 from, Vector2 to, int durationInMillis)
        {
            followsFocusObject = false;

            moveEaser.From = from;
            moveEaser.To = to;
            moveEaser.DurationInMillis = durationInMillis;
            moveEaser.Start();
        }

        public Vector2 GetCameraPoint(Screenpoint point)
        {
            Viewport viewport = sceneManager.GraphicsDevice.Viewport;
            switch (focusPoint)
            {
                case Screenpoint.UP_LEFT_EDGE:
                    return (-zoom) * cameraPosition;
                case Screenpoint.UP_RIGHT_EDGE:
                    return (-zoom) * cameraPosition + new Vector2(viewport.Width, 0);
                case Screenpoint.DOWN_RIGHT_EDGE:
                    return (-zoom) * cameraPosition + new Vector2(viewport.Width, viewport.Height);
                case Screenpoint.DOWN_LEFT_EDGE:
                    return (-zoom) * cameraPosition + new Vector2(0, viewport.Height);
                case Screenpoint.UP:
                    return (-zoom) * cameraPosition + new Vector2(viewport.Width * 0.5f, 0);
                case Screenpoint.RIGHT:
                    return (-zoom) * cameraPosition + new Vector2(viewport.Width, viewport.Height * 0.5f);
                case Screenpoint.DOWN:
                    return (-zoom) * cameraPosition + new Vector2(viewport.Width * 0.5f, viewport.Height);
                case Screenpoint.LEFT:
                    return (-zoom) * cameraPosition + new Vector2(0, viewport.Height * 0.5f);
                case Screenpoint.CENTER:
                    return (-zoom) * cameraPosition + new Vector2(viewport.Width * 0.5f, viewport.Height * 0.5f);
                default:
                    return Vector2.Zero;
            }
        }

        public Matrix GetTransformationMatrix()
        {
            return new Matrix(
                     new Vector4(zoom, 0, 0, 0),
                     new Vector4(0, zoom, 0, 0),
                     new Vector4(0, 0, 1, 0),
                     new Vector4(cameraPosition.X, cameraPosition.Y, 0, 1));
        }

        //public void MovePath(params PathPoint[] pathPoints)
        //{
        //    // TODO
        //}

        public void Update(GameTime gameTime)
        {
            if (followsFocusObject)
            {
                SetCameraToFocusObject(focusObject);
            }

            /*if (!moveEaser.IsFinished)
            {
                moveEaser.Update(gameTime);
                SetCameraToPosition(moveEaser.CurrentValue);
            }*/

            // Zooming
            if (InputManager.HasRightGamePadStickMoved)
            {
                currentYRightThumbStick = InputManager.CurrentThumbSticks(PlayerIndex.One).Right.Y;

                if (currentYRightThumbStick > rightThumbStickMaxZoomRate)
                    currentYRightThumbStick = rightThumbStickMaxZoomRate;
                else if (currentYRightThumbStick < -rightThumbStickMaxZoomRate)
                    currentYRightThumbStick = -rightThumbStickMaxZoomRate;

                zoom += currentYRightThumbStick;

                Console.Write("Zoom: " + zoom);
            }
            if (InputManager.IsKeyPressed(Keys.OemMinus))
            {
                zoom -= 0.02f;
            }
            if (InputManager.IsKeyPressed(Keys.OemPlus))
            {
                zoom += 0.02f;
            }
        }
    }
}
