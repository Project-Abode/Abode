using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PLYFiles
{

    public static float pointsSize = 1.0f;
    public static int particlesToSkip = 6;

    private static bool IsUnusedParticle(ParticleSystem.Particle p)
    {
        Vector3 pos = p.position;
        Color32 col = p.startColor;
        return pos.x == 0 && pos.y == 0 && pos.z == 0 && col.r == 0 && col.g == 0 && col.b == 0;
    }

	public static void WritePLY(string filePath, ParticleSystem.Particle[] particles)
    {
        List<string> text = new List<string>();
        text.Add("ply");
        text.Add("format ascii 1.0");
        text.Add("element vertex ");
        text.Add("property float32 x");
        text.Add("property float32 y");
        text.Add("property float32 z");
        text.Add("property uchar red");
        text.Add("property uchar green");
        text.Add("property uchar blue");
        text.Add("end_header");

        int particleCount = 0;
        int skip = 0;
        foreach (ParticleSystem.Particle p in particles)
        {
            if (!IsUnusedParticle(p))
            {
                skip++;
                if (skip == particlesToSkip)
                {
                    skip = 0;
                    particleCount++;
                    Vector3 pPos = p.position;
                    string particlePosInfo = pPos.x + " " + pPos.y + " " + pPos.z + " ";
                    Color32 pCol = p.startColor;
                    string particleColInfo = pCol.r + " " + pCol.g + " " + pCol.b + " ";
                    text.Add(particlePosInfo + particleColInfo);
                }
            }
        }

        text[2] = text[2] + particleCount;

        File.WriteAllLines(filePath, text.ToArray());
    }

    public static ParticleSystem.Particle[] ReadPLY(string filePath)
    {
        ParticleSystem.Particle[] particles;

        using (StreamReader file = new StreamReader(filePath))
        {
            file.ReadLine();
            file.ReadLine();
            string vertexInfo = file.ReadLine();
            int numParticles = int.Parse(vertexInfo.Split(' ')[2]);
            particles = new ParticleSystem.Particle[numParticles];

            string line = file.ReadLine();
            while (line != "end_header")
            {
                line = file.ReadLine();
            }
            for (int i = 0; i < numParticles; i++)
            {
                ParticleSystem.Particle particle = new ParticleSystem.Particle();
                string[] particleInfo = file.ReadLine().Split(' ');

                float pX = float.Parse(particleInfo[0]);
                float pY = float.Parse(particleInfo[1]);
                float pZ = float.Parse(particleInfo[2]);
                particle.position = new Vector3(pX, pY, pZ);
                particle.startSize = pZ * pointsSize * 0.02f;

                if (particleInfo.Length >= 6)
                {
                    byte pcR = byte.Parse(particleInfo[3]);
                    byte pcG = byte.Parse(particleInfo[4]);
                    byte pcB = byte.Parse(particleInfo[5]);
                    particle.startColor = new Color32(pcR, pcG, pcB, 255);
                }
                else
                {
                    particle.startColor = Color.white;
                }

                particles[i] = particle;
            }
        }
        return particles;
    }
}
