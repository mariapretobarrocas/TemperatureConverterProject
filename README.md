# TemperatureConverterProject

A console application that given a csv file with a SensorID, a Reading value, a Time in Unix Epoch format and a Format in Celsius, Farenheit or Kelvin returns a Json
file grouped by SensorID, where the reading value is displayed in Celsius and the Time is converted to a date and time in ISO 8601 date format.

I am getting the cvs file and reading all rows and saving them to a List of TemperatureConverterParam, where I have the SensorId and a List of Reading, that has a 
reading value and a reading time. I am then groupying by Sensor Id and returning a new List to be added to the Json file. 
