#include <WiFi.h>
#include <HTTPClient.h>
#include <ArduinoJson.h>
#include <TinyGPS.h>

const char* ssid = "Wokwi-GUEST";
const char* password = "";
const char* serverUrl = "https://5ed9-178-165-27-143.ngrok-free.app/Vehicles/UpdateLocation/1";

const char* gpsData = "$GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,46.9,M,,*47";

TinyGPS gps;

void setup() {
  Serial.begin(115200);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(1000);
    Serial.println("Connecting to WiFi...");
  }
  Serial.println("Connected to WiFi");
  randomSeed(analogRead(0));
}

void loop() {
  if (WiFi.status() == WL_CONNECTED) {
    HTTPClient http;
    http.begin(serverUrl, true);
    
    http.addHeader("Content-Type", "application/json");

    DynamicJsonDocument doc(256);
    doc["newLocation"] = gpsData; 
    char buffer[256];
    serializeJson(doc, buffer);
    String jsonData = String(buffer);

    Serial.println("Sending PUT request to: " + String(serverUrl));
    Serial.println("Data: " + jsonData);

    int httpResponseCode = http.PUT(jsonData);

    if (httpResponseCode > 0) {
      Serial.print("HTTP Response code: ");
      Serial.println(httpResponseCode);
    } else {
      Serial.print("Error code: ");
      Serial.println(httpResponseCode);
    }

    http.end();
  } else {
    Serial.println("Error in WiFi connection");
  }

  delay(10000);
}
