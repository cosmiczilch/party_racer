namespace Data {
    public class CarDataModel {

        public int carId;
        public string carName;
        public string garagePrefabPath;
        public string racePrefabPath;

        public float mass;
        public float drag;

        public float engineForceMin;
        public float engineForceMax;
        public float engineForceRampUpTime;
        public float engineForceRampDownTime;

        public float brakingForce;

        public float expectedSpeedRealUnits_display;

    }
}