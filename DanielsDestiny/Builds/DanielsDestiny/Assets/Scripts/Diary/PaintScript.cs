using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PaintScript : MonoBehaviour {
	public LayerMask layerMask;
	public GameObject paintPlane;
	public Texture2D[] backgrounds;
	public GameObject Eraser;
	int _curPage = 0;
	int _paintPage = 1;
	Ray _ray;
	RaycastHit _hit;
	bool _canPaint = false;
	bool _mouseIsDown = false;
	Mesh mesh;
	Vector3[] vertices;
	Color[] colors;
	Color[] saveColor;
	bool _initialized = false;
	// Use this for initialization
	void Start () {
		if(!_initialized)
			ChangePage(1);
	}
	
	// Update is called once per frame
	void Update () {
		if(InputManager.GetKeyDown("escape"))
		   TogglePaint(false);
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
		if(!_initialized)
		{
			mesh = paintPlane.GetComponent<MeshFilter>().mesh;
			vertices = mesh.vertices;
			colors = new Color[vertices.Length];
			colors = mesh.colors;
			paintPlane.SetActive(false);
			_initialized = true;
		}
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
		paintPlane.SetActive(_set);
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
		if(_curPage == _paintPage && (p > 0 || p < 0))
		{
			saveColor = mesh.colors;
			_mouseIsDown = false;
			_canPaint = false;
			if(colors.Length < saveColor.Length)
				colors = saveColor;
			for (int i = 0; i < colors.Length; i++) 
			{
				colors[i] = Color.white;
			}
			mesh.colors = colors;
		}
		_curPage += p;
		if(_curPage < 0)
			_curPage = 0;
		if(_curPage > backgrounds.Length-1)
			_curPage = backgrounds.Length-1;

		paintPlane.renderer.material.mainTexture = backgrounds[_curPage];
		if(_curPage != _paintPage)
		{
			Eraser.SetActive(false);
			_canPaint = false;
		}
		if(_initialized)
		{
			if(_curPage == _paintPage)
			{
				Eraser.SetActive(true);
				_canPaint = true;
				TogglePaint(true);
				_mouseIsDown = false;
				colors = saveColor;
				mesh.colors = saveColor;
			}
		}
		else
		{
			mesh = paintPlane.GetComponent<MeshFilter>().mesh;
			vertices = mesh.vertices;
			colors = new Color[vertices.Length];
			colors = mesh.colors;


			_curPage = 0;
			paintPlane.renderer.material.mainTexture = backgrounds[_curPage];
			Eraser.SetActive(false);
			paintPlane.SetActive(false);
//
			_initialized = true;
		}
	}
}
