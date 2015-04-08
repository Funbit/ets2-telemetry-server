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

        /// <summary>
        /// Brand Id of the current truck. 
        /// Example: "man".
        /// </summary>
        string MakeId { get; }
        /// <summary>
        /// Localized brand name of the current truck for display purposes.
        /// Example: "MAN".
        /// </summary>
        string Make { get; }
        /// <summary>
        /// Localized model name of the current truck.
        /// Example: "TGX".
        /// </summary>
        string Model { get; }

        int Gear { get; }
        /// <summary>
        /// Number of forward gears on undamaged truck.
        /// </summary>
        int ForwardGears { get; }
        /// <summary>
        /// Number of reverse gears on undamaged truck.
        /// </summary>
        int ReverseGears { get; }
        int GearRanges { get; }
        int GearRangeActive { get; }

        float EngineRpm { get; }
        /// <summary>
        /// Maximal RPM value of the current truck's engine (rotates per minute).
        /// </summary>
        float EngineRpmMax { get; }

        float Fuel { get; }
        /// <summary>
        /// Fuel tank capacity in litres.
        /// Example: 700
        /// </summary>
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
        /// <summary>
        /// Number of steps in the retarder.
        /// Set to zero if retarder is not mounted to the truck.
        /// Example: 3
        /// </summary>
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
        /// <summary>
        /// AdBlue tank capacity in litres.
        /// Example: 0
        /// </summary>
        float AdblueCapacity { get; }
        
        float EngineWear { get; }
        float TransmissionWear { get; }
        float CabinWear { get; }
        float ChassisWear { get; }
        float WheelsWear { get; }

        /// <summary>
        /// Default position of the head in the cabin space.
        /// Example: { "x": -0.795116067, "y": 1.43522251, "z": -0.08483863 }
        /// </summary>
        IEts2Vector Head { get; }
        /// <summary>
        /// Position of the cabin in the vehicle space.
        /// This is position of the joint around which the cabin rotates.
        /// This attribute might be not present if the vehicle does not have a separate cabin.
        /// Example: { "x": 0, "y": 1.36506855, "z": -1.70362806 }
        /// </summary>
        IEts2Vector Cabin { get; }
        /// <summary>
        /// Position of the trailer connection hook in vehicle space.
        /// Example: { "x": 0, "y": 0.939669, "z": -6.17736959 }
        /// </summary>
        IEts2Vector Hook { get; }

        IEts2TruckIndicators Indicators { get; }

        /// <summary>
        /// List of all available truck (and trailer) wheels.
        /// </summary>
        IEts2Wheel[] Wheels { get; }
        /// <summary>
        /// All available selectors (e.g. range/splitter toggles).
        /// </summary>
        IEts2GearSlot[] GearSlots { get; }

        /// <summary>
        /// Type of the shifter.
        /// One of the following values: "arcade", "automatic", "manual", "hshifter".
        /// </summary>
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

        /// <summary>
        /// Fraction of the fuel capacity bellow which is activated the fuel warning.
        /// Example: 0.15
        /// </summary>
        float FuelWarningFactor { get; }
        /// <summary>
        /// Pressure of the air in the tank bellow which the warning activates.
        /// Example: 65
        /// </summary>
        float AirPressureWarningValue { get; }
        /// <summary>
        /// Pressure of the air in the tank bellow which the emergency brakes activate.
        /// Example: 30
        /// </summary>
        float AirPressureEmergencyValue { get; }
        /// <summary>
        /// Pressure of the oil bellow which the warning activates.
        /// Example: 10
        /// </summary>
        float OilPressureWarningValue { get; }
        /// <summary>
        /// Temperature of the water above which the warning activates.
        /// Example: 105
        /// </summary>
        float WaterTemperatureWarningValue { get; }
        /// <summary>
        /// Voltage of the battery bellow which the warning activates.
        /// Example: 22
        /// </summary>
        float BatteryVoltageWarningValue { get; }
    }

    public interface IEts2Job
    {
        /// <summary>
        /// Reward in internal game-specific currency.
        /// </summary>
        int Income { get; }

        /// <summary>
        /// Absolute in-game time of end of job delivery window.
        /// Delivering the job after this time will cause it be late.
        /// Example: "0001-01-09T03:34:00Z"
        /// </summary>
        DateTime DeadlineTime { get; }
        /// <summary>
        /// Relative remaining in-game time left before deadline.
        /// Example: "0001-01-01T07:06:00Z"
        /// </summary>
        DateTime RemainingTime { get; }

        /// <summary>
        /// Localized name of the source city for display purposes.
        /// Example: "Linz"
        /// </summary>
        string SourceCity { get; }
        /// <summary>
        /// Localized name of the destination city for display purposes.
        /// Example: "Salzburg"
        /// </summary>
        string DestinationCity { get; }
        /// <summary>
        /// Localized name of the source company for display purposes.
        /// Example: "DHL"
        /// </summary>
        string SourceCompany { get; }
        /// <summary>
        /// Localized name of the destination company for display purposes.
        /// Example: "JCB"
        /// </summary>
        string DestinationCompany { get; }
    }

    public interface IEts2Trailer
    {
        /// <summary>
        /// Id of the cargo for internal use by code.
        /// Example: "derrick"
        /// </summary>
        string Id { get; }
        /// <summary>
        /// Localized name of the current trailer for display purposes.
        /// Example: "Derrick"
        /// </summary>
        string Name { get; }
        bool Attached { get; }
        /// <summary>
        /// Mass of the cargo in kilograms.
        /// Example: 22000
        /// </summary>
        float Mass { get; }
        float Wear { get; }
    }

    public interface IEts2Wheel
    {
        /// <summary>
        /// Is the wheel physicaly simulated or not.
        /// </summary>
        bool Simulated { get; }
        /// <summary>
        /// Is the wheel steerable or not.
        /// </summary>
        bool Steerable { get; }
        /// <summary>
        /// Radius of the wheel.
        /// </summary>
        float Radius { get; }
        /// <summary>
        /// Position of respective wheels in the vehicle space.
        /// Example: { "x": -0.9, "y": 0.506898463, "z": 6.25029 }
        /// </summary>
        IEts2Vector Position { get; }
        /// <summary>
        /// Is the wheel powered or not.
        /// </summary>
        bool Powered { get; }
        /// <summary>
        /// Is the wheel liftable or not.
        /// </summary>
        bool Liftable { get; }
    }

    public interface IEts2GearSlot
    {
        /// <summary>
        /// Gear selected when requirements for this h-shifter slot are meet.
        /// Example: 0
        /// </summary>
        int Gear { get; }
        /// <summary>
        /// Position of h-shifter handle.
        /// Zero corresponds to neutral position. 
        /// Mapping to physical position of the handle depends on input setup.
        /// Example: 0
        /// </summary>
        int HandlePosition { get; }
        /// <summary>
        /// Bitmask of required on/off state of selectors.
        /// Only first N number of bits are relevant (where N is the number of IEts2GearSlot objects).
        /// </summary>
        int SlotSelectors { get; }
    }
}