CREATE TABLE TemperatureLog
(
    origin_datetime DATETIME2(0) NOT NULL PRIMARY KEY DEFAULT GETUTCDATE(),
    temperature DECIMAL(5, 2) NOT NULL
)
