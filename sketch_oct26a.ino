void setup() {
  // put your setup code here, to run once:
  pinMode(2, OUTPUT);
  pinMode(3, OUTPUT);
  pinMode(4, OUTPUT);
  pinMode(5, OUTPUT);
  pinMode(6, OUTPUT);
  pinMode(7, OUTPUT);
  pinMode(8, OUTPUT);
  pinMode(9, OUTPUT);
  pinMode(10, OUTPUT);
  pinMode(16, OUTPUT);
  pinMode(14, OUTPUT);
  pinMode(15, OUTPUT);
  Serial.begin(9600);
}

int n = 0;
void loop() {
  if (Serial.available()) {
    logic(Serial.read());
  }
}

void logic(int state) {
  int head = state >> 5;

  if (head == 0) {
    digitalWrite(2, state >> 0 & 1);
    digitalWrite(3, state >> 1 & 1);
    digitalWrite(4, state >> 2 & 1);
    digitalWrite(5, state >> 3 & 1);
  }
  else if (head == 1) {
    digitalWrite(6, state >> 0 & 1);
    digitalWrite(7, state >> 1 & 1);
    digitalWrite(8, state >> 2 & 1);
    digitalWrite(9, state >> 3 & 1);
  }
  else if (head == 2) {
    digitalWrite(10, state >> 0 & 1);
    digitalWrite(16, state >> 1 & 1);
    digitalWrite(14, state >> 2 & 1);
    digitalWrite(15, state >> 3 & 1);
  }
}
