using OpenTK;

namespace computergraphics
{
	/**
	 * Shared interfaces for triangle meshes.
	 * */
	public interface ITriangleMesh
	{

		/**
		 * Add vertex. Return index of vertex in vertex list.
		 * */
		int AddVertex(Vector3 vertex);

		/**
		 * Add triangle by indices.
		 * */
		void AddTriangle(int a, int b, int c);

		/**
		 * Add triangle.
		 * */
		void AddTriangle(Triangle t);

		/**
		 * Add texture coordinate.
		 * */
		void AddTextureCoordinate(Vector2 texCoord);

		/**
		 * Set a texture object for the mesh.
		 * */
		void SetTexture(Texture texture);

		/**
		 * Clear datastructure.
		 * */
		void Clear();

		/**
		 * Compute normals for all triangles.
		 * */
		void ComputeTriangleNormals();

		/**
		 * Create a mesh of the shadow polygons.
		 * 
		 * lightPosition: Position of the light source.
		 * extend: Length of the polygons
		 * shadowPolygonMesh: Result is put in there
		 * */
		void CreateShadowPolygons(Vector3 lightPosition, float extend, ITriangleMesh shadowPolygonMesh);

		int GetNumberOfTriangles();

		Triangle GetTriangle(int triangleIndex);

		int GetNumberOfVertices();

		Vector3 GetVertex(int vertexIndex);

		Vector2 GetTextureCoordinate(int index);

		Texture GetTexture();
	}
}

