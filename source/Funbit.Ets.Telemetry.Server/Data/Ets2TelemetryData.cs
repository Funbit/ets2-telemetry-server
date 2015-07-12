using System;
using System.Linq;
using System.Text;
using Funbit.Ets.Telemetry.Server.Data.Reader;
using Funbit.Ets.Telemetry.Server.Helpers;

namespace Funbit.Ets.Telemetry.Server.Data
{
    class Ets2TelemetryData : IEts2TelemetryData
    {
        Box<Ets2TelemetryStructure> _rawData;
        
        public void Update(Ets2TelemetryStructure rawData)
        {
            _rawData = new Box<Ets2TelemetryStructure>(rawData);
        }

        internal static DateTime SecondsToDate(int seconds)
        {
            if (seconds < 0) seconds = 0;
            return new DateTime((long)seconds * 10000000, DateTimeKind.Utc);
        }

        internal static DateTime MinutesToDate(int minutes)
        {
            return SecondsToDate(minutes * 60);
        }

        internal static string BytesToString(byte[] bytes)
        {
            if (bytes == null)
                return string.Empty;
            return Encoding.UTF8.GetString(bytes, 0, Array.FindIndex(bytes, b => b == 0));
        }

        public IEts2Game Game
        {
            get { return new Ets2Game(_rawData); }
        }
        
        public IEts2Truck Truck
        {
            get { return new Ets2Truck(_rawData); }
        }

        public IEts2Trailer Trailer
        {
            get { return new Ets2Trailer(_rawData); }
        }

        public IEts2Job Job
        {
            get { return new Ets2Job(_rawData); }
        }

        public IEts2Navigation Navigation
        {
            get { return new Ets2Navigation(_rawData); }
        }
    }

    class Ets2Game : IEts2Game
    {
        readonly Box<Ets2TelemetryStructure> _rawData;

        public Ets2Game(Box<Ets2TelemetryStructure> rawData)
        {
            _rawData = rawData;
        }

        public bool Connected
        {
            get
            {
                return _rawData.Struct.ets2_telemetry_plugin_revision != 0 &&
                    Ets2ProcessHelper.IsEts2Running &&
                    _rawData.Struct.timeAbsolute != 0;
            }
        }

        public bool Paused
        {
            get { return _rawData.Struct.paused != 0; }
        }

        public DateTime Time
        {
            get { return Ets2TelemetryData.MinutesToDate(_rawData.Struct.timeAbsolute); }
        }

        public float TimeScale
        {
            get { return _rawData.Struct.localScale; }
        }

        public DateTime NextRestStopTime
        {
            get { return Ets2TelemetryData.MinutesToDate(_rawData.Struct.nextRestStop); }
        }

        public string Version
        {
            get { return string.Format("{0}.{1}", _rawData.Struct.ets2_version_major, _rawData.Struct.ets2_version_minor); }
        }

        public string TelemetryPluginVersion
        {
            get { return _rawData.Struct.ets2_telemetry_plugin_revision.ToString(); }
        }
    }

    class Ets2Vector : IEts2Vector
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }

        public Ets2Vector(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    class Ets2Placement : IEts2Placement
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }
        public float Heading { get; private set; }
        public float Pitch { get; private set; }
        public float Roll { get; private set; }

        public Ets2Placement(float x, float y, float z,
            float heading, float pitch, float roll)
        {
            X = x;
            Y = y;
            Z = z;
            Heading = heading;
            Pitch = pitch;
            Roll = roll;
        }
    }

    class Ets2Truck : IEts2Truck
    {
        readonly Box<Ets2TelemetryStructure> _rawData;

        public Ets2Truck(Box<Ets2TelemetryStructure> rawData)
        {
            _rawData = rawData;
        }

        public string Id
        {
            get { return Ets2TelemetryData.BytesToString(_rawData.Struct.truckMakeId); }
        }

        public string Make
        {
            get { return Ets2TelemetryData.BytesToString(_rawData.Struct.truckMake); }
        }

        public string Model
        {
            get { return Ets2TelemetryData.BytesToString(_rawData.Struct.truckModel); }
        }

        /// <summary>
        /// Truck speed in km/h.
        /// </summary>
        public float Speed
        {
            get { return _rawData.Struct.speed * 3.6f; }
        }

        /// <summary>
        /// Cruise control speed in km/h.
        /// </summary>
        public float CruiseControlSpeed
        {
            get { return _rawData.Struct.cruiseControlSpeed * 3.6f; }
        }

        public bool CruiseControlOn
        {
            get { return _rawData.Struct.cruiseControl != 0; }
        }

        public float Odometer
        {
            get { return _rawData.Struct.truckOdometer; }
        }

        public int Gear
        {
            get { return _rawData.Struct.gear; }
        }

        public int DisplayedGear
        {
            get { return _rawData.Struct.displayedGear; }
        }

        public int ForwardGears
        {
            get { return _rawData.Struct.gearsForward; }
        }

        public int ReverseGears
        {
            get { return _rawData.Struct.gearsReverse; }
        }

        public string ShifterType
        {
            get { return Ets2TelemetryData.BytesToString(_rawData.Struct.shifterType); }
        }

        public float EngineRpm
        {
            get { return _rawData.Struct.engineRpm; }
        }

        public float EngineRpmMax
        {
            get { return _rawData.Struct.engineRpmMax; }
        }

        public float Fuel
        {
            get { return _rawData.Struct.fuel; }
        }

        public float FuelCapacity
        {
            get { return _rawData.Struct.fuelCapacity; }
        }

        public float FuelAverageConsumption
        {
            get { return _rawData.Struct.fuelAvgConsumption; }
        }

        public float FuelWarningFactor
        {
            get { return _rawData.Struct.fuelWarningFactor; }
        }

        public bool FuelWarningOn
        {
            get { return _rawData.Struct.fuelWarning != 0; }
        }

        public float WearEngine
        {
            get { return _rawData.Struct.wearEngine; }
        }

        public float WearTransmission
        {
            get { return _rawData.Struct.wearTransmission; }
        }

        public float WearCabin
        {
            get { return _rawData.Struct.wearCabin; }
        }

        public float WearChassis
        {
            get { return _rawData.Struct.wearChassis; }
        }

        public float WearWheels
        {
            get { return _rawData.Struct.wearWheels; }
        }

        public float UserSteer
        {
            get { return _rawData.Struct.userSteer; }
        }

        public float UserThrottle
        {
            get { return _rawData.Struct.userThrottle; }
        }

        public float UserBrake
        {
            get { return _rawData.Struct.userBrake; }
        }

        public float UserClutch
        {
            get { return _rawData.Struct.userClutch; }
        }

        public float GameSteer
        {
            get { return _rawData.Struct.gameSteer; }
        }

        public float GameThrottle
        {
            get { return _rawData.Struct.gameThrottle; }
        }

        public float GameBrake
        {
            get { return _rawData.Struct.gameBrake; }
        }

        public float GameClutch
        {
            get { return _rawData.Struct.gameClutch; }
        }

        public int ShifterSlot
        {
            get { return _rawData.Struct.shifterSlot; }
        }

        //public int ShifterToggle
        //{
        //    get { return _rawData.Struct.shifterToggle; }
        //}

        public bool EngineOn
        {
            get { return _rawData.Struct.engineEnabled != 0; }
        }

        public bool ElectricOn
        {
            get { return _rawData.Struct.electricEnabled != 0; }
        }

        public bool WipersOn
        {
            get { return _rawData.Struct.wipers != 0; }
        }

        public int RetarderBrake
        {
            get { return _rawData.Struct.retarderBrake; }
        }

        public int RetarderStepCount
        {
            get { return (int)_rawData.Struct.retarderStepCount; }
        }

        public bool ParkBrakeOn
        {
            get { return _rawData.Struct.parkBrake != 0; }
        }

        public bool MotorBrakeOn
        {
            get { return _rawData.Struct.motorBrake != 0; }
        }

        public float BrakeTemperature
        {
            get { return _rawData.Struct.brakeTemperature; }
        }

        public float Adblue
        {
            get { return _rawData.Struct.adblue; }
        }

        public float AdblueCapacity
        {
            get { return _rawData.Struct.adblueCapacity; }
        }

        public float AdblueAverageConsumpton
        {
            get { return _rawData.Struct.adblueConsumption; }
        }

        public bool AdblueWarningOn
        {
            get { return _rawData.Struct.adblueWarning != 0; }
        }

        public float AirPressure
        {
            get { return _rawData.Struct.airPressure; }
        }

        public bool AirPressureWarningOn
        {
            get { return _rawData.Struct.airPressureWarning != 0; }
        }

        public float AirPressureWarningValue
        {
            get { return _rawData.Struct.airPressureWarningValue; }
        }

        public bool AirPressureEmergencyOn
        {
            get { return _rawData.Struct.airPressureEmergency != 0; }
        }

        public float AirPressureEmergencyValue
        {
            get { return _rawData.Struct.airPressureEmergencyValue; }
        }

        public float OilTemperature
        {
            get { return _rawData.Struct.oilTemperature; }
        }

        public float OilPressure
        {
            get { return _rawData.Struct.oilPressure; }
        }

        public bool OilPressureWarningOn
        {
            get { return _rawData.Struct.oilPressureWarning != 0; }
        }

        public float OilPressureWarningValue
        {
            get { return _rawData.Struct.oilPressureWarningValue; }
        }

        public float WaterTemperature
        {
            get { return _rawData.Struct.waterTemperature; }
        }

        public bool WaterTemperatureWarningOn
        {
            get { return _rawData.Struct.waterTemperatureWarning != 0; }
        }

        public float WaterTemperatureWarningValue
        {
            get { return _rawData.Struct.waterTemperatureWarningValue; }
        }

        public float BatteryVoltage
        {
            get { return _rawData.Struct.batteryVoltage; }
        }

        public bool BatteryVoltageWarningOn
        {
            get { return _rawData.Struct.batteryVoltageWarning != 0; }
        }

        public float BatteryVoltageWarningValue
        {
            get { return _rawData.Struct.batteryVoltageWarningValue; }
        }

        public float LightsDashboardValue
        {
            get { return _rawData.Struct.lightsDashboard; }
        }

        public bool LightsDashboardOn
        {
            get { return _rawData.Struct.lightsDashboard > 0; }
        }

        public bool BlinkerLeftActive
        {
            get { return _rawData.Struct.blinkerLeftActive != 0; }
        }

        public bool BlinkerRightActive
        {
            get { return _rawData.Struct.blinkerRightActive != 0; }
        }

        public bool BlinkerLeftOn
        {
            get { return _rawData.Struct.blinkerLeftOn != 0; }
        }

        public bool BlinkerRightOn
        {
            get { return _rawData.Struct.blinkerRightOn != 0; }
        }

        public bool LightsParkingOn
        {
            get { return _rawData.Struct.lightsParking != 0; }
        }

        public bool LightsBeamLowOn
        {
            get { return _rawData.Struct.lightsBeamLow != 0; }
        }

        public bool LightsBeamHighOn
        {
            get { return _rawData.Struct.lightsBeamHigh != 0; }
        }

        public bool LightsAuxFrontOn
        {
            get { return _rawData.Struct.lightsAuxFront != 0; }
        }

        public bool LightsAuxRoofOn
        {
            get { return _rawData.Struct.lightsAuxRoof != 0; }
        }

        public bool LightsBeaconOn
        {
            get { return _rawData.Struct.lightsBeacon != 0; }
        }

        public bool LightsBrakeOn
        {
            get { return _rawData.Struct.lightsBrake != 0; }
        }

        public bool LightsReverseOn
        {
            get { return _rawData.Struct.lightsReverse != 0; }
        }

        public IEts2Placement Placement
        {
            get
            {
                return new Ets2Placement(
                    _rawData.Struct.coordinateX,
                    _rawData.Struct.coordinateY,
                    _rawData.Struct.coordinateZ,
                    _rawData.Struct.rotationX,
                    _rawData.Struct.rotationY,
                    _rawData.Struct.rotationZ);
            }
        }

        public IEts2Vector Acceleration
        {
            get
            {
                return new Ets2Vector(
                    _rawData.Struct.accelerationX,
                    _rawData.Struct.accelerationY,
                    _rawData.Struct.accelerationZ);
            }
        }

        public IEts2Vector Head
        {
            get
            {
                return new Ets2Vector(
                    _rawData.Struct.headPositionX,
                    _rawData.Struct.headPositionY,
                    _rawData.Struct.headPositionZ);
            }
        }

        public IEts2Vector Cabin
        {
            get
            {
                return new Ets2Vector(
                    _rawData.Struct.cabinPositionX,
                    _rawData.Struct.cabinPositionY,
                    _rawData.Struct.cabinPositionZ);
            }
        }

        public IEts2Vector Hook
        {
            get
            {
                return new Ets2Vector(
                    _rawData.Struct.hookPositionX,
                    _rawData.Struct.hookPositionY,
                    _rawData.Struct.hookPositionZ);
            }
        }

        /*
        public IEts2GearSlot[] GearSlots
        {
            get
            {
                var array = new IEts2GearSlot[_rawData.Struct.selectorCount];
                for (int i = 0; i < array.Length; i++)
                    array[i] = new Ets2GearSlot(_rawData, i);
                return array;
            }
        }
                
        public IEts2Wheel[] Wheels
        {
            get
            {
                var array = new IEts2Wheel[_rawData.Struct.wheelCount];
                for (int i = 0; i < array.Length; i++)
                    array[i] = new Ets2Wheel(_rawData, i);
                return array;
            }
        }
        */
    }

    class Ets2Trailer : IEts2Trailer
    {
        readonly Box<Ets2TelemetryStructure> _rawData;

        public Ets2Trailer(Box<Ets2TelemetryStructure> rawData)
        {
            _rawData = rawData;
        }

        public bool Attached
        {
            get { return _rawData.Struct.trailer_attached != 0; }
        }

        public string Id
        {
            get { return Ets2TelemetryData.BytesToString(_rawData.Struct.trailerId); }
        }

        public string Name
        {
            get { return Ets2TelemetryData.BytesToString(_rawData.Struct.trailerName); }
        }

        /// <summary>
        /// Trailer mass in kilograms.
        /// </summary>
        public float Mass
        {
            get { return _rawData.Struct.trailerMass; }
        }

        public float Wear
        {
            get { return _rawData.Struct.wearTrailer; }
        }

        public IEts2Placement Placement
        {
            get
            {
                return new Ets2Placement(
                    _rawData.Struct.trailerCoordinateX,
                    _rawData.Struct.trailerCoordinateY,
                    _rawData.Struct.trailerCoordinateZ,
                    _rawData.Struct.trailerRotationX,
                    _rawData.Struct.trailerRotationY,
                    _rawData.Struct.trailerRotationZ);
            }
        }
    }

    class Ets2Navigation : IEts2Navigation
    {
        readonly Box<Ets2TelemetryStructure> _rawData;
        
        public Ets2Navigation(Box<Ets2TelemetryStructure> rawData)
        {
            _rawData = rawData;
        }
        
        public DateTime EstimatedTime
        {
            get { return Ets2TelemetryData.SecondsToDate((int)_rawData.Struct.navigationTime); }
        }

        public int EstimatedDistance
        {
            get { return (int)_rawData.Struct.navigationDistance; }
        }

        public int SpeedLimit
        {
            get { return _rawData.Struct.navigationSpeedLimit > 0 ? ((int)Math.Round(_rawData.Struct.navigationSpeedLimit * 3.6f)) : 0; }
        }
    }

    class Ets2Job : IEts2Job
    {
        readonly Box<Ets2TelemetryStructure> _rawData;

        public Ets2Job(Box<Ets2TelemetryStructure> rawData)
        {
            _rawData = rawData;
        }

        public int Income
        {
            get { return _rawData.Struct.jobIncome; }
        }

        public DateTime DeadlineTime
        {
            get { return Ets2TelemetryData.MinutesToDate(_rawData.Struct.jobDeadline); }
        }

        public DateTime RemainingTime
        {
            get { return Ets2TelemetryData.MinutesToDate(_rawData.Struct.jobDeadline - _rawData.Struct.timeAbsolute); }
        }

        public string SourceCity
        {
            get { return Ets2TelemetryData.BytesToString(_rawData.Struct.jobCitySource); }
        }

        public string SourceCompany
        {
            get { return Ets2TelemetryData.BytesToString(_rawData.Struct.jobCompanySource); }
        }

        public string DestinationCity
        {
            get { return Ets2TelemetryData.BytesToString(_rawData.Struct.jobCityDestination); }
        }

        public string DestinationCompany
        {
            get { return Ets2TelemetryData.BytesToString(_rawData.Struct.jobCompanyDestination); }
        }
    }

    /*
    class Ets2Wheel : IEts2Wheel
    {
        public Ets2Wheel(Box<Ets2TelemetryStructure> rawData, int wheelIndex)
        {
            Simulated = rawData.Struct.wheelSimulated[wheelIndex] != 0;
            Steerable = rawData.Struct.wheelSteerable[wheelIndex] != 0;
            Radius = rawData.Struct.wheelRadius[wheelIndex];
            Position = new Ets2Vector(
                rawData.Struct.wheelPositionX[wheelIndex],
                rawData.Struct.wheelPositionY[wheelIndex],
                rawData.Struct.wheelPositionZ[wheelIndex]);
            Powered = rawData.Struct.wheelPowered[wheelIndex] != 0;
            Liftable = rawData.Struct.wheelLiftable[wheelIndex] != 0;
        }

        public bool Simulated { get; private set; }
        public bool Steerable { get; private set; }
        public bool Powered { get; private set; }
        public bool Liftable { get; private set; }
        public float Radius { get; private set; }
        public IEts2Vector Position { get; private set; }
    }
    
    class Ets2GearSlot : IEts2GearSlot
    {
        public Ets2GearSlot(Box<Ets2TelemetryStructure> rawData, int slotIndex)
        {
            Gear = rawData.Struct.slotGear[slotIndex];
            HandlePosition = (int)rawData.Struct.slotHandlePosition[slotIndex];
            SlotSelectors = (int)rawData.Struct.slotSelectors[slotIndex];
        }

        public int Gear { get; private set; }
        public int HandlePosition { get; private set; }
        public int SlotSelectors { get; private set; }
    }
    */

    class Box<T> where T : struct 
    {
        public T Struct { get; set; }

        public Box(T @struct)
        {
            Struct = @struct;
        }
    }
}