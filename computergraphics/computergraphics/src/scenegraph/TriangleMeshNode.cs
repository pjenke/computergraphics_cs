using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace computergraphics
{
	/**
	 * Scene graph node for triangle meshes.
	 * */
	public class TriangleMeshNode : LeafNode
	{

		/**
		 * Contained triangle mesh.
		 * */
		private ITriangleMesh mesh;

		/**
		 * Show the triangle (facet) normals.
		 * */
		private bool showNormals = false;

		/**
		 * This flag allows to mark meshes to cast shadows or not.
		 * */
		private bool castsShadow = true;

		/**
		 * Mesh color.
		 * */
		private Color4 color = new Color4(0.75f, 0.25f, 0.25f, 1.0f);

		/**
		 * This node renders the shadowVolume mesh if required.
		 * */
		private TriangleMeshNode shadowVolumeNode = null;

		/**
		 * VBO for the mesh.
		 * */
		private VertexBufferObject trianglesVBO = new VertexBufferObject();

		/**
		 * VBO for the normals.
		 * */
		private VertexBufferObject normalsVBO = null;

		/**
		 * VBO for the border.
		 * */
		private VertexBufferObject borderVBO = null;

		/**
		 * Render the boundary
		*/
		private bool showBorder;

		public ITriangleMesh Mesh
		{
			get { return mesh; }
		}

		public bool ShowNormals
		{
			get { return showNormals; }
			set { showNormals = value; }
		}

		public bool ShowBorder
		{
			get { return showBorder; }
			set { showBorder = value; }
		}

		public bool CastsShadow
		{
			get { return castsShadow; }
			set { castsShadow = value; }
		}

		public TriangleMeshNode(ITriangleMesh mesh)
		{
			this.mesh = mesh;
			CreateTrianglesVBO();
		}

		/**
		 * Create the VBO for the triangles
		 * */
		void CreateTrianglesVBO()
		{
			trianglesVBO.Invalidate();
			List<RenderVertex> renderVertices = new List<RenderVertex>();
			for (int triangleIndex = 0; triangleIndex < mesh.GetNumberOfTriangles(); triangleIndex++)
			{
				Triangle t = mesh.GetTriangle(triangleIndex);
				for (int vertexIndex = 0; vertexIndex < 3; vertexIndex++)
				{
					Vector3 pos = mesh.GetVertex(t.GetVertexIndex(vertexIndex));
					Vector2 textCoords = mesh.GetTextureCoordinate(t.GetTexCoordIndex(vertexIndex));
					renderVertices.Add(new RenderVertex(pos, t.Normal, color, textCoords));
				}
			}
			trianglesVBO.Setup(renderVertices, PrimitiveType.Triangles);
		}

		/*
		 * Vreate the VBO for the normals
		 * */
		void CreateNormalsVBO()
		{
			normalsVBO = new VertexBufferObject();
			List<RenderVertex> renderVertices = new List<RenderVertex>();
			float scale = 0.05f;
			Color4 normalColor = Color4.DarkGray;
			for (int triangleIndex = 0; triangleIndex < mesh.GetNumberOfTriangles(); triangleIndex++)
			{
				Triangle t = mesh.GetTriangle(triangleIndex);
				Vector3 centroid = Vector3.Multiply(
									   Vector3.Add(mesh.GetVertex(t.GetVertexIndex(0)),
										   Vector3.Add(mesh.GetVertex(t.GetVertexIndex(1)), mesh.GetVertex(t.GetVertexIndex(2)))), 1.0f / 3.0f);
				Vector3 centroid2normal = Vector3.Add(centroid, Vector3.Multiply(t.Normal, scale));
				renderVertices.Add(new RenderVertex(centroid, t.Normal, normalColor));
				renderVertices.Add(new RenderVertex(centroid2normal, t.Normal, normalColor));
			}
			normalsVBO.Setup(renderVertices, PrimitiveType.Lines);
		}

		/*
		 * Create the VBO for the border
		 * */
		void CreateBorderVBO()
		{
			borderVBO = new VertexBufferObject();
			List<TriangleEdge> borderEdges = mesh.GetBorder();
			Color4 borderColor = Color4.Pink;
			List<RenderVertex> renderVertices = new List<RenderVertex>();
			for (int i = 0; i < borderEdges.Count; i++)
			{
				TriangleEdge edge = borderEdges[i];
				Vector3 a = mesh.GetVertex(edge.A);
				Vector3 b = mesh.GetVertex(edge.B);
				renderVertices.Add(new RenderVertex(a, new Vector3(0,1,0), borderColor));
				renderVertices.Add(new RenderVertex(b, new Vector3(0, 1, 0), borderColor));
			}
			borderVBO.Setup(renderVertices, PrimitiveType.Lines);
		}

		public override void TimerTick(int counter)
		{
		}

		public override void DrawGL(RenderMode mode, Matrix4 modelMatrix)
		{
			Matrix4 invertedTransformation = modelMatrix.Inverted();
			invertedTransformation.Transpose();
			Vector4 light4 = new Vector4(GetRootNode().LightPosition.X, GetRootNode().LightPosition.Y, GetRootNode().LightPosition.Z, 1);
			Vector4 transformedLight = MathHelper.Multiply(invertedTransformation, light4);
			Vector3 lightPosition = Vector3.Multiply(transformedLight.Xyz, 1.0f / transformedLight.W);

			if (mode == RenderMode.REGULAR)
			{
				DrawRegular();
			}
			else if (mode == RenderMode.SHADOW_VOLUME && castsShadow)
			{
				if (shadowVolumeNode != null)
				{
					shadowVolumeNode.SetColor(new Color4(0, 0, 1, 1));
				}
				DrawShadowVolume(lightPosition, modelMatrix);
			}
			else if (mode == RenderMode.DEBUG_SHADOW_VOLUME && castsShadow)
			{
				if (shadowVolumeNode != null)
				{
					shadowVolumeNode.SetColor(new Color4(0.25f, 0.25f, 0.75f, 0.4f));
				}
				DrawShadowVolume(lightPosition, modelMatrix);
			}
		}

		/**
		 * Draw mesh regularly
		*/
		private void DrawRegular()
		{
			if (mesh.GetTexture() != null)
			{
				if (!mesh.GetTexture().IsLoaded())
				{
					mesh.GetTexture().Load();
				}
				mesh.GetTexture().Bind();
			}

			// Use vertex buffer objects
			trianglesVBO.Draw();

			// Normals
			if (showNormals)
			{
				if (normalsVBO == null)
				{
					CreateNormalsVBO();
				}
				normalsVBO.Draw();
			}

			// Border
			if (showBorder)
			{
				if (borderVBO == null)
				{
					CreateBorderVBO();
				}
				borderVBO.Draw();
			}
		}

		/**
		 * Draw shadow volume
		 * */
		public void DrawShadowVolume(Vector3 lightPosition, Matrix4 transformation)
		{
			if (shadowVolumeNode == null)
			{
				TriangleMesh silhouetteMesh = new TriangleMesh();
				shadowVolumeNode = new TriangleMeshNode(silhouetteMesh);
				shadowVolumeNode.SetParentNode(this);
			}
			float extend = 500;
			mesh.CreateShadowPolygons(lightPosition, extend, shadowVolumeNode.Mesh);
			shadowVolumeNode.Traverse(RenderMode.REGULAR, transformation);
			shadowVolumeNode.CreateTrianglesVBO();
		}

		public void SetColor(Color4 color)
		{
			this.color.R = color.R;
			this.color.G = color.G;
			this.color.B = color.B;
			this.color.A = color.A;
			CreateTrianglesVBO();
		}
	}
}

