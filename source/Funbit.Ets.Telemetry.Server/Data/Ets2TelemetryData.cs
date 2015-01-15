using System;
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

        static DateTime MinutesToDate(int minutes)
        {
            return new DateTime((long)minutes * 10000000 * 60, DateTimeKind.Utc);
        }

        static string BytesToString(byte[] bytes)
        {
            if (bytes == null)
                return string.Empty;
            return Encoding.UTF8.GetString(bytes, 0, Array.FindIndex(bytes, b => b == 0));
        }

        public bool Connected
        {
            get { return _rawData.ets2_telemetry_plugin_revision != 0 && Ets2ProcessHelper.IsEts2Running; }
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

        public bool TrailerAttached
        {
            get { return _rawData.trailerMass > 0; }
        }

        /// <summary>
        /// Truck speed in km/h.
        /// </summary>
        public float TruckSpeed
        {
            get { return _rawData.speed * 3.6f; }
        }

        public float AccelerationX
        {
            get { return _rawData.accelerationX; }
        }

        public float AccelerationY
        {
            get { return _rawData.accelerationY; }
        }

        public float AccelerationZ
        {
            get { return _rawData.accelerationZ; }
        }

        public float CoordinateX
        {
            get { return _rawData.coordinateX; }
        }

        public float CoordinateY
        {
            get { return _rawData.coordinateY; }
        }

        public float CoordinateZ
        {
            get { return _rawData.coordinateZ; }
        }

        public float RotationX
        {
            get { return _rawData.rotationX; }
        }

        public float RotationY
        {
            get { return _rawData.rotationY; }
        }

        public float RotationZ
        {
            get { return _rawData.rotationZ; }
        }

        public int Gear
        {
            get { return _rawData.gear; }
        }

        public int Gears
        {
            get { return _rawData.gears; }
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
        public float TruckMass
        {
            get { return _rawData.TruckMass; }
        }

        public long TruckModelLength
        {
            get { return _rawData.modelLength; }
        }

        public long TruckModelOffset
        {
            get { return _rawData.modelOffset; }
        }

        /// <summary>
        /// Trailer mass in kilograms.
        /// </summary>
        public float TrailerMass
        {
            get { return _rawData.trailerMass; }
        }

        public string TrailerId
        {
            get { return BytesToString(_rawData.trailerId); }
        }

        public string TrailerName
        {
            get { return BytesToString(_rawData.trailerName); }
        }

        public int JobIncome
        {
            get { return _rawData.jobIncome; }
        }

        public DateTime JobDeadlineTime
        {
            get { return MinutesToDate(_rawData.jobDeadline); }
        }

        public DateTime JobRemainingTime
        {
            get { return MinutesToDate(_rawData.jobDeadline - _rawData.timeAbsolute); }
        }

        public string SourceCity
        {
            get { return BytesToString(_rawData.jobCitySource); }
        }

        public string DestinationCity
        {
            get { return BytesToString(_rawData.jobCityDestination); }
        }

        public string SourceCompany
        {
            get { return BytesToString(_rawData.jobCompanySource); }
        }

        public string DestinationCompany
        {
            get { return BytesToString(_rawData.jobCompanyDestination); }
        }

        public int RetarderBrake
        {
            get { return _rawData.retarderBrake; }
        }

        public int ShifterSlot
        {
            get { return _rawData.shifterSlot; }
        }

        public int ShifterToggle
        {
            get { return _rawData.shifterToggle; }
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
            get { return _rawData.engine_enabled != 0; }
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

        public bool BatteryVoltageWarning
        {
            get { return _rawData.batteryVoltageWarning != 0; }
        }

        public bool AirPressureWarning
        {
            get { return _rawData.airPressureWarning != 0; }
        }

        public bool AirPressureEmergency
        {
            get { return _rawData.airPressureEmergency != 0; }
        }

        public bool AdblueWarning
        {
            get { return _rawData.adblueWarning != 0; }
        }

        public bool OilPressureWarning
        {
            get { return _rawData.oilPressureWarning != 0; }
        }

        public bool WaterTemperatureWarning
        {
            get { return _rawData.waterTemperatureWarning != 0; }
        }

        public float AirPressure
        {
            get { return _rawData.airPressure; }
        }

        public float BrakeTemperature
        {
            get { return _rawData.brakeTemperature; }
        }

        public float FuelWarning
        {
            get { return _rawData.fuelWarning; }
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

        public float LightsDashboard
        {
            get { return _rawData.lightsDashboard; }
        }

        public float WearEngine
        {
            get { return _rawData.wearEngine; }
        }

        public float WearTransmission
        {
            get { return _rawData.wearTransmission; }
        }

        public float WearCabin
        {
            get { return _rawData.wearCabin; }
        }

        public float WearChassis
        {
            get { return _rawData.wearChassis; }
        }

        public float WearWheels
        {
            get { return _rawData.wearWheels; }
        }

        public float WearTrailer
        {
            get { return _rawData.wearTrailer; }
        }

        public float TruckOdometer
        {
            get { return _rawData.truckOdometer; }
        }
    }
}