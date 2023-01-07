using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CoopPuzzle
{
    public class ParticleSystem
    {
        private Random random;
        public Vector2 EmitterLocation { get; set; }
        private List<Particle> particles;
        float timeElapsed;

        public ParticleSystem(Vector2 location)
        {
            EmitterLocation = location;
            particles = new List<Particle>();
            random = new Random();
        }

        //private Particle GenerateNewParticle()
        //{
        //    Vector2 position = EmitterLocation;
        //    Vector2 velocity = new Vector2(
        //            0.2f * (float)(random.NextDouble() * 2 - 1),
        //            0.2f * (float)(random.NextDouble() * 2 - 1));
        //    float angle = 0;
        //    float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
        //    Color color = new Color(
        //            (float)random.NextDouble(),
        //            (float)random.NextDouble(),
        //            (float)random.NextDouble());
        //    int size = random.Next(3);
        //    int ttl = 200 + random.Next(40);

        //    return new Particle(position, velocity, angle, angularVelocity, color, size, ttl);
        //}

        public void Update(float dt, Vector2 playerVel, Game1 game1, Vector2 emitterPos)
        {
            Color color = Color.Black;
            if (game1.camera.BoundingRectangle.Contains(emitterPos))
             color = game1.GetColorOfPixel(emitterPos - game1.camera.Position);
            timeElapsed += dt;

            if(playerVel != Vector2.Zero && timeElapsed > 0.05f)
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector2 velocity = new Vector2((playerVel.X * (float)random.NextDouble() + (float)random.NextDouble() * random.Next(-1, 2)) / 10,
                                                    (playerVel.Y * (float)random.NextDouble() + (float)random.NextDouble() * random.Next(-1, 2)) / 10) * -5;


                    // Color variation
                    if (random.Next(2) == 1)
                    {
                        if (color.R > 230) color.R = 250;
                        else color.R += 20;
                        if (color.G > 230) color.G = 250;
                        else color.G += 20;
                        if (color.B > 230) color.B = 250;
                        else color.B += 20;
                    }
                    else
                    {
                        if (color.R < 20) color.R = 0;
                        else color.R -= 20;
                        if (color.G < 20) color.G = 0;
                        else color.G -= 20;
                        if (color.B < 20) color.B = 0;
                        else color.B -= 20;
                    }

                    if(game1.camera.BoundingRectangle.Contains(emitterPos))
                    particles.Add(new Particle(EmitterLocation, velocity, 0, 0, color, 4, 50)); //position, velocity, angle, angularVelocity, color, size, ttl
                }

                timeElapsed = 0;
            }

            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update(dt);
                if (particles[particle].TTL <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }
        public void Draw(SpriteBatch sb)
        {
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(sb);
            }
        }
    }
}
