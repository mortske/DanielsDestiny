using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PaintScript : MonoBehaviour {
	public LayerMask layerMask;
	public GameObject paintPlane;
	public Texture2D[] backgrounds;
	public GameObject Erasor;
	int _curPage = 0;
	Ray _ray;
	RaycastHit _hit;
	bool _canPaint = false;
	bool _mouseIsDown = false;
	Mesh mesh;
	Vector3[] vertices;
	Color[] colors;
	Color[] saveColor;
	// Use this for initialization
	void Start () {
		mesh = paintPlane.GetComponent<MeshFilter>().mesh;
		vertices = mesh.vertices;
		colors = new Color[vertices.Length];
		colors = mesh.colors;
		int i = 0;
		paintPlane.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.Period))
			BiomeManager.instance.SaveGame();
		if(Input.GetKeyUp(KeyCode.Comma))
			BiomeManager.instance.LoadBiomes();
		if(_canPaint)
		{
			if(_mouseIsDown)
			{
				_ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				
				if (!Physics.Raycast(_ray, out _hit, layerMask))
				{
					_mouseIsDown = false;
					return;
				}
				
				MeshCollider meshCollider = _hit.collider as MeshCollider;
				if (meshCollider == null || meshCollider.sharedMesh == null)
				{
					_mouseIsDown = false;
					return;
				}
				
				Mesh mesh = meshCollider.sharedMesh;
				Vector3[] vertices = mesh.vertices;
				int[] triangles = mesh.triangles;
				Vector3 p0 = vertices[triangles[_hit.triangleIndex * 3 + 0]];
				Vector3 p1 = vertices[triangles[_hit.triangleIndex * 3 + 1]];
				Vector3 p2 = vertices[triangles[_hit.triangleIndex * 3 + 2]];
				Transform hitTransform = _hit.collider.transform;
				p0 = hitTransform.TransformPoint(p0);
				p1 = hitTransform.TransformPoint(p1);
				p2 = hitTransform.TransformPoint(p2);
				ChangeColor(_hit.triangleIndex);
				Debug.DrawLine(p0, p1);
				Debug.DrawLine(p1, p2);
				Debug.DrawLine(p2, p0);
				
				
			//		transform.parent.gameObject.transform.eulerAngles.y += 0.2;
			}
			if (Input.GetMouseButtonDown(0)){
				_mouseIsDown = true; 
			}
			if (Input.GetMouseButtonUp(0)){
				_mouseIsDown = false;
			}
		}
	}
	public void LoadColor(List<Color> col)
	{
		if(col != null)
		{
			for (int i = 0; i < colors.Length; i++) 
			{
				colors[i] = col[i];
			}
		}
		mesh.colors = colors;
	}
	public List<Color> GetColorsPainted()
	{
		List<Color> SaveString = new List<Color>();
		for(int y = 0; y < colors.Length; y++)
		{
			SaveString.Add(colors[y]);
		}
		return SaveString;
	}
	public void ChangeColor(int iTriangle)
	{
		Color colorT = Color.black;
		colors = mesh.colors;
		int iStart = mesh.triangles[iTriangle*3];
//		for (int i = iStart; i < iStart+4; i++)
			colors[iStart] = colorT;
		mesh.colors = colors;
	}
	public void TogglePaint(bool _set)
	{
		_canPaint = _set;
		paintPlane.SetActive(_set);
		Erasor.SetActive(_set);
		PauseSystem.Pause(_set);
	}
	public void Erase()
	{
		_mouseIsDown = false;
		for (int i = 0; i < colors.Length; i++) 
		{
			colors[i] = Color.white;
		}
		mesh.colors = colors;
	}
	public void ChangePage(int p)
	{
		if(_curPage == 0 && p > 0)
		{
			saveColor = mesh.colors;
			_mouseIsDown = false;
			
			_canPaint = false;
			for (int i = 0; i < colors.Length; i++) 
			{
				colors[i] = Color.white;
			}
			mesh.colors = colors;
		}
		_curPage += p;
		if(_curPage < 0)
			_curPage = 0;
		if(_curPage > 1)
			_curPage = 1;
		paintPlane.renderer.material.mainTexture = backgrounds[_curPage];
		
		if(_curPage == 0)
		{
			Erasor.SetActive(true);
			TogglePaint(true);
			_mouseIsDown = false;
			colors = saveColor;
			mesh.colors = saveColor;
		}
	}
}
