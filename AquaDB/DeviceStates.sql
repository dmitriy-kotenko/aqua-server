CREATE TABLE DeviceStates
(
	device SMALLINT PRIMARY KEY,
	current_state BIT,
	use_schedule BIT NOT NULL DEFAULT 1,	
	target_state BIT
)
