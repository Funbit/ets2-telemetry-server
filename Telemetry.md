# Telemetry reference

This document describes all telemetry properties supported by the [ETS2 Telemetry Web Server](https://github.com/Funbit/ets2-telemetry-server). 

## Property types

#### boolean

A boolean value that can be either true or false.

#### integer

An integer value. Example: 40

#### float

A floating point value. Example: 40.1233

#### date

Date types are always serialized to [ISO 8601 ](http://en.wikipedia.org/wiki/ISO_8601)string in [UTC](http://en.wikipedia.org/wiki/Coordinated_Universal_Time) time zone. Counting starts from 0001 year when 1st January is Monday. Example: "0001-01-05T05:11:00Z".

If you want to convert date string to Javascript date object inside dashboard.js skin file you may use the following technique:

    var nextRestStopTimeDate = new Date(data.game.nextRestStopTime);
    var hours = nextRestStopTimeDate.getUTCHours();   
    var minutes = nextRestStopTimeDate.getUTCMinutes();
	...

For more information about Javascript date object see [the reference](https://developer.mozilla.org/en/docs/Web/JavaScript/Reference/Global_Objects/Date).

#### string

A string value. Example: "hshifter"

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

##### game.gameName

Returns the acronym of the currently running game (ETS2, ATS), or null if game name could not be detected.

		Type: 		string
		Example: 	"ATS"
		CSS Class: 	game-gameName

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

Current version of the game engine (internal).

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

Brand Id of the current truck. Standard values are: "daf", "iveco", "man", "mercedes", "renault", "scania", "volvo".

		Type: 		string
		Example: 	"man"
		CSS Class: 	truck-id

##### truck.make

Localized brand name of the current truck for display purposes.

		Type: 		string
		Example: 	"MAN"
		CSS Class: 	truck-make

##### truck.model

Localized model name of the current truck.

		Type: 		string
		Example: 	"TGX"
		CSS Class: 	truck-model

##### truck.speed

Current truck speed in km/h. If truck is moving backwards the value is negative.

		Type: 		float
		Example: 	50.411231
		CSS Class: 	truck-speed

##### truck.cruiseControlSpeed

Speed selected for the cruise control in km/h.

		Type: 		float
		Example: 	75.0
		CSS Class: 	truck-cruiseControlSpeed

##### truck.cruiseControlOn

Indicates whether cruise control is turned on or off. 

		Type: 		boolean
		Example: 	false
		CSS Class: 	truck-cruiseControlOn

##### truck.odometer

The value of the truck's odometer in km.

		Type: 		float
		Example: 	105809.25
		CSS Class: 	truck-odometer

##### truck.gear

Gear that is currently selected in the engine (physical gear). Positive values reflect forward gears, negative - reverse.

		Type: 		integer
		Example: 	9
		CSS Class: 	truck-gear

##### truck.displayedGear

Gear that is currently displayed on the main dashboard inside the game. Positive values reflect forward gears, negative - reverse.

		Type: 		integer
		Example: 	9
		CSS Class: 	truck-displayedGear

##### truck.forwardGears

Number of forward gears on undamaged truck.

		Type: 		integer
		Example: 	12
		CSS Class: 	truck-forwardGears

##### truck.reverseGears

Number of reverse gears on undamaged truck.

		Type: 		integer
		Example: 	1
		CSS Class: 	truck-reverseGears

##### truck.shifterType

Type of the shifter selected in the game's settings. One of the following values: "arcade", "automatic", "manual", "hshifter".

		Type: 		string
		Example: 	"arcade"
		CSS Class: 	truck-shifterType

##### truck.engineRpm

Current RPM value of the truck's engine (rotates per minute).

		Type: 		float
		Example: 	2490.033
		CSS Class: 	truck-engineRpm

##### truck.engineRpmMax

Maximal RPM value of the truck's engine.

		Type: 		float
		Example: 	2500
		CSS Class: 	truck-engineRpmMax

##### truck.fuel

Current amount of fuel in liters.

		Type: 		float
		Example: 	696.7544
		CSS Class: 	truck-fuel

##### truck.fuelCapacity

Fuel tank capacity in litres.

		Type: 		float
		Example: 	700
		CSS Class: 	truck-fuelCapacity

##### truck.fuelAverageConsumption

Average consumption of the fuel in liters/km.

		Type: 		float
		Example: 	0.4923077
		CSS Class: 	truck-fuelAverageConsumption

##### truck.fuelWarningFactor

Fraction of the fuel capacity bellow which is activated the fuel warning.

		Type: 		float
		Example: 	0.15
		CSS Class: 	truck-fuelWarningFactor

##### truck.fuelWarningOn

Indicates whether low fuel warning is active or not.

		Type: 		boolean
		Example: 	false
		CSS Class: 	truck-fuelWarningOn

##### truck.wearEngine

Current level of truck's engine wear/damage between 0 (min) and 1 (max).

		Type: 		float
		Example: 	0.00675457
		CSS Class: 	truck-wearEngine

##### truck.wearTransmission

Current level of truck's transmission wear/damage between 0 (min) and 1 (max).

		Type: 		float
		Example: 	0.047
		CSS Class: 	truck-wearTransmission

##### truck.wearCabin

Current level of truck's cabin wear/damage between 0 (min) and 1 (max).

		Type: 		float
		Example: 	0.044
		CSS Class: 	truck-wearCabin

##### truck.wearChassis

Current level of truck's chassis wear/damage between 0 (min) and 1 (max).

		Type: 		float
		Example: 	0.043
		CSS Class: 	truck-wearChassis

##### truck.wearWheels

Current level of truck's wheel wear/damage between 0 (min) and 1 (max).

		Type: 		float
		Example: 	0.041
		CSS Class: 	truck-wearWheels

##### truck.userSteer

Steering received from input (-1;1). Note that it is interpreted counterclockwise. If the user presses the steer right button on digital input (e.g. keyboard) this value goes immediatelly to -1.0.

		Type: 		float
		Example: 	-1.0
		CSS Class: 	truck-userSteer

##### truck.userThrottle

Throttle received from input (-1;1). If the user presses the forward button on digital input (e.g. keyboard) this value goes immediatelly to 1.0.

		Type: 		float
		Example: 	0
		CSS Class: 	truck-userThrottle

##### truck.userBrake

Brake received from input (-1;1). If the user presses the brake button on digital input (e.g. keyboard) this value goes immediatelly to 1.0.

		Type: 		float
		Example: 	0
		CSS Class: 	truck-userBrake

##### truck.userClutch

Clutch received from input (-1;1). If the user presses the clutch button on digital input (e.g. keyboard) this value goes immediatelly to 1.0.

		Type: 		float
		Example: 	0
		CSS Class: 	truck-userClutch

##### truck.gameSteer

Steering as used by the simulation (-1;1). Note that it is interpreted counterclockwise. Accounts for interpolation speeds and simulated counterfoces for digital inputs.

		Type: 		float
		Example: 	-0.423521
		CSS Class: 	truck-gameSteer

##### truck.gameThrottle

Throttle pedal input as used by the simulation (0;1). Accounts for the press attack curve for digital inputs or cruise-control input.

		Type: 		float
		Example: 	0.17161
		CSS Class: 	truck-gameThrottle

##### truck.gameBrake

Brake pedal input as used by the simulation (0;1). Accounts for the press attack curve for digital inputs. Does not contain retarder, parking or motor brake.

		Type: 		float
		Example: 	0
		CSS Class: 	truck-gameBrake

##### truck.gameClutch

Clutch pedal input as used by the simulation (0;1). Accounts for the automatic shifting or interpolation of player input.

		Type: 		float
		Example: 	0
		CSS Class: 	truck-gameClutch

##### truck.shifterSlot

Gearbox slot the h-shifter handle is currently in. 0 means that no slot is selected.

		Type: 		integer
		Example: 	4
		CSS Class: 	truck-shifterSlot

##### truck.engineOn

Indicates whether the engine is currently turned on or off.

		Type: 		boolean
		Example: 	false
		CSS Class: 	truck-engineOn

##### truck.electricOn

Indicates whether the electric is enabled or not.

		Type: 		boolean
		Example: 	true
		CSS Class: 	truck-electricOn

##### truck.wipersOn

Indicates whether wipers are currently turned on or off.

		Type: 		boolean
		Example: 	false
		CSS Class: 	truck-wipersOn

##### truck.retarderBrake

Current level of the retarder brake. Ranges from 0 to RetarderStepCount.

		Type: 		integer
		Example: 	0
		CSS Class: 	truck-retarderBrake

##### truck.retarderStepCount

Number of steps in the retarder. Set to zero if retarder is not mounted to the truck.

		Type: 		integer
		Example: 	3
		CSS Class: 	truck-retarderStepCount

##### truck.parkBrakeOn

Is the parking brake enabled or not.

		Type: 		boolean
		Example: 	false
		CSS Class: 	truck-parkBrakeOn

##### truck.motorBrakeOn

Is the motor brake enabled or not.

		Type: 		boolean
		Example: 	true
		CSS Class: 	truck-motorBrakeOn

##### truck.brakeTemperature

Temperature of the brakes in degrees celsius.

		Type: 		float
		Example: 	15.9377184
		CSS Class: 	truck-brakeTemperature

##### truck.adblue

Amount of [AdBlue](http://en.wikipedia.org/wiki/Diesel_exhaust_fluid) in liters.

		Type: 		float
		Example: 	0
		CSS Class: 	truck-adblue

##### truck.adblueCapacity

AdBlue tank capacity in litres.

		Type: 		float
		Example: 	0
		CSS Class: 	truck-adblueCapacity

##### truck.adblueAverageConsumption

Average consumption of the adblue in liters/km.

		Type: 		float
		Example: 	0
		CSS Class: 	truck-adblueAverageConsumption

##### truck.adblueWarningOn

Is the low adblue warning active or not.

		Type: 		boolean
		Example: 	false
		CSS Class: 	truck-adblueWarningOn

##### truck.airPressure

Pressure in the brake air tank in psi.

		Type: 		float
		Example: 	133.043961
		CSS Class: 	truck-airPressure

##### truck.airPressureWarningOn

Is the air pressure warning active or not.

		Type: 		boolean
		Example: 	false
		CSS Class: 	truck-airPressureWarningOn

##### truck.airPressureWarningValue

Pressure of the air in the tank bellow which the warning activates.

		Type: 		float
		Example: 	65
		CSS Class: 	truck-airPressureWarningValue

##### truck.airPressureEmergencyOn

Are the emergency brakes active as result of low air pressure or not.

		Type: 		boolean
		Example: 	false
		CSS Class: 	truck-airPressureEmergencyOn

##### truck.airPressureEmergencyValue

Pressure of the air in the tank bellow which the emergency brakes activate.

		Type: 		float
		Example: 	30
		CSS Class: 	truck-airPressureEmergencyValue

##### truck.oilTemperature

Temperature of the oil in degrees celsius.

		Type: 		float
		Example: 	16.2580566
		CSS Class: 	truck-oilTemperature

##### truck.oilPressure

Pressure of the oil in psi.

		Type: 		float
		Example: 	36.4550362
		CSS Class: 	truck-oilPressure

##### truck.oilPressureWarningOn

Is the oil pressure warning active or not.

		Type: 		boolean
		Example: 	true
		CSS Class: 	truck-oilPressureWarningOn

##### truck.oilPressureWarningValue

Pressure of the oil bellow which the warning activates.

		Type: 		float
		Example: 	10
		CSS Class: 	truck-oilPressureWarningValue

##### truck.waterTemperature

Temperature of the water in degrees celsius.

		Type: 		float
		Example: 	16.1623955
		CSS Class: 	truck-waterTemperature

##### truck.waterTemperatureWarningOn

Is the water temperature warning active or not.

		Type: 		boolean
		Example: 	true
		CSS Class: 	truck-waterTemperatureWarningOn

##### truck.waterTemperatureWarningValue

Temperature of the water above which the warning activates.

		Type: 		float
		Example: 	105
		CSS Class: 	truck-waterTemperatureWarningValue

##### truck.batteryVoltage

Voltage of the battery in volts.

		Type: 		float
		Example: 	23.4985161
		CSS Class: 	truck-batteryVoltage

##### truck.batteryVoltageWarningOn

Is the battery voltage/not charging warning active or not.

		Type: 		boolean
		Example: 	true
		CSS Class: 	truck-batteryVoltageWarningOn

##### truck.batteryVoltageWarningValue

Voltage of the battery bellow which the warning activates.

		Type: 		float
		Example: 	22
		CSS Class: 	truck-batteryVoltageWarningValue

##### truck.lightsDashboardValue

Intensity of the dashboard backlight between 0 (off) and 1 (max).

		Type: 		float
		Example: 	1
		CSS Class: 	truck-lightsDashboardValue

##### truck.lightsDashboardOn

Is the dashboard backlight currently turned on or off.

		Type: 		boolean
		Example: 	true
		CSS Class: 	truck-lightsDashboardOn

##### truck.blinkerLeftActive

Indicates whether the left blinker currently emits light or not.

		Type: 		boolean
		Example: 	false
		CSS Class: 	truck-blinkerLeftActive

##### truck.blinkerRightActive

Indicates whether the right blinker currently emits light or not.

		Type: 		boolean
		Example: 	false
		CSS Class: 	truck-blinkerRightActive

##### truck.blinkerLeftOn

Is left blinker currently turned on or off.

		Type: 		boolean
		Example: 	false
		CSS Class: 	truck-blinkerLeftOn

##### truck.blinkerRightOn

Is right blinker currently turned on or off.

		Type: 		boolean
		Example: 	false
		CSS Class: 	truck-blinkerRightOn

##### truck.lightsParkingOn

Are parking lights enabled or not.

		Type: 		boolean
		Example: 	false
		CSS Class: 	truck-lightsParkingOn

##### truck.lightsBeamLowOn

Are low beam lights enabled or not.

		Type: 		boolean
		Example: 	false
		CSS Class: 	truck-lightsBeamLowOn

##### truck.lightsBeamHighOn

Are high beam lights enabled or not.

		Type: 		boolean
		Example: 	false
		CSS Class: 	truck-lightsBeamHighOn

##### truck.lightsAuxFrontOn

Are auxiliary front lights active or not.

		Type: 		boolean
		Example: 	false
		CSS Class: 	truck-lightsAuxFrontOn

##### truck.lightsAuxRoofOn

Are auxiliary roof lights active or not.

		Type: 		boolean
		Example: 	false
		CSS Class: 	truck-lightsAuxRoofOn

##### truck.lightsBeaconOn

Are beacon lights enabled or not.

		Type: 		boolean
		Example: 	false
		CSS Class: 	truck-lightsBeaconOn

##### truck.lightsBrakeOn

Is brake light active or not.

		Type: 		boolean
		Example: 	false
		CSS Class: 	truck-lightsBrakeOn

##### truck.lightsReverseOn

Is reverse light active or not.

		Type: 		boolean
		Example: 	false
		CSS Class: 	truck-lightsReverseOn

##### truck.placement

Current truck placement in the game world.

		Type: 			placement
		Example: 		-
		CSS Classes: 	truck-placement-x
						truck-placement-y
						truck-placement-z
						truck-placement-heading
						truck-placement-pitch
						truck-placement-roll

##### truck.acceleration

Represents vehicle space linear acceleration of the truck measured in meters per second^2.

		Type: 			vector
		Example: 		-
		CSS Classes: 	truck-acceleration-x
						truck-acceleration-y
						truck-acceleration-z

##### truck.head

Default position of the head in the cabin space.

		Type: 			vector
		Example: 		-
		CSS Classes: 	truck-head-x
						truck-head-y
						truck-head-z

##### truck.cabin

Position of the cabin in the vehicle space. This is position of the joint around which the cabin rotates. This attribute might be not present if the vehicle does not have a separate cabin.

		Type: 			vector
		Example: 		-
		CSS Classes: 	truck-cabin-x
						truck-cabin-y
						truck-cabin-z

##### truck.hook

Position of the trailer connection hook in vehicle space.

		Type: 			vector
		Example: 		-
		CSS Classes: 	truck-hook-x
						truck-hook-y
						truck-hook-z

## Trailer

##### trailer.attached

Is the trailer attached to the truck or not.

		Type: 		boolean
		Example: 	true
		CSS Class: 	trailer-attached

##### trailer.id

Id of the cargo (internal).

		Type: 		string
		Example: 	"derrick"
		CSS Class: 	trailer-id

##### trailer.name

Localized name of the current trailer for display purposes.

		Type: 		string
		Example: 	"Derrick"
		CSS Class: 	trailer-name

##### trailer.mass

Mass of the cargo in kilograms.

		Type: 		float
		Example: 	22000.5
		CSS Class: 	trailer-mass

##### trailer.wear

Current level of trailer wear/damage between 0 (min) and 1 (max).

		Type: 		float
		Example: 	0.0314717
		CSS Class: 	trailer-wear

##### trailer.placement

Current trailer placement in the game world.

		Type: 			placement
		Example: 		-
		CSS Classes: 	trailer-placement-x
						trailer-placement-y
						trailer-placement-z
						trailer-placement-heading
						trailer-placement-pitch
						trailer-placement-roll

## Job

##### job.income

Reward in internal game-specific currency.

		Type: 		integer
		Example: 	2310
		CSS Class: 	job-income

##### job.deadlineTime

Absolute in-game time of end of job delivery window. Delivering the job after this time will cause it be late.

		Type: 		date
		Example: 	"0001-01-09T03:34:00Z"
		CSS Class: 	job-deadlineTime

##### job.remainingTime

Relative remaining in-game time left before deadline.

		Type: 		date
		Example: 	"0001-01-01T07:06:00Z"
		CSS Class: 	job-remainingTime

##### job.sourceCity

Localized name of the source city for display purposes.

		Type: 		string
		Example: 	"Linz"
		CSS Class: 	job-sourceCity

##### job.sourceCompany

Localized name of the source company for display purposes.

		Type: 		string
		Example: 	"DHL"
		CSS Class: 	job-sourceCompany

##### job.destinationCity

Localized name of the destination city for display purposes.

		Type: 		string
		Example: 	"Salzburg"
		CSS Class: 	job-destinationCity

##### job.destinationCompany

Localized name of the destination company for display purposes.

		Type: 		string
		Example: 	"JCB"
		CSS Class: 	job-destinationCompany

## Navigation

##### navigation.estimatedTime

Relative estimated time of arrival.

		Type: 		date
		Example: 	"0001-01-01T02:05:00Z"
		CSS Class: 	navigation-estimatedTime
	
##### navigation.estimatedDistance

Estimated distance to the destination in meters.

		Type: 		integer
		Example: 	132500
		CSS Class: 	navigation-estimatedDistance

##### navigation.speedLimit

Current value of the "Route Advisor speed limit" in km/h.

		Type: 		integer
		Example: 	50
		CSS Class: 	navigation-speedLimit