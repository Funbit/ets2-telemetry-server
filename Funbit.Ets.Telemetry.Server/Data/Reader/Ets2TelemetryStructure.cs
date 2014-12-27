using System.Runtime.InteropServices;

namespace Funbit.Ets.Telemetry.Server.Data.Reader
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct Ets2TelemetryStructure
    {
        public uint time;
        public uint paused;

        public uint ets2_telemetry_plugin_revision;
        public uint ets2_version_major;
        public uint ets2_version_minor;

        // ***** REVISION 1 ****** //

        public short engine_enabled;
        public short trailer_attached;

        public float speed;
        public float accelerationX;
        public float accelerationY;
        public float accelerationZ;
        
        public float coordinateX;
        public float coordinateY;
        public float coordinateZ;
        
        public float rotationX;
        public float rotationY;
        public float rotationZ;
        
        public int gear;
        public int gears;
        public int gearRanges;
        public int gearRangeActive;

        public float engineRpm;
        public float engineRpmMax;

        public float fuel;
        public float fuelCapacity;
        public float fuelRate;
        public float fuelAvgConsumption;

        public float userSteer;
        public float userThrottle;
        public float userBrake;
        public float userClutch;
        
        public float gameSteer;
        public float gameThrottle;
        public float gameBrake;
        public float gameClutch;
        
        public float TruckMass;
        public float trailerWeight;

        public int modelOffset;
        public int modelLength;

        public int trailerOffset;
        public int trailerLength;
        
        public int timeAbsolute;
        public int gearsReverse;

        public float trailerMass;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)] 
        public byte[] trailerId;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)] 
        public byte[] trailerName;

        public int jobIncome;
        public int jobDeadline;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)] 
        public byte[] jobCitySource;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)] 
        public byte[] jobCityDestination;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)] 
        public byte[] jobCompanySource;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)] 
        public byte[] jobCompanyDestination;

        // ***** REVISION 3 ****** //

        public int retarderBrake;
        public int shifterSlot;
        public int shifterToggle;
        public int fill;

        public byte cruiseControl;
        public byte wipers;
               
        public byte parkBrake;
        public byte motorBrake;
               
        public byte electricEnabled;
        public byte engineEnabled;
               
        public byte blinkerLeftActive;
        public byte blinkerRightActive;
        public byte blinkerLeftOn;
        public byte blinkerRightOn;
               
        public byte lightsParking;
        public byte lightsBeamLow;
        public byte lightsBeamHigh;
        public byte lightsAuxFront;
        public byte lightsAuxRoof;
        public byte lightsBeacon;
        public byte lightsBrake;
        public byte lightsReverse;
               
        public byte batteryVoltageWarning;
        public byte airPressureWarning;
        public byte airPressureEmergency;
        public byte adblueWarning;
        public byte oilPressureWarning;
        public byte waterTemperatureWarning;

        public float airPressure;
        public float brakeTemperature;
        public float fuelWarning;
        public float adblue;
        public float adblueConsumption;
        public float oilPressure;
        public float oilTemperature;
        public float waterTemperature;
        public float batteryVoltage;
        public float lightsDashboard;
        public float wearEngine;
        public float wearTransmission;
        public float wearCabin;
        public float wearChassis;
        public float wearWheels;
        public float wearTrailer;
        public float truckOdometer;
    }
}