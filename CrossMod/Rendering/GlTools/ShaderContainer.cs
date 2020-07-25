﻿using AnimatedGif;
using OpenTK.Graphics.OpenGL;
using SFGraphics.GLObjects.Shaders;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CrossMod.Rendering.GlTools
{
    /// <summary>
    /// Stores all <see cref="Shader"/> instances used for rendering.
    /// </summary>
    public static class ShaderContainer
    {
        private static readonly Dictionary<string, ShaderType> shaderTypeByExtension = new Dictionary<string, ShaderType>
        {
            { ".vert", ShaderType.VertexShader },
            { ".frag", ShaderType.FragmentShader },
            { ".geom", ShaderType.GeometryShader },
        };

        private static readonly SFShaderLoader.ShaderLoader shaderLoader = new SFShaderLoader.ShaderLoader();

        public static bool HasSetUp { get; private set; }
        public static Shader GetCurrentRModelShader()
        {
            if (RenderSettings.Instance.RenderUVs)
                return GetShader("RModelUV"); 
            if (RenderSettings.Instance.UseDebugShading)
                return GetShader("RModelDebug");
            
            return GetShader("RModel");
        }

        public static Shader GetShader(string name)
        {
            return shaderLoader.GetShader(name);
        }

        public static void SetUpShaders()
        {
            UpdateShaderSources();
            CreateAllShaders();

            HasSetUp = true;
        }

        private static void CreateAllShaders()
        {
            CreateRModelShader();
            CreateRModelUvShader();
            CreateTextureShader();
            CreateSphereShader();
            CreateCapsuleShader();
            CreateLineShader();
            CreatePolygonShader();

            // TODO: This shader can be generated by SFGraphics.
            CreateRModelDebugShader();
        }

        private static void UpdateShaderSources()
        {
            var shaderFolder = $"{Path.GetDirectoryName(Application.ExecutablePath)}//Shaders";
            var shaderFiles = Directory.EnumerateFiles(shaderFolder, "*.*", SearchOption.AllDirectories).Where(f => shaderTypeByExtension.Keys.Contains(Path.GetExtension(f)));
            foreach (var file in shaderFiles)
            {
                // TODO: The names may not be unique if a file appears in multiple folders.
                var source = File.ReadAllText(file);
                var name = Path.GetFileName(file);
                shaderLoader.AddSource(name, source, shaderTypeByExtension[Path.GetExtension(file)]);
            }
        }

        public static void ReloadShaders()
        {
            SetUpShaders();

            var modelShader = GetShader("RModel");
            var debugShader = GetShader("RModelDebug");
            if (!modelShader.LinkStatusIsOk || !debugShader.LinkStatusIsOk)
            {
                MessageBox.Show("One or more shaders failed to compile. See the generated error logs for details.",
                    "Shader Compilation Error");
            }

            Directory.CreateDirectory("Error Logs");
            File.WriteAllText("Error Logs//RModel_shader_errors.txt", modelShader.GetErrorLog());
            File.WriteAllText("Error Logs//RModelDebug_shader_errors.txt", debugShader.GetErrorLog());
        }

        private static void CreateTextureShader()
        {
            shaderLoader.AddShader("RTexture",
                "texture.vert",
                "texture.frag",
                "Gamma.frag"
            );
        }

        private static void CreateRModelDebugShader()
        {
            shaderLoader.AddShader("RModelDebug",
                "RModel.vert",
                "RModelDebug.frag",
                "NormalMap.frag",
                "Gamma.frag",
                "Wireframe.frag",
                "TextureLayers.frag",
                "RModel.geom"
            );
        }

        private static void CreateRModelUvShader()
        {
            shaderLoader.AddShader("RModelUV",
                "RModelUV.vert",
                "RModelUV.frag",
                "NormalMap.frag",
                "Gamma.frag",
                "Wireframe.frag",
                "RModel.geom"
            );
        }

        private static void CreateRModelShader()
        {
            shaderLoader.AddShader("RModel", 
                "RModel.vert",
                "RModel.geom",
                "RModel.frag",
                "NormalMap.frag",
                "Gamma.frag",
                "Wireframe.frag",
                "TextureLayers.frag"
            );
        }

        private static void CreateSphereShader()
        {
            shaderLoader.AddShader("Sphere",
                "Sphere.vert",
                "SolidColor.frag"
            );
        }

        private static void CreateCapsuleShader()
        {
            shaderLoader.AddShader("Capsule",
                "Capsule.vert",
                "SolidColor.frag"
            );
        }

        private static void CreateLineShader()
        {
            shaderLoader.AddShader("Line",
                "Line.vert",
                "SolidColor.frag"
            );
        }

        private static void CreatePolygonShader()
        {
            shaderLoader.AddShader("Polygon",
                "Polygon.vert",
                "SolidColor.frag"
            );
        }
    }
}
