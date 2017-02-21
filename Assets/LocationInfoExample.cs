using UnityEngine;
using System.Collections;

public class LocationInfoExample : MonoBehaviour {

    private float time;

    // Use this for initialization
    IEnumerator Start () {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            yield break;

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
    }


    private void sendLocation(float latitude, float longitude, string nick)
    {
        HttpUtils httpUtils = new HttpUtils();
        string url = "http://openmultiplayer.com/mobileserver/position";
        string jsonString = "{\"latitude\": " + latitude + ",\"longitude\": " + longitude + ",\"nick\": \"" + nick + "\"}";
        StartCoroutine(httpUtils.sendHttpDataJson(url, jsonString));
    }

    // Update is called once per frame
    void Update () {
        time += Time.deltaTime;
        if(time > 5)
        {
            time = 0;
            float latitude = Input.location.lastData.latitude;
            float longitude = Input.location.lastData.longitude;
            print("Location: " + latitude + " " + latitude + " " + longitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            sendLocation(latitude, longitude, "TestStudent");
        }
    }
}
