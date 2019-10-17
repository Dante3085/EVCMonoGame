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

namespace EVCMonoGame.src.scenes
{
    public abstract class Scene
    {
        protected float cameraPositionX = 0;
        protected float cameraPositionY = 0;
        protected float Zoom = 1;
        protected List<Updateable> updateables;
        protected List<IDrawable> drawables;
        protected SceneManager sceneManager;

        public Scene(SceneManager screenManager)
        {
            this.sceneManager = screenManager;
            updateables = new List<Updateable>();
            drawables = new List<IDrawable>();
        }

        public virtual void LoadContent(ContentManager contentManager)
        {
            foreach (IDrawable d in drawables)
            {
                d.LoadContent(contentManager);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (Updateable u in updateables)
            {
                u.Update(gameTime);
            }
            if (InputManager.IsKeyPressed(Keys.OemPlus)) Zoom += 0.1f;
            if (InputManager.IsKeyPressed(Keys.OemMinus)) Zoom -= 0.1f;
            if (InputManager.IsKeyPressed(Keys.Q)) cameraPositionX += 2f; //Kameraschwenk links
            if (InputManager.IsKeyPressed(Keys.E)) cameraPositionX -= 2f; //Kameraschwenk rechts
            if (InputManager.IsKeyPressed(Keys.R)) cameraPositionY += 2f; //Kameraschwenk oben
            if (InputManager.IsKeyPressed(Keys.F)) cameraPositionY -= 2f; //Kameraschwenk unten
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            Matrix transformMatrix = new Matrix(new Vector4(Zoom,            0,               0,    0), 
                                  new Vector4(0,               Zoom,            0,    0), 
                                  new Vector4(0,               0,               1,    0), 
                                  new Vector4(cameraPositionX, cameraPositionY, 0,    1));

            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: transformMatrix);
            foreach (IDrawable d in drawables)
            {
                d.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }
    }
}
