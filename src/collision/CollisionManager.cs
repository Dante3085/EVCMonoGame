using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C3.MonoGame;

using EVCMonoGame.src.scenes;
using EVCMonoGame.src.utility;
using EVCMonoGame.src.characters;
using EVCMonoGame.src.input;
using EVCMonoGame.src.states;

namespace EVCMonoGame.src.collision
{
    // TODO: Fix ResolveGeometryCollision()
    // TODO: Figure out how to properly convey CombatArgs to victim and handle them in a unified way
    //       so that the same attack always has the same effect.
    // TODO: How to properly remove CombatCollidables when they are dead ?
    // TODO: 

    public static class CollisionManager
    {
        public static List<Collidable> allCollisionsChannel = new List<Collidable>();
        public static List<Collidable> obstacleCollisionChannel = new List<Collidable>();
        public static List<Collidable> enemyCollisionChannel = new List<Collidable>();
        public static List<Collidable> itemCollisionChannel = new List<Collidable>();
        public static List<Collidable> playerCollisionChannel = new List<Collidable>();
        public static List<Collidable> projectileCollisionChannel = new List<Collidable>();
        public static List<CombatCollidable> combatCollisionChannel = new List<CombatCollidable>();

		public static List<CombatCollidable> combatCollidableMarkAsRemove = new List<CombatCollidable>();

        private static byte[,] navGrid;
        private static int debugGridCellSize;
        private static int removeIntervall = 0;

        private static List<Rectangle> raycasts;

        public static void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (DebugOptions.showCollision)
            {
                foreach (Collidable c in allCollisionsChannel)
                {
                    Primitives2D.DrawRectangle(spriteBatch, c.CollisionBox, Color.BlanchedAlmond, 5);
                    Primitives2D.DrawCircle(spriteBatch, c.CollisionBox.Center.ToVector2(), 5f, 10, Color.Red, 2);
                }
            }

            if (DebugOptions.showAttackBounds)
            {
                Color attackBoundColor = Color.DarkRed;
                attackBoundColor.A = 50;

                foreach (CombatCollidable c in combatCollisionChannel)
                {
                    Primitives2D.FillRectangle(spriteBatch, c.AttackBounds, attackBoundColor);
                }
            }

            if (DebugOptions.showHurtBounds)
            {
                Color hurtBoundColor = Color.Green;
                hurtBoundColor.A = 50;

                foreach (CombatCollidable c in combatCollisionChannel)
                {
                    Primitives2D.FillRectangle(spriteBatch, c.HurtBounds, hurtBoundColor);
                }
            }

            if (raycasts != null && DebugOptions.showRaycasts)
            {
                foreach (Rectangle ray in raycasts)
                    Primitives2D.DrawRectangle(spriteBatch, ray, Color.Violet, 4);
            }


            // Draw Grid
            if (navGrid != null && DebugOptions.showNavgrid)
            {
                for (var i = 0; i < navGrid.GetLength(0); i++)
                {
                    for (var j = 0; j < navGrid.GetLength(1); j++)
                    {
                        if (navGrid[i, j] == 1)
                            Primitives2D.DrawRectangle(spriteBatch, new Rectangle(i * debugGridCellSize, j * debugGridCellSize, debugGridCellSize, debugGridCellSize), Color.Red, 2);
                        //else
                        //	Primitives2D.DrawRectangle(spriteBatch, new Rectangle(i * debugGridCellSize, j * debugGridCellSize, debugGridCellSize, debugGridCellSize), Color.Green, 2);
                    }
                }
            }
        }

        public static void Update(GameTime gameTime)
        {
            //removeIntervall++;
            //if (removeIntervall > 10)
            //{
            //    removeIntervall = 0;
            //    allCollisionsChannel.RemoveAll((a) => { return a.FlaggedForRemove; });
            //    obstacleCollisionChannel.RemoveAll((a) => { return a.FlaggedForRemove; });
            //    enemyCollisionChannel.RemoveAll((a) => { return a.FlaggedForRemove; });
            //    itemCollisionChannel.RemoveAll((a) => { return a.FlaggedForRemove; });
            //    playerCollisionChannel.RemoveAll((a) => { return a.FlaggedForRemove; });
            //    combatCollisionChannel.RemoveAll((a) => { return a.FlaggedForRemove; });
            //}
        }

        public static void AddCollidables(List<Collidable> channel, bool excludeFromAllCollisonChannel = false,
                                          params Collidable[] collidables)
        {
            foreach (Collidable c in collidables)
            {
                if (!channel.Contains(c))
                    channel.Add(c);

                if (!excludeFromAllCollisonChannel && !allCollisionsChannel.Contains(c))
                    CollisionManager.allCollisionsChannel.Add(c);

                if (c is Player && !playerCollisionChannel.Contains(c))
                    playerCollisionChannel.Add((Collidable)c);
            }
        }

        public static void AddCollidable(Collidable collidable, List<Collidable> collisionChannel, bool excludeFromAllCollisonChannel = false)
        {
            if (!excludeFromAllCollisonChannel && !allCollisionsChannel.Contains(collidable))
                allCollisionsChannel.Add(collidable);

            if (!collisionChannel.Contains(collidable))
                collisionChannel.Add((Collidable)collidable);
        }

        public static void AddCombatCollidable(CombatCollidable combatCollidable)
        {
            if (!combatCollisionChannel.Contains(combatCollidable))
            {
                combatCollisionChannel.Add(combatCollidable);
            }
        }

        public static void RemoveCombatCollidable(CombatCollidable combatCollidable)
        {
            combatCollisionChannel.Remove(combatCollidable);
        }

        public static void RemoveCollidable(Collidable c, List<Collidable> collisionChannel)
        {
            CollisionManager.allCollisionsChannel.Remove(c);
            collisionChannel.Remove(c);
        }

        public static void CleanCollisonManager()
        {
            allCollisionsChannel.Clear();
            obstacleCollisionChannel.Clear();
            enemyCollisionChannel.Clear();
            playerCollisionChannel.Clear();
            itemCollisionChannel.Clear();
            combatCollisionChannel.Clear();
			combatCollidableMarkAsRemove.Clear();
            projectileCollisionChannel.Clear();

            navGrid = null;
            raycasts = null;
        }

        public static bool CheckCombatCollisions(CombatCollidable g1)
        {
            if (!g1.HasActiveAttackBounds)
                return false;

            foreach (CombatCollidable g2 in combatCollisionChannel)
            {
                if (g1 == g2 || !g2.HasActiveHurtBounds)
                    continue;

                if (g1.AttackBounds.Intersects(g2.HurtBounds) && g1.CombatArgs.targetType == g2.Combatant)
                {
                    CombatArgs combatArgs = g1.CombatArgs;
                    //combatArgs.attacker = g1;
                    combatArgs.victim = g2;
                    g1.OnCombatCollision(combatArgs);
                    g2.OnCombatCollision(combatArgs);
                    return true;
                }
            }
            return false;
        }

        public static bool IsObstacleCollision(Collidable g1)
        {
            foreach (Collidable obstacle in obstacleCollisionChannel)
            {
                if (g1 != obstacle)
                {
                    if (g1.CollisionBox.Intersects(obstacle.CollisionBox))
                        return true;
                }
            }

            return false;
        }

        public static bool IsCollisionInArea(Rectangle area, List<Collidable> collisionChannel)
        {
            foreach (Collidable obstacle in collisionChannel)
            {
                if (area.Intersects(obstacle.CollisionBox))
                    return true;
            }

            return false;
        }

        public static bool IsCollidableInRange(Collidable collidable, float range,
                                                        List<Collidable> collisionChannel)
        {
            List<Collidable> collidablesInRange = new List<Collidable>();

            foreach (Collidable c in collisionChannel)
            {
                if (collidable != c)
                {
                    float distance = Vector2.Distance(collidable.CollisionBox.Center.ToVector2(),
                                                      c.CollisionBox.Center.ToVector2());

                    if (distance < range)
                        return true;
                }
            }
            return false;
        }

        public static void ResolveGeometryCollision(Collidable g1, Collidable g2)
        {
            Vector2 g1Shift = g1.WorldPosition - g1.PreviousWorldPosition;
            Vector2 g2Shift = g2.WorldPosition - g2.PreviousWorldPosition;

            Vector2 g1CollisionPosition = g1.WorldPosition;
            Vector2 g2CollisionPosition = g2.WorldPosition;
            if (g1Shift == Vector2.Zero && g2Shift == Vector2.Zero) return;
            if (g2Shift == Vector2.Zero)
            {
                float length = 0.5f;
                Vector2 backShift = g1Shift * (-1);
                Vector2 startPosition = g1.PreviousWorldPosition;
                while (g1.CollisionBox.Intersects(g2.CollisionBox))
                {
                    backShift = Utility.ScaleVectorTo(backShift, length);
                    g1.WorldPosition = g1CollisionPosition + backShift;
                    length += 0.5f;
                }
                g1.WorldPosition += new Vector2((g1CollisionPosition - g1.WorldPosition).X, 0);
                if (g1.CollisionBox.Intersects(g2.CollisionBox)) g1.WorldPosition = g1.PreviousWorldPosition;
                g1.WorldPosition += new Vector2(0, (g1CollisionPosition - g1.WorldPosition).Y);
                if (g1.CollisionBox.Intersects(g2.CollisionBox)) g1.WorldPosition = g1.PreviousWorldPosition;
                g1.WorldPosition = startPosition;
                g1.WorldPosition = g1.PreviousWorldPosition;

            }
            else if (g1Shift == Vector2.Zero)
            {
                float length = 0.5f;
                Vector2 backShift = g2Shift * (-1);
                Vector2 startPosition = g2.PreviousWorldPosition;
                while (g1.CollisionBox.Intersects(g2.CollisionBox))
                {
                    backShift = Utility.ScaleVectorTo(backShift, length);
                    g2.WorldPosition = g2CollisionPosition + backShift;
                    length += 0.5f;
                }
                g2.WorldPosition += new Vector2((g2CollisionPosition - g2.WorldPosition).X, 0);
                if (g1.CollisionBox.Intersects(g2.CollisionBox)) g2.WorldPosition = g2.PreviousWorldPosition;
                g2.WorldPosition += new Vector2(0, (g2CollisionPosition - g2.WorldPosition).Y);
                if (g1.CollisionBox.Intersects(g2.CollisionBox)) g2.WorldPosition = g2.PreviousWorldPosition;
                g2.WorldPosition = startPosition;
                g2.WorldPosition = g2.PreviousWorldPosition;
            }
            else if (g1Shift != Vector2.Zero && g2Shift != Vector2.Zero)
            {
                Vector2 g1StartPosition = g1.PreviousWorldPosition;
                Vector2 g2StartPosition = g2.PreviousWorldPosition;
                Vector2 g1CollisionSolution;
                Vector2 g2CollisionSolution;
                float g1CollisionSolutionLength = 0;
                float g2CollisionSolutionLength = 0;

                //g1 Collision Solution
                float length = 0.5f;
                Vector2 backShift = g1Shift * (-1);
                Vector2 startPosition = g1.PreviousWorldPosition;
                while (g1.CollisionBox.Intersects(g2.CollisionBox))
                {
                    backShift = Utility.ScaleVectorTo(backShift, length);
                    g1.WorldPosition = g1CollisionPosition + backShift;
                    length += 0.5f;
                }
                g1.WorldPosition += new Vector2((g1CollisionPosition - g1.WorldPosition).X, 0);
                if (g1.CollisionBox.Intersects(g2.CollisionBox)) g1.WorldPosition = g1.PreviousWorldPosition;
                g1.WorldPosition += new Vector2(0, (g1CollisionPosition - g1.WorldPosition).Y);
                if (g1.CollisionBox.Intersects(g2.CollisionBox)) g1.WorldPosition = g1.PreviousWorldPosition;
                g1CollisionSolution = g1.WorldPosition;
                g1.WorldPosition = g1StartPosition;
                g1.WorldPosition = g1CollisionPosition;
                g1CollisionSolutionLength = (g1CollisionSolution - g1CollisionPosition).Length();

                //g2 Collision Solution
                length = 0.5f;
                backShift = g2Shift * (-1);
                while (g1.CollisionBox.Intersects(g2.CollisionBox))
                {
                    backShift = Utility.ScaleVectorTo(backShift, length);
                    g2.WorldPosition = g2CollisionPosition + backShift;
                    length += 0.5f;
                }
                g2.WorldPosition += new Vector2((g2CollisionPosition - g2.WorldPosition).X, 0);
                if (g1.CollisionBox.Intersects(g2.CollisionBox)) g2.WorldPosition = g2.PreviousWorldPosition;
                g2.WorldPosition += new Vector2(0, (g2CollisionPosition - g2.WorldPosition).Y);
                if (g1.CollisionBox.Intersects(g2.CollisionBox)) g2.WorldPosition = g2.PreviousWorldPosition;
                g2CollisionSolution = g2.WorldPosition;
                g2.WorldPosition = g2StartPosition;
                g2.WorldPosition = g2CollisionPosition;
                g2CollisionSolutionLength = (g2CollisionSolution - g2CollisionPosition).Length();

                //chosing of the correct solution
                if (g1CollisionSolutionLength < g2CollisionSolutionLength)
                {
                    g1.WorldPosition = g1StartPosition;
                    g1.WorldPosition = g1CollisionSolution;
                }
                else
                {
                    g2.WorldPosition = g2StartPosition;
                    g2.WorldPosition = g2CollisionSolution;
                }
            }
        }

        public static List<Collidable> GetCollidablesOnCollision(Collidable c1)
        {
            List<Collidable> intersectingCollidables = new List<Collidable>();

            foreach (Collidable c2 in allCollisionsChannel)
            {
                if (c1 != c2)
                {
                    if (c1.CollisionBox.Intersects(c2.CollisionBox))
                    {
                        intersectingCollidables.Add(c2);
                    }
                }
            }


            return intersectingCollidables;
        }

        public static List<Collidable> GetAllCollidablesInArea(Rectangle area,
                                                                  List<Collidable> collisionChannel)
        {

            List<Collidable> collidableList = new List<Collidable>();
            foreach (Collidable collidable in collisionChannel)
            {
                if (collidable.CollisionBox.Intersects(area))
                    collidableList.Add(collidable);
            }
            return collidableList;
        }

        public static List<Collidable> GetAllCollidablesByPosition(Vector2 WorldPosition, Vector2 size,
                                                                  List<Collidable> collisionChannel)
        {
            return GetAllCollidablesInArea(new Rectangle(WorldPosition.ToPoint(), size.ToPoint()),
                                               collisionChannel);
        }

        public static List<Collidable> GetAllCollidablesInRange(Collidable collidable, float range,
                                                                List<Collidable> collisionChannel)
        {
            List<Collidable> collidablesInRange = new List<Collidable>();

            foreach (Collidable c in collisionChannel)
            {
                if(collidable != c)
                {
                    float distance = Vector2.Distance(collidable.CollisionBox.Center.ToVector2(),
                                                      c.CollisionBox.Center.ToVector2());

                    if (distance < range)
                        collidablesInRange.Add(c);
                }
            }
            return collidablesInRange;
        }

        public static bool IsPlayerInArea(PlayerIndex playerIndex, Rectangle bounds)
        {
            //foreach (Player player in playerCollisionChannel)
            //{
            //    if (bounds.Intersects(player.CollisionBox))
            //    {
            //        return true;
            //    }
            //}
            //return false;

            if (playerIndex == PlayerIndex.One)
            {
                return bounds.Intersects(GameplayState.PlayerOne.CollisionBox);
            }
            else
            {
                return bounds.Intersects(GameplayState.PlayerTwo.CollisionBox);
            }
        }

        public static bool IsPlayerInRange(Collidable collidable, float range)
        {
            foreach (Player player in playerCollisionChannel)
            {
                float distance = Vector2.Distance(collidable.CollisionBox.Center.ToVector2(),
                                                  player.CollisionBox.Center.ToVector2());

                if (distance >= range)
                    return false;
            }
            return true;
        }

        public static List<Player> GetAllPlayersInArea(Rectangle bounds)
        {
            List<Player> intersectingPlayers = new List<Player>();

            foreach (Player player in playerCollisionChannel)
            {
                if (bounds.Intersects(player.CollisionBox))
                {
                    intersectingPlayers.Add(player);
                }
            }

            return intersectingPlayers;
        }

        public static List<Player> GetAllPlayersInRange(Collidable collidable, float range)
        {
            List<Player> playersInRange = new List<Player>();

            foreach (Player player in playerCollisionChannel)
            {
                float distance = Vector2.Distance(collidable.CollisionBox.Center.ToVector2(),
                                                  player.CollisionBox.Center.ToVector2());

                if (distance < range)
                    playersInRange.Add(player);
            }
            return playersInRange;
        }

        public static Player GetNearestPlayerInRange(Collidable collidable, float range)
        {
            Player nearestPlayer = null;

            foreach (Player player in playerCollisionChannel)
            {
                float distance = Vector2.Distance(collidable.CollisionBox.Center.ToVector2(),
                                                  player.CollisionBox.Center.ToVector2());

                if (distance < range)
                    if (nearestPlayer == null || distance < Vector2.Distance(collidable.CollisionBox.Center.ToVector2(), nearestPlayer.CollisionBox.Center.ToVector2()))
                    {
                        nearestPlayer = player;
                    }
            }
            return nearestPlayer;
        }


        //public static bool IsCollisionAfterMove(Collidable g1, bool fixCollision)
        //{
        //    bool isCollision = false;

        //    foreach (Collidable g2 in allCollisionsChannel)
        //    {
        //        if (g1 != g2)
        //        {
        //            if (g1.CollisionBox.Intersects(g2.CollisionBox))
        //            {
        //                isCollision = true;

        //                if (fixCollision && IsObstacleCollision(g1))
        //                {
        //                    ResolveGeometryCollision(g1, g2);
        //                }
        //            }
        //        }
        //    }
        //    return isCollision;
        //}

        public static bool IsCollisionInChannel(Collidable g1, List<Collidable> channel)
        {
            return channel.Any((g2) => { return g1.CollisionBox.Intersects(g2.CollisionBox) && g1 != g2; });
        }

        public static bool IsCollisionWithWall(Collidable g1)
        {
            return IsCollisionInChannel(g1,
                obstacleCollisionChannel.Except(playerCollisionChannel).ToList().Except(enemyCollisionChannel).ToList());
        }



        /// <summary>
        /// returns the first Wall in obstacleChannel with which g1 is colliding, 
        /// returns rectangle.empty if g1 doesn't collides with any wall
        /// </summary>
        public static Rectangle GetCollidingWall(Collidable g1)
        {
            if (IsCollisionWithWall(g1))
            {
                return obstacleCollisionChannel.Except(playerCollisionChannel).ToList().Except(enemyCollisionChannel).ToList().Find((g2) =>
                {
                    return g1.CollisionBox.Intersects(g2.CollisionBox)&&
                           g1 != g2;
                }).CollisionBox;
            }
            else
            {
                return Rectangle.Empty;
            }
        }

        public static List<Collidable> GetAllCollidingWalls(Collidable g1)
        {
            if (IsCollisionWithWall(g1))
            {
                return obstacleCollisionChannel.Except(playerCollisionChannel).ToList().Except(enemyCollisionChannel).ToList();
            }
            else
            {
                return new List<Collidable>();
            }
        }


        public static void ResolveCollisionWithWall(Collidable g1)
        {
            Vector2 startPosition = g1.PreviousWorldPosition;
            Vector2 endPosition = g1.WorldPosition;
            Vector2 backShift = Utility.ScaleVectorTo((g1.WorldPosition - g1.PreviousWorldPosition), 1) * (-1);
            while (IsCollisionWithWall(g1))
            {
                g1.WorldPosition += backShift;
                endPosition = g1.WorldPosition;
                Console.WriteLine("Position missile" + g1.WorldPosition);
            }
            g1.WorldPosition = startPosition;
            g1.WorldPosition = endPosition;

        }

        public static bool IsCollisionAfterMove(Collidable g1, bool fixMyCollision, bool resolveCollisionWithSliding)
        {
            bool isCollision = false;
            foreach (Collidable g2 in allCollisionsChannel)
            {
                if (g1 != g2)
                {
                    if (g1.CollisionBox.Intersects(g2.CollisionBox))
                    {
                        isCollision = true;

                        if (fixMyCollision && IsObstacleCollision(g1))
                        {

                            // Zurück gelegte Distanz aka auch Richtung
                            Vector2 direction = g1.WorldPosition - g1.PreviousWorldPosition;

                            // Only Integers
                            direction.X = (int)Math.Round(direction.X);
                            direction.Y = (int)Math.Round(direction.Y);

                            Vector2 tempDirection = direction;


                            // Interpoliere Bewegung soweit raus, bis keine Collision mehr entsteht (Diagonale Achsenkollision)
                            while (IsObstacleCollision(g1))
                            {
                                // Math.Sign(0) == 0
                                tempDirection.X = tempDirection.X - Math.Sign(tempDirection.X);
                                tempDirection.Y = tempDirection.Y - Math.Sign(tempDirection.Y);

                                if (tempDirection == Vector2.Zero)
                                    break;

                                // g1.CollisionBox = new Rectangle((g1.PreviousWorldPosition + tempDirection).ToPoint(), g1.CollisionBox.Size);
                                g1.WorldPosition = g1.PreviousWorldPosition + tempDirection;
                            }

                            // Fixed Position
                            Vector2 bumpedPosition = g1.WorldPosition = g1.PreviousWorldPosition + tempDirection;
                            tempDirection = direction;

                            //Slide entlang der Wand und überprüfe auf erneuter Collision
                            if (resolveCollisionWithSliding)
                            {
                                // Teste X Achse
                                while (tempDirection.X != 0)
                                {

                                    tempDirection.X = tempDirection.X - Math.Sign(tempDirection.X);
                                    // g1.CollisionBox = new Rectangle((bumpedPosition + new Vector2(tempDirection.X, 0)).ToPoint(), g1.CollisionBox.Size);
                                    g1.WorldPosition = bumpedPosition + new Vector2(tempDirection.X, 0);

                                    if (!IsObstacleCollision(g1))
                                    {
                                        g1.WorldPosition = bumpedPosition + new Vector2(tempDirection.X, 0);
                                        bumpedPosition = g1.WorldPosition;
                                        tempDirection = direction;
                                        break;
                                    }

                                }

                                // Teste Y Achse
                                while (tempDirection.Y != 0)
                                {

                                    tempDirection.Y = tempDirection.Y - Math.Sign(tempDirection.Y);
                                    // g1.CollisionBox = new Rectangle((bumpedPosition + new Vector2(0, tempDirection.Y)).ToPoint(), g1.CollisionBox.Size);
                                    g1.WorldPosition = bumpedPosition + new Vector2(0, tempDirection.Y);

                                    if (!IsObstacleCollision(g1))
                                    {
                                        g1.WorldPosition = bumpedPosition + new Vector2(0, tempDirection.Y);
                                        bumpedPosition = g1.WorldPosition;
                                        break;
                                    }

                                }
                            }
                        }
                    }
                }


            }

            return isCollision;
        }



        ///	<summary>
        ///	Generiert ein 2D-Grid worin sämtliche Kollisionen auftreten
        ///	</summary>
        ///	<param name="levelWidth">Höhe des Grids</param>
        /// <param name="levelHeight">Breite des Grids</param>
        /// <param name="gridCellSize">Agent navigations breite. Bestimmt die größe der einzelnen Gridzellen.</param>
        public static byte[,] GenerateLevelNavGrid(int levelWidth, int levelHeight, int agentMindestBreite = 10)
        {
            debugGridCellSize = agentMindestBreite;

            navGrid = new byte[levelWidth, levelHeight];

            foreach (Collidable gc in obstacleCollisionChannel)
            {
                if (!playerCollisionChannel.Contains(gc) && !enemyCollisionChannel.Contains(gc))
                {
                    Point startPos = new Point((int)(gc.WorldPosition.X / agentMindestBreite), (int)(gc.WorldPosition.Y / agentMindestBreite)); // if in bounce fehlt

                    // X Achse Displacement
                    int xDisplacement = 0;

                    float xCollisionPositionOffset = gc.WorldPosition.X - agentMindestBreite * startPos.X;
                    float collisionGridWidth = agentMindestBreite;

                    if (gc.CollisionBox.Width % agentMindestBreite > 0)
                        collisionGridWidth = (gc.CollisionBox.Width / agentMindestBreite) * agentMindestBreite;

                    else if (xCollisionPositionOffset + gc.CollisionBox.Width > agentMindestBreite)
                        xDisplacement = 1;

                    if (xCollisionPositionOffset + gc.CollisionBox.Width - collisionGridWidth > agentMindestBreite)
                    {
                        xDisplacement = 1;
                    }

                    // Y Achse Displacement
                    int yDisplacement = 0;

                    float yCollisionPositionOffset = gc.WorldPosition.Y - agentMindestBreite * startPos.Y;
                    float collisionGridHeight = agentMindestBreite;

                    if (gc.CollisionBox.Height % agentMindestBreite > 0)
                        collisionGridHeight = (gc.CollisionBox.Height / agentMindestBreite) * agentMindestBreite;

                    else if (yCollisionPositionOffset + gc.CollisionBox.Height > agentMindestBreite)
                        yDisplacement = 1;

                    if (yCollisionPositionOffset + gc.CollisionBox.Height - collisionGridHeight > agentMindestBreite)
                    {
                        yDisplacement = 1;
                    }

                    for (int i = 0; i < Math.Ceiling((decimal)gc.CollisionBox.Width / agentMindestBreite) + xDisplacement; i++)
                    {
                        for (int j = 0; j < Math.Ceiling((decimal)gc.CollisionBox.Height / agentMindestBreite) + yDisplacement; j++)
                        {
                            navGrid[startPos.X + i, startPos.Y + j] = 1;
                        }
                    }
                }


            }

            return navGrid;
        }

        ///	<summary>
        ///	Generiert ein 2D-Grid innerhalb einer Bounds worin sämtliche Kollisionen auftreten
        ///	</summary>
        ///	<param name="gridBounds">Bounds in der das Grid augezogen und in Gridzellen unterteilt wird. WorldPosition wird beachtet!</param>
        /// <param name="gridCellSize">Bestimmt die Breite der Zellen in dem die GridBounds unterteilt wird.</param>
        public static byte[,] GenerateNavGrid(Rectangle gridBounds, int gridCellSize = 10)
        {
            debugGridCellSize = gridCellSize;

            // Grid aufgeteilt in Zellen
            navGrid = new byte[gridBounds.Width / gridCellSize, gridBounds.Height / gridCellSize];



            List<Collidable> obstacles = GetAllCollidablesByPosition(gridBounds.Location.ToVector2(), gridBounds.Size.ToVector2(), obstacleCollisionChannel);

            foreach (Collidable gc in obstacles)
            {
                if (!playerCollisionChannel.Contains(gc) && !enemyCollisionChannel.Contains(gc))
                {



                    Point startPos = new Point((int)(gc.WorldPosition.X / gridCellSize), (int)(gc.WorldPosition.Y / gridCellSize)); // if in bounce fehlt

                    int displacement = 0;

                    float collisionPositionOffset = gc.WorldPosition.X - gridCellSize * startPos.X;
                    float collisionGridWidth = gridCellSize;

                    if (gc.CollisionBox.Width % gridCellSize > 0)
                        collisionGridWidth = (gc.CollisionBox.Width / gridCellSize) * gridCellSize;

                    if (collisionPositionOffset + gc.CollisionBox.Width - collisionGridWidth > gridCellSize)
                    {
                        displacement = 1;
                    }
                    //todo für Y

                    for (int i = 0; i < Math.Ceiling((decimal)gc.CollisionBox.Width / gridCellSize) + displacement; i++)
                    {
                        for (int j = 0; j < Math.Ceiling((decimal)gc.CollisionBox.Height / gridCellSize); j++)
                        {
                            navGrid[startPos.X + i, startPos.Y + j] = 1;
                        }
                    }
                }


            }

            return navGrid;
        }

        // Zieht eine Box Zwischen 2 Collidables auf und meldet ob eine Kollision mit Obstakles stattgefunden hat.
        public static bool IsBlockedRaycast(Collidable fromCollidable, Collidable toCollidable, List<Collidable> collisionChannel)
        {

            raycasts = new List<Rectangle>();

            int posX = fromCollidable.CollisionBox.X;
            int posY = fromCollidable.CollisionBox.Y;

            int agentWidth = fromCollidable.CollisionBox.Width;


            Vector2 distance = toCollidable.CollisionBox.Center.ToVector2() - fromCollidable.CollisionBox.Center.ToVector2();
            Vector2 unitDirection = distance;
            unitDirection.Normalize();

            int iterations = (int)distance.Length() / agentWidth;



            for (int i = 0; i < iterations; i++)
            {
                raycasts.Add(new Rectangle((int)(unitDirection.X * 5) + posX + (int)(distance.X / iterations) * i, (int)(unitDirection.Y * 5) + posY + (int)(distance.Y / iterations) * i, fromCollidable.CollisionBox.Width, fromCollidable.CollisionBox.Height));
            }

            foreach (Rectangle ray in raycasts)
            {
                foreach (Collidable obstacle in obstacleCollisionChannel.Except<Collidable>(enemyCollisionChannel))
                {
                    if (fromCollidable != obstacle && toCollidable != obstacle)
                    {
                        if (ray.Intersects(obstacle.CollisionBox))
                            return true;
                    }
                }
            }

            return false;
        }

    } // End of Class
}
