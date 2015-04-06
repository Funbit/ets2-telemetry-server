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

        IEts2Truck Truck { get; }
        IEts2Trailer Trailer { get; }
        IEts2Job Job { get; }
    }

    public interface IEts2Vector
    {
        float X { get; }
        float Y { get; }
        float Z { get; }
    }

    public interface IEts2Truck
    {
        float Speed { get; }

        IEts2Vector Acceleration { get; }
        IEts2Vector Coordinate { get; }
        IEts2Vector Rotation { get; }

        float Odometer { get; }
        float CruiseControlSpeed { get; }

        string MakeId { get; }
        string Make { get; }
        string Model { get; }

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

        float Mass { get; }

        long ModelLength { get; }
        long ModelOffset { get; }

        int RetarderBrake { get; }
        int RetarderStepCount { get; }
        int ShifterSlot { get; }
        int ShifterToggle { get; }
        
        float AirPressure { get; }
        float BrakeTemperature { get; }
        float Adblue { get; }
        float AdblueConsumpton { get; }
        float OilPressure { get; }
        float OilTemperature { get; }
        float WaterTemperature { get; }
        float BatteryVoltage { get; }
        float AdblueCapacity { get; }
        
        float EngineWear { get; }
        float TransmissionWear { get; }
        float CabinWear { get; }
        float ChassisWear { get; }
        float WheelsWear { get; }

        IEts2Vector Head { get; }
        IEts2Vector Cabin { get; }
        IEts2Vector Hook { get; }

        IEts2TruckIndicators Indicators { get; }

        IEts2Wheel[] Wheels { get; }
        IEts2GearSlot[] GearSlots { get; }

        string ShifterType { get; }
    }

    public interface IEts2TruckIndicators
    {
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

        bool BatteryVoltageWarningOn { get; }
        bool AirPressureWarningOn { get; }
        bool AirPressureEmergencyOn { get; }
        bool AdblueWarningOn { get; }
        bool OilPressureWarningOn { get; }
        bool WaterTemperatureWarningOn { get; }

        float LightsDashboardValue { get; }
        bool LightsDashboardOn { get; }

        bool FuelWarningOn { get; }

        float FuelWarningFactor { get; }
        float AirPressureWarningValue { get; }
        float AirPressureEmergencyValue { get; }
        float OilPressureWarningValue { get; }
        float WaterTemperatureWarningValue { get; }
        float BatteryVoltageWarningValue { get; }
    }

    public interface IEts2Job
    {
        int Income { get; }

        DateTime DeadlineTime { get; }
        DateTime RemainingTime { get; }

        string SourceCity { get; }
        string DestinationCity { get; }
        string SourceCompany { get; }
        string DestinationCompany { get; }
    }

    public interface IEts2Trailer
    {
        bool Attached { get; }

        float Mass { get; }

        string Id { get; }
        string Name { get; }

        float Wear { get; }
    }

    public interface IEts2Wheel
    {
        bool Simulated { get; }
        bool Steerable { get; }
        float Radius { get; }
        IEts2Vector Position { get; }
        bool Powered { get; }
        bool Liftable { get; }
    }

    public interface IEts2GearSlot
    {
        int Gear { get; }
        int HandlePosition { get; }
        int SlotSelectors { get; }
    }
}