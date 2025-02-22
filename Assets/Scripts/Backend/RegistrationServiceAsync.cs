﻿using UnityEngine;
using System.Collections;
using KiiCorp.Cloud.Storage;
using System;

public class RegistrationServiceAsync : MonoBehaviour {

	public delegate void MethodReferenceWithResponse(Response response);

	public void sendRegistrationData(RegistrationData registrationData, MethodReferenceWithResponse responseHandler) {
		
		Response response = (Response)gameObject.AddComponent("Response"); 
		bool inHandler = true;
		Debug.Log("Attempting registration...");
		if (registrationData.password.Equals (registrationData.passwordConfirm)) {
			Debug.Log("Creating user builder");
			KiiUser.Builder builder;
			builder = KiiUser.BuilderWithName (registrationData.username);
			builder.WithEmail(registrationData.email);
			KiiUser user = builder.Build();
			Debug.Log("Attempting signup...");
			user.Register(registrationData.password, (KiiUser user2, Exception e) => {
				if (e != null) {
					response.error = true;
					response.message = "Signup failed: " + e.ToString();
					inHandler = false;
					Debug.Log ("Signup failed: " + e.ToString());
				} else {
					response.error = false;
					response.message = "";
					inHandler = false;
					Debug.Log ("Signup succeeded");
				}
			});
		} else {
			response.error = true;
			response.message = "Passwords don't match!";
			inHandler = false;
		}
		// Calling response handler:
		while(inHandler) {}
		responseHandler(response);
	}
}
