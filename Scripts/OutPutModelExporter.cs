
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class OutPutModelExporter : MonoBehaviour
{
    [SerializeField] private Transform model;
    string filePath;
    // Start is called before the first frame update
    void Start()
    {
        filePath = Application.streamingAssetsPath + "/";

        modelToEnvFile();
        envToModel();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    const int cubemapTextureCount = 6;
    private void modelToEnvFile() {
        Transform camGroup = model.GetChild(0);
        int camGroupChildCount = camGroup.childCount;

        byte[] positions = new byte[camGroupChildCount * 12];
        byte[] rotations = new byte[camGroupChildCount * 12];

        int panoramaIter = camGroupChildCount * 12;
        int panoramaIndex = 0;
        for(int i = 0; i < panoramaIter; i = i + 12)
        { 
            panoramaIndex = i/12;
            byte[] bytes = BitConverter.GetBytes(camGroup.GetChild(panoramaIndex).position.x);
            positions[i] = bytes[0];
            positions[i + 1] = bytes[1];
            positions[i + 2] = bytes[2];
            positions[i + 3] = bytes[3];
            bytes = BitConverter.GetBytes(camGroup.GetChild(panoramaIndex).position.y);
            positions[i + 4] = bytes[0];
            positions[i + 5] = bytes[1];
            positions[i + 6] = bytes[2];
            positions[i + 7] = bytes[3];
            bytes = BitConverter.GetBytes(camGroup.GetChild(panoramaIndex).position.z);
            positions[i + 8] = bytes[0];
            positions[i + 9] = bytes[1];
            positions[i + 10] = bytes[2];
            positions[i + 11] = bytes[3];

            bytes = BitConverter.GetBytes(camGroup.GetChild(panoramaIndex).localEulerAngles.x);
            rotations[i] = bytes[0];
            rotations[i + 1] = bytes[1];
            rotations[i + 2] = bytes[2];
            rotations[i + 3] = bytes[3];
            bytes = BitConverter.GetBytes(camGroup.GetChild(panoramaIndex).localEulerAngles.y);
            rotations[i + 4] = bytes[0];
            rotations[i + 5] = bytes[1];
            rotations[i + 6] = bytes[2];
            rotations[i + 7] = bytes[3];
            bytes = BitConverter.GetBytes(camGroup.GetChild(panoramaIndex).localEulerAngles.z);
            rotations[i + 8] = bytes[0];
            rotations[i + 9] = bytes[1];
            rotations[i + 10] = bytes[2];
            rotations[i + 11] = bytes[3];
        }

        int textureCount = camGroupChildCount * cubemapTextureCount;
        List<byte[]> textures = new List<byte[]>();

        List<byte[]> textureByteLengths = new List<byte[]>();

        for (int i = 0; i < textureCount; ++i)
        {
            string path = filePath + (i / 6).ToString() + "_" + (i % 6).ToString() + ".jpg";
         
            if (File.Exists(path))
            {
                textures.Add((File.ReadAllBytes(path)));

                textureByteLengths.Add((BitConverter.GetBytes(textures[i].Length)));
            }
            else
            {
            }
        }
        int meshCount = model.childCount - 1;

        List<byte[]> meshesVertices = new List<byte[]>(meshCount);
        List<byte[]> meshesIndicies = new List<byte[]>(meshCount);

        for(int i = 1; i < meshCount + 1; ++i)
        {
            Mesh mesh = model.GetChild(i).GetComponent<MeshFilter>().mesh;
            
            Vector3[] vertices = mesh.vertices;
            int[] indicies = mesh.triangles;

            int verticeCount = vertices.Length;
            int indiciesCount = indicies.Length;

            byte[] verticesBytes = new byte[verticeCount * 12];
            byte[] indiciesBytes = new byte[indiciesCount * 4];

            for(int j = 0; j < verticeCount * 12; j = j + 12)
            {
                int index = j / 12;

                byte[] bytes = BitConverter.GetBytes(vertices[index].x);
                verticesBytes[j] = bytes[0];
                verticesBytes[j + 1] = bytes[1];
                verticesBytes[j + 2] = bytes[2];
                verticesBytes[j + 3] = bytes[3];    
                bytes = BitConverter.GetBytes(vertices[index].y);
                verticesBytes[j + 4] = bytes[0];
                verticesBytes[j + 5] = bytes[1];
                verticesBytes[j + 6] = bytes[2];
                verticesBytes[j + 7] = bytes[3];
                bytes = BitConverter.GetBytes(vertices[index].z);
                verticesBytes[j + 8] = bytes[0];
                verticesBytes[j + 9] = bytes[1];
                verticesBytes[j + 10] = bytes[2];
                verticesBytes[j + 11] = bytes[3];

            }

            for (int j = 0; j < indiciesCount * 4; j = j + 4)
            {
                int index = j / 4;

                byte[] bytes = BitConverter.GetBytes(indicies[index]);
                indiciesBytes[j] = bytes[0];
                indiciesBytes[j + 1] = bytes[1];
                indiciesBytes[j + 2] = bytes[2];
                indiciesBytes[j + 3] = bytes[3];
            }

            meshesVertices.Add(verticesBytes);
            meshesIndicies.Add(indiciesBytes);
        }

        byte dataByteLength;
        byte panoramaNumber;
        byte meshNumber;
        byte textureNumber;
        
        List<byte> verticesByteLengths = new List<byte>();
        List<byte> indiciesByteLengths = new List<byte>(); 
   
        panoramaNumber = ((byte)camGroupChildCount);
        meshNumber = ((byte)meshCount);
        textureNumber = ((byte)textureCount);

        foreach (byte[] vertices in meshesVertices)
        {
            byte[] verticesBytes = BitConverter.GetBytes(vertices.Length);
          
            foreach(byte mbyte in verticesBytes)
            {
                verticesByteLengths.Add(mbyte);
            }
        }

        foreach (byte[] indicies in meshesIndicies)
        {
            byte[] indicesBytes = BitConverter.GetBytes(indicies.Length);

            foreach (byte mbyte in indicesBytes)
            {
                indiciesByteLengths.Add(mbyte);
            }
        }

        dataByteLength = ((byte)(4 + verticesByteLengths.Count + indiciesByteLengths.Count + textureByteLengths.Count));

        byte[] infoHead = new byte[4];
        infoHead[0] = dataByteLength;
        infoHead[1] = panoramaNumber;
        infoHead[2] = meshNumber;
        infoHead[3] = textureNumber;

        var list = new List<byte>();
        list.AddRange(infoHead);
        list.AddRange(verticesByteLengths);
        list.AddRange(indiciesByteLengths);

        foreach (byte[] length in textureByteLengths)
        {
            list.AddRange(length);
        }
     
        list.AddRange(positions);
        list.AddRange(rotations);

        foreach (byte[] texture in textures){
            list.AddRange(texture);
        }

        foreach (byte[] vertice in meshesVertices)
        {
            list.AddRange(vertice);
        }

        foreach (byte[] indice in meshesIndicies)
        {
            list.AddRange(indice);
        }

        byte[] output = list.ToArray();
        File.WriteAllBytes(Application.dataPath + "/output.env" , output);
    }

    //public List<Texture2D> debugtextures;
    private void envToModel()
    {
        string path = Application.dataPath + "/output.env";

        byte[] content = File.ReadAllBytes(path);

        int index = 4; 
        int[] verticeLengths = new int[content[2]];
        int iterCount = content[2] * 4;
        for (int i = 0; i < iterCount; i = i + 4)
        {
            verticeLengths[i/4] = BitConverter.ToInt32(content, index + i);
        }

        index += iterCount; 
        int[] indiciesLengths = new int[content[2]];
        iterCount = content[2] * 4;
    
        for (int i = 0; i < iterCount; i = i + 4)
        {
            indiciesLengths[i/4] = BitConverter.ToInt32(content, index + i); 
        }

        index += iterCount; 
        int[] textureLengths = new int[content[3]];
        iterCount = content[3] * 4;
        for(int i = 0; i < iterCount; i = i + 4)
        {
            textureLengths[i / 4] = BitConverter.ToInt32(content, index + i); 
        }

        index += iterCount;
        Vector3[] panoramaPosition = new Vector3[content[1]];
        Vector3[] panoramaRotation = new Vector3[content[1]];
        iterCount = content[1] * 12;
        for (int i = 0; i < iterCount; i = i + 12)
        {
            panoramaPosition[i/12] = new Vector3(BitConverter.ToSingle(content, index + i), BitConverter.ToSingle(content, index + i + 4), BitConverter.ToSingle(content, index + i + 8));
        }

        index += iterCount;
        iterCount = content[1] * 12;
        for (int i = 0; i < iterCount; i = i + 12)
        {
            panoramaRotation[i / 12] = new Vector3(BitConverter.ToSingle(content, index + i), BitConverter.ToSingle(content, index + i + 4), BitConverter.ToSingle(content, index + i + 8));
        }

        List<byte[]> textures = new List<byte[]>();
        List<byte[]> indiciesList = new List<byte[]>();
        List<byte[]> verticiesList = new List<byte[]>();

        index += iterCount;

        foreach (int length in textureLengths)
        {
            byte[] bytes = new byte[length];
            Array.Copy(content, index, bytes, 0, length);
            index += length;

            textures.Add(bytes);
        }

        foreach (int length in verticeLengths)
        {
            byte[] bytes = new byte[length];
            Array.Copy(content, index, bytes, 0, length);
            index += length;

            verticiesList.Add(bytes);
        }

        foreach (int length in indiciesLengths)
        {
            byte[] bytes = new byte[length];
            Array.Copy(content, index, bytes, 0, length);
            index += length;

            indiciesList.Add(bytes);
        }

        List<Vector3[]> verticies = new List<Vector3[]>();
        List<int[]> indicies = new List<int[]>();

 
        foreach (byte[] verticesBytes in verticiesList)
        {   
            int byteLength = verticesBytes.Length;

            Vector3[] localVertices = new Vector3[byteLength/12];
            for (int i = 0; i < byteLength; i = i + 12)
            {
                localVertices[i/12] = new Vector3(BitConverter.ToSingle(verticesBytes, i), BitConverter.ToSingle(verticesBytes, i + 4), BitConverter.ToSingle(verticesBytes, i + 8));   
            }
            verticies.Add(localVertices);
          
        }

        foreach (byte[] indiciesBytes in indiciesList)
        {
            int byteLength = indiciesBytes.Length;
            int[] localIndicies = new int[byteLength/4];
            for (int i = 0; i < byteLength; i = i + 4)
            {
                localIndicies[i/4] = BitConverter.ToInt32(indiciesBytes,i);
            }
            indicies.Add(localIndicies);
        }

        for(int i = 0; i < content[2]; ++i)
        {
            GameObject o =  new GameObject();
            Mesh mesh = new Mesh();
            mesh.vertices = verticies[i];
            mesh.triangles = indicies[i];
            o.AddComponent<MeshFilter>();
            o.GetComponent<MeshFilter>().mesh = mesh;
        }
    } 
}
