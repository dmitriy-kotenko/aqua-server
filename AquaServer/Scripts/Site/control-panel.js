var forcedPollingInterval = 2 * 1000;
var hotPollingInterval = 10 * 1000;
var coolPollingInterval = hotPollingInterval * 10;
var forcedPollingMaxAttempts = 8;

var pollingState = {};

function getPollingInterval() {
	if (pollingState.forced) {
		return forcedPollingInterval;
	}
	return document.hidden ? coolPollingInterval : hotPollingInterval;
}

document.addEventListener("visibilitychange", () => {
	if (!document.hidden) {
		executeDevicePolling();
	}
});

function startForcedDevicePolling(device, expectedState) {
	pollingState.forced = true;
	pollingState.expected = pollingState.expected || [];

	var indexOfDevice = pollingState.expected.findIndex((expected) => { return expected.device === device });
	if (indexOfDevice !== -1) {
		pollingState.expected.splice(indexOfDevice, 1);
	}
	pollingState.expected.push({ device, state: expectedState });
	pollingState.attempts = 0;

	executeDevicePolling();
}

function allDevicesHaveTheExpectedState(controlPanelModel) {
	var result = true;
	for (var i = pollingState.expected.length - 1; i >= 0; i--) {
		var expected = pollingState.expected[i];
		// ReSharper disable once PossiblyUnassignedProperty
		if (controlPanelModel[expected.device].CurrentState === expected.state) {
			pollingState.expected.splice(i, 1);
		} else {
			result = false;
		}
	}

	return result;
}

function endForcedDevicePolling() {
	pollingState.forced = false;
	pollingState.expected = [];
}

function executeDevicePolling() {
	clearTimeout(pollingState.timerId);

	if (pollingState.forced && ++pollingState.attempts >= forcedPollingMaxAttempts) {
		endForcedDevicePolling();
	}

	getDeviceStates()
		.complete((xhr, textStatus) => {
			if (textStatus === "success") {
				var res = $.parseJSON(xhr.responseText);

				var needRefreshDeviceStateLog = false;
				for (var currentDevice in res) {
					if (res.hasOwnProperty(currentDevice)) {
						var deviceOptions = res[currentDevice];
						// ReSharper disable PossiblyUnassignedProperty
						needRefreshDeviceStateLog |= updateDisplayedDeviceState(currentDevice, deviceOptions.CurrentState);

						switchInnerToggle(getDeviceScheduleToggle(currentDevice), deviceOptions.UseSchedule);
						handleCurrentStateToggleVisibility(currentDevice, !deviceOptions.UseSchedule);
						switchInnerToggle(getDeviceStateToggle(currentDevice), deviceOptions.TargetState === 1);
						// ReSharper restore PossiblyUnassignedProperty

					}
				}

				if (pollingState.forced && allDevicesHaveTheExpectedState(res)) {
					endForcedDevicePolling();
				}

				if (needRefreshDeviceStateLog) {
					refreshDeviceStateLogSection();
				}
			}

			pollingState.timerId = setTimeout(executeDevicePolling, getPollingInterval());
		});
}

function getDeviceStates() {
	return $.ajax({
		url: "/api/controlPanel/deviceStates",
		method: "GET"
	});
}

function updateDisplayedDeviceState(device, stateToDisplay) {
	if (stateToDisplay == undefined) {
		return false;
	}

	var stateLabel = getDeviceStateLabel(device);
	var currentState = stateLabel.data("current-state");
	if (currentState === stateToDisplay) {
		return false;
	}

	stateLabel
		.toggleClass("state-on", stateToDisplay === 1)
		.toggleClass("state-off", stateToDisplay === 0)
		.data("current-state", stateToDisplay);
	return true;
}

function postDeviceUseSchedule(device, useSchedule) {
	return $.ajax({
		url: "/api/controlPanel/deviceUseSchedule",
		method: "POST",
		data: { device: device, useSchedule: useSchedule },
		success: () => {
			startForcedDevicePolling(
				device,
				useSchedule
					? getDeviceStateLabel(device).data("current-state") ^ 1
					: getDeviceStateToggle(device).prop("checked") ? 1 : 0);
		}
	});
}

function postDeviceTargetState(device, state) {
	return $.ajax({
		url: "/api/controlPanel/deviceTargetState",
		method: "POST",
		data: { device: device, state: state },
		success: () => {
			startForcedDevicePolling(device, state);
		}
	});
}

function handleCurrentStateToggleVisibility(device, visible) {
	var deviceStateToggleWrapper = $(".device-state-toggle-wrapper[data-device='" + device + "']");
	deviceStateToggleWrapper.toggleClass("invisible", !visible);
}

function toggleInnerToggle(toggle) {
	switchInnerToggle(toggle, !toggle.checked);
}

function switchInnerToggle(toggle, switchOn) {
	var innerToggle = $(toggle).data("bs.toggle");
	if (switchOn) {
		innerToggle.on(true);
	} else {
		innerToggle.off(true);
	}
}

function getDeviceStateLabel(device) {
	return $(".device-current-state-label[data-device='" + device + "']");
}

function getDeviceScheduleToggle(device) {
	return $(".device-schedule-toggle[data-device='" + device + "']");
}

function getDeviceStateToggle(device) {
	return $(".device-state-toggle[data-device='" + device + "']");
}

$(".device-schedule-toggle").change(function () {
	var device = $(this).data("device");
	var lockControls = $("input[data-device='" + device + "']");
	lockControls.bootstrapToggle("disable");

	postDeviceUseSchedule(device, this.checked)
		.error(() => {
			lockControls.bootstrapToggle("enable");
			toggleInnerToggle(this);
		})
		.success(() => {
			lockControls.bootstrapToggle("enable");
			handleCurrentStateToggleVisibility(device, !this.checked);
		});
});

$(".device-state-toggle").change(function () {
	var device = $(this).data("device");
	var lockControls = $("input[data-device='" + device + "']");
	lockControls.bootstrapToggle("disable");

	postDeviceTargetState(device, this.checked ? 1 : 0)
		.error(() => {
			lockControls.bootstrapToggle("enable");
			toggleInnerToggle(this);
		})
		.success(() => {
			lockControls.bootstrapToggle("enable");
		});
});

$(() => {
	pollingState.timerId = setTimeout(executeDevicePolling, getPollingInterval());
});