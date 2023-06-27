namespace RitaEngine.Resources.Models.ObjLoader
{
    using System.Collections.Generic;
    /// <summary>
    /// Méthodes pour charger les fichiers Obj (un Model contiens plusieurs meshs )
    /// </summary>
    public static class ModelObj
    {
        // / <summary>
        // / Fonction unique qui permet derécuperer tous les materiaux du fichiers passé en parameteres
        // / et stock le résultat dans la liste materials
        // /  List<Material> materials = new(20);  20 nombre approximatif de materiaux que peut contenir le fichier
        // /  MCJ.Engine.Resources.ObModels.LoadMaterials(ref materials , "mon_fichier.mtl");
        /// <param name="materials"></param>
        /// <param name="filename"></param>
        public static void LoadMaterials(ref List<Material> materials, string filename)
        {
            // materiaux unique temporaire a remplir avant de l'ajouter a la liste
            var material = new Material();

            // Initialise materials si egal a null au cas ou
            if (materials.Equals(default))
                materials = new List<Material>(10);

            // lecture du fichier complet chaque lignes est dans un string[] d'ou le foreach 
            var lines = System.IO.File.ReadLines(filename);

            // lecture complete du fichier ligne par ligne 
            foreach (var line in lines)
            {
                // separer la ligne en mots 
                var words = line.Replace('\t', ' ').Split(' ', System.StringSplitOptions.RemoveEmptyEntries);

                //remove empty line
                if (words.Length == 0 || words[0] == "#")
                    continue;

                if (words[0].Equals("newmtl"))
                {
                    if (material.ka != null)
                        materials.Add(material);

                    material.Init();
                    material.Name = words[1];
                }
                if (words[0].Equals("Ns"))
                {
                    material.Ns = float.Parse(words[1], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                }
                if (words[0].Equals("Ni"))
                {
                    material.Ni = float.Parse(words[1], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                }
                if (words[0].Equals("d"))
                {
                    material.D = float.Parse(words[1], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                }
                if (words[0].Equals("Tr"))
                {
                    material.Tr = float.Parse(words[1], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                }
                if (words[0].Equals("illum"))
                {
                    material.illum = int.Parse(words[1], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                }
                if (words[0].Equals("Tf"))
                {
                    ReadMaterialK("Tf", ref words, ref material.Tf!);
                }
                if (words[0].Equals("Ka"))
                {
                    ReadMaterialK("Ka", ref words, ref material.ka!);
                }
                if (words[0].Equals("Kd"))
                {
                    ReadMaterialK("Kd", ref words, ref material.Kd!);
                }
                if (words[0].Equals("Ks"))
                {
                    ReadMaterialK("Ks", ref words, ref material.Ks!);
                }
                if (words[0].Equals("Ke"))
                {
                    ReadMaterialK("Ke", ref words, ref material.Ke!);
                }
                if (words[0].Equals("map_Ka"))
                {
                    material.Map_Ka = words[1];
                }
                if (words[0].Equals("map_Kd"))
                {
                    material.Map_Kd = words[1];
                }
                if (words[0].Equals("map_bump"))
                {
                    material.Map_bump = words[1];
                }
                if (words[0].Equals("bump"))
                {
                    material.Bump = words[1];
                }
            }
            if (material.Kd != null)
                materials.Add(material);// un seul materials dans le fichier
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="objFileName"></param>
        /// <param name="basePath"></param>
        public static void LoadModel(ref Model model, string objFileName, string basePath)
        {
            int error = 1;//pour situer l'erreur dans le obj 
            try
            {
                var contents = System.IO.File.ReadAllLines(basePath + "\\" + objFileName + ".obj");

                var linescount = contents.Length;
                if (model.Equals(default))
                    model = new();

                model.NameFileObj = objFileName;
                model.RawData = new();
                model.RawData.V = new List<float>(linescount);
                model.RawData.Vn = new List<float>(linescount);
                model.RawData.Vt = new List<float>(linescount);
                model.RawData.Groups = new(linescount/10);
                model.Materials = new(30);

                Group group = new();
                group.Faces = new(linescount/10);

                string groupName = string.Empty;
                string materialName = string.Empty;

                foreach (string line in contents)
                {
                    var words = line.Replace('\t', ' ').Split(' ', System.StringSplitOptions.RemoveEmptyEntries);

                    if (words.Length == 0 || words[0].Equals("#"))
                        continue;

                    if (words[0].Equals("g") || words[0].Equals("o"))
                    {
                        groupName = words[1];
                        if (!string.IsNullOrEmpty(materialName) && group.Faces.Count != 0)
                        {
                            group.MaterialName = materialName;
                            group.NameGroup = groupName;
                            model.RawData.Groups.Add(group);
                            group = new();
                            group.Faces = new(linescount/10);
                        }
                    }
                    if (words[0] == "f")
                    {
                        Face face = new();
                        face.Init();
                        ReadFaces(ref words, ref face);
                        face.Material = materialName;
                        face.Group = groupName;
                        group.Faces.Add(face);
                    }
                    if (words[0] == "vn")
                    {
                        ReadFloats(ref words, ref model.RawData.Vn);
                    }
                    if (words[0] == "vt")
                    {
                        ReadFloats(ref words, ref model.RawData.Vt, false);
                    }
                    if (words[0] == "v")
                    {
                        ReadFloats(ref words, ref model.RawData.V);
                    }
                    if (words[0] == "usemtl")
                    {
                        if (!string.IsNullOrEmpty(materialName) && group.Faces.Count != 0)
                        {
                            group.MaterialName = materialName;
                            group.NameGroup = groupName;
                            model.RawData.Groups.Add(group);
                            group = new();
                            group.Faces = new(linescount/10);
                        }
                        materialName = words[1];
                    }
                    if (words[0] == "mtllib")
                    {
                        model.NameFileMaterial = basePath + "\\" + words[1];
                        LoadMaterials(ref model.Materials, model.NameFileMaterial);
                    }
                    error++;
                }
                // le cas ou il y a qu'un seul mesh 
                if (!string.IsNullOrEmpty(materialName) && group.Faces.Count != 0)
                {
                    group.MaterialName = materialName;
                    group.NameGroup = groupName;
                    model.RawData.Groups.Add(group);
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message + " at line :" + error);
            }
        }

        private static void OptimiseOneMesh(ref Mesh mesh, ref List<Face> faces, ref RawData rawData)
        {
            uint[] tmpIndice = new uint[4];
            uint indice = 0;
            float v1, v2, v3;
            float t1 = 0.0f; float t2 = 0.0f;
            float n1 = 0.0f; float n2 = 0.0f; float n3 = 0.0f;
            mesh.Indices = new(faces.Count * 10);
            mesh.Vertices = new(rawData.V.Count);

            for (int index = 0; index < faces.Count; index++)
            {
                var face = faces[index];

                if (face.IndiceType == 1) mesh.Stride = 3;
                if (face.IndiceType == 3) mesh.Stride = 8;
                if (face.IndiceType == 2) mesh.Stride = 5;
                if (face.IndiceType == 4) mesh.Stride = 6;

                for (int i = 0; i < face.FaceType; i++)
                {
                    v1 = rawData.V[(((int)face.Vertex[i] - 1) * 3) + 0];
                    v2 = rawData.V[(((int)face.Vertex[i] - 1) * 3) + 1];
                    v3 = rawData.V[(((int)face.Vertex[i] - 1) * 3) + 2];
                    if (face.IndiceType == 2 || face.IndiceType == 3)
                    {
                        t1 = rawData.Vt[(((int)face.TextureUV[i] - 1) * 2) + 0];
                        t2 = rawData.Vt[(((int)face.TextureUV[i] - 1) * 2) + 1];
                    }
                    if (face.IndiceType >= 3)
                    {
                        n1 = rawData.Vn[(((int)face.Normal[i] - 1) * 3) + 0];
                        n2 = rawData.Vn[(((int)face.Normal[i] - 1) * 3) + 1];
                        n3 = rawData.Vn[(((int)face.Normal[i] - 1) * 3) + 2];
                    }

                    var result = VertexFound(ref mesh, face.IndiceType, v1, v2, v3, t1, t2, n1, n2, n3);

                    if (result == -1)
                    {
                        //Recherche si exist dans mesh? si oui store l'indice trouver
                        //sinon
                        tmpIndice[i] = indice;
                        indice++;
                        mesh.Vertices.Add(v1); mesh.Vertices.Add(v2); mesh.Vertices.Add(v3);
                        if (face.IndiceType == 2 || face.IndiceType == 3)
                        {
                            mesh.Vertices.Add(t1); mesh.Vertices.Add(t2);
                        }
                        if (face.IndiceType >= 3)
                        {
                            mesh.Vertices.Add(n1); mesh.Vertices.Add(n2); mesh.Vertices.Add(n3);
                        }
                    }
                    else
                    {
                        tmpIndice[i] = (uint)result;
                    }
                }
                //si la ligne face comporte 4 element gl ne doit avoir que des triangles
                if (face.FaceType == 4)
                {
                    mesh.Indices.Add(tmpIndice[0]); mesh.Indices.Add(tmpIndice[1]); mesh.Indices.Add(tmpIndice[3]);
                    mesh.Indices.Add(tmpIndice[1]); mesh.Indices.Add(tmpIndice[2]); mesh.Indices.Add(tmpIndice[3]);
                }
                // on ne touche pas la ligne si 3element
                if (face.FaceType == 3)
                {
                    mesh.Indices.Add(tmpIndice[0]); mesh.Indices.Add(tmpIndice[1]); mesh.Indices.Add(tmpIndice[2]);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void CreateMeshsOptimisedForOpenGL(ref Model model)
        {
            model.Meshs = new(model.RawData.Groups.Count);

            // creation des meshs simple per object 
            for (int g = 0; g < model.RawData.Groups.Count; g++)
            {
                Mesh mesh = new();
                var faces = model.RawData.Groups[g].Faces;
                OptimiseOneMesh(ref mesh, ref faces, ref model.RawData);

                mesh.TriangleToDraw = mesh.Indices.Count;
                // Found good materials for this group
                for (int m = 0; m < model.Materials.Count; m++)
                {
                    if (model.RawData.Groups[g].MaterialName.Equals(model.Materials[m].Name))
                    {
                        mesh.Material = model.Materials[m];
                        break;
                    }
                }
                model.Meshs.Add(mesh);
                System.Console.WriteLine(g + " / " + model.RawData.Groups.Count + "Grp : " + model.RawData.Groups[g].NameGroup + " Material : " + model.RawData.Groups[g].MaterialName + " Count : " + mesh.Indices.Count + " Stride : " + mesh.Stride);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static void CreateGroupedMeshsOptimisedForOpenGL(ref Model model)
        {
            model.Meshs = new(model.RawData.Groups.Count);

            // creation des meshs simple per object 
            for (int m = 0; m < model.Materials.Count; m++)
            {
                int g;
                Mesh mesh = new();

                var name = model.Materials[m].Name;

                for (g = 0; g < model.RawData.Groups.Count; g++)
                {
                    if (model.RawData.Groups[g].MaterialName.Equals(name))
                    {
                        var faces = model.RawData.Groups[g].Faces;
                        OptimiseOneMesh(ref mesh, ref faces, ref model.RawData);
                    }
                }
                mesh.TriangleToDraw = mesh.Indices!.Count;
                mesh.Material = model.Materials[m];
                model.Meshs.Add(mesh);
                int numero = m+1;
                System.Console.WriteLine(numero.ToString() + " / " + model.Materials.Count + " Material : " + mesh.Material.Name + " Count : " + mesh.Indices.Count + " Stride : " + mesh.Stride);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="materials"></param>
        public static void OptimiseMaterialsByTextureName(ref List<Material> materials)
        {
            _ = materials;
            // construct texture atlas with name 
            // une fois les vertex optimiser par faces ( ou objet avec même materials ?)
            // redo Vertex avec UV => u + offsetTextureatlasU et V + offesetTextureAtlasV 
            // ici faire une liste de toutes les textures ????????? 
            //orderd list by textures ( in map_kd ( diffuse is principal ))
        }

        private static void ReadMaterialK(string value, ref string[] words, ref float[] tableau)
        {
            if (words.Length != 4)
                System.Console.WriteLine("Erreur in line " + value + ":");

            tableau[0] = float.Parse(words[1], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            tableau[1] = float.Parse(words[2], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            tableau[2] = float.Parse(words[3], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        }

        private static void ReadFloats(ref string[] words, ref List<float> list, bool limit = true)
        {
            if (words.Length >= 2 && !string.IsNullOrEmpty(words[1]))
                list.Add(float.Parse(words[1], System.Globalization.CultureInfo.InvariantCulture.NumberFormat));
            if (words.Length >= 3 && !string.IsNullOrEmpty(words[2]))
                list.Add(float.Parse(words[2], System.Globalization.CultureInfo.InvariantCulture.NumberFormat));
            if (limit && words.Length >= 4 && !string.IsNullOrEmpty(words[3]))// cas ou texture cube pas pour le moment
                list.Add(float.Parse(words[3], System.Globalization.CultureInfo.InvariantCulture.NumberFormat));
        }

        private static void ReadFaces(ref string[] words, ref Face face)
        {
            face.FaceType = words.Length - 1;// 4 = quad , 3 = triangle
            var indicetype = words[1].Split('/');
            face.IndiceType = indicetype.Length;// 1 vertex, 2 vertex/ texture , 3 vertex /texture/normal , 4 vertex/color
            if (string.IsNullOrEmpty(indicetype[1]))// cas ou 1//2 (pas de texture)
                face.IndiceType = 4;

            for (int i = 1; i <= face.FaceType; i++)
            {
                var indice = words[i].Split('/');
                face.Vertex[i - 1] = uint.Parse(indice[0]);
                if (face.IndiceType == 2 || face.IndiceType == 3)
                    face.TextureUV[i - 1] = uint.Parse(indice[1]);
                if (face.IndiceType >= 3)
                    face.Normal[i - 1] = uint.Parse(indice[2]);
            }
        }

        private static int VertexFound(ref Mesh mesh, int indiceType, float v1, float v2, float v3, float t1, float t2, float n1, float n2, float n3)
        {
            for (int i = 0; i < mesh.Vertices.Count; i += mesh.Stride)
            {
                bool result = mesh.Vertices[i + 0].Equals(v1) && mesh.Vertices[i + 1].Equals(v2) && mesh.Vertices[i + 2].Equals(v3);
                if (indiceType == 2)
                    result = result && mesh.Vertices[i + 3].Equals(t1) && mesh.Vertices[i + 4].Equals(t2);
                if (indiceType == 3)
                    result = result && mesh.Vertices[i + 3].Equals(t1) && mesh.Vertices[i + 4].Equals(t2) && mesh.Vertices[i + 5].Equals(n1) && mesh.Vertices[i + 6].Equals(n2) && mesh.Vertices[i + 7].Equals(n3);
                if (indiceType == 4)
                    result = result && mesh.Vertices[i + 3].Equals(n1) && mesh.Vertices[i + 4].Equals(n2) && mesh.Vertices[i + 5].Equals(n3);
                if (result)
                    return i / mesh.Stride;
            }
            return -1;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public struct RawData
    {
        /// <summary> . </summary>
        public List<float> V;
        /// <summary> . </summary>
        public List<float> Vn;
        /// <summary> . </summary>
        public List<float> Vt;
        /// <summary> . </summary>
        public List<Face> Faces;
        /// <summary> . </summary>
        public List<Group> Groups;
    }

    /// <summary>
    /// 
    /// </summary>
    public struct Face
    {
        /// <summary> . </summary>
        public uint[] Vertex;//max 4 if quad else 3 for triangle
        /// <summary> . </summary>
        public uint[] TextureUV;
        /// <summary> . </summary>
        public uint[] Normal;
        /// <summary> . </summary>
        public int FaceType;//4 = quad or 3 = triangle ?
        /// <summary> . </summary>
        public int IndiceType; // 1= only vertex,2 = vertex/texture ,3 = vertex/texture/normal,4 = vertex//normal = color
        /// <summary> . </summary>
        public string Group;
        /// <summary> . </summary>
        public string Material;

        /// <summary> . </summary>
        public void Init()
        {
            Vertex = new uint[4];
            TextureUV = new uint[4];
            Normal = new uint[4];
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public struct Group
    {
        /// <summary> . </summary>
        public List<Face> Faces;
        /// <summary> . </summary>
        public string MaterialName; // 'usemtl'
        /// <summary> . </summary>
        public string NameGroup;// 'o' ou 'g'
    }
    /// <summary>
    /// 
    /// </summary>
    public struct Material
    {
        /// <summary> . </summary>
        public string Name; //usemtl
        /// <summary> . </summary>
        public float Ns;
        /// <summary> . </summary>
        public float Ni;
        /// <summary> . </summary>
        public float D;
        /// <summary> . </summary>
        public float Tr;
        /// <summary> . </summary>
        public float[] Tf;//3
        /// <summary> . </summary>
        public int illum;
        /// <summary> . </summary>
        public float[] ka;
        /// <summary> . </summary>
        public float[] Kd;
        /// <summary> . </summary>
        public float[] Ks;
        /// <summary> . </summary>
        public float[] Ke;
        /// <summary> . </summary>
        public string Map_Ka;
        /// <summary> . </summary>
        public string Map_Kd;
        /// <summary> . </summary>
        public string Map_bump;
        /// <summary> . </summary>
        public string Bump;

        /// <summary> . </summary>
        public void Init()
        {
            Name = string.Empty; //usemtl
            Ns = 0.0f;
            Ni = 0.0f;
            D = 0.0f;
            Tr = 0.0f;
            Tf = new float[3] { 0.0f, 0.0f, 0.0f };//3
            illum = -1;
            ka = new float[3] { 0.0f, 0.0f, 0.0f };
            Kd = new float[3] { 0.0f, 0.0f, 0.0f };
            Ks = new float[3] { 0.0f, 0.0f, 0.0f };
            Ke = new float[3] { 0.0f, 0.0f, 0.0f };
            Map_Ka = string.Empty;
            Map_Kd = string.Empty;
            Map_bump = string.Empty;
            Bump = string.Empty;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public struct Model
    {
        /// <summary> . </summary>
        public string NameFileObj;
        /// <summary> . </summary>
        public string NameFileMaterial;
        /// <summary>
        /// Resultat exploitable par opengl
        /// </summary>
        public List<Mesh> Meshs;
        /// <summary>
        /// List des textures et effets pour le mesh
        /// </summary>
        public List<Material> Materials;
        /// <summary>
        /// tous les elemnts contenu apres avoir parser le fichiers obj
        /// </summary>
        public RawData RawData;
    }
    /// <summary>
    /// 
    /// </summary>
    public struct Mesh
    {
        /// <summary> . </summary>
        public List<float> Vertices;
        /// <summary> . </summary>
        public List<uint> Indices;
        //public string TextureName
        /// <summary> . </summary>;
        public Material Material;
        /// <summary> . </summary>
        public int Stride;
        // public int Format; // 1 (only vertex) 2 vertex + UV, 3 vertex+UV + normal , 4 vertex + color
        /// <summary> . </summary>
        public int TriangleToDraw;
    }
}

// namespace MCJ.Engine.Resources
// {
//     using System;
//     using MCJ.Engine;
//     public class Obj
//     {
//         public List<float> Vertices;
//         public List<float> Normals;
//         public List<float> Textures;
//         public List<int> Faces;
//         public string TextureName;
//         public int Length;
//         public List<float> Vertex;
//         public List<uint> Indices;
//         /// <summary>
//         /// si 4 = quad recompose les indices sinon trois triangle
//         /// </summary>
//         public int FaceType;
//         public int IndiceType;

//         public int Stride;

//         public Obj()
//         {
//             Vertices = new(30);
//             Normals = new(30);
//             Textures = new(30);
//             Faces=new(60);
//             Length =1;
//         }

//         public void OpenFile(string file)
//         {
//             try{
//                 var contents = System.IO.File.ReadLines(file);
//                 foreach( string line in contents)
//                 {
//                     //line.Replace('\t',' ').Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
//                     var words = line.Replace('\t',' ').Split(' ', System.StringSplitOptions.RemoveEmptyEntries);

//                     if(words.Length==0)
//                         continue;

//                     //read first 
//                     if ( words[0] == "f")
//                         ReadFaces(ref words );
//                     if ( words[0] =="vn")
//                         ReadFloats(ref words, ref Normals);
//                     if ( words[0]== "vt")
//                         ReadFloats(ref words, ref Textures);
//                     if ( words[0] == "v")
//                         ReadFloats(ref words, ref Vertices);
//                     if ( words[0] =="usemtl")
//                         ReadTextureName(ref words);
//                     if ( words[0] == "mtllib")
//                     {
//                         // not yet
//                     }
//                 }
//             }
//             catch( System.Exception ex)
//             {
//                 System.Console.WriteLine(ex.Message);
//             }
//         }

//         private void ReadTextureName(ref string[] words)
//         {
//             this.TextureName = words[1];
//         }

//         private static void ReadFloats(ref string[] words, ref List<float> list)
//         {
//             try{
//                 for(int i = 1 ; i < words.Length; i++)
//                 {
//                     float result = float.Parse( words[i],System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
//                     list.Add(result);
//                 }
//             }
//             catch( System.Exception ex)
//             {
//                 System.Console.WriteLine(ex.Message);
//             }
//         }

//         private void ReadFaces(ref string[] words)
//         {
//             FaceType = words.Length -1;
//             try{
//                 for(int i = 1 ; i < words.Length; i++)
//                 {
//                     var indices = words[i].Split('/');

//                     IndiceType = indices.Length;

//                     if (IndiceType == 1 )
//                     {
//                         this.Faces.Add(int.Parse(indices[0]));
//                     }
//                     if ( IndiceType == 2 )
//                     {
//                         this.Faces.Add(int.Parse(indices[0]));
//                         this.Faces.Add(int.Parse(indices[1]));
//                     }
//                     if( IndiceType == 3 )
//                     {
//                         this.Faces.Add(int.Parse(indices[0]));
//                         if (  ! string.IsNullOrEmpty(indices[1]))
//                             this.Faces.Add(int.Parse(indices[1]));
//                         this.Faces.Add(int.Parse(indices[2]));
//                         IndiceType =4;
//                     }
//                 }
//             }
//             catch( System.Exception ex)
//             {
//                 System.Console.WriteLine(ex.Message);
//             }
//         }
//         /// <summary>
//         /// Convert parsed obj into  opengl draw elements  one face => 6 indices ( quatre sommets)
//         /// </summary>
//         public void OptimisedForOpengl( )
//         {
//             Length = 0;
//             int divisor = IndiceType ;//> 1 ? 3 : 1 ;
//             if ( IndiceType==4)
//                 divisor =2;

//             int size = Faces.Count /divisor ;
//             Indices = new(size+size);
//             if ( IndiceType == 1)
//                 Stride = 3;

//             if ( IndiceType == 3)
//                 Stride =8;

//             if ( IndiceType == 2)
//                 Stride =5;
//             if( IndiceType == 4)
//                 Stride =6;

//             Vertex = new( size * Stride );

//             int indexfacestored = 0;
//             // int faceoptimised=0 ;
//             int[] facesStored = new int[FaceType];
//             float t1 =0.0f;
//             float t2 =0.0f;
//             float n1 =0.0f;
//             float n2 =0.0f;
//             float n3 =0.0f;
//             // var nombrefloats = size * Stride;
//             for(int i = 0 ; i <  Faces.Count ; i+= divisor)
//             {
//                 int indiceVertex  = Faces[i+0];

//                 float v1 =Vertices[((indiceVertex -1) *3) + 0 ];
//                 float v2 =Vertices[((indiceVertex -1) *3) + 1 ];
//                 float v3  =Vertices[((indiceVertex -1) *3) + 2 ];
//                 if ( IndiceType == 2)
//                 {
//                     int indiceTexture = Faces[i+1];
//                     t1  =Textures[((indiceTexture -1) *2) + 0 ];
//                     t2  =Textures[((indiceTexture -1) *2) + 1 ];
//                 }
//                 if ( IndiceType ==3 )
//                 {
//                     int indiceTexture = Faces[i+1];
//                     int indicenormal  = Faces[i+2];
//                     t1  =Textures[((indiceTexture -1) *2) + 0 ];
//                     t2  =Textures[((indiceTexture -1) *2) + 1 ];
//                     n1  =Normals[((indicenormal -1) *3) + 0 ];
//                     n2 =Normals[((indicenormal -1) *3) + 1 ];
//                     n3  =Normals[((indicenormal -1) *3) + 2 ];
//                 }
//                 if ( IndiceType ==3 )
//                 {
//                     int indicenormal  = Faces[i+2];
//                     n1  =Normals[((indicenormal -1) *3) + 0 ];
//                     n2 =Normals[((indicenormal -1) *3) + 1 ];
//                     n3  =Normals[((indicenormal -1) *3) + 2 ];
//                 }

//                 var indicefound =-1;
//                 var count =0;
//                 for ( int y=0 ; y <=  Vertex.Count ; y+=Stride)
//                 {
//                     if ( IndiceType == 1 )
//                     {
//                         if ( Vertex[y+ 0].Equals(v1) && Vertex[y+1].Equals(v2) && Vertex[y+2].Equals(v3)  )
//                         {
//                             indicefound = count;
//                             break;
//                         }
//                     }
//                     if ( IndiceType == 2)
//                     {
//                         if ( Vertex[y+0].Equals(v1) && Vertex[y+1].Equals(v2) && Vertex[y+2].Equals(v3) && Vertex[y+3].Equals(t1) &&
//                         Vertex[y+4].Equals(t2) )
//                         {
//                             indicefound = count;
//                             break;
//                         }
//                     }
//                     if ( IndiceType ==3 )
//                     {
//                         if ( Vertex[y+0].Equals(v1) && Vertex[y+1].Equals(v2) && Vertex[y+2].Equals(v3) &&
//                              Vertex[y+3].Equals(t1) && Vertex[y+4].Equals(t2) &&
//                              Vertex[y+5].Equals(n1) && Vertex[y+6].Equals(n2) && Vertex[y+7].Equals(n3) )
//                         {
//                             indicefound = count;
//                             break;
//                         }
//                     }
//                     if ( IndiceType ==4 )
//                     {
//                         if ( Vertex[y+0].Equals(v1) && Vertex[y+1].Equals(v2) && Vertex[y+2].Equals(v3) &&
//                              Vertex[y+5].Equals(n1) && Vertex[y+6].Equals(n2) && Vertex[y+7].Equals(n3) )
//                         {
//                             indicefound = count;
//                             break;
//                         }
//                     }

//                     count++;
//                 }

//                 if( indicefound ==-1)
//                 {
//                     // si la face n'exist pas 
//                     Vertex.Add( v1 );
//                     Vertex.Add( v2 );
//                     Vertex.Add( v3 );
//                     if ( IndiceType == 2)
//                     {
//                         Vertex.Add( t1 );
//                         Vertex.Add( t2 );
//                     }
//                     if ( IndiceType==3)
//                     {
//                         Vertex.Add( t1 );
//                         Vertex.Add( t2 );
//                         Vertex.Add( n1 );
//                         Vertex.Add( n2 );
//                         Vertex.Add( n3 );
//                     }
//                     if ( IndiceType == 4)
//                     {
//                         Vertex.Add( n1 );
//                         Vertex.Add( n2 );
//                         Vertex.Add( n3 );
//                     }
//                     facesStored[indexfacestored] = Length++;
//                 }
//                 else
//                 {
//                     // si elle exist 
//                     facesStored[indexfacestored] = indicefound;
//                     // System.Console.WriteLine("Face found : "+indicefound);
//                     // faceoptimised++;
//                 }
//                 //dans le cas OPENGL OU UNE FACE = DEUX TRAIANGLE SOIT 6 INDICES( il n'y a pas de quad)
//                 if( indexfacestored== FaceType-1)
//                 {
//                     // Indices.Add((uint)Length++);
//                     if ( FaceType == 4)
//                     {
//                         Indices.Add((uint)facesStored[0]);
//                         Indices.Add((uint)facesStored[1]);
//                         Indices.Add((uint)facesStored[3]);
//                         Indices.Add((uint)facesStored[1]);
//                         Indices.Add((uint)facesStored[2]);
//                         Indices.Add((uint)facesStored[3]);
//                     }
//                     if ( FaceType == 3)
//                     {
//                         Indices.Add((uint)facesStored[0]);
//                         Indices.Add((uint)facesStored[1]);
//                         Indices.Add((uint)facesStored[2]);
//                     }
//                     indexfacestored =-1;
//                 }
//                 indexfacestored++;
//             }
//         }

//         public void RapportObj()
//         {
//             // RAPPORT
//             // System.Console.WriteLine("Cube Vertex Length" + Vertex.Count + " Before optimisation : " +nombrefloats  +" suppression :" + (nombrefloats-Vertex.Count)  +" Face optimiser : "+ faceoptimised);
//             //write humanreadable vertex 
//             for (int i = 0 ;i < Vertex.Count; i+=Stride )
//             {
//                 int indice= i/Stride;
//                 string stride ="["+indice+"] Vertex : "+ Vertex[i+0] +" "+Vertex[i+1] +" "+Vertex[i+2] ;
//                 if ( Stride >=5 )
//                     stride += " Texture : " +Vertex[i+3] +" " +Vertex[i+4] ;
//                 if ( Stride >=8)
//                     stride += " Normal : "+Vertex[i+5] +" "+Vertex[i+6] +" "+Vertex[i+7] +" ";
//                 System.Console.WriteLine(stride);
//             }
//             //FIN RAPPORT
//         }

//         public void Dispose()
//         {
//             Vertices?.Dispose();
//             Normals?.Dispose();
//             Textures?.Dispose();
//             Indices?.Dispose();
//             Vertex?.Dispose();
//             Faces?.Dispose();
//         }
//     }
// }

