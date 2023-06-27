/*//handle checkbox change event
$('input[type="checkbox"]').on('change', function () {
    var selectedBrands = $('input[type="checkbox"][name="car-brand"]:checked').map(function () {
        return $(this).val();
    }).get();

    var selectedCategories = $('input[type="checkbox"][name="car-category"]:checked').map(function () {
        return $(this).val();
    }).get();

    var selectedModels = $('input[type="checkbox"][name="car-model"]:checked').map(function () {
        return $(this).val();
    }).get();

    var selectedEnergies = $('input[type="checkbox"][name="car-energy"]:checked').map(function () {
        return $(this).val();
    }).get();

    var selectedGears = $('input[type="checkbox"][name="car-gear"]:checked').map(function () {
        return $(this).val();
    }).get();

    var selectedCritair = $('input[type="checkbox"][name="car-critair"]:checked').map(function () {
        return $(this).val();
    }).get();

    var hasAircon = $('input[type="checkbox"][name="AirConditionner"]:checked').map(function () {
        return $(this).val();
    }).get()

    var hasBt = $('input[type="checkbox"][name="Bluetooth"]:checked').map(function () {
        return $(this).val();
    }).get()

    var hasRevRad = $('input[type="checkbox"][name="ReversingRadar"]:checked').map(function () {
        return $(this).val();
    }).get()

    var hasGps = $('input[type="checkbox"][name="Gps"]:checked').map(function () {
        return $(this).val();
    }).get()

    var hasSpeedReg = $('input[type="checkbox"][name="SpeedRegulator"]:checked').map(function () {
        return $(this).val();
    }).get()

    var hasAbs = $('input[type="checkbox"][name="Abs"]:checked').map(function () {
        return $(this).val();
    }).get()


    //send AJAX request to retrieve filtered product data
    $.ajax({
        url: '@Url.Action("GetFilteredCars", "Cars")',
        type: 'POST',
        dataType: 'html',
        data: {
            brandList: selectedBrands,
            categoryList: selectedCategories,
            modelList: selectedModels,
            energyList: selectedEnergies,
            gearList: selectedGears,
            critairList: selectedCritair,
            hasAirConditionner: hasAircon,
            hasBluetooth: hasBt,
            hasReversingRadar: hasRevRad,
            hasGPS: hasGps,
            hasSpeedRegulator: hasSpeedReg,
            hasABS: hasAbs
        },
        success: function (result) {
            $('.cars-cards').html(result);
        },
        error: function () {
            console.log('An error occured');
        }
    });
});*/