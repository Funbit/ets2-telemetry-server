using System;
using System.Linq;
using System.Text;
using Funbit.Ets.Telemetry.Server.Data.Reader;
using Funbit.Ets.Telemetry.Server.Helpers;

namespace Funbit.Ets.Telemetry.Server.Data
{
    class Ets2TelemetryData : IEts2TelemetryData
    {
        Ets2TelemetryStructure _rawData;
        
        public void Update(Ets2TelemetryStructure rawData)
        {
            _rawData = rawData;
        }

        internal static DateTime MinutesToDate(int minutes)
        {
            if (minutes < 0) minutes = 0;
            return new DateTime((long)minutes * 10000000 * 60, DateTimeKind.Utc);
        }

        internal static string BytesToString(byte[] bytes)
        {
            if (bytes == null)
                return string.Empty;
            return Encoding.UTF8.GetString(bytes, 0, Array.FindIndex(bytes, b => b == 0));
        }
        
        public bool Connected
        {
            get { return _rawData.ets2_telemetry_plugin_revision != 0 && 
                Ets2ProcessHelper.IsEts2Running && 
                _rawData.timeAbsolute != 0; }
        }

        public DateTime GameTime
        {
            get { return MinutesToDate(_rawData.timeAbsolute); }
        }

        public bool GamePaused
        {
            get { return _rawData.paused != 0; }
        }

        public string TelemetryPluginVersion
        {
            get { return _rawData.ets2_telemetry_plugin_revision.ToString(); }
        }

        public string GameVersion
        {
            get { return string.Format("{0}.{1}", _rawData.ets2_version_major, _rawData.ets2_version_minor); }
        }

        public DateTime NextRestStopTime
        {
            get { return MinutesToDate(_rawData.nextRestStop); }
        }

        public float GameTimeScale
        {
            get { return _rawData.localScale; }
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

    class Ets2Truck : IEts2Truck
    {
        readonly Ets2TelemetryStructure _rawData;

        public Ets2Truck(Ets2TelemetryStructure rawData)
        {
            _rawData = rawData;
        }

        /// <summary>
        /// Truck speed in km/h.
        /// </summary>
        public float Speed
        {
            get { return _rawData.speed * 3.6f; }
        }

        public IEts2Vector Acceleration
        {
            get
            {
                return new Ets2Vector(
                      _rawData.accelerationX,
                      _rawData.accelerationY,
                      _rawData.accelerationZ);
            }
        }

        public IEts2Vector Coordinate
        {
            get
            {
                return new Ets2Vector(
                      _rawData.coordinateX,
                      _rawData.coordinateY,
                      _rawData.coordinateZ);
            }
        }

        public IEts2Vector Rotation
        {
            get
            {
                return new Ets2Vector(
                    _rawData.rotationX,
                    _rawData.rotationY,
                    _rawData.rotationZ);
            }
        }

        public int Gear
        {
            get { return _rawData.gear; }
        }

        public int ForwardGears
        {
            get { return _rawData.gearsForward; }
        }

        public int ReverseGears
        {
            get { return _rawData.gearsReverse; }
        }

        public int GearRanges
        {
            get { return _rawData.gearRanges; }
        }

        public int GearRangeActive
        {
            get { return _rawData.gearRangeActive; }
        }

        public float EngineRpm
        {
            get { return _rawData.engineRpm; }
        }

        public float EngineRpmMax
        {
            get { return _rawData.engineRpmMax; }
        }

        public float Fuel
        {
            get { return _rawData.fuel; }
        }

        public float FuelCapacity
        {
            get { return _rawData.fuelCapacity; }
        }

        public float FuelAverageConsumption
        {
            get { return _rawData.fuelAvgConsumption; }
        }

        public float UserSteer
        {
            get { return _rawData.userSteer; }
        }

        public float UserThrottle
        {
            get { return _rawData.userThrottle; }
        }

        public float UserBrake
        {
            get { return _rawData.userBrake; }
        }

        public float UserClutch
        {
            get { return _rawData.userClutch; }
        }

        public float GameSteer
        {
            get { return _rawData.gameSteer; }
        }

        public float GameThrottle
        {
            get { return _rawData.gameThrottle; }
        }

        public float GameBrake
        {
            get { return _rawData.gameBrake; }
        }

        public float GameClutch
        {
            get { return _rawData.gameClutch; }
        }

        /// <summary>
        /// Truck mass in kilograms.
        /// </summary>
        public float Mass
        {
            get { return _rawData.truckWeight; }
        }

        public long ModelLength
        {
            get { return _rawData.modelLength; }
        }

        public long ModelOffset
        {
            get { return _rawData.modelOffset; }
        }

        public int RetarderBrake
        {
            get { return _rawData.retarderBrake; }
        }

        public int RetarderStepCount
        {
            get { return (int)_rawData.retarderStepCount; }
        }

        public int ShifterSlot
        {
            get { return _rawData.shifterSlot; }
        }

        public int ShifterToggle
        {
            get { return _rawData.shifterToggle; }
        }

        public float AirPressure
        {
            get { return _rawData.airPressure; }
        }

        public float BrakeTemperature
        {
            get { return _rawData.brakeTemperature; }
        }
        
        public float Adblue
        {
            get { return _rawData.adblue; }
        }

        public float AdblueConsumpton
        {
            get { return _rawData.adblueConsumption; }
        }

        public float OilPressure
        {
            get { return _rawData.oilPressure; }
        }

        public float OilTemperature
        {
            get { return _rawData.oilTemperature; }
        }

        public float WaterTemperature
        {
            get { return _rawData.waterTemperature; }
        }

        public float BatteryVoltage
        {
            get { return _rawData.batteryVoltage; }
        }

        public float AdblueCapacity
        {
            get { return _rawData.adblueCapacity; }
        }
        
        public float EngineWear
        {
            get { return _rawData.wearEngine; }
        }

        public float TransmissionWear
        {
            get { return _rawData.wearTransmission; }
        }

        public float CabinWear
        {
            get { return _rawData.wearCabin; }
        }

        public float ChassisWear
        {
            get { return _rawData.wearChassis; }
        }

        public float WheelsWear
        {
            get { return _rawData.wearWheels; }
        }

        public float Odometer
        {
            get { return _rawData.truckOdometer; }
        }

        /// <summary>
        /// Cruise control speed in km/h.
        /// </summary>
        public float CruiseControlSpeed
        {
            get { return _rawData.cruiseControlSpeed * 3.6f; }
        }

        public string Make
        {
            get { return Ets2TelemetryData.BytesToString(_rawData.truckMake); }
        }

        public string MakeId
        {
            get { return Ets2TelemetryData.BytesToString(_rawData.truckMakeId); }
        }

        public string Model
        {
            get { return Ets2TelemetryData.BytesToString(_rawData.truckModel); }
        }

        public IEts2TruckIndicators Indicators
        {
            get { return new Ets2TruckIndicators(_rawData); }
        }
        
        public IEts2Vector Head
        {
            get
            {
                return new Ets2Vector(
                    _rawData.headPositionX,
                    _rawData.headPositionY,
                    _rawData.headPositionZ);
            }
        }

        public IEts2Vector Cabin
        {
            get
            {
                return new Ets2Vector(
                    _rawData.cabinPositionX,
                    _rawData.cabinPositionY,
                    _rawData.cabinPositionZ);
            }
        }

        public IEts2Vector Hook
        {
            get
            {
                return new Ets2Vector(
                    _rawData.hookPositionX,
                    _rawData.hookPositionY,
                    _rawData.hookPositionZ);
            }
        }

        public IEts2Wheel[] Wheels
        {
            get
            {
                var array = new IEts2Wheel[_rawData.wheelCount];
                for (int i = 0; i < array.Length; i++)
                    array[i] = new Ets2Wheel(_rawData, i);
                return array;
            }
        }

        public IEts2GearSlot[] GearSlots
        {
            get
            {
                var array = new IEts2GearSlot[_rawData.selectorCount];
                for (int i = 0; i < array.Length; i++)
                    array[i] = new Ets2GearSlot(_rawData, i);
                return array;
            }
        }

        public string ShifterType
        {
            get { return Ets2TelemetryData.BytesToString(_rawData.shifterType); }
        }
    }

    class Ets2TruckIndicators : IEts2TruckIndicators
    {
        readonly Ets2TelemetryStructure _rawData;

        public Ets2TruckIndicators(Ets2TelemetryStructure rawData)
        {
            _rawData = rawData;
        }

        public float FuelWarningFactor
        {
            get { return _rawData.fuelWarningFactor; }
        }
        
        public float AirPressureWarningValue
        {
            get { return _rawData.airPressureWarningValue; }
        }

        public float AirPressureEmergencyValue
        {
            get { return _rawData.airPressureEmergencyValue; }
        }

        public float OilPressureWarningValue
        {
            get { return _rawData.oilPressureWarningValue; }
        }

        public float WaterTemperatureWarningValue
        {
            get { return _rawData.waterTemperatureWarningValue; }
        }

        public float BatteryVoltageWarningValue
        {
            get { return _rawData.batteryVoltageWarningValue; }
        }

        public float LightsDashboardValue
        {
            get { return _rawData.lightsDashboard; }
        }

        public bool LightsDashboardOn
        {
            get { return _rawData.lightsDashboard > 0; }
        }

        public bool FuelWarningOn
        {
            get { return _rawData.fuelWarning != 0; }
        }

        public bool CruiseControlOn
        {
            get { return _rawData.cruiseControl != 0; }
        }

        public bool WipersOn
        {
            get { return _rawData.wipers != 0; }
        }

        public bool ParkBrakeOn
        {
            get { return _rawData.parkBrake != 0; }
        }

        public bool MotorBrakeOn
        {
            get { return _rawData.motorBrake != 0; }
        }

        public bool ElectricOn
        {
            get { return _rawData.electricEnabled != 0; }
        }

        public bool EngineOn
        {
            get { return _rawData.engineEnabled != 0; }
        }

        public bool BlinkerLeftActive
        {
            get { return _rawData.blinkerLeftActive != 0; }
        }

        public bool BlinkerRightActive
        {
            get { return _rawData.blinkerRightActive != 0; }
        }

        public bool BlinkerLeftOn
        {
            get { return _rawData.blinkerLeftOn != 0; }
        }

        public bool BlinkerRightOn
        {
            get { return _rawData.blinkerRightOn != 0; }
        }

        public bool LightsParkingOn
        {
            get { return _rawData.lightsParking != 0; }
        }

        public bool LightsBeamLowOn
        {
            get { return _rawData.lightsBeamLow != 0; }
        }

        public bool LightsBeamHighOn
        {
            get { return _rawData.lightsBeamHigh != 0; }
        }

        public bool LightsAuxFrontOn
        {
            get { return _rawData.lightsAuxFront != 0; }
        }

        public bool LightsAuxRoofOn
        {
            get { return _rawData.lightsAuxRoof != 0; }
        }

        public bool LightsBeaconOn
        {
            get { return _rawData.lightsBeacon != 0; }
        }

        public bool LightsBrakeOn
        {
            get { return _rawData.lightsBrake != 0; }
        }

        public bool LightsReverseOn
        {
            get { return _rawData.lightsReverse != 0; }
        }

        public bool BatteryVoltageWarningOn
        {
            get { return _rawData.batteryVoltageWarning != 0; }
        }

        public bool AirPressureWarningOn
        {
            get { return _rawData.airPressureWarning != 0; }
        }

        public bool AirPressureEmergencyOn
        {
            get { return _rawData.airPressureEmergency != 0; }
        }

        public bool AdblueWarningOn
        {
            get { return _rawData.adblueWarning != 0; }
        }

        public bool OilPressureWarningOn
        {
            get { return _rawData.oilPressureWarning != 0; }
        }

        public bool WaterTemperatureWarningOn
        {
            get { return _rawData.waterTemperatureWarning != 0; }
        }
    }

    class Ets2Trailer : IEts2Trailer
    {
        readonly Ets2TelemetryStructure _rawData;

        public Ets2Trailer(Ets2TelemetryStructure rawData)
        {
            _rawData = rawData;
        }

        public bool Attached
        {
            get { return _rawData.trailer_attached != 0; }
        }

        /// <summary>
        /// Trailer mass in kilograms.
        /// </summary>
        public float Mass
        {
            get { return _rawData.trailerMass; }
        }

        public string Id
        {
            get { return Ets2TelemetryData.BytesToString(_rawData.trailerId); }
        }

        public string Name
        {
            get { return Ets2TelemetryData.BytesToString(_rawData.trailerName); }
        }

        public float Wear
        {
            get { return _rawData.wearTrailer; }
        }
    }

    class Ets2Job : IEts2Job
    {
        readonly Ets2TelemetryStructure _rawData;

        public Ets2Job(Ets2TelemetryStructure rawData)
        {
            _rawData = rawData;
        }

        public int Income
        {
            get { return _rawData.jobIncome; }
        }

        public DateTime DeadlineTime
        {
            get { return Ets2TelemetryData.MinutesToDate(_rawData.jobDeadline); }
        }

        public DateTime RemainingTime
        {
            get { return Ets2TelemetryData.MinutesToDate(_rawData.jobDeadline - _rawData.timeAbsolute); }
        }

        public string SourceCity
        {
            get { return Ets2TelemetryData.BytesToString(_rawData.jobCitySource); }
        }

        public string DestinationCity
        {
            get { return Ets2TelemetryData.BytesToString(_rawData.jobCityDestination); }
        }

        public string SourceCompany
        {
            get { return Ets2TelemetryData.BytesToString(_rawData.jobCompanySource); }
        }

        public string DestinationCompany
        {
            get { return Ets2TelemetryData.BytesToString(_rawData.jobCompanyDestination); }
        }
    }

    class Ets2Wheel : IEts2Wheel
    {
        public Ets2Wheel(Ets2TelemetryStructure rawData, int wheelIndex)
        {
            Simulated = rawData.wheelSimulated[wheelIndex] != 0;
            Steerable = rawData.wheelSteerable[wheelIndex] != 0;
            Radius = rawData.wheelRadius[wheelIndex];
            Position = new Ets2Vector(
                rawData.wheelPositionX[wheelIndex],
                rawData.wheelPositionY[wheelIndex],
                rawData.wheelPositionZ[wheelIndex]);
            Powered = rawData.wheelPowered[wheelIndex] != 0;
            Liftable = rawData.wheelLiftable[wheelIndex] != 0;
        }

        public bool Simulated { get; private set; }
        public bool Steerable { get; private set; }
        public float Radius { get; private set; }
        public IEts2Vector Position { get; private set; }
        public bool Powered { get; private set; }
        public bool Liftable { get; private set; }
    }

    class Ets2GearSlot : IEts2GearSlot
    {
        public Ets2GearSlot(Ets2TelemetryStructure rawData, int slotIndex)
        {
            Gear = rawData.slotGear[slotIndex];
            HandlePosition = (int)rawData.slotHandlePosition[slotIndex];
            SlotSelectors = (int)rawData.slotSelectors[slotIndex];
        }

        public int Gear { get; private set; }
        public int HandlePosition { get; private set; }
        public int SlotSelectors { get; private set; }
    }
}