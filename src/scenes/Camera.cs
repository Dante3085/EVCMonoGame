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
using EVCMonoGame.src.states;

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

    public class Camera : IUpdateable
    {
        #region Fields

        private Vector2 cameraPosition;
        private SceneManager sceneManager;
        private ITranslatable focusObject;
        private bool followsFocusObject = true;
        private Screenpoint focusPoint = Screenpoint.CENTER;
        private Vector2 offset = Vector2.Zero;
        private Easer moveEaser = new Easer(new Vector2(900, -200), new Vector2(900, 700), 5000, Easing.SineEaseInOut);

        private float currentYRightThumbStick = 0;
        private float rightThumbStickMaxZoomRate = 0.025f;

        private bool isMoving = false;

        ITranslatablePosition playerFocus;
        private bool followsPlayers = false;

        private const float closeZoom = 0.8f;
        private const float wideZoom = 0.5f;
        private const float veryWideZoom = 0.25f;
        private float zoom = closeZoom;
        private Easer zoomEaser;
        private float distanceBetweenPlayers = 0;
        private float previousDistanceBetweenPlayers = 0;
        private float zoomBorderCloseWide = 700;
        private float zoomBorderWideVeryWide = 1500;

        #endregion

        #region Properties

        public float ZoomBorderCloseWide
        {
            get { return zoomBorderCloseWide; }
            set { zoomBorderCloseWide = value; }
        }

        public float ZoomBorderWideVeryWide
        {
            get { return zoomBorderWideVeryWide; }
            set { zoomBorderWideVeryWide = value; }
        }

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
            set 
            { 
                zoom = value; 
            }
        }

        public float CloseZoom => closeZoom;
        public float WideZoom => wideZoom;
        public float VeryWideZoom => veryWideZoom;

        public bool DynamicZoomActivated
        {
            get; set;
        } = true;

        #endregion


        public Camera(SceneManager manager, ITranslatable focusObject, Screenpoint focusPoint = Screenpoint.CENTER)
        {
            sceneManager = manager;
            this.focusObject = focusObject;
            this.focusPoint = focusPoint;
            Viewport viewport = sceneManager.GraphicsDevice.Viewport;
            cameraPosition = focusObject.Position + new Vector2(viewport.Width * 0.5f, viewport.Height * 0.5f);
            SetCameraToFocusObject(focusObject);

            zoomEaser = new Easer(new Vector2(closeZoom, 0), new Vector2(wideZoom, 0), 1000, Easing.SineEaseInOut);
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
            followsPlayers = false;
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

        public void MoveCamera(Vector2 from, Vector2 to, int durationInMillis)
        {
            followsFocusObject = false;
            isMoving = true;

            moveEaser.From = from;
            moveEaser.To = to;
            moveEaser.DurationInMillis = durationInMillis;
            moveEaser.Start();
        }

        public void FollowPlayers()
        {
            followsPlayers = true;
            playerFocus = new ITranslatablePosition(GameplayState.PlayerOne.WorldPosition +
                (GameplayState.PlayerTwo.WorldPosition - GameplayState.PlayerOne.Sprite.WorldPosition) / 2);

            SetCameraToFocusObject(playerFocus);
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

        public void Update(GameTime gameTime)
        {
            if (followsFocusObject && !isMoving)
            {
                if (followsPlayers)
                {
                    playerFocus.Position = GameplayState.PlayerOne.WorldPosition +
                    (GameplayState.PlayerTwo.WorldPosition - GameplayState.PlayerOne.Sprite.WorldPosition) / 2;


                    previousDistanceBetweenPlayers = distanceBetweenPlayers;
                    distanceBetweenPlayers = (GameplayState.PlayerTwo.WorldPosition - 
                                                   GameplayState.PlayerOne.WorldPosition).Length();

                    if (DynamicZoomActivated)
                    {
                        // Zoom: closeZoom -> wideZoom
                        if (distanceBetweenPlayers >= zoomBorderCloseWide &&
                            previousDistanceBetweenPlayers < zoomBorderCloseWide)
                        {
                            zoomEaser.From = new Vector2(zoom, 0);
                            zoomEaser.To = new Vector2(wideZoom, 0);
                            zoomEaser.DurationInMillis = 1000;
                            zoomEaser.Start();
                        }
                        // Zoom: wideZoom -> closeZoom
                        else if (distanceBetweenPlayers <= zoomBorderCloseWide &&
                                 previousDistanceBetweenPlayers > zoomBorderCloseWide)
                        {
                            zoomEaser.From = new Vector2(zoom, 0);
                            zoomEaser.To = new Vector2(closeZoom, 0);
                            zoomEaser.DurationInMillis = 1000;
                            zoomEaser.Start();
                        }
                        // Zoom: wideZoom -> veryWideZoom
                        else if (distanceBetweenPlayers >= zoomBorderWideVeryWide &&
                                 previousDistanceBetweenPlayers < zoomBorderWideVeryWide)
                        {
                            zoomEaser.From = new Vector2(zoom, 0);
                            zoomEaser.To = new Vector2(veryWideZoom, 0);
                            zoomEaser.DurationInMillis = 1000;
                            zoomEaser.Start();
                        }
                        // veryWideZoom -> wideZoom
                        else if (distanceBetweenPlayers <= zoomBorderWideVeryWide &&
                                 previousDistanceBetweenPlayers > zoomBorderWideVeryWide)
                        {
                            zoomEaser.From = new Vector2(zoom, 0);
                            zoomEaser.To = new Vector2(wideZoom, 0);
                            zoomEaser.DurationInMillis = 1000;
                            zoomEaser.Start();
                        }

                        if (!zoomEaser.IsFinished)
                        {
                            zoomEaser.Update(gameTime);
                            zoom = zoomEaser.CurrentValue.X;
                        }
                    }
                }
                SetCameraToFocusObject(focusObject);
            }

            if (isMoving && !moveEaser.IsFinished)
            {
                moveEaser.Update(gameTime);
                SetCameraToPosition(moveEaser.CurrentValue);
            }
            else
            {
                isMoving = false;
            }

            UpdateZoomWithRightStick();
        }

        private void UpdateZoomWithRightStick()
        {
            // Zooming
            if (InputManager.HasRightGamePadStickMoved)
            {
                currentYRightThumbStick = InputManager.CurrentThumbSticks(PlayerIndex.One).Right.Y;

                if (currentYRightThumbStick > rightThumbStickMaxZoomRate)
                    currentYRightThumbStick = rightThumbStickMaxZoomRate;
                else if (currentYRightThumbStick < -rightThumbStickMaxZoomRate)
                    currentYRightThumbStick = -rightThumbStickMaxZoomRate;

                zoom += currentYRightThumbStick;
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
