// SEE Dashboard Skin Tutorial FOR MORE INFORMATION ABOUT THIS FILE

Funbit.Ets.Telemetry.Dashboard.prototype.filter = function (data, utils) {
    // round truck speed    
    data.truckSpeedRounded = Math.abs(data.truckSpeed > 0
        ? Math.floor(data.truckSpeed)
        : Math.round(data.truckSpeed));
    // convert kilometers per hour to miles per hour (just an example)
    data.truckSpeedMph = data.truckSpeed * 0.621371;
    // convert kg to t
    data.trailerMass = (data.trailerMass / 1000.0) + 't';
    // format odometer data as: 00000.0
    data.truckOdometer = utils.formatFloat(data.truckOdometer, 1);
    // convert gear to readable format
    data.gear = data.gear > 0 ? 'D' + data.gear : (data.gear < 0 ? 'R' : 'N');
    // convert rpm to rpm * 100
    data.engineRpm = data.engineRpm / 100;
    // return changed data to the core for rendering
    return data;
};

Funbit.Ets.Telemetry.Dashboard.prototype.render = function (data, utils) {    
}

Funbit.Ets.Telemetry.Dashboard.prototype.initialize = function (skinConfig, utils) {
    utils.preloadImages(['images/bg-on.jpg']);    
    $(document).add('body').on('click', function () {
        window.history.back();
    });
}