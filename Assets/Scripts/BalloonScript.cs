using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonScript : MonoBehaviour {
    private bool fadingIn;
    private Material material;
    private Color matColor;
    public bool isPoppable;
    private MeshRenderer meshRenderer;
    Vector3 initialPosition;
	Vector3 spawnPosition;
    float startup = 0;

	// Use this for initialization
	void Start () {
        spawnPosition = transform.localPosition;
		startup = Random.value;
        material = GetComponent<Renderer>().material;
        matColor = material.color;
        fadingIn = false;
        isPoppable = true;
        PickRandomColor(false);
	}

	
	//Update is called once per frame
	void Update () {
        if(fadingIn && matColor.a < 1){
            matColor.a += 0.01f;
            material.color = matColor;

            return;
        }

        fadingIn = false;
        isPoppable = true;
        
        transform.localPosition = spawnPosition + (1+startup)*new Vector3(0.05f*Mathf.Sin(0.837f*Time.time + startup),0.1f*Mathf.Sin(Time.time + startup),0.023f*Mathf.Sin(0.776f*Time.time + startup));
		
	}

	void Fade(bool fadeIn){

	}

	void OnTriggerEnter(Collider other) {
        if(other.name.Equals("Bird")){
            transform.GetChild(0).gameObject.SetActive(false);
            Pop();
        }
       
    }

    public void Pop(){
        isPoppable = false;
        BirdControllerScript.main.ChangeTarget();
        BalloonPoolingScript.main.RemoveBalloon(gameObject);
        StartCoroutine(SplitMesh());
    }

     private IEnumerator Respawn(float t){
         yield return new WaitForSeconds(t);
         spawnPosition = new Vector3(Random.Range(-1.4f, 1.4f), Random.Range(-0.5f, 0.6f), Random.Range(-0.9f, 1f));
		 transform.localPosition = spawnPosition;
         BalloonPoolingScript.main.AddBalloon(gameObject);
         PickRandomColor(true);
         GetComponent<MeshRenderer>().enabled = true;
         GetComponent<MeshCollider>().enabled = true;
         transform.GetChild(0).gameObject.SetActive(true);
         fadingIn = true;
     }

     private void PickRandomColor(bool reduceAlphaToZero){
         Color color = BalloonPoolingScript.main.colorList[Random.Range(0, BalloonPoolingScript.main.colorList.Count)];
         color.a = (reduceAlphaToZero)? 0 : color.a;
         matColor = color;
         material.color = matColor;
     }

     private IEnumerator SplitMesh ()    {
 
         if(GetComponent<MeshFilter>() == null || GetComponent<SkinnedMeshRenderer>() == null) {
             yield return null;
         }
 
         if(GetComponent<Collider>()) {
             GetComponent<Collider>().enabled = false;
         }
 
         Mesh M = new Mesh();
         if(GetComponent<MeshFilter>()) {
             M = GetComponent<MeshFilter>().mesh;
         }
         else if(GetComponent<SkinnedMeshRenderer>()) {
             M = GetComponent<SkinnedMeshRenderer>().sharedMesh;
         }
 
         Material[] materials = new Material[0];
         if(GetComponent<MeshRenderer>()) {
             materials = GetComponent<MeshRenderer>().materials;
         }
         else if(GetComponent<SkinnedMeshRenderer>()) {
             materials = GetComponent<SkinnedMeshRenderer>().materials;
         }
 
         Vector3[] verts = M.vertices;
         Vector3[] normals = M.normals;
         Vector2[] uvs = M.uv;
         for (int submesh = 0; submesh < M.subMeshCount; submesh++) {
 
             int[] indices = M.GetTriangles(submesh);
 
             for (int i = 0; i < indices.Length; i += 3)    {
                 Vector3[] newVerts = new Vector3[3];
                 Vector3[] newNormals = new Vector3[3];
                 Vector2[] newUvs = new Vector2[3];
                 for (int n = 0; n < 3; n++)    {
                     int index = indices[i + n];
                     newVerts[n] = verts[index];
                     newUvs[n] = uvs[index];
                     newNormals[n] = normals[index];
                 }
 
                 Mesh mesh = new Mesh();
                 mesh.vertices = newVerts;
                 mesh.normals = newNormals;
                 mesh.uv = newUvs;
                 
                 mesh.triangles = new int[] { 0, 1, 2, 2, 1, 0 };
 
                 GameObject GO = new GameObject("Triangle " + (i / 3));
                 GO.layer = LayerMask.NameToLayer("Particle");
                 GO.transform.position = transform.position;
                 GO.transform.rotation = transform.rotation;
                 GO.AddComponent<MeshRenderer>().material = materials[submesh];
                 GO.AddComponent<MeshFilter>().mesh = mesh;
                 GO.AddComponent<BoxCollider>();
                 Vector3 explosionPos = new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y + Random.Range(0f, 0.5f), transform.position.z + Random.Range(-0.5f, 0.5f));
                 GO.AddComponent<Rigidbody>().AddExplosionForce(Random.Range(300,500), explosionPos, 5);
                 Destroy(GO, 0.5f + Random.Range(0.0f, 0.8f));
                
             }
         }
 
         GetComponent<Renderer>().enabled = false;
         
         yield return new WaitForSeconds(1.0f);
         StartCoroutine(Respawn(Random.Range(1f, 3f)));
 
     }
 
}
