using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using Vector3 = OpenTK.Vector3;
using OpenTKUtilities = OpenTK.Platform.Utilities;

namespace tmp
{
    internal sealed class Window : GameWindow
    {
        public Window() : base(900, 600, new GraphicsMode(32, 24, 0, 16))
        {
            VSync = VSyncMode.Off;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            Title = "Minecraft";
            
            InitShaders();
            InitCubes();
            InitData();
            SetMatrix(0f);
            GL.UseProgram(_shaderProgramId);
            OnUpdateFrame1();
            GL.ClearColor(Color.Gray);
        }

        private int c;

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    _camera.Move(0, 0.1f, 0);
                    break;
                case Key.S:
                    _camera.Move(0, -0.1f, 0);
                    break;
                case Key.A:
                    _camera.Move(-0.1f, 0f, 0);
                    break;
                case Key.D:
                    _camera.Move(0.1f, 0f, 0);
                    break;
                case Key.F:
                    for (var i = 0; i < 10; i++)
                        AddCube(new Cube(new Vector3(i-3, 3, -c)));
                    c++;
                    

                    break;
            }

            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        protected override void OnResize(EventArgs sizeEventArgs)
        {
            base.OnResize(sizeEventArgs);
            GL.Viewport(0, 0, Size.Width, Size.Height);
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 1.0f, 64.0f);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
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
        private int _vboMView;

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
            GL.GenBuffers(1, out _vboMView);
            GL.GenBuffers(1, out _iboId);
        }

        private void SetMatrix(float a)
        {
            _modelMatrix = Matrix4.CreateRotationZ(a);
            _viewMatrix = Matrix4.CreateTranslation(-0.5f, -0.5f, -50);
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(1.3f, Size.Width / (float) Size.Height, 0.1f, 500f);
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
        

        private Matrix4 _modelMatrix;
        private Matrix4 _viewMatrix;
        private Matrix4 _projectionMatrix;


        private readonly Camera _camera = new Camera(new Vector3(3, 0, 5));
        private readonly List<Cube> _cubes = new List<Cube>();

        private void InitCubes()
        {
            //for (var i = 0; i < 3; i++)
            for (var i = -200; i < 200; i++)
            for (var j = -200; j < 200; j++)
            {
                //var j = 0;
                _cubes.Add(new Cube(new Vector3(i * 2, -2, j)));
            }
        }

        private List<Vector3> _vertexes = new List<Vector3>();
        private List<Vector3> _colors = new List<Vector3>();
        private List<int> _indices = new List<int>();

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            OnUpdateFrame1();
        }

        private void OnUpdateFrame1()
        {
            GL.BindVertexArray(_vaoId);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vboPosition);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertexes.Count * Vector3.SizeInBytes, _vertexes.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(_attributeVertexPosition, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(_attributeVertexPosition);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vboColor);
            GL.BufferData(BufferTarget.ArrayBuffer, _colors.Count * Vector3.SizeInBytes, _colors.ToArray(), BufferUsageHint.DynamicDraw);
            GL.VertexAttribPointer(_attributeVertexColor, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(_attributeVertexColor);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _iboId);
            GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(int) * _indices.Count, _indices.ToArray(), BufferUsageHint.StaticDraw);


            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        private void InitData()
        {
            var t = 0;
            foreach (var cube in _cubes)
            {
                _vertexes.AddRange(cube.GetVertexes());
                _colors.AddRange(cube.GetColorData());
                _indices.AddRange(cube.GetIndices(t * 8));
                t++;
            }
        }

        public void AddCube(Cube cube)
        {
            _vertexes.AddRange(cube.GetVertexes());
            _colors.AddRange(cube.GetColorData());
            _indices.AddRange(cube.GetIndices(_cubes.Count * 8));
            _cubes.Add(cube);
        }


        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            GL.BindVertexArray(_vaoId);
            SetMatrix(0f);
            _viewMatrix = _camera.GetViewMatrix();
            GL.UniformMatrix4(_uniformModel, false, ref _modelMatrix);
            GL.UniformMatrix4(_uniformView, false, ref _viewMatrix); 
            GL.UniformMatrix4(_uniformProjection, false, ref _projectionMatrix);
            
            GL.DrawElements(BeginMode.Quads, _indices.Count, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);

            GL.Flush();
            SwapBuffers();
        }
    }
}