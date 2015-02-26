using System;

namespace Funbit.Ets.Telemetry.Server.Data
{
    public interface IEts2TelemetryData
    {
        bool Connected { get; }

        DateTime GameTime { get; }
        bool GamePaused { get; }

        string TelemetryPluginVersion { get; }
        string GameVersion { get; }

        bool TrailerAttached { get; }

        float TruckSpeed { get; }

        float AccelerationX { get; }
        float AccelerationY { get; }
        float AccelerationZ { get; }

        float CoordinateX { get; }
        float CoordinateY { get; }
        float CoordinateZ { get; }

        float RotationX { get; }
        float RotationY { get; }
        float RotationZ { get; }

        int Gear { get; }
        int Gears { get; }
        int GearRanges { get; }
        int GearRangeActive { get; }

        float EngineRpm { get; }
        float EngineRpmMax { get; }

        float Fuel { get; }
        float FuelCapacity { get; }
        float FuelAverageConsumption { get; }

        float UserSteer { get; }
        float UserThrottle { get; }
        float UserBrake { get; }
        float UserClutch { get; }

        float GameSteer { get; }
        float GameThrottle { get; }
        float GameBrake { get; }
        float GameClutch { get; }

        float TruckMass { get; }

        long TruckModelLength { get; }
        long TruckModelOffset { get; }

        float TrailerMass { get; }

        string TrailerId { get; }
        string TrailerName { get; }

        bool HasJob { get; }
        int JobIncome { get; }
        DateTime JobDeadlineTime { get; }
        DateTime JobRemainingTime { get; }

        string SourceCity { get; }
        string DestinationCity { get; }
        string SourceCompany { get; }
        string DestinationCompany { get; }

        int RetarderBrake { get; }
        int ShifterSlot { get; }
        int ShifterToggle { get; }

        bool CruiseControlOn { get; }
        bool WipersOn { get; }

        bool ParkBrakeOn { get; }
        bool MotorBrakeOn { get; }

        bool ElectricOn { get; }
        bool EngineOn { get; }

        bool BlinkerLeftActive { get; }
        bool BlinkerRightActive { get; }
        bool BlinkerLeftOn { get; }
        bool BlinkerRightOn { get; }

        bool LightsParkingOn { get; }
        bool LightsBeamLowOn { get; }
        bool LightsBeamHighOn { get; }
        bool LightsAuxFrontOn { get; }
        bool LightsAuxRoofOn { get; }
        bool LightsBeaconOn { get; }
        bool LightsBrakeOn { get; }
        bool LightsReverseOn { get; }

        bool BatteryVoltageWarning { get; }
        bool AirPressureWarning { get; }
        bool AirPressureEmergency { get; }
        bool AdblueWarning { get; }
        bool OilPressureWarning { get; }
        bool WaterTemperatureWarning { get; }

        float AirPressure { get; }
        float BrakeTemperature { get; }
        bool FuelWarning { get; }
        float Adblue { get; }
        float AdblueConsumpton { get; }
        float OilPressure { get; }
        float OilTemperature { get; }
        float WaterTemperature { get; }
        float BatteryVoltage { get; }
        float LightsDashboard { get; }
        float WearEngine { get; }
        float WearTransmission { get; }
        float WearCabin { get; }
        float WearChassis { get; }
        float WearWheels { get; }
        float WearTrailer { get; }
        float TruckOdometer { get; }
        float CruiseControlSpeed { get; }

        string TruckMake { get; }
        string TruckMakeId { get; }
        string TruckModel { get; }
    }
}