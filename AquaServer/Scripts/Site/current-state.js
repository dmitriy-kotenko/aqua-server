function handleProgressBarValue() {
	$(".progress-bar").each(function () {
		var min = $(this).attr("aria-valuemin");
		var max = $(this).attr("aria-valuemax");
		var now = $(this).attr("aria-valuenow");
		var siz = (now - min) * 100 / (max - min);
		$(this).css("width", siz + "%");
	});
}
handleProgressBarValue();

function handleMinutesSinceUpdateValue() {
	$(".minutes-ago").each(function () {
		if ($(this).data("toggle") === "tooltip") {
			var minutesSinceUpdate = this.getAttribute("title");

			var formattedText = moment.duration(-minutesSinceUpdate, "minutes").humanize(true);
			this.setAttribute("title", formattedText);
		}
	});

	$('[data-toggle="tooltip"]').tooltip();
}
handleMinutesSinceUpdateValue();