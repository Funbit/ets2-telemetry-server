Funbit.Ets.Telemetry.Dashboard.prototype.initialize = function (skinConfig, utils) {
    //
    // skinConfig - a copy of the skin configuration from config.json
    // utils - an object containing several utility functions (see skin tutorial for more information)
    //

    // this function is called before everything else, 
    // so you may perform any DOM or resource initializations / image preloading here

    utils.preloadImages([
        'images/bg-off.png', 'images/bg-on.png',
        'images/blinker-left-off.png', 'images/blinker-left-on.png',
        'images/blinker-right-off.png', 'images/blinker-right-on.png',
        'images/cruise-off.png', 'images/cruise-on.png',
        'images/highbeam-off.png', 'images/highbeam-on.png',
        'images/lowbeam-off.png', 'images/lowbeam-on.png',
        'images/parklights-off.png', 'images/parklights-on.png',
        'images/trailer-off.png', 'images/trailer-on.png'
    ]);

    // return to menu by a click
    $(document).add('body').on('click', function () {
        window.history.back();
    });
}

Funbit.Ets.Telemetry.Dashboard.prototype.filter = function (data, utils) {
    //
    // data - telemetry data JSON object
    // utils - an object containing several utility functions (see skin tutorial for more information)
    //

    // This filter is used to change telemetry data 
    // before it is displayed on the dashboard.
    // You may convert km/h to mph, kilograms to tons, etc.

    // round truck speed    
    data.truckSpeed = data.truckSpeed * 0.621371;
    data.cruiseControlSpeed = data.cruiseControlSpeed * 0.621371;
    data.truckSpeedRounded = Math.abs(data.truckSpeed > 0
        ? Math.floor(data.truckSpeed)
        : Math.round(data.truckSpeed));
    data.cruiseControlSpeedRounded = data.cruiseControlOn
        ? Math.floor(data.cruiseControlSpeed)
        : 0;
    // convert kg to t
    data.trailerMass = data.hasJob ? ((data.trailerMass / 1000.0) + 't') : '';
    // format odometer data as: 00000.0
    data.truckOdometer = utils.formatFloat(data.truckOdometer * 0.621371, 1);
    // convert gear to readable format
    data.gear = data.gear > 0 ? 'D' + data.gear : (data.gear < 0 ? 'R' : 'N');
    // convert rpm to rpm * 100
    data.engineRpm = data.engineRpm / 100;
    // calculate wear
    var wearSumPercent = data.wearEngine * 100 +
        data.wearTransmission * 100 +
        data.wearCabin * 100 +
        data.wearChassis * 100 +
        data.wearWheels * 100;
    wearSumPercent = Math.min(wearSumPercent, 100);
    data.wearSum = Math.round(wearSumPercent) + '%';
    data.wearTrailer = Math.round(data.wearTrailer * 100) + '%';
    // return changed data to the core for rendering
    return data;
};

Funbit.Ets.Telemetry.Dashboard.prototype.render = function (data, utils) {
    //
    // data - same data object as in the filter function
    // utils - an object containing several utility functions (see skin tutorial for more information)
    //

    // we don't have anything custom to render in this skin,
    // but you may use jQuery here to update DOM or CSS
}