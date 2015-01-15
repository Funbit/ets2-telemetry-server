/*
    ======================================
    Custom dashboard telemetry data filter
    ======================================    
*/

// This filter may be used to change telemetry data 
// before it is displayed on the dashboard.
// For example, you may convert km/h to mph, kilograms to tons, etc.
// "data" parameter has all properties defined in
// IEts2TelemetryData interface in dashboard-core.ts

Funbit.Ets.Telemetry.Dashboard.prototype.filter = function (data) {
    // convert kilometers per hour to miles per hour
    //data.truckSpeed = data.truckSpeed * 0.621371;
    return data;
};