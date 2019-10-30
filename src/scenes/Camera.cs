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

namespace EVCMonoGame.src.scenes
{
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
    public class Camera : scenes.Updateable
    {
        protected Vector2 cameraPosition;
        protected float zoom = 1;
        protected SceneManager sceneManager;
        protected ITranslatable focusObject;
        protected Screenpoint focusPoint = Screenpoint.CENTER;
        protected Vector2 offset = Vector2.Zero;

        public Camera(SceneManager manager, ITranslatable focusObject)
        {
            sceneManager = manager;
            this.focusObject = focusObject;
            Viewport viewport = sceneManager.GraphicsDevice.Viewport;
            cameraPosition = focusObject.Position + new Vector2(viewport.Width * 0.5f, viewport.Height * 0.5f);

        }
        public Camera(SceneManager manager, ITranslatable focusObject, Screenpoint screenpoint)
        {
            sceneManager = manager;
            this.focusObject = focusObject;
            this.focusPoint = screenpoint;
            Viewport viewport = sceneManager.GraphicsDevice.Viewport;
            cameraPosition = focusObject.Position + new Vector2(viewport.Width * 0.5f, viewport.Height * 0.5f);
            setCameraToFocusObject(focusObject);
        }
        public Camera(SceneManager manager, ITranslatable focusObject, Screenpoint screenpoint, Vector2 offset)
        {
            this.offset = offset;
            sceneManager = manager;
            this.focusObject = focusObject;
            this.focusPoint = screenpoint;
            Viewport viewport = sceneManager.GraphicsDevice.Viewport;
            cameraPosition = focusObject.Position + new Vector2(viewport.Width * 0.5f, viewport.Height * 0.5f);
            setCameraToFocusObject(focusObject);
        }
        public Camera(SceneManager manager, Vector2 position) : this(manager, new ITranslatablePosition(position)) { }
        public Camera(SceneManager manager, Vector2 position, Screenpoint screenpoint) : this(manager, new ITranslatablePosition(position), screenpoint) { }
        public void setOffset(Vector2 offset)
        {
            this.offset = offset;
        }
        public void setCameraToPosition(Vector2 position)
        {
            setCameraToFocusObject(new ITranslatablePosition(position));
        }
        public void setCameraToFocusObject(ITranslatable focusObject, Screenpoint screenpoint)
        {
            this.focusPoint = screenpoint;
            setCameraToFocusObject(focusObject);
        }
        public void setCameraToFocusObject(ITranslatable focusObject)
        {
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
        public void setCameraToPosition(Vector2 position, Screenpoint point)
        {
            this.focusPoint = point;
            setCameraToPosition(position);
        }
        public void setZoom(float zoom)
        {
            this.zoom = zoom;
        }
        public void moveCamera(Vector2 direction, float distance, float time)
        {
            return;
        }
        public Vector2 getCameraPoint(Screenpoint point)
        {

            Viewport viewport = sceneManager.GraphicsDevice.Viewport;
            switch (focusPoint)
            {
                case Screenpoint.UP_LEFT_EDGE:
                    return (-1) * cameraPosition;
                case Screenpoint.UP_RIGHT_EDGE:
                    return (-1) * cameraPosition + new Vector2(viewport.Width, 0);
                case Screenpoint.DOWN_RIGHT_EDGE:
                    return (-1) * cameraPosition + new Vector2(viewport.Width, viewport.Height);
                case Screenpoint.DOWN_LEFT_EDGE:
                    return (-1) * cameraPosition + new Vector2(0, viewport.Height);
                case Screenpoint.UP:
                    return (-1) * cameraPosition + new Vector2(viewport.Width * 0.5f, 0);
                case Screenpoint.RIGHT:
                    return (-1) * cameraPosition + new Vector2(viewport.Width, viewport.Height * 0.5f);
                case Screenpoint.DOWN:
                    return (-1) * cameraPosition + new Vector2(viewport.Width * 0.5f, viewport.Height);
                case Screenpoint.LEFT:
                    return (-1) * cameraPosition + new Vector2(0, viewport.Height * 0.5f);
                case Screenpoint.CENTER:
                    return (-1) * cameraPosition + new Vector2(viewport.Width * 0.5f, viewport.Height * 0.5f);
                default:
                    return Vector2.Zero;
            }
        }
        public Matrix getTransformationMatrix()
        {
            return new Matrix(
                     new Vector4(zoom, 0, 0, 0),
                     new Vector4(0, zoom, 0, 0),
                     new Vector4(0, 0, 1, 0),
                     new Vector4(cameraPosition.X, cameraPosition.Y, 0, 1));
        }
        public override void Update(GameTime gameTime)
        {
            Console.WriteLine(focusObject.Position);
            setCameraToFocusObject(focusObject);
        }
    }
}
