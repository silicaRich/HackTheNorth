using UnityEngine;
using System.Collections;
using System.Threading;
using System; 

public class loadGoogleImage : MonoBehaviour {
	private int imgWidth = 1080;
	private int imgHeight = 960;

	// Use this for initialization
	void Start () {
		//Download all of the appropriate textures
		Texture2D[] textures = getTexturesFromURL("http://maps.googleapis.com/maps/api/streetview", 
		                                          48.8577569,2.2953444, imgWidth, imgHeight); //lat, long, width, height

	//debug length
	Debug.Log ("texture length:" + textures.Length);
	
	//Grab both of the cameras for each lens on the Oculus
	GameObject[] cams = {GameObject.FindWithTag("LeftCam"), GameObject.FindWithTag("RightCam")};
	
	//Map each texture to the correct location on the SkyBox
	foreach (GameObject cam in cams) {
						Skybox skyBox = cam.GetComponent<Skybox> ();
		
						Material material = skyBox.material;
						// Box is inverted along y axis (Rotating counter-clockwise)
						material.SetTexture ("_LeftTex", textures [0]);
						material.SetTexture ("_BackTex", textures [1]);
						material.SetTexture ("_RightTex", textures [2]);
						material.SetTexture ("_FrontTex", textures [3]);
						material.SetTexture ("_UpTex", textures [4]);
						material.SetTexture ("_DownTex", textures [5]);//debug length
						Debug.Log ("texture length:" + textures.Length);
				}

			
		}


	// URL is the "base" URL of the current point in Street View
	// @Param
	// url: the base URL of the Google Street Maps API image location
	// @Return
	// An array of textures corresponding to the 360 degree skybox map
	Texture2D[] getTexturesFromURL(string _url, double lat, double lng, int imageHeight, int imageWidth) {

		//These values may need to be updated based on how long the downloads take
		string url = _url + "?size=" + imageHeight + "x" + imageWidth + "&location=" + lat + "," + lng + "&fov=90";
		Texture2D[] textures = new Texture2D[6];

		//Iterate around the 360 degree horizontal axis, downloading each corresponding Street View image
		//at 90 degree intervals.
		for (int i=0; i<6; i++) {
			string imageURL;
			if(i<4) 
				imageURL = url + "&heading=" + (i * 90);
			else if(i == 4)
				imageURL = url + "&pitch=90&heading=270";
			else
				imageURL = url + "&pitch=270&heading=270";
			
			WWW www = new WWW(imageURL);
			//TODO: Can we make this call asynchronous? Currently it blocks until the image is downloaded.
			while (!www.isDone) {
				
			}	
			textures[i] = www.texture;
			/*Thread t = new Thread(new ThreadStart(this, () => {
			})).Start();
			 */
		}

		//TODO: Download the remaining two images

		return textures;
	}

	//bool done = false;
	// Update is called once per frame
	void Update () {
		//TODO: Should the images be updated here every frame or every X frames
		//if (!done) {
		//	something();
		//	done = true;
		//}

		if (Input.GetKey("up")){
			Texture2D[] currentTextures = getTexturesFromURL();
			getTexturesFromURL(string _url, double lat, double lng, int imageHeight, int imageWidth);
		}
		if (Input.GetKey("down")){
					getTexturesFromURL(string _url, double lat, double lng, int imageHeight, int imageWidth);
		}


	}
}
