using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour {

	public float size;
	public float speed;
	public bool loop;
	public bool lights;
	public bool trails;

	public List<GameObject> ParticleSystems;
	public List<bool> ActiveParticleSystems;

	private List<ParticleSystem> psList = new List<ParticleSystem> ();

	void Start () {

		for(int i = 0; i< ParticleSystems.Count; i++){
			if (ActiveParticleSystems [i] == true)
				ParticleSystems [i].SetActive (true);
			else
				ParticleSystems [i].SetActive (false);
		}

		if (GetComponent<ParticleSystem> ()) {			
			psList.Add (GetComponent<ParticleSystem> ());

			foreach (Transform t in transform) {
				if (t.GetComponent<ParticleSystem> ())
					psList.Add (t.GetComponent<ParticleSystem>());
			}

			for (int i = 0; i < psList.Count; i ++) {				
				var ps = psList[i].GetComponent<ParticleSystem> ();
				var main = ps.main;
				var shape = ps.shape;
				var startSize = main.startSize;
				var startSpeed = main.startSpeed;
				var startDelay = main.startDelay;
				var startLifetime = main.startLifetime;
				var psLights = ps.lights;
				var psTrails = ps.trails;

				//LOOP
				if (loop)
					main.loop = loop;
				else
					main.loop = loop;

				//LIGHTS
				if (!lights && psLights.enabled) 					
					psLights.enabled = false;				

				//TRAILS
				if (!trails && psTrails.enabled)
					psTrails.enabled = false;	

				//POSITION
				if(psList[i].transform.position.y != 0 && i > 0){
					var newPos = psList[i].transform.position;
					newPos.y *= size;
					psList [i].transform.position = newPos;
				}					
					
				//SIZE
				if (startSize.mode == ParticleSystemCurveMode.TwoConstants) {
					startSize.constantMax *= size;
					startSize.constantMin *= size;
					main.startSize = startSize;
				} else {
					startSize.constant *= size;
					main.startSize = startSize;
				}

				//START_SPEED (affected by size)
				if (startSpeed.mode == ParticleSystemCurveMode.TwoConstants) {
					startSpeed.constantMax *= size;
					startSpeed.constantMin *= size;
					main.startSpeed = startSpeed;
				} else {
					startSpeed.constant *= size;
					main.startSpeed = startSpeed;
				}

				//START_SPEED (affected by speed)
				if (startSpeed.mode == ParticleSystemCurveMode.TwoConstants) {
					startSpeed.constantMax *= speed;
					startSpeed.constantMin *= speed;
					main.startSpeed = startSpeed;
				} else {
					startSpeed.constant *= speed;
					main.startSpeed = startSpeed;
				}

				//LIFETIME
				if (main.startLifetime.mode == ParticleSystemCurveMode.TwoConstants) {
					startLifetime.constantMax *= 1/speed;
					startLifetime.constantMin *= 1/speed;
					main.startLifetime = startLifetime;
				} else {
					startLifetime.constant *= 1/speed;
					main.startLifetime = startLifetime;
				}

				//START_DELAY
				if (startDelay.mode == ParticleSystemCurveMode.TwoConstants) {
					startDelay.constantMax *= 1/speed;
					startDelay.constantMin *= 1/speed;
					main.startDelay = startDelay;
				} else {
					startDelay.constant *= 1/speed;
					main.startDelay = startDelay;
				}

				//RADIUS
				if(shape.enabled){
					shape.radius *= size;
				}
			}				
		}
		else
			Debug.Log("This GameObject contains no Particle System");
	}
		
}
