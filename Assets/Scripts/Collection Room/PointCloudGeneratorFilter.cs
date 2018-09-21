using System;
using UnityEngine;
using Intel.RealSense;

public class PointCloudGeneratorFilter : MonoBehaviour
{

    private const string PLY_SAVE_PATH = "Assets/Resources/PointClouds/";

    public bool mirrored;
    public float pointsSize = 1;
    public int skipParticles = 2;
    public float depthFilter = 1;
    public ParticleSystem pointCloudParticles;
    public GameObject pointCloudCopy;
    public GameObject snapshotDisplay;

    private ParticleSystem.Particle[] particles = new ParticleSystem.Particle[0];
    private PointCloud pc = new PointCloud();
    private Points.Vertex[] vertices;
    private byte[] lastColorImage;
    private Align aligner;
    
    private void Start()
    {
        aligner = new Align(Intel.RealSense.Stream.Color);
        RealSenseDevice.Instance.onNewSampleSet += OnFrames;

    }
    
    private void OnFrames(FrameSet frames)
    {
        using (var aligned = aligner.Process(frames))
        {
            using (var colorFrame = aligned.ColorFrame)
            using (var depthFrame = aligned.DepthFrame)
            {
                if (depthFrame == null)
                {
                    Debug.Log("No depth frame in frameset, can't create point cloud");
                    return;
                }

                if (!UpdateParticleParams(depthFrame.Width, depthFrame.Height))
                {
                    Debug.Log("Unable to create point cloud");
                    return;
                }

                using (var points = pc.Calculate(depthFrame))
                {
                    SetParticles(points, colorFrame);
                }
            }
        }
    }

    private void SetParticles(Points points, VideoFrame colorFrame)
    {
        if (points == null)
            throw new Exception("Frame in queue is not a points frame");

        if (lastColorImage == null)
        {
            int colorFrameSize = colorFrame.Height * colorFrame.Stride;
            lastColorImage = new byte[colorFrameSize];
        }
        colorFrame.CopyTo(lastColorImage);

        vertices = vertices ?? new Points.Vertex[points.Count];
        points.CopyTo(vertices);

        Debug.Assert(vertices.Length == particles.Length);
        int mirror = mirrored ? -1 : 1;
        for (int index = 0; index < vertices.Length; index += skipParticles)
        {
            var v = vertices[index];
            if (v.z > 0 && v.z < depthFilter)
            {
                particles[index].position = new Vector3(v.x * mirror, v.y, v.z);
                particles[index].startSize = v.z * pointsSize * 0.02f;
                particles[index].startColor = new Color32(lastColorImage[index * 3], lastColorImage[index * 3 + 1], lastColorImage[index * 3 + 2], 255);
            }
            else //Required since we reuse the array
            {
                particles[index].position = Vector3.zero;
                particles[index].startSize = 0;
                particles[index].startColor = Color.clear;
            }
        }
    }

    private bool UpdateParticleParams(int width, int height)
    {
        var numParticles = (width * height);
        if (particles.Length != numParticles)
        {
            particles = new ParticleSystem.Particle[numParticles];
        }

        return true;
    }

    private void Update()
    {
        //ParticleSystem.Particle[] filteredParticles = particles.Where(p => !PLYFiles.IsUnusedParticle(p)).ToArray();
        //pointCloudParticles.SetParticles(filteredParticles, filteredParticles.Length);
        pointCloudParticles.SetParticles(particles, particles.Length);

        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject copy = Instantiate(pointCloudCopy);
            ParticleSystem.Particle[] keepParticles = new ParticleSystem.Particle[particles.Length];
            particles.CopyTo(keepParticles, 0);
            copy.GetComponent<KeepParticles>().SetParticles(keepParticles);
        }*/
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DateTime now = DateTime.Now;
            string time = now.ToString("yyyy-MM-dd_HH.mm.ss.ffff");
            string filename = time + ".ply";
            PLYFiles.WritePLY(PLY_SAVE_PATH + filename, particles);

            ParticleSystem.Particle[] plyParticles = PLYFiles.ReadPLY(PLY_SAVE_PATH + filename);
            snapshotDisplay.GetComponent<KeepParticles>().SetParticles(plyParticles);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            string filename = "head.ply";
            PLYFiles.WritePLY(PLY_SAVE_PATH + filename, particles);

            ParticleSystem.Particle[] plyParticles = PLYFiles.ReadPLY(PLY_SAVE_PATH + filename);
            snapshotDisplay.GetComponent<KeepParticles>().SetParticles(plyParticles);
        }
        /*if (Input.GetKeyDown(KeyCode.L))
        {
            //TODO: remove
            GameObject copy = Instantiate(pointCloudCopy);
            ParticleSystem.Particle[] plyParticles = PLYFiles.ReadPLY(PLY_SAVE_PATH + "save.ply");
            copy.GetComponent<KeepParticles>().SetParticles(plyParticles);
        }*/
    }
}