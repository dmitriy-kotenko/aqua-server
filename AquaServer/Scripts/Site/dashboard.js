var hotRefreshTemperatureChartInterval = 20 * 1000;
var hotRefreshCurrentStateInterval = 15 * 1000;
var coolRefreshTemperatureChartInterval = hotRefreshTemperatureChartInterval * 10;
var coolRefreshCurrentStateInterval = hotRefreshCurrentStateInterval * 10;
var minNumberOfPoints = 60;
var lastKnownMillis = 0;

var refreshTemperatureChartTimeoutId;
var refreshCurrentStateSectionTimeoutId;

function getRefreshTemperatureChartInterval() {
	return document.hidden ? coolRefreshTemperatureChartInterval : hotRefreshTemperatureChartInterval;
}

function getRefreshCurrentStateInterval() {
	return document.hidden ? coolRefreshCurrentStateInterval : hotRefreshCurrentStateInterval;
}

document.addEventListener("visibilitychange", () => {
	if (!document.hidden) {
		executeRefreshTemperatureChart();
		executeRefreshCurrentStateSection();
	}
});

function refreshDeviceStateLogSection() {
	$.get("/dashboard/deviceStateLogSection", function (result) {
		$("#device-state-log-section").html(result);
	});
}

function refreshCurrentStateSection() {
	$.get("/dashboard/currentStateSection", function (result) {
		$("#current-state-section").html(result);
		handleProgressBarValue();
		handleMinutesSinceUpdateValue();
	});
}

function refreshTemperatureChart() {
	$.get("/api/dashboard/getRefreshData", { lastKnownMillis: lastKnownMillis }, function (response) {
		var mainSeries = chart.series[0];
		var newPointsCount = response.newData.length;

		if (newPointsCount === 0) {
			return;
		}

		for (var i = 0; i < newPointsCount; i++) {
			mainSeries.addPoint(response.newData[i], false, mainSeries.xData.length > minNumberOfPoints);
		}
		chart.redraw();

		lastKnownMillis = response.newData[newPointsCount - 1][0];

		if (!zonesAreChanged(mainSeries.options.zones, response.zones)) {
			return;
		}

		for (var j = 0; j < chart.series.length; j++) {
			var series = chart.series[j];
			var options = series.options;
			options.animation = false;
			options.zones = response.zones;

			series.update(options);
		}
	});
}

function zonesAreChanged(currentZones, newZones) {
	if (currentZones.length !== newZones.length) {
		return true;
	}

	if (newZones.length === 0) {
		return false;
	}

	var lastCurrentZone = currentZones[currentZones.length - 1];
	var lastNewZone = newZones[newZones.length - 1];
	return JSON.stringify(lastCurrentZone) !== JSON.stringify(lastNewZone);
}

function executeRefreshCurrentStateSection() {
	clearTimeout(refreshCurrentStateSectionTimeoutId);
	refreshCurrentStateSection();
	refreshCurrentStateSectionTimeoutId = setTimeout(executeRefreshCurrentStateSection, getRefreshCurrentStateInterval());
}

function executeRefreshTemperatureChart() {
	clearTimeout(refreshTemperatureChartTimeoutId);
	refreshTemperatureChart();
	refreshTemperatureChartTimeoutId = setTimeout(executeRefreshTemperatureChart, getRefreshTemperatureChartInterval());
}

$(() => {
	refreshCurrentStateSectionTimeoutId = setTimeout(executeRefreshCurrentStateSection, getRefreshCurrentStateInterval());

	var chartSeries = chart.series[0];
	var dataLength = chartSeries.xData.length;
	if (dataLength !== 0) {
		lastKnownMillis = chartSeries.xData[dataLength - 1];
	}
	refreshTemperatureChartTimeoutId = setTimeout(executeRefreshTemperatureChart, getRefreshTemperatureChartInterval());
});