#include <HID-Project.h>
#include <HID-Settings.h>

// Receiving serial data variables
const byte numBytes = 16;
byte receivedBytes[numBytes];
byte numReceived = 0;
bool newData = false;

bool mouseInitialized = false;

void setup() {
  Serial.begin(115200);
  while (!Serial) { ; }

  Keyboard.begin();
  AbsoluteMouse.begin();
  Serial.setTimeout(50);

  Serial.println("Configured");
}

void loop() {
  RecvBytesFromSerial();
  ProcessSerialData();
}

void RecvBytesFromSerial()
{
  static boolean recvInProgress = false;
  static byte ndx = 0;
  byte startMarker = 0xfe;
  byte endMarker = 0xff;
  byte rb;

  while (Serial.available() > 0 && newData == false)
  {
    rb = Serial.read();
    if (recvInProgress == true)
    {
      if (rb != endMarker)
      {
        receivedBytes[ndx] = rb;
        ndx++;
        if (ndx >= numBytes)
        {
          ndx = numBytes - 1;
        }
      }
      else
      {
        receivedBytes[ndx] = 0xff;
        recvInProgress = false;
        numReceived = ndx;
        ndx = 0;
        newData = true;
      }
    }
    else if (rb == startMarker)
    {
      recvInProgress = true;
    }
  }
}

int ConvertTo16BitInt(byte high, byte low)
{
  int val = high;
  val = (val << 8) | low;
  return val;
}

void PressKey(byte keyCode) {
  // 對應 HID Usage ID 按下鍵
  Keyboard.press(keyCode);
}

// 單鍵按壓指令(0x03): [cmd, key, holdTimeHigh, holdTimeLow]
void PerformKeyboardPress()
{
  byte keyCode = receivedBytes[1];
  int holdTime = ConvertTo16BitInt(receivedBytes[2], receivedBytes[3]);

  PressKey(keyCode);
  delay(holdTime);
  Keyboard.releaseAll();
  Serial.println("DONE");
}

// 組合鍵按壓指令(0x0A): [cmd, key1, key2, holdTimeHigh, holdTimeLow]
void PerformComboPress() {
  byte key1 = receivedBytes[1];
  byte key2 = receivedBytes[2];
  int holdTime = ConvertTo16BitInt(receivedBytes[3], receivedBytes[4]);

  PressKey(key1);
  PressKey(key2);
  delay(holdTime);
  Keyboard.releaseAll();
  Serial.println("DONE");
}

// 初始化滑鼠(0x04): [cmd, xHigh, xLow, yHigh, yLow]
void InitMouse()
{
  int width = ConvertTo16BitInt(receivedBytes[1], receivedBytes[2]);
  int height = ConvertTo16BitInt(receivedBytes[3], receivedBytes[4]);
  AbsoluteMouse.moveTo(width, height);
  mouseInitialized = true;
  Serial.println("DONE");
}

// 滑鼠移動(0x05): [cmd, xHigh, xLow, yHigh, yLow]
void PerformMouseMove()
{
  int x = ConvertTo16BitInt(receivedBytes[1], receivedBytes[2]);
  int y = ConvertTo16BitInt(receivedBytes[3], receivedBytes[4]);
  AbsoluteMouse.moveTo(x, y);
  Serial.println("DONE");
}

// 滑鼠點擊(0x06): [cmd, button, holdTimeHigh, holdTimeLow]
void PerformMouseClick()
{
  byte button = receivedBytes[1];
  int holdTime = ConvertTo16BitInt(receivedBytes[2], receivedBytes[3]);

  if (!mouseInitialized) {
    Serial.println("DONE");
    return;
  }

  switch(button)
  {
    case 0x01:
      AbsoluteMouse.press(MOUSE_LEFT);
      delay(holdTime);
      AbsoluteMouse.release(MOUSE_LEFT);
      break;
    case 0x02:
      AbsoluteMouse.press(MOUSE_MIDDLE);
      delay(holdTime);
      AbsoluteMouse.release(MOUSE_MIDDLE);
      break;
    case 0x03:
      AbsoluteMouse.press(MOUSE_RIGHT);
      delay(holdTime);
      AbsoluteMouse.release(MOUSE_RIGHT);
      break;
  }
  Serial.println("DONE");
}

// Ping(0x01)用於測試
void Ping()
{
  Serial.println("PONG");
  Serial.println("DONE");
}

void ProcessSerialData()
{
  if (newData == true)
  {
    switch (receivedBytes[0])
    {
      case 0x01:
        Ping();
        break;
      case 0x03:
        PerformKeyboardPress();
        break;
      case 0x04:
        InitMouse();
        break;
      case 0x05:
        PerformMouseMove();
        break;
      case 0x06:
        PerformMouseClick();
        break;
      case 0x0A:
        PerformComboPress();
        break;
      default:
        Serial.println("Unknown command ID received.");
        Serial.println("DONE");
        break;
    }
    newData = false;
  }
}
