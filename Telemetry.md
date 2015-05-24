## Telemetry reference

This document describes all telemetry properties supported by the ETS2 Telemetry Web Server. 

## Property types

#### boolean

A boolean value that can be either true or false.

#### integer

An integer value. Example: 40

#### float

A floating point value. Example: 40.1233.

#### date

Date types are always serialized to [ISO 8601 ](http://en.wikipedia.org/wiki/ISO_8601)string in [UTC](http://en.wikipedia.org/wiki/Coordinated_Universal_Time) time zone. Counting starts from 0001 year when 1st January is Monday. Example: "0001-01-05T05:11:00Z".

#### string

A string value. Example: "hshifter".

#### placement

A complex property that describes object placement in the game world. Contains the following sub-properties:

	"placement": {
		"x": 13475.5762,		
			// X coordinate of the placement		
		"y": 67.3605652,
			// Y coordinate of the placement
		"z": 14618.6211,
			// Z coordinate of the placement
		"heading": 0.185142234,
			// The angle is measured counterclockwise in horizontal plane 
			// when looking from top where 0 corresponds to forward (north), 
			// 0.25 to left (west), 0.5 to backward (south) and 0.75 to right (east).
			// Stored in unit range where (0,1) corresponds to (0,360).
		"pitch": -0.0067760786,
			// The pitch angle is zero when in horizontal direction,
			// with positive values pointing up (0.25 directly to zenith),
			// and negative values pointing down (-0.25 directly to nadir).
			// Stored in unit range where (-0.25,0.25) corresponds to (-90,90).
		"roll": -0.000293774
			// The angle is measured in counterclockwise 
			// when looking in direction of the roll axis.
        	// Stored in unit range where (-0.5,0.5) corresponds to (-180,180).
	}

#### vector

A complex property that describes a vector or position. Contains the following sub-properties:

	"vector": {
		"x": 13475.5762,		
			// X coordinate 
		"y": 67.3605652,
			// Y coordinate 
		"z": 14618.6211,
			// Z coordinate 
	}

## Game

##### game.connected 

Indicates whether the telemetry server is connected to the simulator (ETS) or not.

		Type: 		boolean
		Example: 	true
		CSS Class: 	game-connected

##### game.paused 

True if game is currently paused, false otherwise.

		Type: 		boolean
		Example: 	true
		CSS Class: 	game-paused

##### game.time 

Current absolute game time. Always starts from 0001 year when 1st January is Monday. 

		Type: 		date
		Example: 	"0001-01-08T21:09:00Z"
		CSS Class: 	game-time

##### game.timeScale 

Scale applied to distance and time to compensate for the scale of the map (e.g. 1s of real time corresponds to local_scale seconds of simulated game time).

		Type: 		float
		Example: 	19
		CSS Class: 	game-timeScale

##### game.nextRestStopTime 

When the fatique simulation is enabled contains relative remaining time before player starts experiencing yawning (time to stop at the next rest stop).

		Type: 		date
		Example: 	"0001-01-01T10:52:00Z"
		CSS Class: 	game-nextRestStopTime

##### game.version 

Current version of the game (internal).

		Type: 		string
		Example: 	"1.10"
		CSS Class: 	game-version

##### game.telemetryPluginVersion 

Current version of the telemetry plugin DLL file (revision).

		Type: 		string
		Example: 	"4"
		CSS Class: 	game-telemetryPluginVersion

## Truck

##### truck.id

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-id

##### truck.make

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-make

##### truck.model

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-model

##### truck.speed

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-speed

##### truck.cruiseControlSpeed

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-cruiseControlSpeed

##### truck.cruiseControlOn

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-cruiseControlOn

##### truck.odometer

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-odometer

##### truck.gear

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-gear

##### truck.displayedGear

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-displayedGear

##### truck.forwardGears

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-forwardGears

##### truck.reverseGears

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-reverseGears

##### truck.shifterType

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-shifterType

##### truck.engineRpm

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-engineRpm

##### truck.engineRpmMax

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-engineRpmMax

##### truck.fuel

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-fuel

##### truck.fuelCapacity

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-fuelCapacity

##### truck.fuelAverageConsumption

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-fuelAverageConsumption

##### truck.fuelWarningFactor

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-fuelWarningFactor

##### truck.fuelWarningOn

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-fuelWarningOn

##### truck.wearEngine

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-wearEngine

##### truck.wearTransmission

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-wearTransmission

##### truck.wearCabin

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-wearCabin

##### truck.wearChassis

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-wearChassis

##### truck.wearWheels

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-wearWheels

##### truck.userSteer

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-userSteer

##### truck.userThrottle

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-userThrottle

##### truck.userBrake

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-userBrake

##### truck.userClutch

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-userClutch

##### truck.gameSteer

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-gameSteer

##### truck.gameThrottle

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-gameThrottle

##### truck.gameBrake

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-gameBrake

##### truck.gameClutch

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-gameClutch

##### truck.shifterSlot

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-shifterSlot

##### truck.engineOn

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-engineOn

##### truck.electricOn

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-electricOn

##### truck.wipersOn

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-wipersOn

##### truck.retarderBrake

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-retarderBrake

##### truck.retarderStepCount

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-retarderStepCount

##### truck.parkBrakeOn

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-parkBrakeOn

##### truck.motorBrakeOn

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-motorBrakeOn

##### truck.brakeTemperature

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-brakeTemperature

##### truck.adblue

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-adblue

##### truck.adblueCapacity

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-adblueCapacity

##### truck.adblueAverageConsumpton

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-adblueAverageConsumpton

##### truck.adblueWarningOn

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-adblueWarningOn

##### truck.airPressure

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-airPressure

##### truck.airPressureWarningOn

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-airPressureWarningOn

##### truck.airPressureWarningValue

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-airPressureWarningValue

##### truck.airPressureEmergencyOn

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-airPressureEmergencyOn

##### truck.airPressureEmergencyValue

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-airPressureEmergencyValue

##### truck.oilTemperature

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-oilTemperature

##### truck.oilPressure

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-oilPressure

##### truck.oilPressureWarningOn

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-oilPressureWarningOn

##### truck.oilPressureWarningValue

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-oilPressureWarningValue

##### truck.waterTemperature

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-waterTemperature

##### truck.waterTemperatureWarningOn

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-waterTemperatureWarningOn

##### truck.waterTemperatureWarningValue

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-waterTemperatureWarningValue

##### truck.batteryVoltage

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-batteryVoltage

##### truck.batteryVoltageWarningOn

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-batteryVoltageWarningOn

##### truck.batteryVoltageWarningValue

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-batteryVoltageWarningValue

##### truck.lightsDashboardValue

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-lightsDashboardValue

##### truck.lightsDashboardOn

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-lightsDashboardOn

##### truck.blinkerLeftActive

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-blinkerLeftActive

##### truck.blinkerRightActive

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-blinkerRightActive

##### truck.blinkerLeftOn

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-blinkerLeftOn

##### truck.blinkerRightOn

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-blinkerRightOn

##### truck.lightsParkingOn

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-lightsParkingOn

##### truck.lightsBeamLowOn

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-lightsBeamLowOn

##### truck.lightsBeamHighOn

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-lightsBeamHighOn

##### truck.lightsAuxFrontOn

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-lightsAuxFrontOn

##### truck.lightsAuxRoofOn

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-lightsAuxRoofOn

##### truck.lightsBeaconOn

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-lightsBeaconOn

##### truck.lightsBrakeOn

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-lightsBrakeOn

##### truck.lightsReverseOn

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-lightsReverseOn

##### truck.placement

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-placement

##### truck.acceleration

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-acceleration

##### truck.head

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-head

##### truck.cabin

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-cabin

##### truck.hook

.

		Type: 		.
		Example: 	.
		CSS Class: 	truck-hook

## Trailer

##### trailer.attached

.

		Type: 		.
		Example: 	.
		CSS Class: 	trailer-attached

##### trailer.id

.

		Type: 		.
		Example: 	.
		CSS Class: 	trailer-id

##### trailer.name

.

		Type: 		.
		Example: 	.
		CSS Class: 	trailer-name

##### trailer.mass

.

		Type: 		.
		Example: 	.
		CSS Class: 	trailer-mass

##### trailer.wear

.

		Type: 		.
		Example: 	.
		CSS Class: 	trailer-wear

##### trailer.placement

.

		Type: 		.
		Example: 	.
		CSS Class: 	trailer-placement

## Job

##### job.income

.

		Type: 		.
		Example: 	.
		CSS Class: 	job-income

##### job.deadlineTime

.

		Type: 		.
		Example: 	.
		CSS Class: 	job-deadlineTime

##### job.remainingTime

.

		Type: 		.
		Example: 	.
		CSS Class: 	job-remainingTime

##### job.sourceCity

.

		Type: 		.
		Example: 	.
		CSS Class: 	job-sourceCity

##### job.sourceCompany

.

		Type: 		.
		Example: 	.
		CSS Class: 	job-sourceCompany

##### job.destinationCity

.

		Type: 		.
		Example: 	.
		CSS Class: 	job-destinationCity

##### job.destinationCompany

.

		Type: 		.
		Example: 	.
		CSS Class: 	job-destinationCompany

## Navigation

##### navigation.estimatedTime

.

		Type: 		.
		Example: 	.
		CSS Class: 	navigation-estimatedTime
	
##### navigation.estimatedDistance

.

		Type: 		.
		Example: 	.
		CSS Class: 	navigation-estimatedDistance

##### navigation.speedLimit

.

		Type: 		.
		Example: 	.
		CSS Class: 	navigation-speedLimit