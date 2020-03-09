using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using SFML.Graphics;
using SFML.Window;
using Vector3 = OpenTK.Vector3;

namespace tmp
{
    internal sealed class Window : RenderWindow
    {
        public Window() : base(new VideoMode(800, 600), "First", Styles.Default, new ContextSettings {DepthBits = 2, AntialiasingLevel = 2})
        {
            GL.Enable(EnableCap.DepthTest);
                //SetFramerateLimit(240);
            SetVerticalSyncEnabled(false);
            Resized += OnResized;
            KeyPressed += OnKeyPressed;
            Closed += OnClose;
            OnResized(null, null);
            InitShaders();

            GL.ClearColor(1f, 1f, 0f, 0.5f);


            UpdateFrame();
            while (IsOpen)
            {
                DispatchEvents();
                RenderFrame();

                Display();
            }
        }
        
        private void OnKeyPressed(object sender, KeyEventArgs e)
        {
            var window = (RenderWindow) sender;
            switch (e.Code)
            {
                case Keyboard.Key.W:
                    camera.Move(0, 0.1f, 0);
                    break;
                case Keyboard.Key.S:
                    camera.Move(0, -0.1f, 0);
                    break;
                case Keyboard.Key.A:
                    camera.Move(-0.1f, 0f, 0);
                    break;
                case Keyboard.Key.D:
                    camera.Move(0.1f, 0f, 0);
                    break;
            }

            if (e.Code == Keyboard.Key.Escape)
            {
                window.Close();
            }
        }

        private void OnResized(object sender, SizeEventArgs e)
        {
            GL.Viewport(0, 0, (int) Size.X, (int) Size.Y);
        }

        private static void OnClose(object sender, EventArgs eventArgs)
        {
            var r = (RenderWindow) sender;
            r.Close();
        }


        private int _shaderProgramId;
        private int _vertexShaderId; 
        private int _fragmentShaderId;

        private int _attributeVertexColor;
        private int _attributeVertexPosition;
        private int _uniformModel;
        private int _uniformView;
        private int _uniformProjection;

        private int _vaoId;

        private int _iboId;

        private int _vboPosition;
        private int _vboColor;

        private void InitShaders()
        {
            _shaderProgramId = GL.CreateProgram();
            LoadShader("vs.glsl", ShaderType.VertexShader, _shaderProgramId, out _vertexShaderId);
            LoadShader("fs.glsl", ShaderType.FragmentShader, _shaderProgramId, out _fragmentShaderId);


            GL.LinkProgram(_shaderProgramId);

            GL.DeleteShader(_vertexShaderId);
            GL.DeleteShader(_fragmentShaderId);

            Console.WriteLine(GL.GetProgramInfoLog(_shaderProgramId));

            _attributeVertexPosition = GL.GetAttribLocation(_shaderProgramId, "vPosition");
            _attributeVertexColor = GL.GetAttribLocation(_shaderProgramId, "vColor");
            _uniformModel = GL.GetUniformLocation(_shaderProgramId, "model");
            _uniformView = GL.GetUniformLocation(_shaderProgramId, "view");
            _uniformProjection = GL.GetUniformLocation(_shaderProgramId, "projection");

            if (_attributeVertexColor == -1 || _attributeVertexPosition == -1)
            {
                Console.WriteLine("Error binding attributes");
            }

            GL.GenVertexArrays(1, out _vaoId);
            GL.GenBuffers(1, out _vboPosition);
            GL.GenBuffers(1, out _vboColor);
            //GL.GenBuffers(1, out _vboMView);
            GL.GenBuffers(1, out _iboId);
        }

        private void SetMatrix(float a)
        {
            _modelMatrix = Matrix4.CreateRotationZ(a);
            _viewMatrix = Matrix4.CreateTranslation(-0.5f, -0.5f, -50);
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(1.3f, Size.X / (float) Size.Y, 0.1f, 1500f);
        }
        private static void LoadShader(string fileName, ShaderType shaderType, int program, out int address)
        {
            address = GL.CreateShader(shaderType);
            using (var sr = new StreamReader(fileName))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }

            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }

        private readonly int[] _indices = {
            0, 1, 2, 3,
            2, 3, 4, 5,
            1, 2, 5, 6,
            0, 1, 6, 7,
            0, 3, 4, 7,
            4, 5, 6, 7,
        };

        private readonly List<Vector3> _vertexes = new List<Vector3>()
        { 
            new Vector3(0f, 1f,  0f),
            new Vector3(1f, 1f,  0f),
            new Vector3(1f, 1f,  1f),
            new Vector3(0f, 1f,  1f),
            new Vector3(0f, 0f, 1f),
            new Vector3(1f, 0f, 1f),
            new Vector3(1f, 0f, 0f),
            new Vector3(0f, 0f, 0f), 
        };

        private Vector3[] _colors = 
        {
            new Vector3(1f, 0f, 0f),
            new Vector3( 0f, 0f, 1f), 
            new Vector3( 0f,  1f, 0f),
            new Vector3(1f, 0f, 0f),
            new Vector3(1f, 0f, 0f),
            new Vector3( 0f, 0f, 1f), 
            new Vector3( 0f,  1f, 0f),
            new Vector3(1f, 0f, 0f),
        };


        private Matrix4 _modelMatrix;
        private Matrix4 _viewMatrix;
        private Matrix4 _projectionMatrix;


        private Camera camera = new Camera();

        private void UpdateFrame()
        {
            GL.BindVertexArray(_vaoId);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vboPosition);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertexes.Count * Vector3.SizeInBytes, _vertexes.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(_attributeVertexPosition, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(_attributeVertexPosition);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vboColor);
            GL.BufferData(BufferTarget.ArrayBuffer, _colors.Length * Vector3.SizeInBytes, _colors, BufferUsageHint.DynamicDraw);
            GL.VertexAttribPointer(_attributeVertexColor, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(_attributeVertexColor);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _iboId);
            GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(int) * _indices.Length, _indices,
                BufferUsageHint.StaticDraw);

            //SetMatrix(0);
            //GL.UniformMatrix4(_uniformModel, false, ref _modelMatrix);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }


        private float _counter;
        private void RenderFrame()
        {
            GL.Viewport(0, 0, (int) Size.X, (int) Size.Y);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            GL.UseProgram(_shaderProgramId);
            //SetMatrix(_counter / 1000);
            //_counter++;
            SetMatrix(1f);
            GL.BindVertexArray(_vaoId);
            GL.UniformMatrix4(_uniformModel, false, ref _modelMatrix);
            GL.UniformMatrix4(_uniformView, false, ref _viewMatrix); 
            GL.UniformMatrix4(_uniformProjection, false, ref _projectionMatrix);
            
            //foreach (var cubePos in Positions)
            _viewMatrix = camera.GetViewMatrix();
            GL.UniformMatrix4(_uniformView, false, ref _viewMatrix); 
            for(var i = -100; i < 100; i++)
                for(var j = -100; j < 100; j++)
            {
                //SetMatrix(1);
                _modelMatrix = Matrix4.CreateTranslation(new Vector3(i, -2, j));
                GL.UniformMatrix4(_uniformModel, false, ref _modelMatrix);
                
                
                GL.DrawElements(BeginMode.Quads, _indices.Length, DrawElementsType.UnsignedInt, 0);
            }
            
            //GL.DrawElements(BeginMode.Quads, indices.Length, DrawElementsType.UnsignedInt, 0);
            //GL.DrawArrays(PrimitiveType.Quads, 0, 4);
            GL.BindVertexArray(0);

            GL.Flush();
        }
    }
}